// SPDX-License-Identifier: MIT
// Copyright (c) [Rohit Ahuja]
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for details.

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using XForm.Utilities.Validations;

namespace XForm.Utilities
{
	[SupportedOSPlatform("Windows")]
	[SecuritySafeCritical]
	public class SecurityUtilities
	{
		#region - Secure/Unsecure String Conversion -

		/// <summary>
		/// Converts an unsecure string to a secure string.
		/// </summary>
		/// <param name="unsecureString"></param>
		/// <returns></returns>
		public static SecureString MakeSecureString(string unsecureString)
		{
			var secure_string = new SecureString();

			foreach (char c in unsecureString.ToCharArray())
			{
				secure_string.AppendChar(c);
			}

			secure_string.MakeReadOnly();

			return secure_string;
		}

		/// <summary>
		/// Converts a secure string back to an unsecure string.
		/// </summary>
		/// <param name="secureString"></param>
		/// <returns></returns>
		public static string MakeUnsecureString(SecureString secureString)
		{
			string unsecure_string = string.Empty;
			var ptr = Marshal.SecureStringToBSTR(secureString);

			try
			{
				unsecure_string = Marshal.PtrToStringBSTR(ptr);
			}
			finally
			{
				Marshal.ZeroFreeBSTR(ptr);
			}

			return unsecure_string;
		}

		#endregion - Secure/Unsecure String Conversion -

		#region - Encryption/Decryption using DPAPI -

