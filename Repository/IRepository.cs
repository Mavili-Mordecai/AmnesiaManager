using System.Collections.Generic;
using AmnesiaManager.Security.EncryptedValue;

namespace AmnesiaManager.Repository
{
    internal interface IRepository<T>
    {
        IEnumerable<T>? GetAll();
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool IsExists();
        bool ChangeEncryptionKey(EncryptedString key);
        bool MarkAsRegistered();
    }
}