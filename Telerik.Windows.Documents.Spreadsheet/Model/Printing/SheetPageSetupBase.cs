using System;
using System.Windows;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public abstract class SheetPageSetupBase
	{
		public PaperTypes PaperType
		{
			get
			{
				return this.paperType;
			}
			set
			{
				if (this.paperType != value)
				{
					this.paperType = value;
					this.OnPaperTypeChanged();
				}
			}
		}

		public PageOrientation PageOrientation
		{
			get
			{
				return this.pageOrientation;
			}
			set
			{
				if (this.pageOrientation != value)
				{
					this.pageOrientation = value;
					this.OnPageOrientationChanged();
				}
			}
		}

		public Size PageSize
		{
			get
			{
				return this.pageSize;
			}
		}

		internal Size RotatedPageSize
		{
			get
			{
				return this.rotatedPageSize;
			}
			set
			{
				this.rotatedPageSize = value;
			}
		}

		internal int? FirstPageNumber
		{
			get
			{
				return this.firstPageNumber;
			}
			set
			{
				if (this.firstPageNumber != value)
				{
					if (value != null)
					{
						Guard.ThrowExceptionIfOutOfRange<int>(-32765, 32767, value.Value, "firstPageNumber");
					}
					this.firstPageNumber = value;
					this.OnPageSetupChanged();
				}
			}
		}

		public PageMargins Margins
		{
			get
			{
				return this.pageMargins;
			}
			set
			{
				if (!TelerikHelper.EqualsOfT<PageMargins>(this.pageMargins, value))
				{
					this.pageMargins = value;
					this.OnPageSetupChanged();
				}
			}
		}

		public HeaderFooterSettings HeaderFooterSettings
		{
			get
			{
				return this.headerFooterSettings;
			}
		}

		internal SheetPageSetupBase(Sheet sheet)
		{
			this.headerFooterSettings = new HeaderFooterSettings(new Action(this.OnPageSetupChanged));
			this.paperType = PaperTypes.Letter;
			this.pageSize = PaperTypeConverter.ToSize(this.PaperType);
			this.PageOrientation = PageOrientation.Portrait;
			this.rotatedPageSize = ((this.PageOrientation == PageOrientation.Portrait) ? this.PageSize : new Size(this.PageSize.Height, this.PageSize.Width));
			this.Margins = PageMargins.NormalMargins;
			this.sheet = sheet;
			this.beginUpdateCounter = 0;
		}

		void OnPaperTypeChanged()
		{
			this.pageSize = PaperTypeConverter.ToSize(this.PaperType);
			this.UpdateRotatedPageSize();
			this.OnPageSetupChanged();
		}

		void OnPageOrientationChanged()
		{
			this.UpdateRotatedPageSize();
			this.OnPageSetupChanged();
		}

		void UpdateRotatedPageSize()
		{
			this.RotatedPageSize = SheetPageSetupBase.CalculateRotatedPageSize(this.PageSize, this.PageOrientation);
		}

		protected void InvalidateSheetLayout()
		{
			this.sheet.InvalidateLayout();
		}

		protected void SuspendSheetLayoutUpdate()
		{
			this.sheet.SuspendLayoutUpdate();
		}

		protected void ResumeSheetLayoutUpdate()
		{
			this.sheet.ResumeLayoutUpdate();
		}

		internal static Size CalculateRotatedPageSize(PaperTypes paperType, PageOrientation pageOrientation)
		{
			return SheetPageSetupBase.CalculateRotatedPageSize(PaperTypeConverter.ToSize(paperType), pageOrientation);
		}

		internal static Size CalculateRotatedPageSize(Size pageSize, PageOrientation pageOrientation)
		{
			if (pageOrientation != PageOrientation.Portrait)
			{
				return new Size(pageSize.Height, pageSize.Width);
			}
			return pageSize;
		}

		internal void BeginUpdate()
		{
			this.beginUpdateCounter++;
		}

		internal void EndUpdate()
		{
			this.beginUpdateCounter--;
			if (this.beginUpdateCounter < 0)
			{
				throw new InvalidOperationException("There is no active update to end.");
			}
			if (this.beginUpdateCounter == 0 && this.shouldFirePageSetupChanged)
			{
				this.OnPageSetupChanged();
			}
		}

		internal virtual void CopyFrom(SheetPageSetupBase fromSheetPageSetupBase)
		{
			this.paperType = fromSheetPageSetupBase.PaperType;
			this.pageOrientation = fromSheetPageSetupBase.PageOrientation;
			this.pageSize = fromSheetPageSetupBase.PageSize;
			this.headerFooterSettings.CopyFrom(fromSheetPageSetupBase.HeaderFooterSettings);
		}

		internal event EventHandler PageSetupChanged;

		internal void OnPageSetupChanged()
		{
			this.shouldFirePageSetupChanged = true;
			if (this.beginUpdateCounter == 0 && this.PageSetupChanged != null)
			{
				this.PageSetupChanged(this, EventArgs.Empty);
				this.shouldFirePageSetupChanged = false;
			}
		}

		readonly HeaderFooterSettings headerFooterSettings;

		PaperTypes paperType;

		PageOrientation pageOrientation;

		Size pageSize;

		Size rotatedPageSize;

		int? firstPageNumber;

		readonly ISheet sheet;

		PageMargins pageMargins;

		int beginUpdateCounter;

		bool shouldFirePageSetupChanged;
	}
}
