using System;

namespace Lup.Software.Engineering.Exceptions
{
    public class DataHadBeenModifiedException : Exception
    {
        public DataHadBeenModifiedException()
        {
        }

        public DataHadBeenModifiedException(string message) 
            : base(message)
        {
        }
    }
}
