namespace proxy_simulator.Constants
{
    public static class ProgramConstants
    {
        public static class Logs
        {
            public static class LifeCycle
            {
                public const string INIT_LOG = "\n\n\n==============================Application API logs:======================================";
                public const string STARTING_LOG = "--> [Lifecycle] Application Started: initializing DB...";
                public const string STOPING_LOG = "--> [Lifecycle] Graceful Shutdown: Server is shutting down";
                public const string STOPED_LOG = "--> [Lifecycle] Application Stopped: Server is completely closed, cleaning up...";
            }
        }

        public static class ServicesIp
        {
            // becasu the containers are in the same docker network, we can use the service name as the hostname
            // we cant use localhost because the containers are isolated from each other, so we need to use the service name as the hostname
            public const string MULTEMEDIA = "http://multimedia-simulator:5000/";
            public const string TELEMETRY = "http://telemetry-simulator:8000/";
        }
    }
}
