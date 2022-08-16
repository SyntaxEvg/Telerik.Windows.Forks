using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	static class ContentElementWriters
	{
		static ContentElementWriters()
		{
			ContentElementWriters.StrokeColorWriter = new StrokeColorWriter();
			ContentElementWriters.ContainerWriter = new ContainerWriter();
			ContentElementWriters.TextPropertiesWriter = new TextPropertiesWriter();
			ContentElementWriters.GeometryPropertiesWriter = new GeometryPropertiesWriter();
			ContentElementWriters.RegisterDefaultElementWriters();
		}

		static void RegisterDefaultElementWriters()
		{
			ContentElementWriters.RegisterElementWriter(typeof(Path), ContentElementWriters.PathWriter);
			ContentElementWriters.RegisterElementWriter(typeof(RgbColor), ContentElementWriters.ColorWriter);
			ContentElementWriters.RegisterElementWriter(typeof(Clipping), ContentElementWriters.ClippingWriter);
			ContentElementWriters.RegisterElementWriter(typeof(PathGeometry), ContentElementWriters.GeometryWriter);
			ContentElementWriters.RegisterElementWriter(typeof(RectangleGeometry), ContentElementWriters.GeometryWriter);
			ContentElementWriters.RegisterElementWriter(typeof(BezierSegment), ContentElementWriters.SegmentWriter);
			ContentElementWriters.RegisterElementWriter(typeof(LineSegment), ContentElementWriters.SegmentWriter);
			ContentElementWriters.RegisterElementWriter(typeof(TextFragment), ContentElementWriters.TextFragmentWriter);
			ContentElementWriters.RegisterElementWriter(typeof(RadFixedPage), ContentElementWriters.ContentRootWriter);
			ContentElementWriters.RegisterElementWriter(typeof(FormSource), ContentElementWriters.ContentRootWriter);
			ContentElementWriters.RegisterElementWriter(typeof(Image), ContentElementWriters.ImageWriter);
			ContentElementWriters.RegisterElementWriter(typeof(Form), ContentElementWriters.FormWriter);
			ContentElementWriters.RegisterElementWriter(typeof(Tiling), ContentElementWriters.ContainerWriter);
			ContentElementWriters.RegisterElementWriter(typeof(UncoloredTiling), ContentElementWriters.ContainerWriter);
			ContentElementWriters.RegisterElementWriter(typeof(TextPropertiesOwner), ContentElementWriters.TextPropertiesWriter);
			ContentElementWriters.RegisterElementWriter(typeof(GeometryPropertiesOwner), ContentElementWriters.GeometryPropertiesWriter);
		}

		public static bool TryGetWriter(object element, out ContentElementWriterBase writer)
		{
			return ContentElementWriters.elementTypeToElementWriter.TryGetValue(element.GetType(), out writer);
		}

		static void RegisterElementWriter(Type type, ContentElementWriterBase writer)
		{
			ContentElementWriters.elementTypeToElementWriter.Add(type, writer);
		}

		static readonly Dictionary<Type, ContentElementWriterBase> elementTypeToElementWriter = new Dictionary<Type, ContentElementWriterBase>();

		internal static readonly PathWriter PathWriter = new PathWriter();

		internal static readonly ColorWriterBase ColorWriter = new ColorWriter();

		internal static readonly ColorWriterBase StrokeColorWriter;

		internal static readonly SegmentWriter SegmentWriter = new SegmentWriter();

		internal static readonly TextFragmentWriter TextFragmentWriter = new TextFragmentWriter();

		internal static readonly ContentRootWriter ContentRootWriter = new ContentRootWriter();

		internal static readonly ImageWriter ImageWriter = new ImageWriter();

		internal static readonly FormWriter FormWriter = new FormWriter();

		internal static readonly GeometryWriterBase GeometryWriter = new GeometryWriterBase();

		internal static readonly ClippingGeometryWriter ClippingGeometryWriter = new ClippingGeometryWriter();

		internal static readonly ClippingWriter ClippingWriter = new ClippingWriter();

		internal static readonly ContainerWriter ContainerWriter;

		internal static readonly TextPropertiesWriter TextPropertiesWriter;

		internal static readonly GeometryPropertiesWriter GeometryPropertiesWriter;
	}
}
