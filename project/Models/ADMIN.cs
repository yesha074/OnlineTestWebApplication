//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ADMIN
    {
        public ADMIN()
        {
            this.CATEGORY = new HashSet<CATEGORY>();
        }
    
        public int ADMIN_ID { get; set; }
        public string ADMIN_NAME { get; set; }
        public string ADMIN_PASSWORD { get; set; }
    
        public virtual ICollection<CATEGORY> CATEGORY { get; set; }
    }
}
