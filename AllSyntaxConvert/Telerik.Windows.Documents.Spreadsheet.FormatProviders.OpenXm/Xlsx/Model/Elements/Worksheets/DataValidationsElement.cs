using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class DataValidationsElement : WorksheetElementBase
	{
		public DataValidationsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("count", 0, false));
		}

		public int Count
		{
			get
			{
				return this.count.Value;
			}
			set
			{
				this.count.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "dataValidations";
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return this.Count > 0;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			this.Count = context.GetDataValidationsCount();
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (IDataValidationRule rule in context.GetDataValidationRules())
			{
				DataValidationElement dataValidationElement = base.CreateElement<DataValidationElement>("dataValidation");
				dataValidationElement.CopyPropertiesFrom(context, rule);
				yield return dataValidationElement;
			}
			yield break;
		}

		readonly IntOpenXmlAttribute count;
	}
}
