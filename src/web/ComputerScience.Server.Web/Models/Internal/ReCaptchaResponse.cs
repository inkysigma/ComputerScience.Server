using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ComputerScience.Server.Web.Models.Internal
{
    public class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("hostname")]
        public string HostName { get; set; }
    }
}
