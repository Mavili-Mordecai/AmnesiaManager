using System;
using System.Collections.Generic;

namespace AmnesiaManager.Repository
{
    internal interface IRepository<T>
    {
        IEnumerable<T>? GetAll();
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool IsExists();
    }
}
