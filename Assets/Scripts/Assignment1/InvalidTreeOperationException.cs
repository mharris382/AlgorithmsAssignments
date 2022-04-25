using System;

namespace Assignment1
{
    public class InvalidTreeOperationException : Exception
    {
        public InvalidTreeOperationException(string msg) : base(msg)
        {
        }
    }

    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException(object node) : base($"The object:{node} was not found in the binary tree")
        {
        }
    }
}