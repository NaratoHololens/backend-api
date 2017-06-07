using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configurations
{
    public class CognitiveServiceConfiguration
    {
        public string Subkey { get; set; }
        public string PersonGroup { get; set; }
        public string Uri { get; set; }
       
    }
}
