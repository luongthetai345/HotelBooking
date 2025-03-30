namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PHONG")]
    public partial class PHONG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHONG()
        {
            DONDATPHONGs = new HashSet<DONDATPHONG>();
        }

        [Key]
        public int id_phong { get; set; }

        [StringLength(30)]
        public string sophong { get; set; }

        public string hinhanh { get; set; }

        public int? gia { get; set; }

        public string mota { get; set; }

        [StringLength(50)]
        public string dichvu { get; set; }

        [StringLength(50)]
        public string loaiphong { get; set; }

        public int? soluonggiuong { get; set; }

        [StringLength(30)]
        public string trangthai { get; set; }

        public int? id_khachsan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONDATPHONG> DONDATPHONGs { get; set; }

        public virtual KHACHSAN KHACHSAN { get; set; }
    }
}
