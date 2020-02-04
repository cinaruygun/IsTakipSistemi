using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
namespace Arch.Data.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Sorgu işlemleri için GetAll metotdunu kullanıyoruz.
        /// </summary>
        IQueryable<TEntity> GetAll();
        /// <summary>
        /// id ye göre veri çekmek için Find metodu.
        /// </summary>
        TEntity Find(int id);
        /// <summary>
        /// Gönderilen entity ye göre kayıt işlemi
        /// </summary>
        void Insert(TEntity entity);
        /// <summary>
        /// Gönderilen entity ye göre güncelleme işlemi
        /// </summary>
        void Update(TEntity entity);
        /// <summary>
        /// Gönderilen entity ye göre silme işlemi
        /// </summary>
        void Delete(TEntity entity);
    }
}