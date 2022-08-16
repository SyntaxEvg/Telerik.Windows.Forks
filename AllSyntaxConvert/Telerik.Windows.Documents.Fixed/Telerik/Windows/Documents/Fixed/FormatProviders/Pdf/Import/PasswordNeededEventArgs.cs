using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	public class PasswordNeededEventArgs : EventArgs
	{
		public PasswordNeededEventArgs()
		{
			this.Password = FixedDocumentDefaults.Password;
		}

		public string Password { get; set; }
	}
}
