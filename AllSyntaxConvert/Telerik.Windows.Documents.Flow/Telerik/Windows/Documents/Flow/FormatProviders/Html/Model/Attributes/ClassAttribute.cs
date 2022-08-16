using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes
{
	class ClassAttribute : HtmlAttribute<ClassNamesCollection>
	{
		public ClassAttribute()
			: base("class", false)
		{
			base.Value = new ClassNamesCollection();
		}

		public override bool HasValue
		{
			get
			{
				return base.Value.Count > 0;
			}
		}

		public override string GetValue()
		{
			return string.Join(" ", base.Value.ToEnumerable());
		}

		public override void SetValue(string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			base.Value.AddRange(value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
		}

		public override void ResetValue()
		{
			base.Value.Clear();
		}

		public override bool ShouldExport(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return base.ShouldExport(writer, context) && (context.Settings.StylesExportMode == StylesExportMode.Embedded || context.Settings.StylesExportMode == StylesExportMode.External);
		}

		const string ClassSeparator = " ";
	}
}
