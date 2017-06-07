using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configurations
{
    public class DbConfiguration
    {

        public string Key { get; set; }
        public Uri Uri { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string CollectionNameLog { get; set; }
    }
}
