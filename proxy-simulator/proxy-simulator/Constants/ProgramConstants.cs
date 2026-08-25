namespace proxy_simulator.Constants
{
    public static class ProgramConstants
    {
       public static class Logs
        {
            public static class LifeCycle
            {
                public static string INIT_LOG = "\n\n\n==============================Application API logs:======================================";
                public static string STARTING_LOG = "--> [Lifecycle] Application Started: initializing DB...";
                public static string STOPING_LOG = "--> [Lifecycle] Graceful Shutdown: Server is shutting down";
                public static string STOPED_LOG = "--> [Lifecycle] Application Stopped: Server is completely closed, cleaning up...";
            }
        }
    }
}
