using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Txt
{
	public class TxtFormatProvider : TextBasedFormatProviderBase<RadFlowDocument>
	{
		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return TxtFormatProvider.SupportedExtensionsList;
			}
		}

		public override bool CanImport
		{
			get
			{
				return true;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		protected override RadFlowDocument ImportOverride(Stream input)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			RadFlowDocumentEditor radFlowDocumentEditor = new RadFlowDocumentEditor(new RadFlowDocument());
			using (StreamReader streamReader = new StreamReader(input))
			{
				radFlowDocumentEditor.InsertText(streamReader.ReadToEnd());
			}
			return radFlowDocumentEditor.Document;
		}

		protected override void ExportOverride(RadFlowDocument document, Stream output)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			using (new RadFlowDocumentLicenseCheck(document))
			{
				StreamWriter streamWriter = new StreamWriter(output);
				bool flag = true;
				foreach (Paragraph paragraph in document.EnumerateChildrenOfType<Paragraph>())
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						streamWriter.WriteLine();
					}
					foreach (InlineBase inlineBase in from inline in paragraph.Inlines
						where inline is Run
						select inline)
					{
						Run run = (Run)inlineBase;
						streamWriter.Write(run.Text);
					}
				}
				streamWriter.Flush();
			}
		}

		static readonly IEnumerable<string> SupportedExtensionsList = new string[] { ".txt" };
	}
}
