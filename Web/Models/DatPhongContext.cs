using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Web.Models
{
    public partial class DatPhongContext : DbContext
    {
        public DatPhongContext()
            : base("name=DatPhongContext5")
        {
        }

        public virtual DbSet<DONDATPHONG> DONDATPHONGs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<KHACHSAN> KHACHSANs { get; set; }
        public virtual DbSet<loginadmin> loginadmins { get; set; }
        public virtual DbSet<PHONG> PHONGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.taikhoan)
                .IsFixedLength();

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.matkhau)
                .IsFixedLength();
        }
    }
}
