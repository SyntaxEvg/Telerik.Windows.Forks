using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text
{
	class TextBasedElement : BodyElementBase, IPhrasingElement
	{
		public TextBasedElement(HtmlContentManager contentManager, string name)
			: base(contentManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
			base.RegisterAttribute(new StyleValueAttribute("dir", "direction", base.Style, null, false, null));
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		protected override bool CanHaveStyle
		{
			get
			{
				return true;
			}
		}

		protected override bool PreserveWhiteSpaces
		{
			get
			{
				return true;
			}
		}

		public void SetAssociatedFlowElement(Run run)
		{
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			this.run = run;
		}

		protected override void PrepareRunOverride(IHtmlImportContext context, Run run)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			base.ApplyStyle(context, run);
			base.CopyLocalPropertiesTo(context, run);
		}

		protected override void OnBeforeWrite(IHtmlWriter reader, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			if (base.ContentManager.ExportListManager.IsInList)
			{
				foreach (IStyleProperty styleProperty in base.ContentManager.ExportListManager.ExportedCharacterProperties)
				{
					IStyleProperty styleProperty2 = this.run.Properties.GetStyleProperty(styleProperty.PropertyDefinition);
					if (styleProperty2.GetActualValueAsObject() != styleProperty.GetActualValueAsObject())
					{
						base.LocalProperties.CopyFrom(context, styleProperty2, this.run.Properties, false);
					}
				}
			}
			base.CopyStyleFrom(context, this.run);
			base.CopyLocalPropertiesFrom(context, this.run);
			base.InnerText = this.run.Text;
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.run = null;
		}

		readonly string name;

		Run run;
	}
}
