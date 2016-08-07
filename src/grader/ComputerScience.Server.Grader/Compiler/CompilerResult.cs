namespace ComputerScience.Server.Grader.Compiler
{
    public class CompilerResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }

        public static CompilerResult Fail(string message)
        {
            return new CompilerResult
            {
                Succeeded = false,
                Message = message
            };
        }

        public static CompilerResult Succeed(string filePath)
        {
            return new CompilerResult
            {
                FilePath = filePath,
                Succeeded = true
            };
        }
    }
}
