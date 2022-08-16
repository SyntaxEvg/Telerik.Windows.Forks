using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	[PdfClass(TypeName = "Pattern", SubtypeProperty = "PatternType", SubtypeValue = "1")]
	class TilingPatternOld : PatternOld
	{
		public TilingPatternOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.paintType = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor("PaintType", true), Converters.PdfIntConverter);
			this.tilingType = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor("TilingType", true), Converters.PdfIntConverter);
			this.boundingBox = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("BBox", true));
			this.xStep = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor("XStep", true), Converters.PdfRealConverter);
			this.yStep = base.CreateInstantLoadProperty<PdfRealOld>(new PdfPropertyDescriptor("YStep", true), Converters.PdfRealConverter);
			this.resources = base.CreateInstantLoadProperty<PdfResourceOld>(new PdfPropertyDescriptor("Resources", true), Converters.PdfDictionaryToPdfObjectConverter);
		}

		public PdfIntOld PaintType
		{
			get
			{
				return this.paintType.GetValue();
			}
			set
			{
				this.paintType.SetValue(value);
			}
		}

		public PdfIntOld TilingType
		{
			get
			{
				return this.tilingType.GetValue();
			}
			set
			{
				this.tilingType.SetValue(value);
			}
		}

		public PdfArrayOld BoundingBox
		{
			get
			{
				return this.boundingBox.GetValue();
			}
			set
			{
				this.boundingBox.SetValue(value);
			}
		}

		public PdfRealOld XStep
		{
			get
			{
				return this.xStep.GetValue();
			}
			set
			{
				this.xStep.SetValue(value);
			}
		}

		public PdfRealOld YStep
		{
			get
			{
				return this.yStep.GetValue();
			}
			set
			{
				this.yStep.SetValue(value);
			}
		}

		public PdfResourceOld Resources
		{
			get
			{
				return this.resources.GetValue();
			}
			set
			{
				this.resources.SetValue(value);
			}
		}

		public byte[] Data { get; set; }

		public Rect Clip
		{
			get
			{
				return this.BoundingBox.ToRect();
			}
		}

		public override void Load(PdfDataStream stream)
		{
			this.Load(stream.Dictionary);
		}

		protected override PatternBrush CreateBrushOverride(Matrix transform, object[] pars)
		{
			byte[] data = base.ContentManager.ReadData(base.Reference);
			ContentStreamParser contentStreamParser;
			switch (this.PaintType.Value)
			{
			case 1:
				contentStreamParser = new ContentStreamParser(base.ContentManager, data, this.Clip, this.Resources, base.Reference);
				break;
			case 2:
				contentStreamParser = new ContentStreamParser(base.ContentManager, data, this.Clip, this.Resources, base.UnderlyingColorSpace.GetBrush(null, pars), base.Reference);
				break;
			default:
				throw new NotSupportedPaintTypeException(this.PaintType.Value);
			}
			Container container = new Container();
			container.Content.AddRange(contentStreamParser.ParseContent());
			PdfElementToFixedElementTranslator.ToSLCoordinates(container);
			return new TileBrush(container, this.BoundingBox.ToRect(), transform);
		}

		readonly InstantLoadProperty<PdfIntOld> paintType;

		readonly InstantLoadProperty<PdfIntOld> tilingType;

		readonly InstantLoadProperty<PdfArrayOld> boundingBox;

		readonly InstantLoadProperty<PdfRealOld> xStep;

		readonly InstantLoadProperty<PdfRealOld> yStep;

		readonly InstantLoadProperty<PdfResourceOld> resources;
	}
}
