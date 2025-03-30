namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KHACHHANG")]
    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            DONDATPHONGs = new HashSet<DONDATPHONG>();
        }

        [Key]
        public int id_khachhang { get; set; }

        [StringLength(50)]
        public string hoten { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaysinh { get; set; }

        [StringLength(15)]
        public string sodienthoai { get; set; }

        [StringLength(30)]
        public string email { get; set; }

        [StringLength(10)]
        public string gioitinh { get; set; }

        [StringLength(30)]
        public string taikhoan { get; set; }

        [StringLength(30)]
        public string matkhau { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONDATPHONG> DONDATPHONGs { get; set; }
    }
}
