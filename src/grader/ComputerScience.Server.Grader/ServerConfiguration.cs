namespace ComputerScience.Server.Grader
{
    public class ServerConfiguration
    {
        public int ThreadCount { get; set; } = 4;

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }

        public bool UseDatabase { get; set; }
    }
}
