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
    using System.ComponentModel.DataAnnotations;

    public partial class TBLADMIN
    {
        public byte? ID { get; set; }

        [Required(ErrorMessage ="Ad alan� zorunludur.")]
        [StringLength(maximumLength: 50,ErrorMessage ="Ad�n�z�n uzunlu�unu kontrol ediniz.",MinimumLength = 2)]       
        public string AD { get; set; }

        [Required(ErrorMessage = "Soyad alan� zorunludur.")]
        [StringLength(maximumLength: 50, ErrorMessage = "Soyad�n�z�n uzunlu�unu kontrol ediniz.", MinimumLength = 2)]
         public string SOYAD { get; set; }

        [Required(ErrorMessage = "Kullan�c� ad� alan� zorunludur.")]
        [StringLength(maximumLength: 20, ErrorMessage = "Kullan�c� ad�n�z 4 ile 20 karakter uzunlu�unda olmal�d�r.", MinimumLength = 4)]
        public string KULLANICIADI { get; set; }

        [Required(ErrorMessage = "�ifre alan� zorunludur.")]
        [StringLength(maximumLength: 20, ErrorMessage = "�ifreniz 6 ile 20 karakter uzunlu�unda olmal�d�r.", MinimumLength = 6)]
        public string SIFRE { get; set; }
        public Nullable<bool> DURUM { get; set; }
    }
}
