using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLySach.Models
{
    public partial class ModelQLS : DbContext
    {
        public ModelQLS()
            : base("name=ModelQLSach")
        {
        }

        public virtual DbSet<LoaiSach> LoaiSach { get; set; }
        public virtual DbSet<Sach> Sach { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiSach>()
                .HasMany(e => e.Sach)
                .WithRequired(e => e.LoaiSach)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sach>()
                .Property(e => e.MaSach)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
