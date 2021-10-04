using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel
{
    /// <summary>
    /// Assign job template base
    /// </summary>
    public class BaseAssignJob
    {
        /// <summary>
        /// Activity ID reference
        /// </summary>
        public Guid ReferenceID { get; set; }

        /// <summary>
        /// Employee
        /// </summary>
        public Guid EmployeeID { get; set; }

        [NotMapped]
        public Employee Employee { get; set; }
    }
}
