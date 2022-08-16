using System;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public sealed class DestinationCollection : DocumentElementCollection<Destination, IDestinationContainer>
	{
		public DestinationCollection(IDestinationContainer parent)
			: base(parent)
		{
		}

		public Location AddLocation()
		{
			Location location = new Location();
			base.Add(location);
			return location;
		}

		public PageFit AddPageFit()
		{
			PageFit pageFit = new PageFit();
			base.Add(pageFit);
			return pageFit;
		}

		public BoundingRectangleFit AddBoundingRectangleFit()
		{
			BoundingRectangleFit boundingRectangleFit = new BoundingRectangleFit();
			base.Add(boundingRectangleFit);
			return boundingRectangleFit;
		}

		public BoundingRectangleHorizontalFit AddBoundingRectangleHorizontalFit()
		{
			BoundingRectangleHorizontalFit boundingRectangleHorizontalFit = new BoundingRectangleHorizontalFit();
			base.Add(boundingRectangleHorizontalFit);
			return boundingRectangleHorizontalFit;
		}

		public BoundingRectangleVerticalFit AddBoundingRectangleVerticalFit()
		{
			BoundingRectangleVerticalFit boundingRectangleVerticalFit = new BoundingRectangleVerticalFit();
			base.Add(boundingRectangleVerticalFit);
			return boundingRectangleVerticalFit;
		}

		public PageVerticalFit AddPageVerticalFit()
		{
			PageVerticalFit pageVerticalFit = new PageVerticalFit();
			base.Add(pageVerticalFit);
			return pageVerticalFit;
		}

		public PageHorizontalFit AddPageHorizontalFit()
		{
			PageHorizontalFit pageHorizontalFit = new PageHorizontalFit();
			base.Add(pageHorizontalFit);
			return pageHorizontalFit;
		}

		public RectangleFit AddRectangleFit()
		{
			RectangleFit rectangleFit = new RectangleFit();
			base.Add(rectangleFit);
			return rectangleFit;
		}
	}
}
