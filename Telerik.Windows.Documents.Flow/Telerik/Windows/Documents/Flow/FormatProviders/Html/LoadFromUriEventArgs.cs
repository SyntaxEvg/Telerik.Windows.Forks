using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class LoadFromUriEventArgs : EventArgs
	{
		internal LoadFromUriEventArgs(string uri)
		{
			Guard.ThrowExceptionIfNullOrEmpty(uri, "uri");
			this.uri = uri;
		}

		public string Uri
		{
			get
			{
				return this.uri;
			}
		}

		public void SetData(byte[] data)
		{
			this.data = data;
		}

		internal byte[] GetData()
		{
			return this.data;
		}

		readonly string uri;

		byte[] data;
	}
}
