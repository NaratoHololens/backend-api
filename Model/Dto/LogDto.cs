using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto
{
    public class LogDto
    {
        public string ID { get; set; }
        public DateTime Timestamp { get; set; }
        public string userID { get; set; }
        private readonly string type = "LOG";
        public string Type { get { return type; } }

    }
}
