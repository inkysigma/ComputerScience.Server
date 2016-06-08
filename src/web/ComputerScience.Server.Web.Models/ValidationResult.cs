namespace ComputerScience.Server.Web.Models
{
    public enum ValidationResult
    {
        /// <summary>
        /// Solution model is valid
        /// </summary>
        Valid,

        /// <summary>
        /// Solution model is invalid and irreparable. Discard immediately.
        /// </summary>
        Invalid,

        /// <summary>
        /// Solution model is invalid but repparable.
        /// </summary>
        Incomplete
    }
}
