//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLogin
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> LoginCount { get; set; }
        public Nullable<int> RegisterId { get; set; }
        public int UserType { get; set; }
        public virtual Register Register { get; set; }
    }
}