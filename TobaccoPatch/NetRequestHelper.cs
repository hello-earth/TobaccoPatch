/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2017/1/6
 * Time: 8:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections;

namespace TobaccoPatch
{
	/// <summary>
	/// Description of NetRequestHelper.
	/// </summary>
	public class NetRequestHelper
	{
		private static CookieContainer cc = new CookieContainer();
		
		public static string WebSoap(string soapAction)
		{
			var uri = "http://183.63.197.99:9080/npserver/services/WService";
			var req = WebRequest.Create(uri);
			req.Headers.Add("SOAPAction","");
			req.Timeout = 30000;  //timeout
			req.Method = "POST";
			req.ContentType = "text/xml;charset=UTF-8";

			using (var writer = new StreamWriter(req.GetRequestStream()))
			{
				writer.Write(soapAction);
				writer.Close();
			}
			
			using (var rsp = req.GetResponse())
			{
				req.GetRequestStream().Close();
				if (rsp != null)
				{
					using (var answerReader = new StreamReader(rsp.GetResponseStream()))
					{
						var readString = answerReader.ReadToEnd();
						return readString;
					}
				}
			}
			return "";
		}
		
		public static string PostAndGetHTML(string targetURL, Hashtable param)
		{
			string formData = "";

			foreach (DictionaryEntry de in param)

			{

				formData += de.Key.ToString() + "=" + de.Value.ToString()+"&";

			}

			if(formData.Length>0)

				formData = formData.Substring(0, formData.Length - 1); //remove last '&'


			ASCIIEncoding encoding = new ASCIIEncoding();

			byte[] data = encoding.GetBytes(formData);


			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetURL);

			request.Method = "POST";    //post
			
			request.AllowAutoRedirect = true;

			request.ContentType = "application/x-www-form-urlencoded";

			request.ContentLength = data.Length;

			request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13 (.NET CLR 3.5.30729)";

			Stream newStream = request.GetRequestStream();

			newStream.Write(data, 0, data.Length);


			newStream.Close();


			request.CookieContainer = NetRequestHelper.cc;

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			NetRequestHelper.cc.Add(response.Cookies);

			Stream stream = response.GetResponseStream();

			return new StreamReader(stream, System.Text.Encoding.UTF8).ReadToEnd();

		}
		
		public static string GetHtml(string uri)
		{
			HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
			request.Method = "GET";
			request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13 (.NET CLR 3.5.30729)";
			request.CookieContainer = NetRequestHelper.cc;
			
			// 接收返回的页面
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			NetRequestHelper.cc.Add(response.Cookies);
			Stream responseStream = response.GetResponseStream();
			StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
			return reader.ReadToEnd();
		}
		
	}
}
