using System;
using System.ComponentModel;

namespace Telerik.Windows.Zip
{
	public class EncryptionSettings : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string Algorithm
		{
			get
			{
				return this.algorithm;
			}
			protected set
			{
				this.algorithm = value;
				this.OnPropertyChanged("Algorithm");
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		string algorithm;
	}
}
