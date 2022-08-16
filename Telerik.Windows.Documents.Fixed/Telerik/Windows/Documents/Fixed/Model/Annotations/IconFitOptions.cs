using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class IconFitOptions
	{
		internal IconFitOptions()
		{
			this.ScaleCondition = IconScaleCondition.AlwaysScale;
			this.ScalingType = IconScalingType.Proportional;
			this.BlankSpaceFromTheLeftSide = 0.5;
			this.BlankSpaceFromTheBottomSide = 0.5;
			this.IgnoreBorderWidth = false;
		}

		internal IconFitOptions(IconFitOptions other)
		{
			this.ScaleCondition = other.ScaleCondition;
			this.ScalingType = other.ScalingType;
			this.BlankSpaceFromTheLeftSide = other.BlankSpaceFromTheLeftSide;
			this.BlankSpaceFromTheBottomSide = other.BlankSpaceFromTheBottomSide;
			this.IgnoreBorderWidth = other.IgnoreBorderWidth;
		}

		public IconScaleCondition ScaleCondition { get; set; }

		public IconScalingType ScalingType { get; set; }

		public double BlankSpaceFromTheLeftSide
		{
			get
			{
				return this.leftSpacePercent;
			}
			set
			{
				Guard.ThrowExceptionIfOutOfRange<double>(0.0, 1.0, value, "value");
				this.leftSpacePercent = value;
			}
		}

		public double BlankSpaceFromTheBottomSide
		{
			get
			{
				return this.bottomSpacePercent;
			}
			set
			{
				Guard.ThrowExceptionIfOutOfRange<double>(0.0, 1.0, value, "value");
				this.bottomSpacePercent = value;
			}
		}

		public bool IgnoreBorderWidth { get; set; }

		double leftSpacePercent;

		double bottomSpacePercent;
	}
}
