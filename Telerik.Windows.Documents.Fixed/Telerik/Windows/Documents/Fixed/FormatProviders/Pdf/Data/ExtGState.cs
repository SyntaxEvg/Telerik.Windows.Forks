using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data
{
	class ExtGState : IInstanceIdOwner
	{
		public ExtGState()
		{
			this.id = InstanceIdGenerator.GetNextId();
		}

		public bool? AlphaSource { get; set; }

		public double? StrokeAlphaConstant { get; set; }

		public double? AlphaConstant { get; set; }

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		readonly int id;
	}
}
