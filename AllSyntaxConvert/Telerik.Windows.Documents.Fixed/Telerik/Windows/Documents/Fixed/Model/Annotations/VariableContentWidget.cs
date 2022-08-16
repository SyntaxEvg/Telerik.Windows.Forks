using System;
using System.IO;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class VariableContentWidget : Widget<DynamicAppearanceCharacteristics>, IContentAnnotation
	{
		internal VariableContentWidget(FormField field)
			: base(field)
		{
			this.content = new AnnotationContentSource();
		}

		public sealed override WidgetContentType WidgetContentType
		{
			get
			{
				return WidgetContentType.VariableContent;
			}
		}

		public AnnotationContentSource Content
		{
			get
			{
				base.EnsureContentIsUpToDate();
				return this.content;
			}
		}

		internal override string ContentMarkerName
		{
			get
			{
				return "Tx";
			}
		}

		public sealed override void RecalculateContent()
		{
			this.Content.NormalContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawRectangularAppearanceContent));
			this.Content.MouseDownContentSource = null;
			this.Content.MouseOverContentSource = null;
		}

		internal sealed override void ConsumeImportedAppearances(AnnotationAppearances appearances)
		{
			if (appearances.AppearancesType == AnnotationAppearancesType.SingleStateAppearances)
			{
				SingleStateAppearances singleStateAppearances = (SingleStateAppearances)appearances;
				this.Content.Initialize(singleStateAppearances);
			}
		}

		internal void PrepareAppearancesForExport()
		{
			base.Appearances = new SingleStateAppearances(this.Content);
		}

		internal sealed override Widget CreateClonedWidgetInstance(RadFixedDocumentCloneContext cloneContext)
		{
			FormField clonedField = cloneContext.GetClonedField(base.Field);
			VariableContentWidget variableContentWidget = new VariableContentWidget(clonedField);
			variableContentWidget.Content.Initialize(this.Content);
			variableContentWidget.AppearanceCharacteristics = new DynamicAppearanceCharacteristics(base.AppearanceCharacteristics);
			return variableContentWidget;
		}

		void DrawRectangularAppearanceContent(FixedContentEditor editor)
		{
			switch (base.Field.FieldType)
			{
			case FormFieldType.CombTextBox:
				this.DrawCombTextBoxContent(editor);
				return;
			case FormFieldType.TextBox:
				this.DrawTextBoxContent(editor);
				return;
			case FormFieldType.ComboBox:
				this.DrawComboBoxContent(editor);
				return;
			case FormFieldType.ListBox:
				this.DrawListBoxContent(editor);
				return;
			default:
				return;
			}
		}

		void DrawComboBoxContent(FixedContentEditor editor)
		{
			ComboBoxField comboBoxField = (ComboBoxField)base.Field;
			if (comboBoxField.Value != null)
			{
				this.DrawTextBoxContent(editor, comboBoxField.Value.UserInterfaceValue ?? comboBoxField.Value.Value, false);
			}
		}

		void DrawListBoxContent(FixedContentEditor editor)
		{
			ListBoxField listBoxField = (ListBoxField)base.Field;
			double borderThicknessIncludingStyleThickness = base.BorderThicknessIncludingStyleThickness;
			double num = editor.Root.Size.Width - 2.0 * borderThicknessIncludingStyleThickness;
			double width = num - 2.0 * FixedDocumentDefaults.ListBoxChoicePadding.Width;
			double num2 = borderThicknessIncludingStyleThickness + FixedDocumentDefaults.ListBoxChoicePadding.Height;
			for (int i = listBoxField.TopIndex; i < listBoxField.Options.Count; i++)
			{
				ChoiceOption choiceOption = listBoxField.Options[i];
				bool flag = listBoxField.Value != null && listBoxField.Value.Contains(choiceOption);
				RectangleGeometry rectangleGeometry = null;
				if (flag)
				{
					using (editor.SaveGraphicProperties())
					{
						editor.GraphicProperties.IsFilled = true;
						editor.GraphicProperties.IsStroked = false;
						editor.GraphicProperties.StrokeThickness = 0.0;
						editor.GraphicProperties.FillColor = FixedDocumentDefaults.ListBoxSelectionColor;
						editor.Position.Translate(borderThicknessIncludingStyleThickness, borderThicknessIncludingStyleThickness);
						rectangleGeometry = new RectangleGeometry();
						editor.Position.Translate(borderThicknessIncludingStyleThickness, num2);
						editor.DrawPath(rectangleGeometry);
					}
				}
				string text = choiceOption.UserInterfaceValue ?? choiceOption.Value;
				editor.Position.Translate(borderThicknessIncludingStyleThickness + FixedDocumentDefaults.ListBoxChoicePadding.Width, num2);
				double num3 = this.DrawTextBlockContent(editor, new Size(width, double.PositiveInfinity), text, true, false).Height + 2.0 * FixedDocumentDefaults.ListBoxChoicePadding.Height;
				num2 += num3;
				if (flag)
				{
					rectangleGeometry.Rect = new Rect(0.0, 0.0, num, num3);
				}
			}
		}

		void DrawCombTextBoxContent(FixedContentEditor editor)
		{
			CombTextBoxField combTextBoxField = (CombTextBoxField)base.Field;
			if (combTextBoxField.Value != null && combTextBoxField.MaxLengthOfInputCharacters > 0)
			{
				double borderThicknessIncludingStyleThickness = base.BorderThicknessIncludingStyleThickness;
				double num = (editor.Root.Size.Width - 2.0 * borderThicknessIncludingStyleThickness - 2.0 * FixedDocumentDefaults.TextBoxTextPadding) / (double)combTextBoxField.MaxLengthOfInputCharacters;
				double height = editor.Root.Size.Height - 2.0 * borderThicknessIncludingStyleThickness - 2.0 * FixedDocumentDefaults.TextBoxTextPadding;
				Size availableSize = new Size(num, height);
				double num2 = borderThicknessIncludingStyleThickness + FixedDocumentDefaults.TextBoxTextPadding;
				double offsetY = borderThicknessIncludingStyleThickness + FixedDocumentDefaults.TextBoxTextPadding;
				int num3 = 0;
				while (num3 < combTextBoxField.MaxLengthOfInputCharacters && num3 < combTextBoxField.Value.Length)
				{
					string text = combTextBoxField.Value[num3].ToString();
					editor.Position.Translate(num2, offsetY);
					this.DrawTextBlockContent(editor, availableSize, text, false, false);
					num2 += num;
					num3++;
				}
			}
		}

		void DrawTextBoxContent(FixedContentEditor editor)
		{
			TextBoxField textBoxField = (TextBoxField)base.Field;
			if (textBoxField.Value != null)
			{
				string text = (textBoxField.IsPassword ? new string('*', textBoxField.Value.Length) : textBoxField.Value);
				if (textBoxField.MaxLengthOfInputCharacters != null && text.Length > textBoxField.MaxLengthOfInputCharacters.Value)
				{
					text = text.Substring(0, textBoxField.MaxLengthOfInputCharacters.Value);
				}
				bool isMultiline = textBoxField.IsMultiline;
				this.DrawTextBoxContent(editor, text, isMultiline);
			}
		}

		void DrawTextBoxContent(FixedContentEditor editor, string textToInsert, bool allowMultilineText)
		{
			double borderThicknessIncludingStyleThickness = base.BorderThicknessIncludingStyleThickness;
			editor.Position.Translate(FixedDocumentDefaults.TextBoxTextPadding + borderThicknessIncludingStyleThickness, FixedDocumentDefaults.TextBoxTextPadding + borderThicknessIncludingStyleThickness);
			double width = editor.Root.Size.Width - 2.0 * borderThicknessIncludingStyleThickness - 2.0 * FixedDocumentDefaults.TextBoxTextPadding;
			double height = (allowMultilineText ? double.PositiveInfinity : (editor.Root.Size.Height - 2.0 * borderThicknessIncludingStyleThickness - 2.0 * FixedDocumentDefaults.TextBoxTextPadding));
			this.DrawTextBlockContent(editor, new Size(width, height), textToInsert, allowMultilineText, allowMultilineText);
		}

		Size DrawTextBlockContent(FixedContentEditor positionedEditor, Size availableSize, string text, bool allowMultilineText, bool wrapText)
		{
			Block block = new Block();
			base.TextProperties.CopyTo(block);
			block.VerticalAlignment = (double.IsPositiveInfinity(availableSize.Height) ? Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Top : Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Center);
			this.InsertText(block, text, allowMultilineText);
			if (wrapText)
			{
				positionedEditor.DrawBlock(block, availableSize);
			}
			else if (block.Measure().Width > availableSize.Width)
			{
				block.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
				positionedEditor.DrawBlock(block);
			}
			else
			{
				positionedEditor.DrawBlock(block, availableSize);
			}
			return block.DesiredSize;
		}

		void InsertText(Block block, string text, bool allowMultilineText)
		{
			if (allowMultilineText)
			{
				using (StringReader stringReader = new StringReader(text))
				{
					string text2 = stringReader.ReadLine();
					while (text2 != null)
					{
						block.InsertText(text2);
						text2 = stringReader.ReadLine();
						if (text2 != null)
						{
							block.InsertLineBreak();
						}
					}
					return;
				}
			}
			block.InsertText(text);
		}

		readonly AnnotationContentSource content;
	}
}
