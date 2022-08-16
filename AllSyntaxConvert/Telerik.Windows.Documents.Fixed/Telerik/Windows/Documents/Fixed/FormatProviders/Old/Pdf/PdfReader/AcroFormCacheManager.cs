using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader
{
	class AcroFormCacheManager
	{
		public AcroFormCacheManager(PdfImportSettings importSettings)
		{
			this.widgetsMapping = new Dictionary<WidgetOld, Widget>();
			this.fieldsAndWidgetImportContext = new FieldsAndWidgetsPropertiesImportContext(importSettings);
		}

		public FieldsAndWidgetsPropertiesImportContext FieldsAndWidgetsImportContext
		{
			get
			{
				return this.fieldsAndWidgetImportContext;
			}
		}

		public int BiggestObjectId
		{
			get
			{
				if (this.currentBiggestImportId != null)
				{
					return this.currentBiggestImportId.Value;
				}
				return this.fieldsAndWidgetImportContext.CrossReferences.MaxObjectNumber;
			}
			set
			{
				this.currentBiggestImportId = new int?(value);
			}
		}

		public bool TryGetWidgetFromWidgetObject(WidgetOld widgetObject, out Widget widget)
		{
			return this.widgetsMapping.TryGetValue(widgetObject, out widget);
		}

		public void MapWidget(WidgetOld widgetObject, Widget widget)
		{
			this.widgetsMapping.Add(widgetObject, widget);
		}

		readonly Dictionary<WidgetOld, Widget> widgetsMapping;

		readonly FieldsAndWidgetsPropertiesImportContext fieldsAndWidgetImportContext;

		int? currentBiggestImportId;
	}
}
