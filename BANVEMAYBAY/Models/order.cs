//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BANVEMAYBAY.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order()
        {
            this.ordersdetails = new HashSet<ordersdetail>();
        }
    
        public int ID { get; set; }
        public int guestTotal { get; set; }
        public string name { get; set; }
        public Nullable<int> CusId { get; set; }
        public string gioitinh { get; set; }
        public string quoctich { get; set; }
        public string mess { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public double total { get; set; }
        public System.DateTime created_ate { get; set; }
        public int status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ordersdetail> ordersdetails { get; set; }
    }
}
