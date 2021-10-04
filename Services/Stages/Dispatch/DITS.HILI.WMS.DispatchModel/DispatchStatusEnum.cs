using System.ComponentModel;

namespace DITS.HILI.WMS.DispatchModel
{
    public enum DispatchStatusEnum
    {
        [Description("เตรียมเบิก")]
        New = 10,

        [Description("จอง")]
        Inprogress = 20,

        [Description("ยืนยันจอง")]
        InprogressConfirm = 30,

        [Description("ยืนยันจอง QA")]
        InprogressConfirmQA = 33,

        [Description("ติด Back Order")]
        InBackOrder = 35,

        [Description("ลงทะเบียนรถ")]
        Register = 40,

        [Description("ยืนยันการจ่าย")]
        WaitingConfirmDispatch = 50,

        [Description("สำเร็จ")]
        Complete = 100,

        [Description("ยกเลิก")]
        Close = 102,

        [Description("ยืนยันการจ่ายแบบรับภายใน")]
        InternalReceive = 201,

        [Description("ยืนยันการจ่ายแบบไม่ลงทะเบียนรถ")]
        WaitingConfirmDispatchNoneRegister = 202,

        [Description("ยืนยันการจ่ายจาก QA")]
        WaitingConfirmDispatchQA = 203,
    }

    public enum DispatchDetailStatusEnum
    {
        [Description("เตรียมเบิก")]
        New = 10,

        [Description("จอง")]
        Inprogress = 20,

        [Description("ยืนยันจอง")]
        InprogressConfirm = 30,

        [Description("ยืนยันจอง QA")]
        InprogressConfirmQA = 33,

        [Description("ติด Back Order")]
        InBackOrder = 35,

        [Description("ลงทะเบียนรถ")]
        Register = 40,

        [Description("ยืนยันการจ่าย")]
        WaitingConfirmDispatch = 50,

        [Description("สำเร็จ")]
        Complete = 100,

        [Description("ยกเลิก")]
        Close = 102,

        [Description("ยืนยันการจ่ายแบบรับภายใน")]
        InternalReceive = 201,

        [Description("ยืนยันการจ่ายแบบไม่ลงทะเบียนรถ")]
        WaitingConfirmDispatchNoneRegister = 202,

        [Description("ยืนยันการจ่ายจาก QA")]
        WaitingConfirmDispatchQA = 203,
    }

    public enum BookingStatusEnum
    { 
        [Description("จอง")]
        Inprogress = 20,

        [Description("ยืนยันจอง")]
        InprogressConfirm = 30,

        [Description("สำเร็จ")]
        Complete = 100,

        [Description("ยกเลิก")]
        Close = 102,

        [Description("รับภายใน")]
        InternalReceive = 201,

    }

    public enum DispatchPreFixTypeEnum
    {
        DISPATHCODE = 100,
        PONO_INTERNAL = 200,
        PONO_QA = 300

    }


}
