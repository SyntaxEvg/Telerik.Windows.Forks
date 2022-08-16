using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class ParagraphIndentation
	{
		public ParagraphIndentation(ParagraphProperties paragraphProperties)
		{
			this.paragraphProperties = paragraphProperties;
		}

		public double FirstLineIndent
		{
			get
			{
				return this.paragraphProperties.FirstLineIndent.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.FirstLineIndent.LocalValue = new double?(value);
			}
		}

		public double HangingIndent
		{
			get
			{
				return this.paragraphProperties.HangingIndent.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.HangingIndent.LocalValue = new double?(value);
			}
		}

		public double LeftIndent
		{
			get
			{
				return this.paragraphProperties.LeftIndent.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.LeftIndent.LocalValue = new double?(value);
			}
		}

		public double RightIndent
		{
			get
			{
				return this.paragraphProperties.RightIndent.GetActualValue().Value;
			}
			set
			{
				this.paragraphProperties.RightIndent.LocalValue = new double?(value);
			}
		}

		ParagraphProperties paragraphProperties;
	}
}
