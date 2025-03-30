namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("loginadmin")]
    public partial class loginadmin
    {
        public int id { get; set; }

        [StringLength(100)]
        public string taikhoan { get; set; }

        [StringLength(100)]
        public string matkhau { get; set; }
    }
}
