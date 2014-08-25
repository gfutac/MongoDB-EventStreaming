namespace Program
{
    using System.Threading;

    using MongoDB.Bson;

    using MongoEventStreaming;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            EventDispatcher ed = new EventDispatcher();
            ed.StartDispatcher();

            while (true)
            {
                Thread.Sleep(50);
            }
        }
    }
}
