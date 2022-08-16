using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder
{
	public class JpegEncoderParameters : INotifyPropertyChanged
	{
		public JpegEncoderParameters()
		{
			this.ChrominanceTable = new byte[JpegEncoder.DefaultChromianceQuantizationTable.Length];
			JpegEncoder.DefaultChromianceQuantizationTable.CopyTo(this.ChrominanceTable, 0);
			this.LuminanceTable = new byte[JpegEncoder.DefaultLuminanceQuantizationTable.Length];
			JpegEncoder.DefaultLuminanceQuantizationTable.CopyTo(this.LuminanceTable, 0);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public byte[] ChrominanceTable
		{
			get
			{
				return this.chromianceTable;
			}
			set
			{
				this.chromianceTable = value;
				this.OnPropertyChanged("ChrominanceTable");
			}
		}

		public JpegEncodingType EncodingType
		{
			get
			{
				return this.encodingType;
			}
			set
			{
				this.encodingType = value;
				this.OnPropertyChanged("EncodingType");
			}
		}

		public byte[] LuminanceTable
		{
			get
			{
				return this.luminanceTable;
			}
			set
			{
				this.luminanceTable = value;
				this.OnPropertyChanged("LuminanceTable");
			}
		}

		public float QuantizingQuality
		{
			get
			{
				return this.quantizingQuality;
			}
			set
			{
				this.quantizingQuality = value;
				this.OnPropertyChanged("QuantizingQuality");
			}
		}

		public int SamplePrecision
		{
			get
			{
				return this.samplePrecision;
			}
			set
			{
				this.samplePrecision = value;
				this.OnPropertyChanged("SamplePrecision");
			}
		}

		void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		byte[] chromianceTable = JpegEncoder.DefaultChromianceQuantizationTable;

		byte[] luminanceTable = JpegEncoder.DefaultLuminanceQuantizationTable;

		float quantizingQuality = 100f;

		JpegEncodingType encodingType;

		int samplePrecision = 8;
	}
}
