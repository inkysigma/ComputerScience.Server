namespace ComputerScience.Server.Web.Models.Response
{
    public enum PostFileResponse
    {
        Success,
        Failure,
        FileSizeError,
        Full,
        FileTypeError,
        IdentificationError,
        ProblemIdentificationError
    }
}
