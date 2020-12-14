using System.Collections.Generic;
using HeritageWebApplication.Models;

namespace HeritageWebApplication.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Get(int id);
        void Save(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}