/*
 * Created by SharpDevelop.
 * User: Young
 * Date: 2017/1/5
 * Time: 21:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Generic;


namespace TobaccoPatch
{
	public class MainProgram
	{
		
		public static void Loger(string msg)
		{
			StreamWriter log = new StreamWriter("log.txt", false);
			log.WriteLine(msg);
			log.Close();
		}
		
		public static string GetEnCustLicence(string lic)
		{
			return EncryptHelper.ExecEncrypt(EncryptTypeEnum.DES, lic);
		}
		
		public static string GetDeCustLicence(string strResult)
		{
			return EncryptHelper.ExecDeEncrypt(strResult);
		}
		
		public static string  CompressJY(string strResult)
		{
			byte[] array = ImageConvertHelper.StringToByteArray(strResult);
			strResult = new CompressHelper().ConvertCompressByteArrayToString(array);
			return strResult;
		}
		
		public static string CompressYS(string strParam)
		{
			byte[] bytes = new CompressHelper().ConvertStringToCompressByteArray(strParam);
			return ImageConvertHelper.ByteArrayToString(bytes);
		}
		
		public static void ReadConfig(string filepath, List<string> sinfos)
		{
			StreamReader sr = new StreamReader(filepath,Encoding.UTF8);
            String line;
            while ((line = sr.ReadLine()) != null) 
            {
            	sinfos.Add(line);
            }
		}
		
	}
}
