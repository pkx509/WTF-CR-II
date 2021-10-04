using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Contacts;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.ReceiveModel
{
    public class Receive : BaseEntity
    {
        public Guid ReceiveID { get; set; }

        /// <summary>
        ///Receive Code, Auto create by service
        /// </summary> 
        public string ReceiveCode { get; set; }
        /// <summary>
        /// Document Type ID (Receive)
        /// </summary>
        public Guid ReceiveTypeID { get; set; }
        /// <summary>
        /// Receive Status Enumerable
        /// </summary>
        public ReceiveStatusEnum ReceiveStatus { get; set; }

        /// <summary>
        /// Product Owner ID
        /// </summary>
        public Guid ProductOwnerID { get; set; }

        /// <summary>
        /// Supplier ID
        /// </summary>
        [Required]
        public Guid SupplierID { get; set; }

        /// <summary>
        /// Estimate Date for receive 
        /// </summary>
        public DateTime EstimateDate { get; set; }

        /// <summary>
        /// Actual receive date
        /// </summary>
        public DateTime? ActualDate { get; set; }

        /// <summary>
        /// Any reference (1) 
        /// Default > OrderNo
        /// </summary>
        public string Reference1 { get; set; }

        /// <summary>
        /// Any reference (2)
        /// </summary>
        public string Reference2 { get; set; }

        /// <summary>
        /// Reason for close receive job
        /// </summary>
        public string CloseJobReason { get; set; }

        /// <summary>
        /// Receive order is urgent
        /// </summary>
        public bool IsUrgent { get; set; }

        /// <summary>
        /// Invoice No reference
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// PO Number reference
        /// </summary>
        public string PONumber { get; set; }

        /// <summary>
        /// License plates of trucks 
        /// </summary>
        public string ContainerNo { get; set; }

        /// <summary>
        /// Loading in location id
        /// </summary>
        public Guid? LocationID { get; set; }

        /// <summary>
        /// Order is crosss docking
        /// </summary>
        public bool IsCrossDock { get; set; }

        /// <summary>
        /// ProductionLine
        /// </summary>
        public Guid? LineID { get; set; }

        /// <summary>
        /// Reference Previous Module/Table
        /// </summary>
        public Guid? ReferenceID { get; set; }

        /// <summary>
        /// Reference Previous BOX
        /// </summary>
        public Guid? PackageID { get; set; }

        public virtual Line Line { get; set; }
        /// <summary>
        /// Receive detail collection
        /// </summary>
        public virtual ICollection<ReceiveDetail> ReceiveDetailCollection { get; set; }

        /// <summary>
        /// Receive assign job collection
        /// </summary>
        public virtual ICollection<ReceiveAssignJob> ReceiveAssignJobCollection { get; set; }

        [NotMapped]
        public int TotalItems { get; set; }

        /// <summary>
        /// Product Owner model
        /// </summary>
        [NotMapped]
        public ProductOwner ProductOwner { get; set; }

        /// <summary>
        /// Supplier model
        /// </summary>
        [NotMapped]
        public Contact Supplier { get; set; }

        /// <summary>
        /// Location model
        /// </summary>
        [NotMapped]
        public Location Location { get; set; }

        /// <summary>
        /// Document Type model
        /// </summary>
        [NotMapped]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Constuctor
        /// </summary>
        public Receive()
        {
            ReceiveDetailCollection = new List<ReceiveDetail>();
            ReceiveAssignJobCollection = new List<ReceiveAssignJob>();
            ProductOwner = new ProductOwner();
            Supplier = new Contact();
            Location = new Location();
            DocumentType = new DocumentType();
        }
    }
}
