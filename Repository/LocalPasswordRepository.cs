using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmnesiaManager.Models;
using AmnesiaManager.Security;
using Newtonsoft.Json;

namespace AmnesiaManager.Repository
{
    internal class LocalPasswordRepository : IRepository<PasswordModel>
    {
        #region Public Properties
        public string FileName { get; set; } = "data.dat";
        #endregion

        #region Public Methods
        public IEnumerable<PasswordModel>? GetAll()
        {
            if (!File.Exists(FileName)) return new List<PasswordModel>();
            if (User.Current.EncryptionKey.IsEmpty) return null;

            string content;
            try
            {
                var bytes = File.ReadAllBytes(FileName);
                if (bytes.Length == 0) return new List<PasswordModel>();

                content = SymmetricEncryptor.DecryptToString(bytes, User.Current.EncryptionKey.Value);
            }
            catch (Exception)
            {
                // TODO: Log this exception
                return null;
            }

            if (string.IsNullOrEmpty(content)) return new List<PasswordModel>();

            try
            {
                return JsonConvert.DeserializeObject<List<PasswordModel>>(content);
            }
            catch (Exception)
            {
                // TODO: Log this exception
                return null;
            }
        }

        public bool Create(PasswordModel entity)
        {
            return true;
        }

        public bool Update(PasswordModel entity)
        {
            return true;
        }

        public bool Delete(PasswordModel entity)
        {
            return true;
        }

        public bool IsExists() => File.Exists(FileName);
        #endregion

        #region Private Methods
        private bool WriteInFile(IReadOnlyCollection<PasswordModel> items)
        {
            if (User.Current.EncryptionKey.IsEmpty) return false;

            try
            {
                var content = JsonConvert.SerializeObject(items);
                var bytes = SymmetricEncryptor.EncryptString(content, User.Current.EncryptionKey.Value);
                File.WriteAllBytes($"{FileName}", bytes);
                return true;
            }
            catch (Exception)
            {
                // TODO: Log this exception
                return false;
            }
        }
        #endregion
    }
}
