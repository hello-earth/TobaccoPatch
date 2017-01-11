/*
 * Created by SharpDevelop.
 * User: Young
 * Date: 2017/1/5
 * Time: 21:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO.Compression;

namespace TobaccoPatch
{
	/// <summary>
	/// Description of CompressHelper.
	/// </summary>
	public class CompressHelper
	{
		private System.Text.Encoding textEncoding;

		public CompressHelper()
		{
			this.textEncoding = System.Text.Encoding.UTF8;
		}

		public CompressHelper(System.Text.Encoding pEncoding)
		{
			this.textEncoding = pEncoding;
		}

		public byte[] ConvertStringToCompressByteArray(string pStr)
		{
			return this.CompressionData(this.textEncoding.GetBytes(pStr));
		}

		public string ConvertCompressByteArrayToString(byte[] pData)
		{
			return this.textEncoding.GetString(this.DecompressionData(pData));
		}

		private byte[] CompressionData(byte[] input)
		{
			byte[] array = null;
			try
			{
				using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
				{
					using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
					{
						gZipStream.Write(input, 0, input.Length);
						gZipStream.Close();
					}
					array = memoryStream.ToArray();
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			this.DecompressionData(array);
			return array;
		}

		private byte[] DecompressionData(byte[] input)
		{
			byte[] result = null;
			try
			{
				using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
				{
					System.IO.MemoryStream memoryStream2 = new System.IO.MemoryStream(input);
					using (GZipStream gZipStream = new GZipStream(memoryStream2, CompressionMode.Decompress))
					{
						byte[] array = new byte[4096];
						int count;
						while ((count = gZipStream.Read(array, 0, array.Length)) != 0)
						{
							memoryStream.Write(array, 0, count);
						}
						gZipStream.Close();
					}
					memoryStream2.Dispose();
					memoryStream2.Close();
					result = memoryStream.ToArray();
				}
			}
			catch (System.Exception)
			{
				result = null;
			}
			return result;
		}
	}
}
