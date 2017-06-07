using System;
using System.Collections.Generic;
using System.Text;

namespace DataProvider.Interfaces
{
    public interface ISocialMediaProvider
    {
        List<String> GetMessages(string username);
    }
}
