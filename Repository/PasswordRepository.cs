using System.Collections.Generic;
using System.Threading.Tasks;
using AmnesiaManager.Models;
using AmnesiaManager.Security.EncryptedValue;
using AmnesiaManager.Security.EncryptionAlgorithms;

namespace AmnesiaManager.Repository
{
    internal class PasswordRepository : IRepository<PasswordModel>
    {
        #region Public Fields

        public delegate void PasswordHandler(PasswordModel password);

        public event PasswordHandler? OnPasswordUpdated;
        public event PasswordHandler? OnPasswordCreated;
        public event PasswordHandler? OnPasswordDeleted;

        #endregion

        #region Private Fields

        private readonly IRepository<PasswordModel> _repository;

        #endregion

        #region Singletone

        private static PasswordRepository? _instance;
        public static PasswordRepository Instance => _instance ??= new PasswordRepository(new AesEncryptionAlgorithm());

        #endregion

        #region Constructor

        private PasswordRepository(ISymmetricEncryptionAlgorithm encryptionAlgorithm)
        {
            _repository = new JsonPasswordRepository(encryptionAlgorithm);
        }

        #endregion

        #region Public Methods

        public IEnumerable<PasswordModel>? GetAll() => _repository.GetAll();

        public bool Create(PasswordModel entity)
        {
            OnPasswordCreated?.Invoke(entity);
            Task.Run(() => { _repository.Create(entity); });
            return true;
        }

        public bool Update(PasswordModel entity)
        {
            OnPasswordUpdated?.Invoke(entity);
            Task.Run(() => { _repository.Update(entity); });
            return true;
        }

        public bool Delete(PasswordModel entity)
        {
            OnPasswordDeleted?.Invoke(entity);
            Task.Run(() => { _repository.Delete(entity); });
            return true;
        }

        public bool ChangeEncryptionKey(EncryptedString key) => _repository.ChangeEncryptionKey(key);
        
        public bool MarkAsRegistered() => _repository.MarkAsRegistered();

        public bool IsExists() => _repository.IsExists();

        #endregion
    }
}