// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Class1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MongoEventStreaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using MongoHandler;

    /// <summary> The event listener. </summary>
    public interface IMongoEventListener
    {
        /// <summary> The notify. </summary>
        /// <param name="e"> The e. </param>
        void Notify(MongoEvent e);
    }
}
