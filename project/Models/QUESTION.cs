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
    
    public partial class QUESTION
    {
        public int QUESTION_ID { get; set; }
        public string QUESTION_TEXT { get; set; }
        public string OPTIONA { get; set; }
        public string OPTIONB { get; set; }
        public string OPTIONC { get; set; }
        public string OPTIOND { get; set; }
        public string COREECT_OPTION { get; set; }
        public Nullable<int> QUE_FKEY_CATEGORYID { get; set; }
    
        public virtual CATEGORY CATEGORY { get; set; }
    }
}