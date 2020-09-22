using System;

namespace Infra.Common
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message)
        {
        }
    }
}