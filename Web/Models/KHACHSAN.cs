namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KHACHSAN")]
    public partial class KHACHSAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHSAN()
        {
            DONDATPHONGs = new HashSet<DONDATPHONG>();
            PHONGs = new HashSet<PHONG>();
        }

        [Key]
        public int id_khachsan { get; set; }

        [StringLength(50)]
        public string tenkhachsan { get; set; }

        public string hinhanh { get; set; }

        [StringLength(100)]
        public string diachi { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        public string mota { get; set; }

        [StringLength(50)]
        public string dichvu { get; set; }

        [StringLength(50)]
        public string hangsao { get; set; }

        [StringLength(30)]
        public string trangthai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONDATPHONG> DONDATPHONGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHONG> PHONGs { get; set; }
    }
}
