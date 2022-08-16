using System;

namespace Telerik.Windows.Documents.Fixed.Model
{
	public class OnDocumentExceptionEventArgs : EventArgs
	{
		public OnDocumentExceptionEventArgs(Exception documentException)
		{
			this.documentException = documentException;
			this.Handle = true;
		}

		public Exception DocumentException
		{
			get
			{
				return this.documentException;
			}
		}

		public bool Handle { get; set; }

		readonly Exception documentException;
	}
}
