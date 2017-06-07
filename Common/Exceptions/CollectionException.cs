using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class CollectionException : Exception
    {
        public CollectionException()
        {
            
        }

        public CollectionException(string message) : base(message)
        {

        }
    }
}
