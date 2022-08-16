using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	public class PdfImportSettings
	{
		public PdfImportSettings()
		{
			this.ReadingMode = ReadingMode.AllAtOnce;
			this.IsPdfViewer = false;
		}

		public event EventHandler<PasswordNeededEventArgs> UserPasswordNeeded;

		internal ReadingMode ReadingMode { get; set; }

		internal bool IsPdfViewer { get; set; }

		internal string GetUserPassword()
		{
			PasswordNeededEventArgs passwordNeededEventArgs = new PasswordNeededEventArgs();
			this.OnUserPasswordNeeded(passwordNeededEventArgs);
			return passwordNeededEventArgs.Password;
		}

		void OnUserPasswordNeeded(PasswordNeededEventArgs args)
		{
			if (this.UserPasswordNeeded != null)
			{
				this.UserPasswordNeeded(this, args);
			}
		}
	}
}
