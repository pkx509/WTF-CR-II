using System.ComponentModel;

namespace DITS.HILI.WMS.ReceiveModel
{
    public enum ReceiveStatusEnum
    {
        /// <summary>
        /// New Receive Order
        /// </summary>
        [Description("เตรียมรับ")]
        New = 10,
        /// <summary>
        /// Loading In
        /// </summary>
        [Description("ท้ายไลน์")]
        LoadIn = 11,
        /// <summary>
        /// Receiving order
        /// </summary>
        [Description("In Progress")]
        Inprogress = 20,
        /// <summary>
        /// Wait for receive order to get more  
        /// </summary>
        [Description("บางส่วน")]
        Partial = 30,
        /// <summary>
        /// Receive Order complete
        /// </summary>
        [Description("สำเร็จ")]
        Complete = 100,
        /// <summary>
        /// Receive order is close
        /// </summary>
        [Description("Close")]
        Close = 101,
        /// <summary>
        /// Cancel receive order
        /// </summary>
        [Description("ยกเลิก")]
        Cancel = 102,
        /// <summary>
        /// Internal Receive check
        /// </summary>
        [Description("ส่งตรวจสอบ")]
        Check = 301,
        /// <summary>
        /// Internal Receive Generate Dispatch
        /// </summary>
        [Description("สร้างใบเบิก")]
        GenDispatch = 302,
    }

    public enum ReceiveDetailStatusEnum
    {
        /// <summary>
        /// New item
        /// </summary>
        [Description("New")]
        New = 10,
        /// <summary>
        /// Loading In
        /// </summary>
        [Description("ท้ายไลน์")]
        LoadIn = 11,
        /// <summary>
        /// Receiving item
        /// </summary>
        [Description("In Progress")]
        Inprogress = 20, //IF ReceivingStatus = New
        /// <summary>
        /// Partial state from receiving
        /// </summary>
        [Description("Partial")]
        Partial = 30,
        /// <summary>
        /// Finish receiving
        /// </summary>
        [Description("Complete")]
        Complete = 100,
        /// <summary>
        /// Close receive item
        /// </summary>
        [Description("Close")]
        Close = 101,
        /// <summary>
        /// Cancel receive item
        /// </summary>
        [Description("Cancel")]
        Cancel = 102
    }

    public enum ReceivingStatusEnum
    {
        /// <summary>
        /// Receiving item
        /// </summary>
        [Description("In Progress")]
        Inprogress = 10,
        /// <summary>
        /// Finish receiving item
        /// </summary>
        [Description("Wait for approve")]
        WaitApprove = 40,
        /// <summary>
        /// Reject status
        /// </summary>
        [Description("Reject")]
        Reject = 50,
        /// <summary>
        /// Approved receiving item
        /// </summary>
        [Description("Complete")]
        Complete = 100 //Approved
    }

}
