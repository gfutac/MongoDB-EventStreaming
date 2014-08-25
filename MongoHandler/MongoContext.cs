// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the MongoHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MongoHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MongoDB.Driver;
    using MongoDB.Driver.Internal;

    /// <summary>
    /// The mongo handler.
    /// </summary>
    public class MongoContext : IDisposable
    {
        /// <summary> The mongo client. </summary>
        private MongoClient mongoClient;

        /// <summary> The mongo server. </summary>
        private MongoServer mongoServer;

        /// <summary> The disposed. </summary>
        private bool disposed;

        /// <summary> Initializes a new instance of the <see cref="MongoContext"/> class. </summary>
        public MongoContext()
            : this("mongodb://localhost")
        {
        }

        /// <summary> Initializes a new instance of the <see cref="MongoContext"/> class. </summary>
        /// <param name="connectionString"> The connection string. </param>
        public MongoContext(string connectionString)
        {
            this.mongoClient = new MongoClient(connectionString);
            this.mongoServer = this.mongoClient.GetServer();           
        }

        /// <summary> The get database. </summary>
        /// <param name="dbName"> The db name. </param>
        /// <returns> The <see cref="MongoDatabase"/>. </returns>
        /// <exception cref="Exception"> </exception>
        public MongoDatabase GetDatabase(string dbName)
        {
            if (!this.mongoServer.DatabaseExists(dbName))
            {
                throw new Exception("Database with given name does not exist.");
            }

            return this.mongoServer.GetDatabase(dbName);
        }

        #region IDisposable
        /// <summary> The dispose. </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary> The dispose. </summary>
        /// <param name="disposing"> The disposing. </param>
        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.mongoServer != null)
                    {
                        this.mongoServer.Disconnect();
                    }
                }
            }
            
            this.disposed = true;
        }

        #endregion
    }
}
