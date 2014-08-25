// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoEvent.cs" company="">
//   
// </copyright>
// <summary>
//   The mongo event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MongoEventStreaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// The mongo event.
    /// </summary>
    public class MongoEvent
    {
        /// <summary> Initializes a new instance of the <see cref="MongoEvent"/> class.  </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="eventName"> The event name.  </param>
        public MongoEvent(string sender, string eventName)
        {
            this.EventName = eventName;
            this.Sender = sender;
        }

        /// <summary> Gets or sets the id. </summary>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary> Gets the event name. </summary>
        public string EventName { get; private set; }

        /// <summary> Gets the sender. </summary>
        public string Sender { get; private set; }

        // TODO
        // Event params?
    }
}
