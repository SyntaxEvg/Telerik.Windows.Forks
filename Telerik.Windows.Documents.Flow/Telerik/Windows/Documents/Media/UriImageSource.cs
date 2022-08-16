using System;
using System.IO;
using System.Net;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Media
{
	public sealed class UriImageSource : ImageSource
	{
		public UriImageSource(Uri uri)
			: this(uri, string.Empty)
		{
		}

		public UriImageSource(Uri uri, string extension)
		{
			Guard.ThrowExceptionIfNull<Uri>(uri, "uri");
			this.uri = uri;
			this.extension = (string.IsNullOrEmpty(extension) ? string.Empty : PathExtension.StripExtension(extension));
		}

		internal UriImageSource(Uri uri, byte[] data)
		{
			Guard.ThrowExceptionIfNull<Uri>(uri, "uri");
			this.uri = uri;
			this.data = data;
			this.extension = string.Empty;
		}

		public Uri Uri
		{
			get
			{
				return this.uri;
			}
		}

		public override byte[] Data
		{
			get
			{
				if (this.data == null)
				{
					this.GetImageData();
				}
				return this.data;
			}
		}

		public override string Extension
		{
			get
			{
				this.UpdateExtensionFromUri();
				if (string.IsNullOrEmpty(this.extension))
				{
					this.GetImageData();
				}
				return this.extension;
			}
		}

		internal override ImageSource Clone()
		{
			return new UriImageSource(this.uri, this.data)
			{
				extension = this.extension
			};
		}

		static string StripExtensionFromResponseHeader(string contentType)
		{
			int num = contentType.IndexOf('/');
			if (num == -1 || num == contentType.Length - 1)
			{
				throw new NotSupportedException(string.Format("The Content-Type: {0}", contentType));
			}
			return contentType.Substring(num + 1);
		}

		void GetImageData()
		{
			using (WebClient webClient = new WebClient())
			{
				this.data = webClient.DownloadData(this.Uri);
				this.UpdateExtensionFromUri();
				this.UpdateExtensionFromResponseHeaders(webClient.ResponseHeaders);
			}
		}

		void UpdateExtensionFromResponseHeaders(WebHeaderCollection responseHeaders)
		{
			if (string.IsNullOrEmpty(this.extension))
			{
				for (int i = 0; i < responseHeaders.Count; i++)
				{
					if (responseHeaders.Keys[i].ToString().ToLowerInvariant() == "Content-Type".ToLowerInvariant())
					{
						this.extension = UriImageSource.StripExtensionFromResponseHeader(responseHeaders[i]);
						return;
					}
				}
			}
		}

		void UpdateExtensionFromUri()
		{
			if (string.IsNullOrEmpty(this.extension))
			{
				string value = Path.GetExtension(this.Uri.OriginalString);
				if (!string.IsNullOrEmpty(value))
				{
					this.extension = PathExtension.StripExtension(value);
				}
			}
		}

		readonly Uri uri;

		byte[] data;

		string extension;
	}
}
