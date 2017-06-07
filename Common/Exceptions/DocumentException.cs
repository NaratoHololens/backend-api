using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class DocumentException : Exception
    {
        public DocumentException()
        {

        }

        public DocumentException(string message) : base(message)
        {

        }
    }
}
