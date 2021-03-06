﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
namespace AttendanceHelper
{

    // Environment.GetFolderPath(Environment.SpecialFolder.) + "/" + AppDomain.CurrentDomain.FriendlyName + "_Log.txt";


    class UserManager
    {
        Logger log;
        private readonly string basePath;
        private readonly string pathToEmailTail = "_crypto_email.txt";
   
     
        private readonly string pathToPasswordTail = "_crypto_password.txt";
        private readonly string pathToUsernameTail = "_crypto.txt";
        private readonly string pathToKeyTail = "_key.txt";
        private readonly string pathToPasswordPrefix;
        private readonly string pathToEmailFromTail = "_crypto_email_from.txt";
        private readonly string pathToEmailPassTail = "_crypt_email_pass.txt";
        private readonly string pathToUsernamePrefix;
        private readonly string pathToEmailPrefix;
        private readonly string pathToEmailFromPrefix;
        private readonly string pathToEmailPassPrefix;
        private readonly string pathToKeyPrefix;
 

        public UserManager(Logger log = null)
        {
            if (log == null)
            {
                this.log = new Logger(Logger.LogLevel.None);
            }
            else
            {
                this.log = log;
            }
            basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/";
            pathToPasswordPrefix = basePath + AppDomain.CurrentDomain.FriendlyName;
            pathToUsernamePrefix = basePath + AppDomain.CurrentDomain.FriendlyName;
            pathToKeyPrefix = basePath + AppDomain.CurrentDomain.FriendlyName;
            pathToEmailPrefix = basePath + AppDomain.CurrentDomain.FriendlyName;
            pathToEmailFromPrefix = basePath + AppDomain.CurrentDomain.FriendlyName;
            pathToEmailPassPrefix = basePath + AppDomain.CurrentDomain.FriendlyName;



        }

        //Null on not found
        //Note that username here isn't the login username, it's the app username that the files were stored under
        public User GetUser(string username)
        {
            User user = null;
            try
            {
                bool allFilesPresent = true;
                string passwordPath = pathToPasswordPrefix + username + pathToPasswordTail;
                string userPath = pathToUsernamePrefix + username + pathToUsernameTail;
                string keyPath = pathToKeyPrefix + username + pathToKeyTail;
                string emailPath = pathToEmailPrefix + username + pathToEmailTail;
                string emailFrom = pathToEmailFromPrefix + username + pathToEmailFromTail;
                string emailPass = pathToEmailPassPrefix + username + pathToEmailPassTail;
                if (!File.Exists(passwordPath))
                {
                    allFilesPresent = false;
                }
                if (!File.Exists(userPath))
                {
                    allFilesPresent = false;
                }
                if (!File.Exists(keyPath))
                {
                    allFilesPresent = false;
                }
                if (allFilesPresent)
                {
                    user = new User();
                    user.appUser = username;
                    user.password = File.ReadAllText(passwordPath);
                    user.key = File.ReadAllBytes(keyPath);
                    user.username = File.ReadAllText(userPath);
                    //user name not really mandatory so make individual checks
                    if (File.Exists(emailPath))
                    {
                        user.email = File.ReadAllText(emailPath);
                    }
                    if (File.Exists(emailFrom))
                    {
                        user.email_from= File.ReadAllText(emailFrom);
                    }
                    if (File.Exists(emailPass))
                    {
                        user.email_password = File.ReadAllText(emailPass);
                    }
                    DecryptUser(user);
                }
            }
            catch (Exception e)
            {
                log.Log(Logger.LogLevel.Errors, "Failed to get user : " + e.Message);
                user = null;
            }

            return user;
        }
        public bool SaveUser(User user)
        {
            bool saveSuccess = true;
            try
            {
                string password = user.password;
                string username = user.username;
                string email = user.email;
                string email_from = user.email_from;
                string email_password = user.email_password;
                byte[] key = user.key;

                password = EncryptionManager.SimpleEncrypt(password, key);
                username = EncryptionManager.SimpleEncrypt(username, key);
                if (!String.IsNullOrEmpty(email))
                {
                    email = EncryptionManager.SimpleEncrypt(email, key);
                    string emailPath = pathToEmailPrefix + user.appUser + pathToEmailTail;
                    File.WriteAllText(emailPath, email);
                }
                if (!String.IsNullOrEmpty(email))
                {
                    email_from = EncryptionManager.SimpleEncrypt(email_from, key);
                    string emailFromPath = pathToEmailFromPrefix + user.appUser + pathToEmailFromTail;
                    File.WriteAllText(emailFromPath, email_from);
                }
                if (!String.IsNullOrEmpty(email))
                {
                    email_password = EncryptionManager.SimpleEncrypt(email_password, key);
                    string emailPassPath = pathToEmailPassPrefix + user.appUser + pathToEmailPassTail;
                    File.WriteAllText(emailPassPath, email_password);
                }
                
            

                string passwordPath = pathToPasswordPrefix + user.appUser + pathToPasswordTail;
                string userPath = pathToUsernamePrefix + user.appUser + pathToUsernameTail;
                string keyPath = pathToKeyPrefix + user.appUser + pathToKeyTail;
               
             
                

                File.WriteAllText(passwordPath, password);
                File.WriteAllText(userPath, username);
               
               
             
                File.WriteAllBytes(keyPath, user.key);
               
              
            }
            catch (Exception e)
            {
                log.Log(Logger.LogLevel.Errors, "Failed to save user : " + e.Message);
                saveSuccess = false;
            }

            return saveSuccess;
        }
        public void DecryptUser(User user)
        {
            user.password = EncryptionManager.SimpleDecrypt(user.password, user.key);
            user.username = EncryptionManager.SimpleDecrypt(user.username, user.key);
            user.email = EncryptionManager.SimpleDecrypt(user.email, user.key);
            user.email_from = EncryptionManager.SimpleDecrypt(user.email_from, user.key);
            user.email_password = EncryptionManager.SimpleDecrypt(user.email_password, user.key);
        }





    }

