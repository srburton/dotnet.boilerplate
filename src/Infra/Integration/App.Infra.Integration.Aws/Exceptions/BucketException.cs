using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infra.Integration.Aws.Exceptions
{
    public class BucketException: Exception
    {
        public BucketException(string message) : base(message)
        {

        }
    }
}
