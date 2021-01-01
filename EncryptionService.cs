using Easy_Licensing.Enums;
using Easy_Licensing.Models;
using Easy_Licensing.Tools;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Easy_Licensing
{
    /// <summary>
    /// Provides services to encrypt and decrypt messages
    /// </summary>
    public static class EncryptionService
    {
        /// <summary>
        /// Contains the settings to use during encryption and decryption operations
        /// </summary>
        public static EncryptionSettings Settings { get; set; } = new EncryptionSettings();

        /// <summary>
        /// Generates a cryptographically secure random byte key for use with encryption methods
        /// </summary>
        public static byte[] NewKey()
        {
            var random = RandomNumberGenerator.Create();
            var key = new byte[Settings.AesKeyByteSize];

            random.GetBytes(key);

            return key;
        }

        /// <summary>
        /// Generates a cryptographically secure random string key
        /// </summary>
        /// <param name="length">The length of the key to generate</param>
        /// <param name="characterSet">The character set to use for key generation</param>
        public static string NewKey(int length, CharacterSets characterSet)
        {
            var fullSet = string.Empty;

            foreach (var set in characterSet.GetFlags())
                fullSet += set switch
                {
                    CharacterSets.Numeric => "0123456789",
                    CharacterSets.Lowercase => "abcdefghijklmnopqrstuvwxyz",
                    CharacterSets.Uppercase => "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                    CharacterSets.Punctuation => "`~!@#$%^&*()_-=+[]{}|;:,.<>?",
                    _ => string.Empty
                };

            return NewKey(length, fullSet.ToCharArray());
        }

        /// <summary>
        /// Generates a cryptographically secure random string key
        /// </summary>
        /// <param name="length">The length of the key to generate</param>
        /// <param name="characters">An array of characters to use in the resulting key</param>
        public static string NewKey(int length, char[] characters)
        {
            var result = string.Empty;
            var data = new byte[4 * length];

            using var crypto = new RNGCryptoServiceProvider();

            crypto.GetBytes(data);

            for (var i = 0; i < length; i++)
            {
                var current = BitConverter.ToUInt32(data, i * 4);
                var index = current % characters.Length;

                result += characters[index];
            }

            return result;
        }

        /// <summary>
        /// Encrypts the provided message using the provided password (combination of AES, HMAC, and PBKDF2).
        /// Note that this overload is less secure than key-based overloads
        /// </summary>
        /// <param name="message">The text to encrypt</param>
        /// <param name="password">The text key to use for encrypting the message</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string EncryptSymmetric(string message, string password)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Must provide text to encrypt.");

            var normalizedMessage = Encoding.UTF8.GetBytes(message);
            var cipher = EncryptSymmetric(normalizedMessage, password);

            return Convert.ToBase64String(cipher);
        }

        /// <summary>
        /// Encrypts the provided message using the provided password (combination of AES, HMAC, and PBKDF2).
        /// Note that this overload is less secure than key-based overloads
        /// </summary>
        /// <param name="message">The byte-array to encrypt</param>
        /// <param name="password">The text key to use for encrypting the message</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptSymmetric(byte[] message, string password)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(password) || password.Length < Settings.MinPasswordLength)
                throw new ArgumentNullException(nameof(password), $"Password must be at least {Settings.MinPasswordLength} characters long.");

            if (message.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(message), "Must provide text to encrypt.");

            // Prepare method variables
            var payload = new byte[Settings.SaltByteSize * 2];
            var payloadIndex = 0;

            byte[] cryptKey;
            byte[] authKey;

            // Use a random salt to generate the crypt key
            using (var generator = new Rfc2898DeriveBytes(password, Settings.SaltByteSize, Settings.KeyGenerationIterations))
            {
                var salt = generator.Salt;

                cryptKey = generator.GetBytes(Settings.AesKeyByteSize);

                Array.Copy(salt, 0, payload, 0, salt.Length);

                payloadIndex += salt.Length;
            }

            // Use a random salt to generate the auth key
            using (var generator = new Rfc2898DeriveBytes(password, Settings.SaltByteSize, Settings.KeyGenerationIterations))
            {
                var salt = generator.Salt;

                authKey = generator.GetBytes(Settings.AesKeyByteSize);

                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
            }

            return EncryptSymmetric(message, cryptKey, authKey, payload);
        }

        /// <summary>
        /// Encrypts the provided message using the provided keys (combination of AES and HMAC).
        /// </summary>
        /// <param name="message">The text to encrypt</param>
        /// <param name="rgbKey">The crypt key</param>
        /// <param name="authKey">The auth key</param>
        /// <param name="unencryptedData">An optional non-secret payload to include in the return data</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string EncryptSymmetric(string message, byte[] rgbKey, byte[] authKey, byte[] unencryptedData = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Must provide text to encrypt.");

            var normalizedMessage = Encoding.UTF8.GetBytes(message);
            var cipher = EncryptSymmetric(normalizedMessage, rgbKey, authKey, unencryptedData);

            return Convert.ToBase64String(cipher);
        }

        /// <summary>
        /// Encrypts the provided message using the provided keys (combination of AES and HMAC).
        /// </summary>
        /// <param name="message">The byte-array to encrypt</param>
        /// <param name="rgbKey">The crypt key</param>
        /// <param name="authKey">The auth key</param>
        /// <param name="unencryptedData">An optional non-secret payload to include in the return data</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptSymmetric(byte[] message, byte[] rgbKey, byte[] authKey, byte[] unencryptedData = null)
        {
            // Validate inputs
            if (rgbKey.IsNullOrEmpty() || rgbKey.Length != Settings.AesKeyByteSize)
                throw new ArgumentException($"Key needs to be {Settings.AesKeySize} bits in size.", nameof(rgbKey));

            if (authKey.IsNullOrEmpty() || authKey.Length != Settings.AesKeyByteSize)
                throw new ArgumentException($"Key needs to be {Settings.AesKeySize} bits in size.", nameof(authKey));

            if (message.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(message), "Must provide text to encrypt.");

            // Prepare method variables
            unencryptedData ??= Array.Empty<byte>();

            byte[] cipherText;
            byte[] iv;

            // Encrypt the message
            using (var aes = new AesManaged
            {
                KeySize = Settings.AesKeySize,
                BlockSize = Settings.AesBlockSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {
                aes.GenerateIV();
                iv = aes.IV;

                using var encrypter = aes.CreateEncryptor(rgbKey, iv);
                using var cipherStream = new MemoryStream();

                using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                using (var binaryWriter = new BinaryWriter(cryptoStream))
                {
                    binaryWriter.Write(message);
                }

                cipherText = cipherStream.ToArray();
            }

            // Assemble encrypted message and add authentication
            using var hmac = new HMACSHA256(authKey);
            using var encryptedStream = new MemoryStream();

            using (var binaryWriter = new BinaryWriter(encryptedStream))
            {
                // Write all components to stream
                binaryWriter.Write(unencryptedData);
                binaryWriter.Write(iv);
                binaryWriter.Write(cipherText);
                binaryWriter.Flush();

                // Authenticate all data and save hash
                var tag = hmac.ComputeHash(encryptedStream.ToArray());

                binaryWriter.Write(tag);
            }

            return encryptedStream.ToArray();
        }

        /// <summary>
        /// Decrypts the provided message using the provided password (combination of AES, HMAC, and PBKDF2).
        /// Note that this overload is less secure than key-based overloads
        /// </summary>
        /// <param name="message">The text to decrypt</param>
        /// <param name="password">The text key to use for decrypting the message</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DecryptSymmetric(string message, string password)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Must provide text to decrypt.");

            var cipher = Convert.FromBase64String(message);
            var normalizedMessage = DecryptSymmetric(cipher, password);

            return Encoding.UTF8.GetString(normalizedMessage);
        }

        /// <summary>
        /// Decrypts the provided message using the provided password (combination of AES, HMAC, and PBKDF2).
        /// Note that this overload is less secure than key-based overloads
        /// </summary>
        /// <param name="message">The byte-array to decrypt</param>
        /// <param name="password">The text key to use for decrypting the message</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] DecryptSymmetric(byte[] message, string password)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(password) || password.Length < Settings.MinPasswordLength)
                throw new ArgumentNullException(nameof(password), $"Password must be at least {Settings.MinPasswordLength} characters long.");

            if (message.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(message), "Must provide text to decrypt.");

            // Prepare method variables
            var cryptSalt = new byte[Settings.SaltByteSize];
            var authSalt = new byte[Settings.SaltByteSize];

            byte[] cryptKey;
            byte[] authKey;

            // Extract salt values
            Array.Copy(message, 0, cryptSalt, 0, cryptSalt.Length);
            Array.Copy(message, cryptSalt.Length, authSalt, 0, authSalt.Length);

            // Generate the crypt key
            using (var generator = new Rfc2898DeriveBytes(password, cryptSalt, Settings.KeyGenerationIterations))
            {
                cryptKey = generator.GetBytes(Settings.AesKeyByteSize);
            }

            // Generate the auth key
            using (var generator = new Rfc2898DeriveBytes(password, authSalt, Settings.KeyGenerationIterations))
            {
                authKey = generator.GetBytes(Settings.AesKeyByteSize);
            }

            return DecryptSymmetric(message, cryptKey, authKey, Settings.SaltByteSize * 2);
        }

        /// <summary>
        /// Decrypts the provided message using the provided keys (combination of AES and HMAC).
        /// </summary>
        /// <param name="message">The text to decrypt</param>
        /// <param name="rgbKey">The crypt key</param>
        /// <param name="authKey">The auth key</param>
        /// <param name="unencryptedDataLength">Length of an optional non-secret payload included in the encrypted message</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DecryptSymmetric(string message, byte[] rgbKey, byte[] authKey, int unencryptedDataLength = 0)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Must provide text to decrypt.");

            var cipher = Convert.FromBase64String(message);
            var normalizedMessage = DecryptSymmetric(cipher, rgbKey, authKey, unencryptedDataLength);

            return Encoding.UTF8.GetString(normalizedMessage);
        }

        /// <summary>
        /// Decrypts the provided message using the provided keys (combination of AES and HMAC).
        /// </summary>
        /// <param name="message">The byte-array to decrypt</param>
        /// <param name="rgbKey">The crypt key</param>
        /// <param name="authKey">The auth key</param>
        /// <param name="unencryptedDataLength">Length of an optional non-secret payload included in the encrypted message</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] DecryptSymmetric(byte[] message, byte[] rgbKey, byte[] authKey, int unencryptedDataLength = 0)
        {
            //Basic Usage Error Checks
            if (rgbKey.IsNullOrEmpty() || rgbKey.Length != Settings.AesKeyByteSize)
                throw new ArgumentException($"Key needs to be {Settings.AesKeySize} bits in size.", nameof(rgbKey));

            if (authKey.IsNullOrEmpty() || authKey.Length != Settings.AesKeyByteSize)
                throw new ArgumentException($"Key needs to be {Settings.AesKeySize} bits in size.", nameof(authKey));

            if (message.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(message), "Must provide text to decrypt.");

            // Prepare method variables and extract tag
            using var hmac = new HMACSHA256(authKey);

            var sentTag = new byte[hmac.HashSize / 8];
            var ivLength = Settings.AesBlockSize / 8;

            var calcTag = hmac.ComputeHash(message, 0, message.Length - sentTag.Length);

            // Ensure message is large enough to be valid
            if (message.Length < sentTag.Length + unencryptedDataLength + ivLength)
                throw new ArgumentException("Invalid text provided.", nameof(message));

            // Extract and validate tag
            var compare = 0;

            Array.Copy(message, message.Length - sentTag.Length, sentTag, 0, sentTag.Length);

            for (var i = 0; i < sentTag.Length; i++)
                compare |= sentTag[i] ^ calcTag[i];

            if (compare != 0)
                throw new ArgumentException("Invalid authentication provided.", nameof(authKey));

            // Extract IV from message
            var iv = new byte[ivLength];
            Array.Copy(message, unencryptedDataLength, iv, 0, iv.Length);

            // Decrypt the message
            using var aes = new AesManaged
            {
                KeySize = Settings.AesKeySize,
                BlockSize = Settings.AesBlockSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            using var decrypter = aes.CreateDecryptor(rgbKey, iv);
            using var decryptedStream = new MemoryStream();

            using (var decrypterStream = new CryptoStream(decryptedStream, decrypter, CryptoStreamMode.Write))
            using (var binaryWriter = new BinaryWriter(decrypterStream))
            {
                binaryWriter.Write(
                    message,
                    unencryptedDataLength + iv.Length,
                    message.Length - unencryptedDataLength - iv.Length - sentTag.Length
                );
            }

            return decryptedStream.ToArray();
        }
    }
}
