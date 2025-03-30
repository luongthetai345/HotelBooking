namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DONDATPHONG")]
    public partial class DONDATPHONG
    {
        [Key]
        public int id_don { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngayden { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaydi { get; set; }

        public int? songuoi { get; set; }

        public int? giatong { get; set; }

        [StringLength(30)]
        public string trangthai { get; set; }

        public int? id_khachhang { get; set; }

        public int? id_phong { get; set; }

        public int? id_khachsan { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }

        public virtual KHACHSAN KHACHSAN { get; set; }

        public virtual PHONG PHONG { get; set; }
    }
}
