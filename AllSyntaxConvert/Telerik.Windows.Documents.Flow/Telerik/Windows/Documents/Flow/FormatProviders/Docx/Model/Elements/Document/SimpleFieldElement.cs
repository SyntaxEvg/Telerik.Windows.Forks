using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class SimpleFieldElement : ParagraphContentElementBase
	{
		public SimpleFieldElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.codeAttribute = base.RegisterAttribute<string>("instr", OpenXmlNamespaces.WordprocessingMLNamespace, true);
			this.isLocked = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("fldLock", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.isDirty = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("dirty", OpenXmlNamespaces.WordprocessingMLNamespace, false));
		}

		public override string ElementName
		{
			get
			{
				return "fldSimple";
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			string text = string.Empty;
			if (this.codeAttribute.HasValue)
			{
				text = this.codeAttribute.Value;
			}
			FieldInfo fieldInfo = new FieldInfo(context.Document);
			fieldInfo.IsLocked = this.isLocked.Value;
			fieldInfo.IsDirty = this.isDirty.Value;
			base.Paragraph.Inlines.Add(fieldInfo.Start);
			base.Paragraph.Inlines.AddRun(text);
			base.Paragraph.Inlines.Add(fieldInfo.Separator);
			base.MoveInlinesToParagraph();
			base.Paragraph.Inlines.Add(fieldInfo.End);
			fieldInfo.LoadFieldFromCode();
		}

		readonly OpenXmlAttribute<string> codeAttribute;

		readonly BoolOpenXmlAttribute isLocked;

		readonly BoolOpenXmlAttribute isDirty;
	}
}
