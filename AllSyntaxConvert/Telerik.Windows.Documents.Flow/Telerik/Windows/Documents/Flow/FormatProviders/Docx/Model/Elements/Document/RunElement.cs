using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class RunElement : DocumentElementBase
	{
		public RunElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.runPropertiesChildElement = base.RegisterChildElement<RunPropertiesElement>("rPr");
		}

		public override string ElementName
		{
			get
			{
				return "r";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public RunPropertiesElement RunPropertiesElement
		{
			get
			{
				return this.runPropertiesChildElement.Element;
			}
		}

		public Watermark Watermark
		{
			get
			{
				return this.watermark;
			}
			set
			{
				Guard.ThrowExceptionIfNull<Watermark>(value, "value");
				this.watermark = value;
			}
		}

		public void SetAssociatedFlowModelElement(InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			this.inline = inline;
		}

		public void SetAssociatedFlowModelElementParent(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.paragraph = paragraph;
		}

		protected override void ClearOverride()
		{
			this.paragraph = null;
			this.inline = null;
			this.run = null;
			this.watermark = null;
		}

		protected override bool ShouldImport(IDocxImportContext context)
		{
			return base.ShouldImport(context) || this.paragraph != null;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			if (this.RunPropertiesElement != null)
			{
				if (this.run != null)
				{
					this.RunPropertiesElement.CopyPropertiesTo(this.run.Properties);
				}
				base.ReleaseElement(this.runPropertiesChildElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			DocumentElementType type = this.inline.Type;
			if (type != DocumentElementType.Run)
			{
				return;
			}
			Run run = (Run)this.inline;
			if (run.Properties.HasLocalValues())
			{
				base.CreateElement(this.runPropertiesChildElement);
				this.RunPropertiesElement.SetAssociatedFlowModelElement(run.Properties);
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Run run = this.inline as Run;
			if (run != null && run.Text != null)
			{
				string[] texts = run.Text.Split(new char[] { '\t' });
				bool isFirst = true;
				foreach (string txt in texts)
				{
					if (!isFirst)
					{
						yield return base.CreateElement<TabElement>("tab");
					}
					if (!string.IsNullOrEmpty(txt))
					{
						DocumentElementBase textElement;
						if (context.FieldContext.IsInInstruction)
						{
							textElement = base.CreateElement<InstructionTextElement>("instrText");
						}
						else
						{
							textElement = base.CreateElement<TextElement>("t");
						}
						textElement.InnerText = txt;
						yield return textElement;
					}
					isFirst = false;
				}
			}
			ShapeInlineBase shape = this.inline as ShapeInlineBase;
			if (shape != null)
			{
				DrawingElement drawingElement = base.CreateElement<DrawingElement>("drawing");
				drawingElement.CopyPropertiesFrom(context, shape);
				yield return drawingElement;
			}
			ShapeAnchorBase anchor = this.inline as ShapeAnchorBase;
			if (anchor != null)
			{
				DrawingElement drawingElement2 = base.CreateElement<DrawingElement>("drawing");
				drawingElement2.CopyPropertiesFrom(context, anchor);
				yield return drawingElement2;
			}
			FieldCharacter fieldChar = this.inline as FieldCharacter;
			if (fieldChar != null)
			{
				context.FieldContext.OnFieldCharacter(fieldChar);
				FieldCharacterElement fieldCharElement = base.CreateElement<FieldCharacterElement>("fldChar");
				fieldCharElement.FieldCharacterType = fieldChar.FieldCharacterType;
				if (fieldChar.FieldCharacterType == FieldCharacterType.Start && fieldChar.FieldInfo != null)
				{
					fieldCharElement.IsLocked = fieldChar.FieldInfo.IsLocked;
					fieldCharElement.IsDirty = fieldChar.FieldInfo.IsDirty;
				}
				yield return fieldCharElement;
			}
			Break br = this.inline as Break;
			if (br != null)
			{
				BreakElement breakElement = base.CreateElement<BreakElement>("br");
				breakElement.CopyPropertiesFrom(context, br);
				yield return breakElement;
			}
			CommentRangeEnd commentEnd = this.inline as CommentRangeEnd;
			if (commentEnd != null)
			{
				CommentReferenceElement refElement = base.CreateElement<CommentReferenceElement>("commentReference");
				refElement.Id = context.CommentContext.GetIdByRegisteredAnnotation(commentEnd.Comment);
				yield return refElement;
			}
			if (this.watermark != null)
			{
				VmlContainerElement pictElement = base.CreateElement<VmlContainerElement>("pict");
				pictElement.Watermark = this.watermark;
				yield return pictElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			string elementName;
			switch (elementName = element.ElementName)
			{
			case "t":
			case "instrText":
			{
				if (this.run == null)
				{
					this.run = new Run(context.Document);
					this.run.Text = string.Empty;
					this.paragraph.Inlines.Add(this.run);
				}
				Run run = this.run;
				run.Text += element.InnerText;
				return;
			}
			case "tab":
			{
				if (this.run == null)
				{
					this.run = new Run(context.Document);
					this.paragraph.Inlines.Add(this.run);
				}
				Run run2 = this.run;
				run2.Text += "\t";
				return;
			}
			case "drawing":
			{
				InlineBase inlineBase = ((DrawingElement)element).CreateInline(context);
				if (inlineBase != null)
				{
					this.paragraph.Inlines.Add(inlineBase);
					return;
				}
				break;
			}
			case "fldChar":
			{
				FieldCharacterElement fieldCharacterElement = (FieldCharacterElement)element;
				FieldCharacter fieldCharacter = new FieldCharacter(context.Document, fieldCharacterElement.FieldCharacterType);
				this.paragraph.Inlines.Add(fieldCharacter);
				context.FieldContext.OnFieldCharacter(fieldCharacter);
				if (fieldCharacter.FieldCharacterType == FieldCharacterType.Start)
				{
					context.FieldContext.SetIsDirty(fieldCharacterElement.IsDirty);
					context.FieldContext.SetIsLocked(fieldCharacterElement.IsLocked);
					return;
				}
				break;
			}
			case "br":
			{
				BreakElement breakElement = (BreakElement)element;
				Break @break = new Break(context.Document);
				@break.BreakType = breakElement.Type;
				@break.TextWrappingRestartLocation = breakElement.TextWrappingRestartLocation;
				this.paragraph.Inlines.Add(@break);
				return;
			}
			case "commentReference":
			{
				CommentReferenceElement commentReferenceElement = (CommentReferenceElement)element;
				Comment comment = context.CommentContext.GetImportedCommentById(commentReferenceElement.Id);
				if (comment != null)
				{
					if (comment.CommentRangeEnd.Paragraph == null)
					{
						this.paragraph.Inlines.Add(comment.CommentRangeEnd);
					}
				}
				else
				{
					comment = CommentContext.CreateComment(context.Document);
					this.paragraph.Inlines.Add(comment.CommentRangeStart);
					this.paragraph.Inlines.Add(comment.CommentRangeEnd);
					context.CommentContext.AddImportedComment(commentReferenceElement.Id, comment);
				}
				context.CommentContext.AddIdToCommentsPartImport(commentReferenceElement.Id);
				return;
			}
			case "pict":
			{
				VmlContainerElement vmlContainerElement = (VmlContainerElement)element;
				if (vmlContainerElement.Watermark != null)
				{
					this.watermark = vmlContainerElement.Watermark;
					return;
				}
				if (vmlContainerElement.Image != null)
				{
					this.paragraph.Inlines.Add(vmlContainerElement.Image);
				}
				break;
			}

				return;
			}
		}

		readonly OpenXmlChildElement<RunPropertiesElement> runPropertiesChildElement;

		InlineBase inline;

		Run run;

		Paragraph paragraph;

		Watermark watermark;
	}
}