    public class User
    {
        public string appUser;
        public string username;
        public string password;
        public string email;
        public string email_from;
        public string email_password;
        public byte[] key;
     
        public User()
        {
            key = EncryptionManager.NewKey();
        }
        public User(User user)
        {
            this.appUser = user.appUser;
            this.username = user.username;
            this.password = user.password;
            this.key = user.key;
            this.email_from = user.email_from;
            this.email_password = user.email_password;
            this.email = user.email;
        }
    }


    //Encryption credit goes to User jbtule
    //http://codereview.stackexchange.com/questions/14892/simplified-secure-encryption-of-a-string
    static class EncryptionManager
    {
        private static readonly SecureRandom Random = new SecureRandom();

        //Preconfigured Encryption Parameters
        public static readonly int NonceBitSize = 128;
        public static readonly int MacBitSize = 128;
        public static readonly int KeyBitSize = 256;

        //Preconfigured Password Key Derivation Parameters
        public static readonly int SaltBitSize = 128;
        public static readonly int Iterations = 10000;
        public static readonly int MinPasswordLength = 1;


        /// <summary>
        /// Helper that generates a random new key on each call.
        /// </summary>
        /// <returns></returns>
        public static byte[] NewKey()
        {
            var key = new byte[KeyBitSize / 8];
            Random.NextBytes(key);
            return key;
        }

        /// <summary>
        /// Simple Encryption And Authentication (AES-GCM) of a UTF8 string.
        /// </summary>
        /// <param name="secretMessage">The secret message.</param>
        /// <param name="key">The key.</param>
        /// <param name="nonSecretPayload">Optional non-secret payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Secret Message Required!;secretMessage</exception>
        /// <remarks>
        /// Adds overhead of (Optional-Payload + BlockSize(16) + Message +  HMac-Tag(16)) * 1.33 Base64
        /// </remarks>
        public static string SimpleEncrypt(string secretMessage, byte[] key, byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(secretMessage))
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            var plainText = Encoding.UTF8.GetBytes(secretMessage);
            var cipherText = SimpleEncrypt(plainText, key, nonSecretPayload);
            return Convert.ToBase64String(cipherText);
        }


