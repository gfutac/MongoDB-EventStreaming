// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDispatcher.cs" company="">
//   
// </copyright>
// <summary>
//   The event dispatcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MongoEventStreaming
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using MongoHandler;

    /// <summary>
    /// The event dispatcher.
    /// </summary>
    public class EventDispatcher
    {
        /// <summary> The mongo listeners. </summary>
        private static ConcurrentBag<IMongoEventListener> mongoListeners = new ConcurrentBag<IMongoEventListener>();

        /// <summary> The last id. </summary>
        private BsonValue lastId;

        /// <summary> Initializes a new instance of the <see cref="EventDispatcher"/> class. </summary>
        public EventDispatcher()
        {
            this.lastId = BsonMinKey.Value;
        }

        /// <summary> The register listener. </summary>
        /// <param name="listener"> The listener. </param>
        public void RegisterListener(IMongoEventListener listener)
        {
            mongoListeners.Add(listener);
        }

        /// <summary> The unregister listener. </summary>
        /// <param name="listener"> The listener. </param>
        public void UnregisterListener(IMongoEventListener listener)
        {
            if (mongoListeners.Contains(listener))
            {
                IMongoEventListener takeMeOut;
                mongoListeners.TryTake(out takeMeOut);
            }
        }

        /// <summary> The start dispatcher. </summary>
        public void StartDispatcher()
        {
            var dispatcherThread = new Thread(() =>
                {
                    using (var mdb = new MongoContext())
                    {
                        var collection = mdb.GetDatabase("test").GetCollection<MongoEvent>("capped_collection");

                        if (!BsonClassMap.IsClassMapRegistered(typeof(MongoEvent)))
                        {
                            BsonClassMap.RegisterClassMap<MongoEvent>();
                        }

                        while (true)
                        {
                            var query = Query.GT("Id", this.lastId);

                            MongoCursor<MongoEvent> cursor =
                                collection.FindAs<MongoEvent>(query)
                                    .SetFlags(QueryFlags.TailableCursor | QueryFlags.AwaitData)
                                    .SetSortOrder("$natural");

                            using (var enumerator = new MongoCursorEnumerator<MongoEvent>(cursor))
                            {
                                while (true)
                                {
                                    if (enumerator.MoveNext())
                                    {
                                        var evt = enumerator.Current;
                                        this.lastId = (ObjectId)evt.GetType().GetProperty("Id").GetValue(evt);
                                        this.DispatchEvent(evt);
                                        Console.WriteLine(evt.ToJson());
                                    }
                                    else
                                    {
                                        if (enumerator.IsDead)
                                        {
                                            break;
                                        }

                                        if (!enumerator.IsServerAwaitCapable)
                                        {
                                            // Thread.Sleep(TimeSpan.FromMilliseconds(50));
                                        }
                                    }
                                }
                            }
                        }
                    }
                });  
         
            dispatcherThread.Start();
        }

        /// <summary> The dispatch event. </summary>
        /// <param name="e"> The e. </param>
        private void DispatchEvent(MongoEvent e)
        {
            foreach (IMongoEventListener listener in mongoListeners)
            {
                listener.Notify(e);
            }
        }
    }
}
