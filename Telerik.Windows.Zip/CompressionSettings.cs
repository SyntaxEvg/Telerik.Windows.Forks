using System;
using System.ComponentModel;

namespace Telerik.Windows.Zip
{
	public class CompressionSettings : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public CompressionMethod Method
		{
			get
			{
				return this.compressionMethod;
			}
			protected set
			{
				this.compressionMethod = value;
				this.OnPropertyChanged("Method");
			}
		}

		internal virtual void CopyFrom(CompressionSettings baseSettings)
		{
		}

		internal virtual void PrepareForZip(CentralDirectoryHeader header = null)
		{
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		CompressionMethod compressionMethod;
	}
}
