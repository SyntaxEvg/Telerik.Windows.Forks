using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Destinations
{
	static class DestinationTypes
	{
		static DestinationTypes()
		{
			DestinationTypes.InitializeMappings();
		}

		public static string GetDestinationType(DestinationType type)
		{
			return DestinationTypes.destinationTypeToString[type];
		}

		public static DestinationType GetDestinationType(string type)
		{
			return DestinationTypes.stringToDestinationType[type];
		}

		static void InitializeMappings()
		{
			DestinationTypes.RegisterMapping("XYZ", DestinationType.Location);
			DestinationTypes.RegisterMapping("Fit", DestinationType.PageFit);
			DestinationTypes.RegisterMapping("FitH", DestinationType.PageHorizontalFit);
			DestinationTypes.RegisterMapping("FitV", DestinationType.PageVerticalFit);
			DestinationTypes.RegisterMapping("FitR", DestinationType.RectangleFit);
			DestinationTypes.RegisterMapping("FitB", DestinationType.BoundingRectangleFit);
			DestinationTypes.RegisterMapping("FitBH", DestinationType.BoundingRectangleHorizontalFit);
			DestinationTypes.RegisterMapping("FitBV", DestinationType.BoundingRectangleVerticalFit);
		}

		static void RegisterMapping(string destinationTypeName, DestinationType destinationType)
		{
			DestinationTypes.stringToDestinationType[destinationTypeName] = destinationType;
			DestinationTypes.destinationTypeToString[destinationType] = destinationTypeName;
		}

		public const string Location = "XYZ";

		public const string FitToPage = "Fit";

		public const string FitHorizontalToPage = "FitH";

		public const string FitVerticalToPage = "FitV";

		public const string FitToRect = "FitR";

		public const string FitToBoundingRect = "FitB";

		public const string FitHorizontalToBoundingRect = "FitBH";

		public const string FitVerticalToBoundingRect = "FitBV";

		static readonly Dictionary<string, DestinationType> stringToDestinationType = new Dictionary<string, DestinationType>();

		static readonly Dictionary<DestinationType, string> destinationTypeToString = new Dictionary<DestinationType, string>();
	}
}
