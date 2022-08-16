using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public abstract class Destination : FixedDocumentElementBase, IContextClonable<Destination>
	{
		public RadFixedPage Page { get; set; }

		internal abstract DestinationType DestinationType { get; }

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.Destination;
			}
		}

		internal static Size GetScale(Size actualSize, Size sizeToFitIn)
		{
			double scale = Destination.GetScale(actualSize.Width, sizeToFitIn.Width);
			double scale2 = Destination.GetScale(actualSize.Height, sizeToFitIn.Height);
			return new Size(scale, scale2);
		}

		internal static double GetScale(double actualSize, double sizeToFitIn)
		{
			return sizeToFitIn / actualSize;
		}

		internal static Destination CreateDestination(DestinationType type, RadFixedPage page)
		{
			switch (type)
			{
			case DestinationType.Location:
				return new Location
				{
					Page = page
				};
			case DestinationType.PageFit:
				return new PageFit
				{
					Page = page
				};
			case DestinationType.PageHorizontalFit:
				return new PageHorizontalFit
				{
					Page = page
				};
			case DestinationType.PageVerticalFit:
				return new PageVerticalFit
				{
					Page = page
				};
			case DestinationType.RectangleFit:
				return new RectangleFit
				{
					Page = page
				};
			case DestinationType.BoundingRectangleFit:
				return new BoundingRectangleFit
				{
					Page = page
				};
			case DestinationType.BoundingRectangleHorizontalFit:
				return new BoundingRectangleHorizontalFit
				{
					Page = page
				};
			case DestinationType.BoundingRectangleVerticalFit:
				return new BoundingRectangleVerticalFit
				{
					Page = page
				};
			default:
				throw new NotSupportedException(string.Format("{0} is not supported destination type.", type.ToString()));
			}
		}

		internal virtual bool TryGetHorizontalOffsetOnPage(Size viewportSize, out double offset)
		{
			offset = 0.0;
			return false;
		}

		internal virtual bool TryGetVerticalOffsetOnPage(Size viewportSize, out double offset)
		{
			offset = 0.0;
			return false;
		}

		internal virtual bool TryGetScaleFactor(Size viewportSize, out double scaleFactor)
		{
			scaleFactor = 0.0;
			return false;
		}

		Destination IContextClonable<Destination>.Clone(RadFixedDocumentCloneContext cloneContext)
		{
			Destination destination = this.CreateClonedInstance();
			destination.Page = cloneContext.GetClonedPage(this.Page);
			return destination;
		}

		internal abstract Destination CreateClonedInstance();
	}
}
