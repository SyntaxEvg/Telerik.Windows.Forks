using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	abstract class CachedColorSpaceOld : ColorSpaceOld
	{
		public CachedColorSpaceOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.cache = new ArrayCache<Color>();
		}

		public override Color GetColor(object[] pars)
		{
			Color colorOverride;
			if (this.cache.TryGetValue(pars, out colorOverride))
			{
				return colorOverride;
			}
			colorOverride = this.GetColorOverride(pars);
			this.cache.AddToCache(pars, colorOverride);
			return colorOverride;
		}

		public override void Clear()
		{
			this.cache.Clear();
		}

		protected abstract Color GetColorOverride(object[] pars);

		readonly ArrayCache<Color> cache;
	}
}
