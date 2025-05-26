using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Shared.Common
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var hashOfInput = HashPassword(enteredPassword);
            return hashOfInput == storedHash;
        }

    }
}
