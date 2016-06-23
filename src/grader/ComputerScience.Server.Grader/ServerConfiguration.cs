namespace ComputerScience.Server.Grader
{
    public class ServerConfiguration
    {
        public int ThreadCount { get; set; } = 4;

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }

        public string Database { get; set; } = "compsci";

        public string[] Directories { get; set; }

        public bool RelativeDirectories { get; set; } = false;
    }
}
