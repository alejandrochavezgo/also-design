using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace providerData.helpers
{
    public static class UserSecurityHelper
    {
        public static string? evaluateHash(UserManager<ApplicationUser> userManager, ApplicationUser user, string providedPassword)
        {
            var decodedHashedPassword = Convert.FromBase64String(user.Password);

            switch (decodedHashedPassword[0])
            {
                case 0x00:
                    if (verifyHashedPasswordV2(decodedHashedPassword, providedPassword))
                    {
                        // This is an old password hash format - the caller needs to rehash if we're not running in an older compat mode.
                        return userManager.PasswordHasher.HashPassword(user, providedPassword);
                    }
                    else
                    {
                        return null;
                    }
                case 0x01:
                    int embeddedIterCount;
                    if (verifyHashedPasswordV3(decodedHashedPassword, providedPassword, out embeddedIterCount))
                    {
                        // If this hasher was configured with a higher iteration count, change the entry now.
                        return user.Password;
                    }
                    else
                    {
                        return null;
                    }
                default:
                    // Unknown format marker.
                    return null;
            }
        }

        private static bool verifyHashedPasswordV2(byte[] hashedPassword, string password)
        {
            // Default for Rfc2898DeriveBytes.
            const KeyDerivationPrf PBKDF2PRF = KeyDerivationPrf.HMACSHA1;

            // Default for Rfc2898DeriveBytes.
            const int PBKDF2ITERCOUNT = 1000;

            // 256 bits.
            const int PBKDF2SUBKEYLENGTH = 256 / 8;

            // 128 bits.
            const int SALTSIZE = 128 / 8;

            // We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != 1 + SALTSIZE + PBKDF2SUBKEYLENGTH)
            {
                // Bad size.
                return false;
            }

            var salt = new byte[SALTSIZE];
            Buffer.BlockCopy(hashedPassword, 1, salt, 0, salt.Length);

            var expectedSubkey = new byte[PBKDF2SUBKEYLENGTH];
            Buffer.BlockCopy(hashedPassword, 1 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it.
            var actualSubkey = KeyDerivation.Pbkdf2(password, salt, PBKDF2PRF, PBKDF2ITERCOUNT, PBKDF2SUBKEYLENGTH);

            return byteArraysEqual(actualSubkey, expectedSubkey);
        }

        private static bool verifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount)
        {
            iterCount = default;
            try
            {
                // Read header information.
                KeyDerivationPrf prf = (KeyDerivationPrf)readNetworkByteOrder(hashedPassword, 1);
                iterCount = (int)readNetworkByteOrder(hashedPassword, 5);
                var saltLength = (int)readNetworkByteOrder(hashedPassword, 9);

                // Read the salt: must be >= 128 bits.
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                var salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits.
                var subkeyLength = hashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                var expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it.
                var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return byteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        private static bool byteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }
            return areSame;
        }

        private static uint readNetworkByteOrder(byte[] buffer, int offset)
        {
            return (uint)buffer[offset + 0] << 24
                | (uint)buffer[offset + 1] << 16
                | (uint)buffer[offset + 2] << 8
                | buffer[offset + 3];
        }
    }
}