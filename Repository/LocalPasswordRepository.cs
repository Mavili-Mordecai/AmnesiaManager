using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AmnesiaManager.Models;
using AmnesiaManager.Security;
using AmnesiaManager.Services;
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
            if (UserService.Current.EncryptionKey.IsEmpty) return null;

            string content;
            try
            {
                var bytes = File.ReadAllBytes(FileName);
                if (bytes.Length == 0) return new List<PasswordModel>();

                content = SymmetricEncryptor.DecryptToString(bytes, UserService.Current.EncryptionKey.Value);
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
            var items = GetAll()?.ToList() ?? new List<PasswordModel>();
            items.Add(entity);
            return WriteInFile(items);
        }

        public bool Update(PasswordModel entity)
        {
            var passwords = GetAll()?.ToList();
            if (passwords == null || passwords.Count == 0) return false;

            var passwordIndex = passwords.FindIndex(
                pwd => pwd.Guid == entity.Guid
            );

            if (passwordIndex == -1) return false;

            passwords[passwordIndex] = entity;

            return WriteInFile(passwords);
        }

        public bool Delete(PasswordModel entity)
        {
            var passwords = GetAll()?.ToList();
            if (passwords == null || passwords.Count == 0) return false;

            var passwordIndex = passwords.FindIndex(
                pwd => pwd.Guid == entity.Guid
            );

            if (passwordIndex == -1) return false;

            passwords.RemoveAt(passwordIndex);

            return WriteInFile(passwords);
        }

        public bool IsExists() => File.Exists(FileName);
        #endregion

        #region Private Methods
        private bool WriteInFile(IReadOnlyCollection<PasswordModel> items)
        {
            if (UserService.Current.EncryptionKey.IsEmpty) return false;

            try
            {
                var content = JsonConvert.SerializeObject(items);
                var bytes = SymmetricEncryptor.EncryptString(content, UserService.Current.EncryptionKey.Value);
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
