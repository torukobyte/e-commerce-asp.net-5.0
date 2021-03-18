using Microsoft.EntityFrameworkCore;
using ETicaret.Models;


namespace ETicaret.Data
{
    public class ETicaretContext : DbContext
    {
        public ETicaretContext (DbContextOptions<ETicaretContext> options)
            : base(options)
        {
        }

        // c#: Urunler listesi    -----migrations, savechanges-----> sqlite:Urunler tablosuna 
        public DbSet<Urun> Urunler { get; set; } //_context.Urunler.Add(new Urun());   _context.Urunler.Remove(x);  _context.Urunler.Where(x=>x.)

        // c#: Resimler listesi    -----migrations, savechanges-----> sqlite:Resimler tablosuna 
        public DbSet<Resim> Resimler { get; set; }

        // c#: Kategoriler listesi    -----migrations, savechanges-----> sqlite:Kategoriler tablosuna 
        public DbSet<Kategori> Kategoriler { get; set; }
        
        public DbSet<KategoriUrun> KategorilerVeUrunler { get; set; }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        //FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>()
                //     Urun  <---N------------------------N---> Kategori
                .HasMany(x=>x.Kategorileri)
                .WithMany(x=>x.Urunleri)

                //     Urun  <--1-------N-   KategoriUrun  --N------------1-> Kategori
                .UsingEntity<KategoriUrun>(
                    j=>j.HasOne(x=>x.Kategori).WithMany(x=>x.KategoriUrunler).HasForeignKey(x=>x.KategoriId).OnDelete(DeleteBehavior.Restrict),   //  1----N
                    j=>j.HasOne(x=>x.Urun).WithMany(x=>x.KategoriUrunler).HasForeignKey(x=>x.UrunId).OnDelete(DeleteBehavior.Cascade),           //  1----N
                    j=>{
                        j.HasKey(x=>new {x.UrunId, x.KategoriId});  //Birleştirilmiş Id : Composite Key
                        // j.Property(x=>x.Aciklama).HasDefaultValue("Ürün kategoriye eklendi");
                        // j.Property(x=>x.EklenmeTarihi).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    }
                );
        }
    }
}