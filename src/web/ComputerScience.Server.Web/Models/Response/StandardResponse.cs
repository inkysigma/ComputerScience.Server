namespace ComputerScience.Server.Web.Models.Response
{
    public class StandardResponse
    {
        public bool Succeeded { get; set; }

        public bool ClientError { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public string Information { get; set; }

        public string Developer { get; set; }

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
