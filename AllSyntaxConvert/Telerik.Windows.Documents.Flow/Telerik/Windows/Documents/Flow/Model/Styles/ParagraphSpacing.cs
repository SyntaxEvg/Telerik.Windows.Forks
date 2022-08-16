using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class ParagraphSpacing
	{
		public ParagraphSpacing(ParagraphProperties paragraphProperties)
		{
			this.paragraphProperties = paragraphProperties;
		}

		public double SpacingBefore
		{
			get
			{
				return this.paragraphProperties.SpacingBefore.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.SpacingBefore.LocalValue = new double?(value);
			}
		}

		public bool AutomaticSpacingBefore
		{
			get
			{
				return this.paragraphProperties.AutomaticSpacingBefore.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.AutomaticSpacingBefore.LocalValue = new bool?(value);
			}
		}

		public double SpacingAfter
		{
			get
			{
				return this.paragraphProperties.SpacingAfter.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.SpacingAfter.LocalValue = new double?(value);
			}
		}

		public bool AutomaticSpacingAfter
		{
			get
			{
				return this.paragraphProperties.AutomaticSpacingAfter.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.AutomaticSpacingAfter.LocalValue = new bool?(value);
			}
		}

		public double LineSpacing
		{
			get
			{
				return this.paragraphProperties.LineSpacing.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.LineSpacing.LocalValue = new double?(value);
			}
		}

		public HeightType LineSpacingType
		{
			get
			{
				return this.paragraphProperties.LineSpacingType.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.LineSpacingType.LocalValue = new HeightType?(value);
			}
		}

		readonly ParagraphProperties paragraphProperties;
	}
}
