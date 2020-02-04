using Arch.Core;
using Arch.Data.GenericRepository;
using System.Data.Entity;
namespace Arch.Data.Context
{
    public partial class CommonContext : DbContext
    {
        DbContextTransaction transaction;
        public CommonContext()
            : base("name=ArchEntities")
        {
            Configuration.LazyLoadingEnabled = false;
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}