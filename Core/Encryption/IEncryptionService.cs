using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Encryption
{
    public interface IEncryptionService
    {
        Task<string> EncryptAsync(string plainText, string? key = null);
        Task<(bool success, string plainText)> TryDecryptAsync(string cipherText, string? key = null);
        Task DropEncryptionKeyAsync(string key) => new(() => { });
    }

    public interface IHasEncryptionKey
    {
        string EncryptionKey { get; }
    }
}
