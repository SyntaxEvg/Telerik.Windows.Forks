using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements
{
	abstract class OpenXmlElementBase<TImportContext, TExportContext, TPartsManager> : OpenXmlElementBase where TImportContext : IOpenXmlImportContext where TExportContext : IOpenXmlExportContext where TPartsManager : OpenXmlPartsManager
	{
		public OpenXmlElementBase(TPartsManager partsManager)
			: base(partsManager)
		{
		}

		public new TPartsManager PartsManager
		{
			get
			{
				return (TPartsManager)((object)base.PartsManager);
			}
		}

		protected sealed override bool ShouldExport(IOpenXmlExportContext context)
		{
			return this.ShouldExport((TExportContext)((object)context));
		}

		protected sealed override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			return this.EnumerateChildElements((TExportContext)((object)context));
		}

		protected sealed override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			this.OnBeforeWrite((TExportContext)((object)context));
		}

		protected sealed override bool ShouldImport(IOpenXmlImportContext context)
		{
			return this.ShouldImport((TImportContext)((object)context));
		}

		protected sealed override void OnAfterReadAttributes(IOpenXmlImportContext context)
		{
			this.OnAfterReadAttributes((TImportContext)((object)context));
		}

		protected sealed override void OnBeforeReadInnerElements(IOpenXmlImportContext context)
		{
			this.OnBeforeReadInnerElements((TImportContext)((object)context));
		}

		protected sealed override void OnBeforeReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			this.OnBeforeReadChildElement((TImportContext)((object)context), childElement);
		}

		protected sealed override void OnAfterReadInnerElements(IOpenXmlImportContext context)
		{
			this.OnAfterReadInnerElements((TImportContext)((object)context));
		}

		protected sealed override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			this.OnAfterReadChildElement((TImportContext)((object)context), childElement);
		}

		protected sealed override void OnAfterRead(IOpenXmlImportContext context)
		{
			this.OnAfterRead((TImportContext)((object)context));
		}

		protected virtual bool ShouldExport(TExportContext context)
		{
			return base.ShouldExport(context);
		}

		protected virtual IEnumerable<OpenXmlElementBase> EnumerateChildElements(TExportContext context)
		{
			return Enumerable.Empty<OpenXmlElementBase>();
		}

		protected virtual void OnBeforeWrite(TExportContext context)
		{
		}

		protected virtual bool ShouldImport(TImportContext context)
		{
			return !context.IsImportSuspended;
		}

		protected virtual void OnAfterReadAttributes(TImportContext context)
		{
		}

		protected virtual void OnBeforeReadInnerElements(TImportContext context)
		{
		}

		protected virtual void OnBeforeReadChildElement(TImportContext context, OpenXmlElementBase childElement)
		{
		}

		protected virtual void OnAfterReadInnerElements(TImportContext context)
		{
		}

		protected virtual void OnAfterReadChildElement(TImportContext context, OpenXmlElementBase childElement)
		{
		}

		protected virtual void OnAfterRead(TImportContext context)
		{
		}
	}
}
