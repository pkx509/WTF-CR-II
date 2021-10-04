using System;

namespace DITS.HILI.WMS.MasterModel
{
    public class BaseEntity
    {
        /// <summary>
        /// Remember 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Item status Active/In Active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User create item
        /// </summary>
        public Guid UserCreated { get; set; }

        /// <summary>
        /// Date created 
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Last user modified item
        /// </summary>
        public Guid UserModified { get; set; }

        /// <summary>
        /// Last date modified item
        /// </summary>
        public DateTime DateModified { get; set; }

        public BaseEntity()
        {
            IsActive = true;
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }
    }
}
