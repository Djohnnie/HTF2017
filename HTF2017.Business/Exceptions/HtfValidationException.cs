using System;

namespace HTF2017.Business.Exceptions
{
    public class HtfValidationException : Exception
    {
        public string ValidationMessage { get; }

        public HtfValidationException(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }
    }
}