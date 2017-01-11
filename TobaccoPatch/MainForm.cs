/*
 * Created by SharpDevelop.
 * User: Young
 * Date: 2017/1/5
 * Time: 21:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TobaccoPatch
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		delegate void SetTextCallback(string text);
		delegate void SetListCallback(int text);
		private static Random random = new Random();
		private static string info_prefix = "<?xml version='1.0' encoding='utf-8'?><DATASETS>";
		private static string info_suffix = "</DATASETS>";
		private static string info_content = "<DATASET><SELL_HEAD><ORDER_ID>{0}</ORDER_ID><ORDER_DATE>{1}</ORDER_DATE><OPERATE_NAME>蔡志强</OPERATE_NAME><RETURN_ORDER></RETURN_ORDER><PAY_TYPE>0</PAY_TYPE><PAY_ACCOUNT></PAY_ACCOUNT><TOTAL_MONEY>{2}</TOTAL_MONEY><CASH>{3}</CASH><ELEC_TICKET>0.0000</ELEC_TICKET><ALL_DISCOUNT>0.0000</ALL_DISCOUNT><VIP_ID></VIP_ID><GET_SCORES>0</GET_SCORES><NOTE></NOTE><ORDER_TYPE>1</ORDER_TYPE></SELL_HEAD><SELL_DETAIL><PRODUCT><ORDER_ID>{4}</ORDER_ID><PRODUCT_ID>{5}</PRODUCT_ID><PRODUCT_NAME>{6}</PRODUCT_NAME><PRODUCT_TYPE_CODE>{7}</PRODUCT_TYPE_CODE><PRODUCT_UNIT >PUD120614003</PRODUCT_UNIT ><PRICE>{8}</PRICE><QTY>{9}</QTY><DISCOUNT>100.0000</DISCOUNT><SUM_MONEY>{10}</SUM_MONEY><BAR_CODE>{11}</BAR_CODE></PRODUCT></SELL_DETAIL></DATASET>";
		private static string info_requset_content="<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:tns=\"http://183.63.197.99:9080/npserver/services/WService\" xmlns:types=\"http://183.63.197.99:9080/npserver/services/WService/encodedTypes\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body soap:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><q1:posNetSell xmlns:q1=\"http://server.client.pserver.icss.com\"><info xsi:type=\"xsd:string\">{0}</info><orderid xsi:type=\"xsd:string\">{1}</orderid><encCustLicence xsi:type=\"xsd:string\">{2}</encCustLicence></q1:posNetSell></soap:Body></soap:Envelope>";
		
		public MainForm()
		{
			InitializeComponent();
			InitList();
		}
		
		void UpdateTextShower(string msg)
		{
			if (this.richTextBox1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
			{
				while (!this.richTextBox1.IsHandleCreated)
				{
					//解决窗体关闭时出现“访问已释放句柄“的异常
					if (this.richTextBox1.Disposing || this.richTextBox1.IsDisposed)
						return;
				}
				SetTextCallback d = new SetTextCallback(UpdateTextShower);
				this.richTextBox1.Invoke(d, new object[] { msg });
			}
			else
			{
				richTextBox1.Text += msg+"\n";
				this.richTextBox1.Focus();
				this.richTextBox1.SelectionStart=richTextBox1.Text.Length;
			}
			
		}
		
		void UpdateListBox(int index)
		{
			if (this.listBox1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
			{
				while (!this.listBox1.IsHandleCreated)
				{
					//解决窗体关闭时出现“访问已释放句柄“的异常
					if (this.listBox1.Disposing || this.listBox1.IsDisposed)
						return;
				}
				SetListCallback d = new SetListCallback(UpdateListBox);
				this.richTextBox1.Invoke(d, new object[] { index });
			}
			else
			{
				listBox1.SelectedIndex = index;
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			ThreadStart threadStart = new ThreadStart(DoWork);
			Thread thread = new Thread(threadStart);
			thread.Start();
		}
		
		string CreateOID_Prefix(string cust, ref int index)
		{
			string content = NetRequestHelper.GetHtml("http://183.63.197.99:9080/terminal/monitors.do?method=getMonitorDetailOfToday&custLicence="+cust);
			if("".Equals(content) || content==null) return "";
			if(content.IndexOf(@"没有满足条件的记录")>0)
			{
				index = 0;
				return "OR"+DateTime.Now.ToString("yy-MM-dd");
			}
			
//			int first = content.IndexOf("nowrap>OR");
//			int last = content.IndexOf("</td>",first);
//			content = content.Substring(first,last-first).Substring(7);
//			UpdateTextShower("查询到当天该商户最新订单号："+content);
//			
//			index =  Convert.ToInt32(content.Substring(8));
//			
//			return content.Substring(0,8);
			UpdateTextShower(@"当前商户今天已有扫码记录");
			return null;
		}
		
		string CreateOID_Indexstr(ref int index)
		{
			index++;
			string indexstr = "";
			if(index<10)
			{
				indexstr = "00"+index.ToString();
			}else if(index<100)
			{
				indexstr = "0"+index.ToString();
			}else
			{
				indexstr = index.ToString();
			}
			return indexstr;
		}
		
		void DoWork()
		{
			string strResult = "";
			string info = "";
			string cust = "";
			string lic = "";
			List<ProductInfo> pInfos = InitProducts();
			ProductInfo pInfo;
			
			System.Collections.Hashtable reqps = new System.Collections.Hashtable();
			reqps.Add("userid",textBox1.Text);
			reqps.Add("j_username",textBox1.Text);
			reqps.Add("userName",textBox1.Text);
			reqps.Add("password",textBox2.Text);
			reqps.Add("j_password",textBox2.Text);
			NetRequestHelper.PostAndGetHTML("http://183.63.197.99:9080/login",reqps);
			
			for(int i=0;i<listBox1.Items.Count;i++)
			{
				//循环进每个商户
				UpdateListBox(i);
				strResult=info_prefix;
				string oid="";
				int index = 0 ;
				
				lic = (listBox1.Items[i].ToString());
				cust = MainProgram.GetEnCustLicence(lic);
				UpdateTextShower("获得商户编码【"+lic+"】的加密字符 "+cust);
				
				
				String oid_prefix = CreateOID_Prefix(lic, ref index);
				if(string.IsNullOrEmpty(oid_prefix)) continue;
				
				int n = random.Next(4,6);
				int pn = 1;
				int qn = 1;
				float summoney = 0;
				
				for(int j=0;j<n;j++)
				{
					//循环生成虚拟订单
					oid = oid_prefix+CreateOID_Indexstr(ref index);
					UpdateTextShower("创建虚拟订单号 "+oid);
					pn = random.Next(0,17);
					qn = random.Next(1,2);
					pInfo = pInfos[pn];
					summoney = pInfo.Price*qn;
					
					strResult += String.Format(info_content,oid,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),summoney.ToString("f4"),summoney.ToString("f4"),oid,pInfo.Product_id,
					                           pInfo.Pname,pInfo.Type_code,pInfo.Price.ToString("f4"),qn.ToString("f4"),summoney.ToString("f4"),pInfo.Barcode);
					Thread.Sleep(2002+n*100);	
				}
				strResult += info_suffix;
				
				info = MainProgram.CompressYS(strResult);
				info = String.Format(info_requset_content,info,oid,cust);
				info = NetRequestHelper.WebSoap(info);
				
				if(info.Length>700)
				{
					int first = info.IndexOf("<posNetSellReturn xsi:type=\"soapenc:string\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">");
					int last = info.IndexOf("</posNetSellReturn>");
					info = info.Substring(first,last-first).Substring(102);
					info = MainProgram.CompressJY(info);
					first = info.IndexOf("<msg>");
					last = info.IndexOf("</msg>");
					
					strResult = info.Substring(first,last-first).Substring(5);
					UpdateTextShower(strResult);
				}
				UpdateTextShower("****************");
			
			}
			
			UpdateTextShower("全部任务已完成");
		}
		
		void InitList()
		{
			List<string> sinfos =  new List<string>();
			MainProgram.ReadConfig(@".\shoplist.conf",sinfos);
			foreach(string sinfo in sinfos)
			{
				listBox1.Items.Add(sinfo);
			}
		}
		
		List<ProductInfo> InitProducts()
		{
			List<ProductInfo> pInfos =  new List<ProductInfo>();
			List<string> sinfos =  new List<string>();
			MainProgram.ReadConfig(@".\products.conf",sinfos);
			foreach(string sinfo in sinfos)
			{
				if(sinfo.StartsWith("#") || "".Equals(sinfo)) continue;
				
				string[] info = sinfo.Split('|');
				ProductInfo pinfo = new ProductInfo();
				pinfo.Barcode=info[0];
				pinfo.Pname=info[1];
				pinfo.Product_id=info[2];
				pinfo.Type_code=info[3];
				pinfo.Price=Convert.ToSingle(info[4]);
				pInfos.Add(pinfo);
			}
			return pInfos;
		}
		
	}
}
