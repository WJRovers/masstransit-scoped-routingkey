namespace MT.DI.Test.Infrastructure.Options
{
    public class EventBusOptions
    {
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// The number of retries to attempt
        /// </summary>
        /// <value>The number of attempts</value>
        public int RetryCount { get; set; } = 1;

        /// <summary>
        /// Specify the maximum number of concurrent messages that are consumed
        /// </summary>
        /// <value>The limit</value>
        public ushort PrefetchCount { get; set; } = 5;
    }
}