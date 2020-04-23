using System;

namespace QueueAdapter
{
    public class QueueSubscriberException : Exception
    {
        public QueueSubscriberException()
            : base()
        { }

        public QueueSubscriberException(string message)
            : base(message)
        { }

        public QueueSubscriberException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}