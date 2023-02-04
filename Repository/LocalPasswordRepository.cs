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
            if (Product.GetApplicationDirectory() is not { } applicationDirectory) return null;
            if (!File.Exists($@"{applicationDirectory}\{FileName}")) return new List<PasswordModel>();
            if (UserService.Current.EncryptionKey.IsEmpty) return null;
            
            string content;
            try
            {
                var bytes = File.ReadAllBytes($@"{applicationDirectory}\{FileName}");
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
                var deserializedStorageModel = JsonConvert.DeserializeObject<StorageModel>(content);
                if (deserializedStorageModel == null) return null;

                return deserializedStorageModel.IsAuthenticated
                    ? deserializedStorageModel?.Passwords ?? new List<PasswordModel>()
                    : null;
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

        public bool MarkAsRegistered() => WriteInFile(null);

        public bool IsExists() => Product.GetApplicationDirectory() is { } applicationDirectory && File.Exists($@"{applicationDirectory}\{FileName}");
        #endregion

        #region Private Methods
        private bool WriteInFile(List<PasswordModel>? items)
        {
            if (
                UserService.Current.EncryptionKey.IsEmpty ||
                Product.GetApplicationDirectory() is not { } applicationDirectory
            ) return false;

            try
            {
                var storageModel = new StorageModel
                {
                    IsAuthenticated = true,
                    Passwords = items
                };

                var content = JsonConvert.SerializeObject(storageModel);
                var bytes = SymmetricEncryptor.EncryptString(content, UserService.Current.EncryptionKey.Value);
                File.WriteAllBytes($@"{applicationDirectory}\{FileName}", bytes);
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