		/// <summary>
		/// Encrypts a string using DPAPI.
		/// </summary>
		/// <param name="plainTextString"></param>
		/// <param name="dataProtectionScope"></param>
		/// <returns></returns>
		public static string EncryptString(string plainTextString, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
		{
			return EncryptString(MakeSecureString(plainTextString), dataProtectionScope);
		}

		/// <summary>
		/// Encrypts the specified <see cref="SecureString"/> using the Windows Data Protection API (DPAPI).
		/// </summary>
		/// <remarks>This method uses the Windows Data Protection API (DPAPI) to encrypt the provided <see
		/// cref="SecureString"/>. The resulting encrypted data is encoded as a Base64 string for storage or
		/// transmission.</remarks>
		/// <param name="secureString">The <see cref="SecureString"/> to encrypt. Cannot be <see langword="null"/>.</param>
		/// <param name="dataProtectionScope">The scope of the data protection. Defaults to <see cref="DataProtectionScope.LocalMachine"/>. Use <see
		/// cref="DataProtectionScope.CurrentUser"/> to restrict decryption to the current user.</param>
		/// <returns>A Base64-encoded string representing the encrypted data.</returns>
		public static string EncryptString(SecureString secureString, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
		{
			byte[] encrypted_data = ProtectedData.Protect(
									 Encoding.Unicode.GetBytes(MakeUnsecureString(secureString)),
									 Encoding.Unicode.GetBytes("XForm.Utilities.SecurityUtilities"),
									 dataProtectionScope);

			return Convert.ToBase64String(encrypted_data);
		}

		/// <summary>
		/// Decrypts a string using DPAPI.
		/// </summary>
		/// <remarks>This method decrypts a Base64-encoded string that was encrypted using the Windows Data Protection API (DPAPI).
		/// <param name="encryptedData"></param>
		/// <param name="dataProtectionScope"></param>
		/// <returns></returns>
		public static SecureString DecryptString(string encryptedString, DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine)
		{
			try
			{
				byte[] decryptedData = ProtectedData.Unprotect(
				 Convert.FromBase64String(encryptedString),
				 Encoding.Unicode.GetBytes("XForm.Utilities.SecurityUtilities"),
				 dataProtectionScope);

				return MakeSecureString(Encoding.Unicode.GetString(decryptedData));
			}
			catch
			{
				return new SecureString();
			}
		}

		#endregion - Encryption/Decryption using DPAPI -

		#region - HMAC SHA256 -

		/// <summary>
		/// Generates an HMAC using SHA256.
		/// </summary>
		/// <param name="content">Content string.</param>
		/// <param name="secret">Shared secret/salt string.</param>
		/// <param name="isTruncated">Truncate the SHA256 HMAC result to 128 bit result.</param>
		/// <param name="isBase64UrlEncoded">Specfies if resulting string is Base64URL-encoded or standard Base64 encoded (i.e. with padding chars, etc.). Default is false.</param>
		/// <returns>Signature of content as a Base64 encoded string from HMAC result.</returns>
		public static string GenerateHmacSignature(string content, string secret, bool isTruncated = true, bool isBase64UrlEncoded = false)
		{
			Xssert.IsNotNull(content);
			Xssert.IsNotNull(secret);

			byte[] secret_bytes = Encoding.UTF8.GetBytes(secret);
			byte[] content_bytes = Encoding.UTF8.GetBytes(content);

			return GenerateHmacSignature(content_bytes, secret_bytes, isTruncated, isBase64UrlEncoded);
		}

		/// <summary>
		/// Generates an HMAC using SHA256.
		/// </summary>
		/// <param name="content">Content string.</param>
		/// <param name="secret">Secret/salt bytes used to initialize HMAC.</param>
		/// <param name="isTruncated">Truncate the SHA256 HMAC result to 128 bit result.</param>
		/// <param name="isBase64UrlEncoded">Specfies is resuling string is Base64URL encoded or standard Base64 encoded (i.e. with padding chars, etc.). Default is false.</param>
		/// <returns>Signature of content as a Base64 encoded string from HMAC result.</returns>
		public static string GenerateHmacSignature(string content, byte[] secret, bool isTruncated = true, bool isBase64UrlEncoded = false)
		{
			Xssert.IsNotNull(content);
			Xssert.IsNotNull(secret);

			byte[] content_bytes = Encoding.UTF8.GetBytes(content);
			return GenerateHmacSignature(content_bytes, secret, isTruncated, isBase64UrlEncoded);
		}

		/// <summary>
		/// Generate an HMAC using SHA256.
		/// </summary>
		/// <param name="content">Content bytes.</param>
		/// <param name="secret">Secret/salt bytes used to initialize HMAC.</param>
		/// <param name="isTruncated">Truncate the SHA256 HMAC result to 128 bit result.</param>
		/// <param name="isBase64UrlEncoded">Specfies is resuling string is Base64URL encoded or standard Base64 encoded (i.e. with padding chars, etc.). Default is false.</param>
		/// <returns>Signature of content as a Base64 encoded string from HMAC result.</returns>
		public static string GenerateHmacSignature(byte[] content, byte[] secret, bool isTruncated = true, bool isBase64UrlEncoded = false)
		{
			string signature = string.Empty;

			if (content != null)
			{
				using (var hmac = new HMACSHA256(secret))
				{
					byte[] full_hash = hmac.ComputeHash(content);

					if (isTruncated)
					{
						byte[] half_hash = new byte[full_hash.Length / 2];
						Array.Copy(full_hash, half_hash, half_hash.Length);
						signature = isBase64UrlEncoded ? Base64UrlEncoder.Encode(half_hash) : Convert.ToBase64String(half_hash);
					}
					else
					{
						signature = isBase64UrlEncoded ? Base64UrlEncoder.Encode(full_hash) : Convert.ToBase64String(full_hash);
					}
				}
			}

			return signature;
		}

		#endregion - HMAC SHA256 -

		#region - RSA Keys in Machine Keys Store -

		/// <summary>
		/// Checks the machine key store to see if the named RSA key has already been registered.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool IsRsaNamedKeyPresent(string name)
		{
			try
			{
				var param = new CspParameters()
				{
					KeyContainerName = name,
					Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore,
					KeyNumber = 1,
				};

				using (var rsa = new RSACryptoServiceProvider(param))
				{
					return true;
				}
			}
			catch (CryptographicException)
			{
				return false;
			}
		}

		/// <summary>
		/// Retrieves an RSA named key from the machine key store. The returned RSACryptoServiceProvider service provider must be disposed off by caller.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static RSACryptoServiceProvider GetRsaNamedKey(string name)
		{
			var param = new CspParameters()
			{
				KeyContainerName = name,
				Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore,
				KeyNumber = 1,
			};
			try
			{
				return new RSACryptoServiceProvider(param);
			}
			catch (Exception ex)
			{
				throw new SecurityException($"Could not find RSA key with name '{name}'.", ex);
			}
		}

		/// <summary>
		/// Installs a named RSA key pair (Public & Private) to the machine key store, 
		/// marked as non-exportable, using the specified key pair saved in xml format.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="keyPairXml"></param>
		/// <param name="identity"></param>
		public static void InstallRsaNamedKey(string name, string keyPairXml)
		{
			var param = new CspParameters()
			{
				KeyContainerName = name,
				Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseNonExportableKey,
				KeyNumber = 1,
			};

			// Load the RSA key and update it to persist in key store when closed.
			using (var rsa = new RSACryptoServiceProvider(param))
			{
				rsa.FromXmlString(keyPairXml);
				rsa.PersistKeyInCsp = true;

				if (rsa.PublicOnly)
				{
					// See http://stackoverflow.com/questions/827518/rsa-encryption-public-key-not-returned-from-container
					rsa.PersistKeyInCsp = false; // Don't bother saving to key store.
					throw new SecurityException($"RSA named keys couldn't be installed because private key is missing. Use public/private key pairs only.");
				}
			}
		}

		/// <summary>
		/// Removes an RSA named key that is already registered in the machine key store.
		/// </summary>
		/// <param name="name"></param>
		public static void RemoveRsaNamedKey(string name)
		{
			var param = new CspParameters()
			{
				KeyContainerName = name,
				Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore,
				KeyNumber = 1,
			};

			// Load the RSA key but update it to not persist in key store when closed.
			using (var rsa = new RSACryptoServiceProvider(param))
			{
				rsa.PersistKeyInCsp = false;
			}
		}

		#endregion - RSA Keys in Machine Keys Store -
	}
}
