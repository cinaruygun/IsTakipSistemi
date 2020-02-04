using Arch.Core;
using Arch.Data.GenericRepository;
using System.Data.Entity;
using System.Linq;
namespace Arch.Data.Context
{
    public partial class ArchContext : DbContext
    {
        private readonly ArchContext _context;
        DbContextTransaction transaction;
        public ArchContext()
            : base("name=ArchEntities")
        {
            Configuration.LazyLoadingEnabled = false;
        }
        /// <summary>
        /// Schema : common
        /// </summary>
        public virtual DbSet<Lookup> Lookup { get; set; }
        public virtual DbSet<LookupList> LookupList { get; set; }
        public virtual DbSet<Parameters> Parameters { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        /// <summary>
        /// Schema : file
        /// </summary>
        public virtual DbSet<Media> Media { get; set; }
        /// <summary>
        /// Schema : log
        /// </summary>
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public virtual DbSet<RequestLog> RequestLog { get; set; }
        public virtual DbSet<TempRequestLog> TempRequestLog { get; set; }
        /// <summary>
        /// Schema : work
        /// </summary>
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskHistory> TaskHistory { get; set; }
        public virtual DbSet<TaskMedia> TaskMedia { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /// <summary>
            /// Schema : common
            /// </summary>
            modelBuilder.Entity<Lookup>().ToTable("Lookup", "common");
            modelBuilder.Entity<LookupList>().ToTable("LookupList", "common");
            modelBuilder.Entity<Parameters>().ToTable("Parameters", "common");
            modelBuilder.Entity<Person>().ToTable("Person", "common");
            modelBuilder.Entity<Unit>().ToTable("Unit", "common");
            /// <summary>
            /// Schema : file
            /// </summary>
            modelBuilder.Entity<Media>().ToTable("Media", "file");
            /// <summary>
            /// Schema : log
            /// </summary>
            modelBuilder.Entity<ExceptionLog>().ToTable("ExceptionLog", "log");
            modelBuilder.Entity<RequestLog>().ToTable("RequestLog", "log");
            modelBuilder.Entity<TempRequestLog>().ToTable("TempRequestLog", "log");
            /// <summary>
            /// Schema : work
            /// </summary>
            modelBuilder.Entity<Comment>().ToTable("Comment", "work");
            modelBuilder.Entity<Project>().ToTable("Project", "work");
            modelBuilder.Entity<Task>().ToTable("Task", "work");
            modelBuilder.Entity<TaskHistory>().ToTable("TaskHistory", "work");
            modelBuilder.Entity<TaskMedia>().ToTable("TaskMedia", "work");

            base.OnModelCreating(modelBuilder);
        }
        public void BeginTransaction()
        {
            transaction = _context.Database.BeginTransaction();
        }
        public void Commit()
        {
            transaction.Commit();
        }
        public void Rollback()
        {
            transaction.Rollback();
        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
        //public IGenericRawSql<T> GetRawSql<T>() where T : class
        //{
        //    return new GenericRawSql<T>(_context);
        //}
    }
}