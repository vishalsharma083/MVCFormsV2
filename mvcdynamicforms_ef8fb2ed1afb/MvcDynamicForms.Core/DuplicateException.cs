using System;

namespace MVCDynamicForms
{
    class DuplicateException : Exception
    {
        private string _message;
        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public DuplicateException(string message)
        {
            _message = message;
        }
    }
}
