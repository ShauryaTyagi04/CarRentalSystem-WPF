//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Car_Rental_System
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public string DriverLicence { get; set; }
        public bool Insurance { get; set; }
        public System.DateTime RentDate { get; set; }
        public System.DateTime ReturnDate { get; set; }
        public double Deposit { get; set; }
        public double TotalRent { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
