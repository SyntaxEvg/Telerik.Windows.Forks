using System;
using System.ComponentModel;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
	{
		protected void OnPropertyChanged(string propertyName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(propertyName, "propertyName");
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, args);
			}
		}
	}
}
