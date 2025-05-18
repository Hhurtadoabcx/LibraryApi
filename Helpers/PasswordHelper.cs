using Microsoft.AspNetCore.Identity;

namespace LibraryApi.Helpers
{
    public static class PasswordHelper
    {
        // Hash de la contraseña
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<string>();
            return hasher.HashPassword(null, password);
        }

        // Verificación de la contraseña
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var hasher = new PasswordHasher<string>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
