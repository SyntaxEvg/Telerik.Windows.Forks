using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Layout;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class TwoStatesButtonWidget : Widget<ButtonAppearanceCharacteristics>
	{
		internal TwoStatesButtonWidget(FormField field)
			: base(field)
		{
			this.onStateContent = new AnnotationContentSource();
			this.offStateContent = new AnnotationContentSource();
		}

		public sealed override WidgetContentType WidgetContentType
		{
			get
			{
				return WidgetContentType.TwoStatesContent;
			}
		}

		public AnnotationContentSource OnStateContent
		{
			get
			{
				base.EnsureContentIsUpToDate();
				return this.onStateContent;
			}
		}

		public AnnotationContentSource OffStateContent
		{
			get
			{
				base.EnsureContentIsUpToDate();
				return this.offStateContent;
			}
		}

		public sealed override void RecalculateContent()
		{
			this.OnStateContent.NormalContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawOnStateContent));
			this.OnStateContent.MouseDownContentSource = null;
			this.OnStateContent.MouseOverContentSource = null;
			this.OffStateContent.NormalContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawOffStateContent));
			this.OffStateContent.MouseDownContentSource = null;
			this.OffStateContent.MouseOverContentSource = null;
		}

		internal override void ConsumeImportedAppearances(AnnotationAppearances appearances)
		{
			switch (appearances.AppearancesType)
			{
			case AnnotationAppearancesType.SingleStateAppearances:
			{
				SingleStateAppearances singleStateAppearances = (SingleStateAppearances)appearances;
				this.OnStateContent.Initialize(singleStateAppearances);
				this.OffStateContent.Initialize(singleStateAppearances);
				return;
			}
			case AnnotationAppearancesType.MultiStateAppearances:
			{
				MultiStateAppearances multiStateAppearances = (MultiStateAppearances)appearances;
				using (IEnumerator<KeyValuePair<string, SingleStateAppearances>> enumerator = multiStateAppearances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, SingleStateAppearances> keyValuePair = enumerator.Current;
						if (keyValuePair.Key.Equals("Off"))
						{
							this.OffStateContent.Initialize(keyValuePair.Value);
						}
						else
						{
							this.OnStateContent.Initialize(keyValuePair.Value);
						}
					}
					return;
				}
				break;
			}
			}
			throw new NotSupportedException(string.Format("Not supported appearances type: {0}", appearances.AppearancesType));
		}

		internal void PrepareAppearancesForExport(string onStateName, bool isChecked)
		{
			MultiStateAppearances multiStateAppearances = new MultiStateAppearances();
			multiStateAppearances.CurrentState = (isChecked ? onStateName : "Off");
			multiStateAppearances.AddAppearance(onStateName, new SingleStateAppearances(this.OnStateContent));
			multiStateAppearances.AddAppearance("Off", new SingleStateAppearances(this.OffStateContent));
			base.Appearances = multiStateAppearances;
		}

		internal sealed override Widget CreateClonedWidgetInstance(RadFixedDocumentCloneContext cloneContext)
		{
			TwoStatesButtonWidget twoStatesButtonWidget = this.CreateClonedTwoStateButtonWidget(cloneContext);
			twoStatesButtonWidget.OnStateContent.Initialize(this.OnStateContent);
			twoStatesButtonWidget.OffStateContent.Initialize(this.OffStateContent);
			twoStatesButtonWidget.AppearanceCharacteristics = new ButtonAppearanceCharacteristics(base.AppearanceCharacteristics);
			return twoStatesButtonWidget;
		}

		internal virtual TwoStatesButtonWidget CreateClonedTwoStateButtonWidget(RadFixedDocumentCloneContext cloneContext)
		{
			FormField clonedField = cloneContext.GetClonedField(base.Field);
			return new TwoStatesButtonWidget(clonedField);
		}

		internal virtual double ScaleBlockSize(Size blockSize)
		{
			double result = 1.0;
			if (base.TextProperties.FontSize == 0.0)
			{
				Size size = new Size(this.ButtonContentBox.Width, this.ButtonContentBox.Height);
				double val = size.Width / blockSize.Width;
				double val2 = size.Height / blockSize.Height;
				result = System.Math.Min(val, val2);
			}
			return result;
		}

		bool IsZapfDingbatsFont
		{
			get
			{
				return base.TextProperties.Font.Name == "ZapfDingbats";
			}
		}

		void DrawOnStateContent(FixedContentEditor editor)
		{
			if (!string.IsNullOrEmpty(base.AppearanceCharacteristics.NormalCaption) && !this.ButtonContentBox.IsEmpty)
			{
				TextFragment textFragment = new TextFragment(base.TextProperties.PropertiesOwner);
				if (this.IsZapfDingbatsFont)
				{
					textFragment.TextCollection.Characters = TwoStatesButtonWidget.GetZapfDingbatsGlyphs(base.AppearanceCharacteristics.NormalCaption);
				}
				else
				{
					textFragment.TextCollection.Text = base.AppearanceCharacteristics.NormalCaption;
				}
				if (base.TextProperties.FontSize == 0.0)
				{
					textFragment.FontSize = FixedDocumentDefaults.FontSize;
				}
				double width = TextFragmentLayoutElement.MeasureWidth(textFragment);
				double height = TextFragmentLayoutElement.MeasureHeight(textFragment);
				Size blockSize = new Size(width, height);
				double num = this.ScaleBlockSize(blockSize);
				if (num != 1.0)
				{
					blockSize = new Size(num * blockSize.Width, num * blockSize.Height);
					textFragment.FontSize *= num;
				}
				double offsetX = (editor.Root.Size.Width - blockSize.Width) / 2.0;
				double num2 = (editor.Root.Size.Height - blockSize.Height) / 2.0;
				double num3 = (this.IsZapfDingbatsFont ? (textFragment.FontSize * 0.16) : 0.0);
				editor.Position.Translate(offsetX, num2 + blockSize.Height - num3);
				editor.Draw(textFragment);
			}
		}

		static IEnumerable<CharInfo> GetZapfDingbatsGlyphs(string normalCaption)
		{
			foreach (char symbol in normalCaption)
			{
				int value = (int)symbol;
				CharCode code = ((value <= 255) ? new CharCode((byte)value) : new CharCode(value, 2));
				char c = symbol;
				yield return new CharInfo(c.ToString(), code);
			}
			yield break;
		}

		void DrawOffStateContent(FixedContentEditor editor)
		{
		}

		internal const string OffState = "Off";

		readonly AnnotationContentSource onStateContent;

		readonly AnnotationContentSource offStateContent;
	}
}
