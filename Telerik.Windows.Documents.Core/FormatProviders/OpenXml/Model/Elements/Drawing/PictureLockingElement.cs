using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class PictureLockingElement : DrawingElementBase
	{
		public PictureLockingElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.noChangeAspect = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("noChangeAspect", false));
		}

		public override string ElementName
		{
			get
			{
				return "picLocks";
			}
		}

		protected override bool ShouldExport(IOpenXmlExportContext context)
		{
			return this.NoChangeAspect == 1;
		}

		public int NoChangeAspect
		{
			get
			{
				return this.noChangeAspect.Value;
			}
			set
			{
				this.noChangeAspect.Value = value;
			}
		}

		readonly IntOpenXmlAttribute noChangeAspect;
	}
}
