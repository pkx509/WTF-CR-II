using System;

namespace DITS.HILI.WMS.Core.CustomException
{
    [Serializable]
    public class HILIException : Exception
    {
        public string ErrorCode = "";

        public HILIException() : base()
        {
        }

        public HILIException(string errorCode) : base(errorCode)
        {
            ErrorCode = errorCode;
        }

    }
}
