namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ItfTransactionType : BaseEntity
    {
        public System.Guid InterfaceTypeId { get; set; } // InterfaceTypeID (Primary key)
        public string Ortp { get; set; } // ORTP (length: 10)
        public string Description { get; set; } // Description (length: 50)
        public bool IsActive { get; set; } // IsActive
        public System.Guid UserCreated { get; set; } // UserCreated
        public System.DateTime DateCreated { get; set; } // DateCreated
        public System.Guid UserModified { get; set; } // UserModified
        public System.DateTime DateModified { get; set; } // DateModified

        // Reverse navigation

        /// <summary>
        /// Child ItfInterfaceMappings where [itf_interface_mapping].[InterfaceTypeID] point to this entity (FK_InterfaceTypeID)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ItfInterfaceMapping> ItfInterfaceMappings { get; set; } // itf_interface_mapping.FK_InterfaceTypeID

        public ItfTransactionType()
        {
            ItfInterfaceMappings = new System.Collections.Generic.List<ItfInterfaceMapping>();
        }
    }
}
