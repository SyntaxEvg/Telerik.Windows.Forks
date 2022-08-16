using System;

namespace Telerik.Windows.Zip
{
	abstract class DeflateTransformBase : CompressionTransformBase
	{
		public DeflateTransformBase(DeflateSettings settings)
		{
			this.Settings = settings;
		}

		protected DeflateSettings Settings { get; set; }
	}
}
