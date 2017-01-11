/*
 * Created by SharpDevelop.
 * User: Young
 * Date: 2017/1/5
 * Time: 21:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TobaccoPatch
{
	public enum EncryptTypeEnum
	{
		DES,
		MD5
	}
	
	public class EncryptHelper
	{
		private string inputString;

		private string outString;

		private string inputFilePath;

		private string outFilePath;

		private string encryptKey = "CHINASOFT";

		private string decryptKey = "CHINASOFT";

		private string noteMessage;

		public string InputString
		{
			get
			{
				return this.inputString;
			}
			set
			{
				this.inputString = value;
			}
		}

		public string OutString
		{
			get
			{
				return this.outString;
			}
			set
			{
				this.outString = value;
			}
		}

		public string InputFilePath
		{
			get
			{
				return this.inputFilePath;
			}
			set
			{
				this.inputFilePath = value;
			}
		}

		public string OutFilePath
		{
			get
			{
				return this.outFilePath;
			}
			set
			{
				this.outFilePath = value;
			}
		}

		public string EncryptKey
		{
			get
			{
				return this.encryptKey;
			}
			set
			{
				this.encryptKey = value;
			}
		}

		public string DecryptKey
		{
			get
			{
				return this.decryptKey;
			}
			set
			{
				this.decryptKey = value;
			}
		}

		public string NoteMessage
		{
			get
			{
				return this.noteMessage;
			}
			set
			{
				this.noteMessage = value;
			}
		}

		public static string ExecEncrypt(string strSource)
		{
			EncryptHelper encryptor = new EncryptHelper();
			encryptor.InputString = strSource;
			encryptor.DesEncrypt();
			return encryptor.OutString;
		}

		public static string ExecDeEncrypt(string strSource)
		{
			EncryptHelper encryptor = new EncryptHelper();
			encryptor.InputString = strSource;
			encryptor.DesDecrypt();
			return encryptor.OutString;
		}

		public static string ExecEncrypt(EncryptTypeEnum encrypttype, string strSource)
		{
			EncryptHelper encryptor = new EncryptHelper();
			encryptor.InputString = strSource;
			switch (encrypttype)
			{
				case EncryptTypeEnum.DES:
					encryptor.DesEncrypt();
					return encryptor.OutString;
				case EncryptTypeEnum.MD5:
					encryptor.MD5Encrypt();
					return encryptor.OutString;
				default:
					encryptor.DesEncrypt();
					return encryptor.OutString;
			}
		}

		public void DesEncrypt()
		{
			byte[] rgbIV = new byte[]
			{
				18,
				52,
				86,
				120,
				144,
				171,
				205,
				239
			};
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(this.encryptKey.Substring(0, 8));
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				byte[] bytes2 = Encoding.UTF8.GetBytes(this.inputString);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(bytes2, 0, bytes2.Length);
				cryptoStream.FlushFinalBlock();
				this.outString = Convert.ToBase64String(memoryStream.ToArray());
			}
			catch (Exception ex)
			{
				this.noteMessage = ex.Message;
			}
		}

		public void DesDecrypt()
		{
			byte[] rgbIV = new byte[]
			{
				18,
				52,
				86,
				120,
				144,
				171,
				205,
				239
			};
			byte[] array = new byte[this.inputString.Length];
			byte[] bytes = Encoding.UTF8.GetBytes(this.decryptKey.Substring(0, 8));
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			array = Convert.FromBase64String(this.inputString);
			
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			Encoding encoding = new UTF8Encoding();
			this.outString = encoding.GetString(memoryStream.ToArray());
		}

		public void FileDesEncrypt()
		{
			byte[] rgbIV = new byte[]
			{
				18,
				52,
				86,
				120,
				144,
				171,
				205,
				239
			};
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(this.encryptKey.Substring(0, 8));
				FileStream fileStream = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read);
				FileStream fileStream2 = new FileStream(this.outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream2.SetLength(0L);
				byte[] buffer = new byte[100];
				long num = 0L;
				long length = fileStream.Length;
				DES dES = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = new CryptoStream(fileStream2, dES.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
				while (num < length)
				{
					int num2 = fileStream.Read(buffer, 0, 100);
					cryptoStream.Write(buffer, 0, num2);
					num += (long)num2;
				}
				cryptoStream.Close();
				fileStream2.Close();
				fileStream.Close();
			}
			catch (Exception ex)
			{
				this.noteMessage = ex.Message.ToString();
			}
		}

		public void FileDesDecrypt()
		{
			byte[] rgbIV = new byte[]
			{
				18,
				52,
				86,
				120,
				144,
				171,
				205,
				239
			};
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(this.decryptKey.Substring(0, 8));
				FileStream fileStream = new FileStream(this.inputFilePath, FileMode.Open, FileAccess.Read);
				FileStream fileStream2 = new FileStream(this.outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream2.SetLength(0L);
				byte[] buffer = new byte[100];
				long num = 0L;
				long length = fileStream.Length;
				DES dES = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = new CryptoStream(fileStream2, dES.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
				while (num < length)
				{
					int num2 = fileStream.Read(buffer, 0, 100);
					cryptoStream.Write(buffer, 0, num2);
					num += (long)num2;
				}
				cryptoStream.Close();
				fileStream2.Close();
				fileStream.Close();
			}
			catch (Exception ex)
			{
				this.noteMessage = ex.Message.ToString();
			}
		}

		public void MD5Encrypt()
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] bytes = mD.ComputeHash(Encoding.Default.GetBytes(this.inputString));
			this.outString = Encoding.Default.GetString(bytes);
		}
	}
}
