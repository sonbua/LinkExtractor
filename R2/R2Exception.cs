using System;
using System.Runtime.Serialization;

namespace R2
{
    [Serializable]
    public class R2Exception : Exception
    {
        public R2Exception()
        {
        }

        public R2Exception(string message)
            : base(message)
        {
        }

        public R2Exception(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected R2Exception(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}