using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class MarkerStyleToStringConverter : IStringConverter<MarkerStyle>
	{
		static MarkerStyleToStringConverter()
		{
			MarkerStyleToStringConverter.RegisterPair("circle", MarkerStyle.Circle);
			MarkerStyleToStringConverter.RegisterPair("dash", MarkerStyle.Dash);
			MarkerStyleToStringConverter.RegisterPair("diamond", MarkerStyle.Diamond);
			MarkerStyleToStringConverter.RegisterPair("dot", MarkerStyle.Dot);
			MarkerStyleToStringConverter.RegisterPair("none", MarkerStyle.None);
			MarkerStyleToStringConverter.RegisterPair("plus", MarkerStyle.Plus);
			MarkerStyleToStringConverter.RegisterPair("square", MarkerStyle.Square);
			MarkerStyleToStringConverter.RegisterPair("star", MarkerStyle.Star);
			MarkerStyleToStringConverter.RegisterPair("triangle", MarkerStyle.Triangle);
			MarkerStyleToStringConverter.RegisterPair("x", MarkerStyle.X);
			MarkerStyleToStringConverter.RegisterPair("auto", MarkerStyle.Auto);
		}

		static void RegisterPair(string name, MarkerStyle value)
		{
			MarkerStyleToStringConverter.stringToMarkerStyle.Add(name, value);
			MarkerStyleToStringConverter.markerStyleToString.Add(value, name);
		}

		public MarkerStyle ConvertFromString(string value)
		{
			if (value == "picture")
			{
				return MarkerStyle.Circle;
			}
			return MarkerStyleToStringConverter.stringToMarkerStyle[value];
		}

		public string ConvertToString(MarkerStyle value)
		{
			return MarkerStyleToStringConverter.markerStyleToString[value];
		}

		static readonly Dictionary<string, MarkerStyle> stringToMarkerStyle = new Dictionary<string, MarkerStyle>();

		static readonly Dictionary<MarkerStyle, string> markerStyleToString = new Dictionary<MarkerStyle, string>();
	}
}
