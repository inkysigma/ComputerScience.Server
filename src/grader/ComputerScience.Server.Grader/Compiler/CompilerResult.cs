namespace ComputerScience.Server.Grader.Compiler
{
    public class CompilerResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }

        public static CompilerResult Fail(string message)
        {
            return new CompilerResult
            {
                Succeeded = false,
                Message = message
            };
        }

        public static CompilerResult Succeed()
        {
            return new CompilerResult
            {
                Succeeded = true
            };
        }
    }
}
