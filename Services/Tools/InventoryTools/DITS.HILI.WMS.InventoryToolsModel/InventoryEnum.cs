using System.ComponentModel;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public enum InspectionStatus
    {
        [Description("QA Approve")]
        QA_Approve = 100,
        [Description("Cancel")]
        Cancel = 102,
        [Description("QA Inspection")]
        QA_Inspection = 301,
        [Description("Send to Reprocess")]
        SendtoReprocess = 302

    }

    public enum CycleCountStatusEnum
    {
        [Description("เอกสารใหม่")]
        New = 10,
        [Description("กำลังนับ")]
        Counting = 20,
        [Description("นับเรียบร้อย")]
        Complete = 30,
        [Description("สำเร็จ")]
        Approve = 100,
        [Description("ยกเลิก")]
        Cancel = 102
    }

    public enum AdjustStatusEnum
    {
        [Description("New")]
        New = 10,
        [Description("Complete")]
        Complete = 100,
        [Description("Cancel")]
        Cancel = 102
    }
    public enum AdjustStockEnum
    {
        AdjustIn = 10,
        AdjustOut = 20,
    }

    public enum AdjustTypeStatusEnum
    {
        [Description("AddStock")]
        AddStock = 10,
        [Description("ReduceStock")]
        ReduceStock = 20,
        [Description("AddOther")]
        AddOther = 100,
        [Description("ReduceOther")]
        ReduceOther = 101
    }

    public enum ReclassifiedStatus
    {
        [Description("Add Reclassified")]
        Reclassified = 10,
        [Description("QA Approve")]
        Approve = 100,
        [Description("Cancel")]
        Cancel = 102,
        [Description("Approve Dispatch")]
        ApproveDispatch = 400,
        [Description("Send to Reprocess")]
        SendtoReprocess = 302

    }


    public enum GoodsReturnStatusEnum
    {
        [Description("QA Inspection")]
        QA_Inspection = 100,
        [Description("Cancel")]
        Cancel = 102,
        [Description("QA Approve")]
        QA_Approve = 301,
        [Description("Send to Reprocess")]
        SendtoReprocess = 302

    }

    public enum TranferMargetingStatus
    {
        [Description("New")]
        New = 10,
        [Description("Assign Pick")]
        Assign = 20,
        [Description("Confirm Pick")]
        Confirm = 30,
        [Description("Approve")]
        Approve = 100,
        [Description("Cancel")]
        Cancel = 102,
    }
}
