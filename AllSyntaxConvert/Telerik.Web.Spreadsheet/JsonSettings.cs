using System;
using System.Text;

namespace Telerik.Web.Spreadsheet
{
	public class JsonSettings
	{
		public JsonSettings()
		{
			this.Encoding = Encoding.UTF8;
			this.BufferSize = 4096;
		}

		public Encoding Encoding { get; set; }

		public int BufferSize { get; set; }
	}
}
