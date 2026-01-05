using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using XForm.Utilities;

namespace Xform.Utilities.Tests.Unit;

[SupportedOSPlatform("Windows")]
public class SecurityUtilitiesTests
{
	#region - Secure/Unsecure String Conversion -

	[Fact]
	public void TestMakeSecureString()
	{
		var input = "MySecurePassword";
		var secureString = SecurityUtilities.MakeSecureString(input);

		Assert.NotNull(secureString);
	}

	[Fact]
	public void TestMakeUnsecureString()
	{
		var input = "MySecurePassword";
		var secureString = SecurityUtilities.MakeSecureString(input);

		var result = SecurityUtilities.MakeUnsecureString(secureString);

		Assert.Equal(input, result); // The unsecure string should match the input
	}

	#endregion

	#region - Encryption/Decryption using DPAPI -

	[Fact]
	public void TestEncryptDecryptString()
	{
		var plainText = "SensitiveData";
		var encrypted = SecurityUtilities.EncryptString(plainText);

		var decrypted = SecurityUtilities.DecryptString(encrypted);

		Assert.NotNull(decrypted);
		Assert.NotEqual(plainText, encrypted); // Ensure encryption happened
		Assert.Equal(plainText, SecurityUtilities.MakeUnsecureString(decrypted)); // Ensure decryption returns the original value
	}

	[Fact]
	public void TestDecryptInvalidString()
	{
		var invalidEncryptedString = "InvalidEncryptedData";
		var result = SecurityUtilities.DecryptString(invalidEncryptedString);

		Assert.NotNull(result);
		Assert.Equal(0, result.Length); // Expecting an empty SecureString due to decryption failure
	}

	#endregion

	#region - HMAC SHA256 -

	[Fact]
	public void TestGenerateHmacSignatureString()
	{
		var content = "HelloWorld";
		var secret = "SharedSecret";
		var result = SecurityUtilities.GenerateHmacSignature(content, secret);

		Assert.NotNull(result);
		Assert.NotEmpty(result);
	}

	[Fact]
	public void TestGenerateHmacSignatureBytes()
	{
		var content = "HelloWorld";
		var secret = Encoding.UTF8.GetBytes("SharedSecret");
		var result = SecurityUtilities.GenerateHmacSignature(content, secret);

		Assert.NotNull(result);
		Assert.NotEmpty(result);
	}

	[Fact]
	public void TestGenerateHmacSignatureTruncation()
	{
		var content = "HelloWorld";
		var secret = "SharedSecret";

		var full_hash_bytes = Convert.FromBase64String(SecurityUtilities.GenerateHmacSignature(content, secret, isTruncated: false));
		var truncated_hash_bytes = Convert.FromBase64String(SecurityUtilities.GenerateHmacSignature(content, secret, isTruncated: true));

		Assert.NotNull(truncated_hash_bytes);
		Assert.Equal(full_hash_bytes.Length/2, truncated_hash_bytes.Length);
	}

	[Fact]
	public void TestGenerateHmacSignatureBase64Url()
	{
		var content = "HelloWorld";
		var secret = "SharedSecret";

		var result = SecurityUtilities.GenerateHmacSignature(content, secret, isTruncated: false, isBase64UrlEncoded: true);

		Assert.NotNull(result);
		Assert.NotEmpty(result);

		Assert.True(result.EndsWith("=") == false); // Base64 URL encoding typically omits padding
	}

	#endregion

	#region - RSA Key Management -

	[Fact]
	public void TestIsRsaNamedKeyPresent_ValidKey()
	{
		var keyName = "ValidRsaKey";
		var result = SecurityUtilities.IsRsaNamedKeyPresent(keyName);

		Assert.False(result); // The key should not exist in a default environment, but this will test behavior
	}

	[Fact]
	public void TestIsRsaNamedKeyPresent_InvalidKey()
	{
		var keyName = "InvalidRsaKey";
		var result = SecurityUtilities.IsRsaNamedKeyPresent(keyName);

		Assert.False(result); // If the key doesn't exist, we expect false
	}

	[Fact]
	public void TestGetRsaNamedKey()
	{
		var keyName = "TestKey";

		try
		{
			using RSACryptoServiceProvider rsa = SecurityUtilities.GetRsaNamedKey(keyName);
			Assert.NotNull(rsa); // If key exists, should not be null
		}
		catch (Exception ex)
		{
			Assert.IsType<SecurityException>(ex); // If key doesn't exist, should throw exception
		}
	}

	[Fact]
	public void TestInstallAndRemoveRsaNamedKey()
	{
		var keyName = "TestKey";
		var keyPairXml = "<RSAKeyValue><Modulus>sqK3gJBKjsRppCNfxsUUXYB4cBvsRvs51/9ORdoVEp8L77Cd9awSSmVKPCuWipu4oOewErXx+sOMxlEsjQec6bpVVSkXoZ0bHPsBrALHBTsET8iXZZ5QlgmahJmQrQ5owViY6Qj/TJy1zrI2bGoFOhOmJquL+GwI0Yp6kPupLrk=</Modulus><Exponent>AQAB</Exponent><P>xdhEEKGbjzc0QZjD0Et/MrP+I1abn4BzgfjPDRWb9UdVI8qUgWb20XEMYxl8itI9HrMKlWIzgCMOf3F9TKmEIw==</P><Q>5yT4AzSaydZ14jhXyjko26NedMZH4J5j9epY1WxJi8zMKJrEPee3ZSNk5yKQLBkYIn6CEtiQElbjmMNYwtmRcw==</Q><DP>NjP4+eF8v/Ds5SfYReHZOGftsXrR6hIEE1C6UShhcQKZBdRDeWxfJKRnM2NRJqtQyW9d7+1WlL2GBE5weKdcpw==</DP><DQ>mWzLhu0q9WB5/P+jHiLUwP1unBpk6W6ZMUktT/TB2J7GwQkBy4l8DRDyUA18HxlFENhiJHpFHzc3eVXpG+Toiw==</DQ><InverseQ>noSSrvh52brQb8H95p8ntf/n9OI0I1vAGM8MjuE61Noc8x5rkaLBN/BxONm8npyoiD6Vnp50xLvLpxhSFwwieA==</InverseQ><D>GeyhYugN2fBJQIfds3QQg41MbUCwJpD5EQ69JkgC/OPWEJ/6HJgvQ1q1zkupGk6FGdzl4aFED9dnS7SmGYV8UPT2zxUVnGKQ7RSruJlY4DrGkrh7XvzQzU5KZQsv+ufY2y1KImt/VU2PFAYuAMt2O44LW/qVQ207XdQwqHLzCH0=</D></RSAKeyValue>"; // XML example

		try
		{
			SecurityUtilities.InstallRsaNamedKey(keyName, keyPairXml);
			Assert.True(SecurityUtilities.IsRsaNamedKeyPresent(keyName)); // Key should be installed successfully
		}
		catch (Exception ex)
		{
			Assert.Fail($"Installation failed: {ex.Message}");
		}
		finally
		{
			// Cleanup after test
			SecurityUtilities.RemoveRsaNamedKey(keyName);
			Assert.False(SecurityUtilities.IsRsaNamedKeyPresent(keyName)); // Ensure key has been removed
		}
	}

	#endregion
}
