//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UrunStokOtomasyonu.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBLSATISHAREKET
    {
        public int ID { get; set; }
        public Nullable<int> URUN { get; set; }
        public Nullable<int> UYE { get; set; }
        public Nullable<System.DateTime> SIPARISTARIHI { get; set; }
        public Nullable<System.DateTime> TESLIMTARIHI { get; set; }
        public Nullable<double> URUNMIKTARI { get; set; }
        public Nullable<double> ISLEMTUTARI { get; set; }
        public string MUSTERI { get; set; }
        public string MUSTERIDETAY { get; set; }
        public Nullable<bool> DURUM { get; set; }
        public string ACTION { get; set; }
    
        public virtual TBLURUN TBLURUN { get; set; }
        public virtual TBLUYE TBLUYE { get; set; }
    }
}
