/*
 * Created by SharpDevelop.
 * User: Young
 * Date: 2017/1/5
 * Time: 21:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;


namespace TobaccoPatch
{
	/// <summary>
	/// Description of ImageConvertHelper.
	/// </summary>
	public class ImageConvertHelper
	{
		public static byte[] StringToByteArray(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}
			return System.Convert.FromBase64String(str);
		}

		public static string ByteArrayToString(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return System.Convert.ToBase64String(bytes);
		}
	}
}