        /// <summary>
        /// Simple Decryption & Authentication (AES-GCM) of a UTF8 Message
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="key">The key.</param>
        /// <param name="nonSecretPayloadLength">Length of the optional non-secret payload.</param>
        /// <returns>Decrypted Message</returns>
        public static string SimpleDecrypt(string encryptedMessage, byte[] key, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrEmpty(encryptedMessage))
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var cipherText = Convert.FromBase64String(encryptedMessage);
            var plaintext = SimpleDecrypt(cipherText, key, nonSecretPayloadLength);
            return plaintext == null ? null : Encoding.UTF8.GetString(plaintext);
        }

        /// <summary>
        /// Simple Encryption And Authentication (AES-GCM) of a UTF8 String
        /// using key derived from a password (PBKDF2).
        /// </summary>
        /// <param name="secretMessage">The secret message.</param>
        /// <param name="password">The password.</param>
        /// <param name="nonSecretPayload">The non secret payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// Adds additional non secret payload for key generation parameters.
        /// </remarks>
        public static string SimpleEncryptWithPassword(string secretMessage, string password,
                                 byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(secretMessage))
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            var plainText = Encoding.UTF8.GetBytes(secretMessage);
            var cipherText = SimpleEncryptWithPassword(plainText, password, nonSecretPayload);
            return Convert.ToBase64String(cipherText);
        }


        /// <summary>
        /// Simple Decryption and Authentication (AES-GCM) of a UTF8 message
        /// using a key derived from a password (PBKDF2)
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="password">The password.</param>
        /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>
        public static string SimpleDecryptWithPassword(string encryptedMessage, string password,
                                 int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage))
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var cipherText = Convert.FromBase64String(encryptedMessage);
            var plaintext = SimpleDecryptWithPassword(cipherText, password, nonSecretPayloadLength);
            return plaintext == null ? null : Encoding.UTF8.GetString(plaintext);
        }

        public static byte[] SimpleEncrypt(byte[] secretMessage, byte[] key, byte[] nonSecretPayload = null)
        {
            //User Error Checks
            if (key == null || key.Length != KeyBitSize / 8)
                throw new ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "key");

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            //Non-secret Payload Optional
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            //Using random nonce large enough not to repeat
            var nonce = new byte[NonceBitSize / 8];
            Random.NextBytes(nonce, 0, nonce.Length);

            var cipher = new GcmBlockCipher(new AesFastEngine());
            var parameters = new AeadParameters(new KeyParameter(key), MacBitSize, nonce, nonSecretPayload);
            cipher.Init(true, parameters);

            //Generate Cipher Text With Auth Tag
            var cipherText = new byte[cipher.GetOutputSize(secretMessage.Length)];
            var len = cipher.ProcessBytes(secretMessage, 0, secretMessage.Length, cipherText, 0);
            cipher.DoFinal(cipherText, len);

            //Assemble Message
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    //Prepend Authenticated Payload
                    binaryWriter.Write(nonSecretPayload);
                    //Prepend Nonce
                    binaryWriter.Write(nonce);
                    //Write Cipher Text
                    binaryWriter.Write(cipherText);
                }
                return combinedStream.ToArray();
            }
        }

        public static byte[] SimpleDecrypt(byte[] encryptedMessage, byte[] key, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (key == null || key.Length != KeyBitSize / 8)
                throw new ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "key");

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            using (var cipherStream = new MemoryStream(encryptedMessage))
            using (var cipherReader = new BinaryReader(cipherStream))
            {
                //Grab Payload
                var nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);

                //Grab Nonce
                var nonce = cipherReader.ReadBytes(NonceBitSize / 8);

                var cipher = new GcmBlockCipher(new AesFastEngine());
                var parameters = new AeadParameters(new KeyParameter(key), MacBitSize, nonce, nonSecretPayload);
                cipher.Init(false, parameters);

                //Decrypt Cipher Text
                var cipherText = cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length);
                var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];

                try
                {
                    var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                    cipher.DoFinal(plainText, len);

                }
                catch (InvalidCipherTextException)
                {
                    //Return null if it doesn't authenticate
                    return null;
                }

                return plainText;
            }

        }

        public static byte[] SimpleEncryptWithPassword(byte[] secretMessage, string password, byte[] nonSecretPayload = null)
        {
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            //User Error Checks
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            var generator = new Pkcs5S2ParametersGenerator();

            //Use Random Salt to minimize pre-generated weak password attacks.
            var salt = new byte[SaltBitSize / 8];
            Random.NextBytes(salt);

            generator.Init(
              PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
              salt,
              Iterations);

            //Generate Key
            var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

            //Create Full Non Secret Payload
            var payload = new byte[salt.Length + nonSecretPayload.Length];
            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length);

            return SimpleEncrypt(secretMessage, key.GetKey(), payload);
        }

        public static byte[] SimpleDecryptWithPassword(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var generator = new Pkcs5S2ParametersGenerator();

            //Grab Salt from Payload
            var salt = new byte[SaltBitSize / 8];
            Array.Copy(encryptedMessage, nonSecretPayloadLength, salt, 0, salt.Length);

            generator.Init(
              PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
              salt,
              Iterations);

            //Generate Key
            var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

            return SimpleDecrypt(encryptedMessage, key.GetKey(), salt.Length + nonSecretPayloadLength);
        }
    }
}