using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class ContentStreamInterpreter : InterpreterBase
	{
		public ContentStreamInterpreter(Stream stream, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			this.context = context;
			this.graphicsStateStack = new Stack<GraphicsState>();
			this.textStateStack = new Stack<TextState>();
			this.operands = new OperandsCollection();
			this.PathFigures = new Telerik.Windows.Documents.Fixed.Model.Collections.PathFigureCollection();
			this.reader = new PostScriptReader(stream, new ContentStreamKeywordCollection());
		}

		public OperandsCollection Operands
		{
			get
			{
				return this.operands;
			}
		}

		public Matrix TextRenderingMatrix
		{
			get
			{
				Matrix textMatrix = this.TextState.TextMatrix;
				Matrix matrix = this.GraphicState.Matrix;
				return textMatrix.MultiplyBy(matrix);
			}
		}

		public GraphicsState GraphicState
		{
			get
			{
				return this.graphicsStateStack.Peek();
			}
		}

		public TextState TextState
		{
			get
			{
				return this.textStateStack.Peek();
			}
		}

		public PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public Telerik.Windows.Documents.Fixed.Model.Graphics.PathFigure CurrentPathFigure { get; set; }

		public Telerik.Windows.Documents.Fixed.Model.Collections.PathFigureCollection PathFigures { get; set; }

		public void Execute()
		{
			foreach (PdfPrimitive pdfPrimitive in this.ReadContentPrimitives())
			{
				if (pdfPrimitive != null)
				{
					if (pdfPrimitive.Type == PdfElementType.Operator)
					{
						ContentStreamOperator contentStreamOperator = (ContentStreamOperator)pdfPrimitive;
						contentStreamOperator.Execute(this, this.context);
					}
					else
					{
						this.Operands.AddLast(pdfPrimitive);
					}
				}
			}
		}

		public void SaveGraphicState()
		{
			this.graphicsStateStack.Push(new GraphicsState(this.GraphicState));
			this.textStateStack.Push(new TextState(this.TextState));
		}

		public void RestoreGraphicState()
		{
			this.graphicsStateStack.Pop();
			this.textStateStack.Pop();
		}

		public GeometryBase CalculateGeometry()
		{
			Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry pathGeometry = new Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry();
			foreach (Telerik.Windows.Documents.Fixed.Model.Graphics.PathFigure pathFigure in this.PathFigures)
			{
				if (pathFigure.Segments.Count != 0)
				{
					pathGeometry.Figures.Add(pathFigure);
				}
			}
			return pathGeometry;
		}

		public void ApplyTextProperties(TextPropertiesOwner textProperties)
		{
			if (this.TextState.Font == null)
			{
				textProperties.Font = FontsRepository.Helvetica;
			}
			else
			{
				textProperties.Font = this.context.Owner.GetFont(this.Reader, this.TextState.Font);
			}
			textProperties.CharacterSpacing = this.TextState.CharacterSpacing;
			textProperties.WordSpacing = this.TextState.WordSpacing;
			textProperties.TextRise = this.TextState.TextRise;
			textProperties.FontSize = this.TextState.FontSize;
			textProperties.HorizontalScaling = this.TextState.HorizontalScaling;
			textProperties.RenderingMode = this.TextState.RenderingMode;
			textProperties.TextMatrix = this.TextState.TextMatrix;
			this.ApplyGeometryProperties(textProperties.GeometryProperties);
		}

		public void ApplyGeometryProperties(GeometryPropertiesOwner geometryProperties)
		{
			if (geometryProperties.IsFilled)
			{
				geometryProperties.Fill = this.GraphicState.CalculateFillColor(this.Reader, this.context);
			}
			if (geometryProperties.IsStroked)
			{
				geometryProperties.Stroke = this.GraphicState.CalculateStrokeColor(this.Reader, this.context);
				geometryProperties.StrokeLineCap = this.GraphicState.StrokeLineCap;
				geometryProperties.StrokeLineJoin = this.GraphicState.StrokeLineJoin;
				geometryProperties.StrokeThickness = this.GraphicState.StrokeLineWidth;
				geometryProperties.StrokeDashArray = this.GraphicState.StrokeDashArray;
				geometryProperties.StrokeDashOffset = this.GraphicState.StrokeDashOffset;
				geometryProperties.MiterLimit = this.GraphicState.MiterLimit;
			}
		}

		public void ClearGeometry()
		{
			this.PathFigures.Clear();
			this.CurrentPathFigure = null;
		}

		public void EnsurePathFigure()
		{
			if (this.CurrentPathFigure == null)
			{
				this.CurrentPathFigure = this.PathFigures.AddPathFigure();
			}
		}

		PdfPrimitive[] ReadContentPrimitives()
		{
			PdfPrimitive[] result;
			using (this.context.Owner.BeginImportOfStreamInnerContent())
			{
				this.Reader.Reader.Seek(0L, SeekOrigin.Begin);
				this.graphicsStateStack.Push(new GraphicsState());
				this.textStateStack.Push(new TextState());
				PdfPrimitive[] array = this.Reader.Read(this.context.Owner);
				result = array;
			}
			return result;
		}

		readonly Stack<GraphicsState> graphicsStateStack;

		readonly Stack<TextState> textStateStack;

		readonly OperandsCollection operands;

		readonly IPdfContentImportContext context;

		readonly PostScriptReader reader;
	}
}
