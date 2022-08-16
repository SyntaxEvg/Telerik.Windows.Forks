using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class GeometryRenderingContext : ContentRenderingContext
	{
		internal GeometryRenderingContext(Path path, Rect currentContainerBounds)
			: base(path, currentContainerBounds)
		{
			this.geometry = path.Geometry;
			this.properties = path.GeometryProperties;
		}

		public GeometryBase Geometry
		{
			get
			{
				return this.geometry;
			}
		}

		public GeometryPropertiesOwner Properties
		{
			get
			{
				return this.properties;
			}
		}

		readonly GeometryBase geometry;

		readonly GeometryPropertiesOwner properties;
	}
}
