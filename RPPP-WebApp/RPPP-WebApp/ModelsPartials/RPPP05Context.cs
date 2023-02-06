using Microsoft.EntityFrameworkCore;
using RPPP_WebApp.Models;
using RPPP_WebApp.ModelsPartials;

namespace RPPP_WebApp.Models
{
    public partial class RPPP05Context
    {
        public virtual DbSet<ViewNaplatnaPostajaENC> NaplatnePostajeENC { get; set; }



        public IQueryable<SlikaDenorm> SlikaDenorms() =>
            FromExpression(() => SlikaDenorms());


        public IQueryable<StavkaCjenikaDenorm> StavkaCjenikaDenorms(int count) => 
            FromExpression(() => StavkaCjenikaDenorms(count));


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewNaplatnaPostajaENC>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_NaplatnePostajeENC");
            });
            
            modelBuilder.Entity<SlikaDenorm>(entity => {
                entity.HasNoKey();       
            });
            modelBuilder.HasDbFunction(typeof(RPPP05Context).GetMethod(
                    nameof(SlikaDenorms)))
                .HasName("fn_Slike");


            modelBuilder.Entity<StavkaCjenikaDenorm>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.HasDbFunction(typeof(RPPP05Context).GetMethod(
                    nameof(StavkaCjenikaDenorms)))
                .HasName("fn_NaplatnePostajePDF");
        }
    }
}
