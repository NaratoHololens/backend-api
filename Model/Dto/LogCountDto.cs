using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto
{
    public class LogCountDto
    {
        public List<int> Statistics { get; set; }
        public List<DateTime> Timespan { get; set; }
    }
}
