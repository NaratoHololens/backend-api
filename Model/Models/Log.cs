using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Models
{
    public class Log
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserID { get; set; }
        private readonly string type = "LOG";
        public string Type { get { return type; } }

        public Log()
        {

        }

        public Log(DateTime timestamp, string userID)
        {
            Timestamp = timestamp;
            UserID = userID;
        }
    }

    
}
