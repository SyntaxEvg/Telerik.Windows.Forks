using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot
{
	[PdfClass]
	class AppearanceOld : PdfObjectOld
	{
		public AppearanceOld(PdfContentManager contentManager, XForm singleStateAppearance)
			: base(contentManager)
		{
			Guard.ThrowExceptionIfNull<XForm>(singleStateAppearance, "singleStateAppearance");
			this.singleStateAppearance = singleStateAppearance;
		}

		public AppearanceOld(PdfContentManager contentManager, Dictionary<string, XForm> stateToAppearanceMapping)
			: base(contentManager)
		{
			Guard.ThrowExceptionIfNull<Dictionary<string, XForm>>(stateToAppearanceMapping, "stateToAppearanceMapping");
			this.stateToAppearanceMapping = stateToAppearanceMapping;
		}

		public XForm SingleStateAppearance
		{
			get
			{
				return this.singleStateAppearance;
			}
		}

		public IEnumerable<string> States
		{
			get
			{
				return this.stateToAppearanceMapping.Keys;
			}
		}

		public bool TryGetStateAppearance(string state, out XForm appearance)
		{
			return this.stateToAppearanceMapping.TryGetValue(state, out appearance);
		}

		readonly XForm singleStateAppearance;

		readonly Dictionary<string, XForm> stateToAppearanceMapping;
	}
}
