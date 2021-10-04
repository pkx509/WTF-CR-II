using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public class itf_interface_error_log
    {
        public Guid TransactionID { get; set; }
        public string APIName { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remark { get; set; }
    }
}
