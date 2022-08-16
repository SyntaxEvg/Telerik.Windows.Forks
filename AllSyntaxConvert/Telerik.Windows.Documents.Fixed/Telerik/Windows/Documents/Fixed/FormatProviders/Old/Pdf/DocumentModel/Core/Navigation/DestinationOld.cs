using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation
{
	class DestinationOld : PdfObjectOld
	{
		public DestinationOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public PageOld Page { get; set; }

		public DestinationTypeOld Type { get; set; }

		public double? Left { get; set; }

		public double? Top { get; set; }

		public double? Right { get; set; }

		public double? Bottom { get; set; }

		public double? Zoom { get; set; }

		public void Load(PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			this.LoadPage(array);
			PdfNameOld element = array.GetElement<PdfNameOld>(1);
			if (element != null)
			{
				DestinationTypeOld type;
				Helper.EnumTryParse<DestinationTypeOld>(element.ToString(), out type);
				this.Type = type;
				switch (this.Type)
				{
				case DestinationTypeOld.XYZ:
					this.LoadXYZ(array);
					break;
				case DestinationTypeOld.Fit:
					this.LoadFit(array);
					break;
				case DestinationTypeOld.FitH:
					this.LoadFitH(array);
					break;
				case DestinationTypeOld.FitV:
					this.LoadFitV(array);
					break;
				case DestinationTypeOld.FitR:
					this.LoadFitR(array);
					break;
				case DestinationTypeOld.FitB:
					this.LoadFitB(array);
					break;
				case DestinationTypeOld.FitBH:
					this.LoadFitBH(array);
					break;
				case DestinationTypeOld.FitBV:
					this.LoadFitBV(array);
					break;
				}
				base.IsLoaded = true;
			}
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			if (indirectObject.Value is PdfArrayOld)
			{
				this.Load((PdfArrayOld)indirectObject.Value);
			}
			base.Load(indirectObject);
		}

		void LoadPage(PdfArrayOld array)
		{
			IndirectReferenceOld indirectReferenceOld = array.First<object>() as IndirectReferenceOld;
			IndirectObjectOld indirectObjectOld;
			if (indirectReferenceOld != null && !base.ContentManager.TryGetIndirectObject(indirectReferenceOld, out indirectObjectOld))
			{
				this.Page = null;
				return;
			}
			this.Page = array.GetElement<PageOld>(0);
			if (this.Page == null)
			{
				PdfSimpleTypeOld<int> element = array.GetElement<PdfSimpleTypeOld<int>>(0);
				if (element != null)
				{
					int value = element.Value;
					if (0 <= value && value < base.ContentManager.DocumentCatalog.Pages.Count.Value)
					{
						this.Page = base.ContentManager.GetPages().Skip(value).FirstOrDefault<PageOld>();
					}
				}
			}
		}

		void ClearValues()
		{
			this.Left = null;
			this.Right = null;
			this.Top = null;
			this.Bottom = null;
			this.Zoom = null;
		}

		void LoadXYZ(PdfArrayOld array)
		{
			this.ClearValues();
			double value;
			if (array.TryGetReal(2, out value))
			{
				this.Left = new double?(value);
			}
			if (array.TryGetReal(3, out value))
			{
				this.Top = new double?(value);
			}
			if (array.TryGetReal(4, out value))
			{
				this.Zoom = new double?(value);
			}
		}

		void LoadFit(PdfArrayOld array)
		{
			this.ClearValues();
		}

		void LoadFitH(PdfArrayOld array)
		{
			this.ClearValues();
			double value;
			if (array.TryGetReal(2, out value))
			{
				this.Top = new double?(value);
			}
		}

		void LoadFitV(PdfArrayOld array)
		{
			this.ClearValues();
			double value;
			if (array.TryGetReal(2, out value))
			{
				this.Left = new double?(value);
			}
		}

		void LoadFitR(PdfArrayOld array)
		{
			this.ClearValues();
			double value;
			if (array.TryGetReal(2, out value))
			{
				this.Left = new double?(value);
			}
			if (array.TryGetReal(3, out value))
			{
				this.Bottom = new double?(value);
			}
			if (array.TryGetReal(4, out value))
			{
				this.Right = new double?(value);
			}
			if (array.TryGetReal(5, out value))
			{
				this.Top = new double?(value);
			}
		}

		void LoadFitB(PdfArrayOld array)
		{
			this.ClearValues();
		}

		void LoadFitBH(PdfArrayOld array)
		{
			this.ClearValues();
			double value;
			if (array.TryGetReal(2, out value))
			{
				this.Top = new double?(value);
			}
		}

		void LoadFitBV(PdfArrayOld array)
		{
			this.ClearValues();
			double value;
			if (array.TryGetReal(2, out value))
			{
				this.Left = new double?(value);
			}
		}
	}
}
