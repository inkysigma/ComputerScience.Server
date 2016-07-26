namespace ComputerScience.Server.Web.Models.Response
{
    /// <summary>
    /// The wrapper for every response given by the API
    /// </summary>
    public class StandardResponse
    {
        /// <summary>
        /// Whether or not the operation succeeded
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Whether or not it was the client's fault
        /// </summary>
        public bool ClientError { get; set; }

        /// <summary>
        /// The error code. Not terribly useful without more documentation. Should be 200 if everything is fine.
        /// </summary>
        public int Code { get; set; }


        /// <summary>
        /// A user friendly information regarding the problem. Displaying it is optional.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Information about the error
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Developer information for logging
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// The actual payload. This is null if the operation fails or does not return anything
        /// </summary>
        public dynamic Payload { get; set; }

        public static StandardResponse Create(dynamic payload)
        {
            return new StandardResponse
            {
                Succeeded = true,
                Code = 200,
                ClientError = false,
                Payload = payload
            };
        }
    }
}
