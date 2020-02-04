using Arch.Data.GenericRepository;
using System;
namespace Arch.Data.UnitofWork
{
    public interface IUnitofWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IGenericRawSql<TEntity> GetRawSql<TEntity>() where TEntity : class;
        /// <summary>
        /// Değişiklikleri kaydet.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// Transaction Başlat.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Kayıt esnasında bir sorun olmaz ise transaction durdur.
        /// </summary>
        void Commit();
        /// <summary>
        /// Kayıt esnasında bir sorun olursa değişiklikleri geri al.
        /// </summary>
        void Rollback();
    }
}