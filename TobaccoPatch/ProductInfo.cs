/*
 * Created by SharpDevelop.
 * User: Young
 * Date: 2017/1/5
 * Time: 21:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TobaccoPatch
{
	public class ProductInfo
	{
		private string barcode;
		private string pname;
		private string product_id;
		private string type_code;
		private float price;
		
		public string Barcode
		{
			get { return barcode; }
			set { barcode = value; }
		}
		
		public string Pname
		{
			get { return pname; }
			set { pname = value; }
		}
		
		public string Product_id
		{
			get { return product_id; }
			set { product_id = value; }
		}
		
		public string Type_code
		{
			get { return type_code; }
			set { type_code = value; }
		}
		
		public float Price
		{
			get { return price; }
			set { price = value; }
		}	
		
	}
}
