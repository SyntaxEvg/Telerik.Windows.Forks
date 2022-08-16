using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Layout;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class Block : FixedContentEditorBase, IBlockElement
	{
		public Block()
		{
			this.lines = new List<LineInfo>();
			this.pendingElements = new OrderedSet<LayoutElementBase>();
			this.hyperlinkStarts = new Stack<HyperlinkStartLayoutElement>();
			this.ResetBulletProperties();
			this.DesiredSize = new Size(0.0, 0.0);
			this.ActualSize = new Size(0.0, 0.0);
			this.LineSpacing = FixedDocumentDefaults.BlockLineSpacing;
			this.LineSpacingType = FixedDocumentDefaults.LineSpacingType;
		}

		public Block(Block other)
			: this()
		{
			this.SpacingBefore = other.SpacingBefore;
			this.SpacingAfter = other.SpacingAfter;
			this.LineSpacing = other.LineSpacing;
			this.FirstLineIndent = other.FirstLineIndent;
			this.LeftIndent = other.LeftIndent;
			this.RightIndent = other.RightIndent;
			this.BackgroundColor = other.BackgroundColor;
			this.HorizontalAlignment = other.HorizontalAlignment;
			this.VerticalAlignment = other.VerticalAlignment;
			this.TabStops = other.TabStops;
			this.DefaultTabStopWidth = other.DefaultTabStopWidth;
			base.TextProperties.CopyFrom(other.TextProperties);
			base.GraphicProperties.CopyFrom(other.GraphicProperties);
		}

		public Size DesiredSize { get; set; }

		public Size ActualSize { get; set; }

		public bool HasPendingContent
		{
			get
			{
				return this.PendingElements.Any<LayoutElementBase>();
			}
		}

		public PositionContentElement Bullet
		{
			get
			{
				return this.bullet;
			}
			set
			{
				if (this.bullet != value)
				{
					this.bullet = value;
					this.InvalidateBullet();
				}
			}
		}

		public double IndentAfterBullet
		{
			get
			{
				return this.indentAfterBullet;
			}
			set
			{
				if (this.indentAfterBullet != value)
				{
					this.indentAfterBullet = value;
					this.InvalidateBullet();
				}
			}
		}

		public double SpacingBefore { get; set; }

		public double SpacingAfter { get; set; }

		public double LineSpacing { get; set; }

		public HeightType LineSpacingType { get; set; }

		public double FirstLineIndent { get; set; }

		public double LeftIndent { get; set; }

		public double RightIndent { get; set; }

		public ColorBase BackgroundColor { get; set; }

		public Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment HorizontalAlignment { get; set; }

		public Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment VerticalAlignment { get; set; }

		public bool IsEmpty
		{
			get
			{
				return !this.lines.Any<LineInfo>() && !this.pendingElements.Any<LayoutElementBase>();
			}
		}

		public IEnumerable<LayoutElementBase> PendingElements
		{
			get
			{
				return this.pendingElements;
			}
		}

		internal TabStopCollection TabStops { get; set; }

		internal double DefaultTabStopWidth { get; set; }

		internal bool HasBackground
		{
			get
			{
				return this.BackgroundColor != null && !this.BackgroundColor.IsTransparent;
			}
		}

		internal IEnumerable<LineInfo> Lines
		{
			get
			{
				foreach (LineInfo line in this.lines)
				{
					yield return line;
				}
				yield break;
			}
		}

		public void InsertText(string text)
		{
			Guard.ThrowExceptionIfNull<string>(text, "text");
			if (!string.IsNullOrEmpty(text))
			{
				foreach (LayoutElementBase layoutElementBase in TextFragmentLayoutElement.CreateTextFragmentLayoutElements(this, text))
				{
					TextFragmentLayoutElement element = (TextFragmentLayoutElement)layoutElementBase;
					this.Insert(element);
				}
			}
		}

		public void InsertText(FontFamily fontFamily, string text)
		{
			Guard.ThrowExceptionIfNull<FontFamily>(fontFamily, "fontFamily");
			Guard.ThrowExceptionIfNull<string>(text, "text");
			base.TextProperties.TrySetFont(fontFamily);
			this.InsertText(text);
		}

		public void InsertText(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, string text)
		{
			Guard.ThrowExceptionIfNull<FontFamily>(fontFamily, "fontFamily");
			Guard.ThrowExceptionIfNull<FontStyle>(fontStyle, "fontStyle");
			Guard.ThrowExceptionIfNull<FontWeight>(fontWeight, "fontWeight");
			Guard.ThrowExceptionIfNull<string>(text, "text");
			base.TextProperties.TrySetFont(fontFamily, fontStyle, fontWeight);
			this.InsertText(text);
		}

		public void InsertLineBreak()
		{
			this.Insert(new LineBreakLayoutElement(base.TextProperties.Font, base.TextProperties.FontSize));
		}

		public void InsertForm(FormSource source)
		{
			base.AppendForm(source);
		}

		public void InsertForm(FormSource source, double width, double height)
		{
			base.AppendForm(source, width, height);
		}

		public void InsertForm(FormSource source, Size size)
		{
			base.AppendForm(source, size);
		}

		public void InsertImage(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			base.ImageEditor.DrawImage(stream);
		}

		public void InsertImage(Stream stream, double width, double height)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			base.ImageEditor.DrawImage(stream, new double?(width), new double?(height));
		}

		public void InsertImage(Stream stream, Size size)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.InsertImage(stream, size.Width, size.Height);
		}

		public void InsertImage(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource source)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>(source, "source");
			base.ImageEditor.DrawImage(source);
		}

		public void InsertImage(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource source, Size size)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>(source, "source");
			this.InsertImage(source, size.Width, size.Height);
		}

		public void InsertImage(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource source, double width, double height)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>(source, "source");
			base.ImageEditor.DrawImage(source, width, height);
		}

		public void InsertLine(Point point1, Point point2)
		{
			base.GraphicsEditor.DrawLine(point1, point2);
		}

		public void InsertRectangle(Rect rectangle)
		{
			base.GraphicsEditor.DrawRectangle(rectangle);
		}

		public void InsertEllipse(Point center, double radiusX, double radiusY)
		{
			base.GraphicsEditor.DrawEllipse(center, radiusX, radiusY);
		}

		public void InsertCircle(Point center, double radius)
		{
			this.InsertEllipse(center, radius, radius);
		}

		public void InsertPath(GeometryBase geometry)
		{
			base.GraphicsEditor.DrawPath(geometry);
		}

		public void InsertHyperlinkStart(Uri uri)
		{
			Guard.ThrowExceptionIfNull<Uri>(uri, "uri");
			UriAction action = new UriAction(uri);
			ActionHyperlinkStartLayoutElement hyperlinkStart = new ActionHyperlinkStartLayoutElement(action, base.TextProperties.Font, base.TextProperties.FontSize);
			this.InsertHyperlinkStart(hyperlinkStart);
		}

		public void InsertHyperlinkStart(Destination destination)
		{
			Guard.ThrowExceptionIfNull<Destination>(destination, "destination");
			DestinationHyperlinkStartLayoutElement hyperlinkStart = new DestinationHyperlinkStartLayoutElement(destination, base.TextProperties.Font, base.TextProperties.FontSize);
			this.InsertHyperlinkStart(hyperlinkStart);
		}

		public void InsertHyperlinkEnd()
		{
			this.Insert(this.hyperlinkStarts.Pop().CreateEnd());
		}

		public void Insert(PositionContentElement element)
		{
			Guard.ThrowExceptionIfNull<PositionContentElement>(element, "element");
			this.Append(element);
		}

		public void Insert(LayoutElementBase element)
		{
			//Guard.ThrowExceptionIfNull<LayoutElementBase>(element, "element");
			LineInfo currentLine = this.GetCurrentLine();
			currentLine.Add(element);
		}

		public void InsertRange(IEnumerable<LayoutElementBase> elements)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<LayoutElementBase>>(elements, "elements");
			if (elements.Any<LayoutElementBase>())
			{
				LineInfo currentLine = this.GetCurrentLine();
				currentLine.AddRange(elements);
			}
		}

		public Size Measure()
		{
			return this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
		}

		public Size Measure(Size availableSize)
		{
			if (this.TryCalculateDesiredSizeOnMinimalSizesScenarios(availableSize))
			{
				return this.DesiredSize;
			}
			Stack<LayoutElementBase> stack = this.PrepareElementsStackBeforeMeasure();
			List<LayoutElementBase> list = new List<LayoutElementBase>();
			double num = 0.0;
			double num2 = this.SpacingBefore;
			double num3 = availableSize.Width - this.LeftIndent - this.RightIndent;
			double height = availableSize.Height;
			double num4 = 0.0;
			double num5 = this.GetFirstLineLeftOffset();
			LineInfo lineInfo = null;
			while (stack.Count > 0)
			{
				LayoutElementBase layoutElementBase = stack.Pop();
				LineMeasureContext context = new LineMeasureContext(stack, num3 - (num5 + num4));
				if (!layoutElementBase.CanFit(num3, num5 + num4))
				{
					IEnumerable<LayoutElementBase> source;
					lineInfo = layoutElementBase.CompleteLine(num3, num5 + num4, list, out source);
					list.Clear();
					num4 = 0.0;
					foreach (LayoutElementBase item in source.Reverse<LayoutElementBase>())
					{
						stack.Push(item);
					}
					Block.CompleteLineEnding(lineInfo, stack);
					if (this.ShouldMoveLineToPending(lineInfo, num2, height))
					{
						break;
					}
					this.AddLineToMeasuring(lineInfo, ref num, ref num2);
					num5 = this.GetLeftOffset();
					lineInfo = null;
					if (num2 > height)
					{
						break;
					}
				}
				else
				{
					list.Add(layoutElementBase);
					layoutElementBase.PrepareCalculatingWidthOnMeasure(context);
					num4 += layoutElementBase.Width;
				}
			}
			bool flag = lineInfo != null || list.Count > 0;
			if (flag)
			{
				lineInfo = lineInfo ?? new LineInfo();
				lineInfo.AddRange(list);
				if (this.ShouldMoveLineToPending(lineInfo, num2, height))
				{
					this.AddToPendingElements(lineInfo.Elements);
				}
				else
				{
					this.AddLineToMeasuring(lineInfo, ref num, ref num2);
				}
			}
			this.AddToPendingElements(stack);
			if (!this.pendingElements.Any<LayoutElementBase>())
			{
				num2 += this.SpacingAfter;
			}
			double num6 = Math.Max(this.RightIndent, 0.0);
			double val = num + this.LeftIndent + num6;
			double width = Math.Max(val, 0.0);
			this.DesiredSize = new Size(width, num2);
			return this.DesiredSize;
		}

		internal void InsertTab()
		{
			this.Insert(new TabLayoutElement(this.TabStops, this.DefaultTabStopWidth, base.TextProperties));
		}

		IEnumerable<LayoutElementBase> GetAllElements()
		{
			foreach (LineInfo line in this.lines)
			{
				foreach (LayoutElementBase element in line.Elements)
				{
					yield return element;
				}
			}
			foreach (LayoutElementBase element2 in this.pendingElements)
			{
				yield return element2;
			}
			yield break;
		}

		bool TryCalculateDesiredSizeOnMinimalSizesScenarios(Size availableSize)
		{
			if (availableSize == RadFixedDocumentEditor.MinimalWidthMeasureSize)
			{
				double num = 0.0;
				foreach (LayoutElementBase layoutElementBase in this.GetAllElements())
				{
					num = Math.Max(layoutElementBase.GetMinWidth(), num);
				}
				this.DesiredSize = new Size(num, availableSize.Height);
				return true;
			}
			if (availableSize == RadFixedDocumentEditor.MinimalMeasureSize)
			{
				LayoutElementBase layoutElementBase2 = this.GetAllElements().FirstOrDefault<LayoutElementBase>();
				if (layoutElementBase2 != null)
				{
					LineInfo lineInfo = new LineInfo();
					lineInfo.Add(layoutElementBase2);
					double actualLineHeight = this.GetActualLineHeight(lineInfo);
					this.DesiredSize = new Size(layoutElementBase2.GetMinMeasureSize().Width, actualLineHeight);
				}
				else
				{
					this.DesiredSize = new Size(0.0, 0.0);
				}
				return true;
			}
			return false;
		}

		static void CompleteLineEnding(LineInfo line, Stack<LayoutElementBase> pendingElements)
		{
			if (line.IsEndingWithLineBreak)
			{
				return;
			}
			while (pendingElements.Count > 0)
			{
				LayoutElementBase layoutElementBase = pendingElements.Peek();
				if (!layoutElementBase.CanAddToLineEnding)
				{
					return;
				}
				pendingElements.Pop();
				LayoutElementBase element;
				LayoutElementBase item;
				if (layoutElementBase.TrySplitToAddToLineEnding(out element, out item))
				{
					line.Add(element);
					pendingElements.Push(item);
				}
				else
				{
					line.Add(layoutElementBase);
				}
			}
		}

		void AddLineToMeasuring(LineInfo line, ref double width, ref double height)
		{
			if (width < line.TrimmedWidth)
			{
				width = line.TrimmedWidth;
			}
			this.AddLine(line);
			double actualLineHeight = this.GetActualLineHeight(line);
			height += actualLineHeight;
		}

		Stack<LayoutElementBase> PrepareElementsStackBeforeMeasure()
		{
			Stack<LayoutElementBase> stack = new Stack<LayoutElementBase>();
			for (int i = this.pendingElements.Count - 1; i >= 0; i--)
			{
				stack.Push(this.pendingElements[i]);
			}
			for (int j = this.lines.Count - 1; j >= 0; j--)
			{
				LineInfo lineInfo = this.lines[j];
				for (int k = lineInfo.Elements.Count<LayoutElementBase>() - 1; k >= 0; k--)
				{
					stack.Push(lineInfo.Elements.ElementAt(k));
				}
			}
			this.InsertListBullet(stack);
			this.lines.Clear();
			this.pendingElements.Clear();
			return stack;
		}

		bool ShouldMoveLineToPending(LineInfo line, double currentBlockHeight, double maxBlockHeight)
		{
			double actualLineHeight = this.GetActualLineHeight(line);
			return this.lines.Any<LineInfo>() && currentBlockHeight + actualLineHeight > maxBlockHeight;
		}

		public void Draw(FixedContentEditor editor, Rect boundingRect)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(editor, "editor");
			this.Measure(new Size(boundingRect.Width, boundingRect.Height));
			this.Arrange(editor, boundingRect);
		}

		public Block Split()
		{
			Block block = new Block(this);
			block.InsertRange(this.PendingElements);
			block.FirstLineIndent = 0.0;
			block.SpacingBefore = 0.0;
			return block;
		}

		IBlockElement IBlockElement.Split()
		{
			return this.Split();
		}

		public void SetBullet(List list, int listLevel)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(FixedDocumentDefaults.FirstListLevelIndex, list.Levels.Count - 1, listLevel, "listLevel");
			list.Indexer.MoveToNextIndex(listLevel);
			ListLevel listLevel2 = list.Levels[listLevel];
			this.Bullet = listLevel2.BulletNumberingFormat.GetBulletNumberingElement(list.Indexer);
			TextFragment textFragment = this.Bullet as TextFragment;
			if (textFragment != null)
			{
				using (base.SaveProperties())
				{
					listLevel2.CharacterProperties.CopyTo(this);
					this.CopyPropertiesTo(textFragment);
				}
			}
			this.FirstLineIndent = listLevel2.ParagraphProperties.FirstLineIndent;
			this.LeftIndent = listLevel2.ParagraphProperties.LeftIndent;
			this.IndentAfterBullet = listLevel2.IndentAfterBullet;
		}

		public void Clear()
		{
			this.lines.Clear();
			this.hyperlinkStarts.Clear();
			this.pendingElements.Clear();
			this.ResetBulletProperties();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LineInfo lineInfo in this.Lines)
			{
				stringBuilder.AppendLine(lineInfo.ToString());
			}
			return stringBuilder.ToString();
		}

		internal static void Draw(IEnumerable<LayoutElementBase> lineElements, DrawLayoutElementContext context)
		{
			double num = context.OffsetX;
			foreach (LayoutElementBase layoutElementBase in lineElements)
			{
				layoutElementBase.Draw(new DrawLayoutElementContext(context.Editor, context.Block, context.Line, num, context.OffsetY, context.BoundingRectangle));
				num += layoutElementBase.Width;
			}
		}

		internal static Telerik.Windows.Documents.Fixed.Model.Graphics.Path CreateBackgroundPath(Rect rect, ColorBase fill)
		{
			return new Telerik.Windows.Documents.Fixed.Model.Graphics.Path
			{
				Geometry = new Telerik.Windows.Documents.Fixed.Model.Graphics.RectangleGeometry(new Rect(rect.X, rect.Y, rect.Width, rect.Height)),
				IsFilled = true,
				IsStroked = false,
				Fill = fill
			};
		}

		internal override void Append(PositionContentElement element)
		{
			IEnumerable<LayoutElementBase> enumerable;
			if (this.TryGetLayoutElements(element, out enumerable))
			{
				foreach (LayoutElementBase element2 in enumerable)
				{
					this.Insert(element2);
				}
			}
		}

		internal IEnumerable<LineInfo> GetLinesBetween(LineInfo first, LineInfo last)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(first, "first");
			Guard.ThrowExceptionIfNull<LineInfo>(last, "last");
			if (first != last)
			{
				bool skip = true;
				foreach (LineInfo line in this.lines)
				{
					if (line == last)
					{
						yield break;
					}
					if (!skip)
					{
						yield return line;
					}
					if (skip && first == line)
					{
						skip = false;
					}
				}
			}
			yield break;
		}

		internal bool IsPending(LayoutElementBase element)
		{
			Guard.ThrowExceptionIfNull<LayoutElementBase>(element, "element");
			return this.pendingElements.Contains(element);
		}

		internal double GetActualLineHeight(LineInfo lineInfo)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(lineInfo, "lineInfo");
			double num = lineInfo.BaselineOffset + lineInfo.Descent;
			if (num == 0.0)
			{
				return 0.0;
			}
			switch (this.LineSpacingType)
			{
			case HeightType.Auto:
				return num + (lineInfo.LineSpacingBaselineOffset + lineInfo.LineSpacingDescent) * (this.LineSpacing - 1.0);
			case HeightType.AtLeast:
				return Math.Max(num, this.LineSpacing);
			case HeightType.Exact:
				return this.LineSpacing;
			default:
				return num;
			}
		}

		void InsertListBullet(Stack<LayoutElementBase> elements)
		{
			if (this.isBulletInvalidated)
			{
				this.isBulletInvalidated = false;
				if (this.isBulletInserted)
				{
					elements.Pop();
					this.isBulletInserted = false;
				}
				if (this.Bullet != null || this.IndentAfterBullet != 0.0)
				{
					LineInfo lineInfo = new LineInfo();
					if (this.Bullet != null)
					{
						IEnumerable<LayoutElementBase> elements2 = null;
						if (this.TryGetLayoutElements(this.Bullet, out elements2))
						{
							lineInfo.AddRange(elements2);
						}
					}
					double num = this.CalculateAlignedIndentAfterBullet(lineInfo);
					elements.Push(new BulletLayoutElement(lineInfo, num, base.TextProperties));
					this.isBulletInserted = true;
				}
			}
		}

		double CalculateAlignedIndentAfterBullet(LineInfo bulletElementsLine)
		{
			if (this.FirstLineIndent < 0.0)
			{
				double num = -this.FirstLineIndent;
				if (num > bulletElementsLine.TrimmedWidth)
				{
					return num - bulletElementsLine.Width;
				}
			}
			return this.IndentAfterBullet;
		}

		void ResetBulletProperties()
		{
			this.indentAfterBullet = 0.0;
			this.isBulletInserted = false;
			this.isBulletInvalidated = false;
			this.bullet = null;
		}

		bool TryGetLayoutElements(PositionContentElement element, out IEnumerable<LayoutElementBase> layoutElements)
		{
			Guard.ThrowExceptionIfNull<PositionContentElement>(element, "element");
			layoutElements = null;
			bool result = true;
			switch (element.ElementType)
			{
			case FixedDocumentElementType.Path:
				layoutElements = new LayoutElementBase[]
				{
					new PathLayoutElement((Telerik.Windows.Documents.Fixed.Model.Graphics.Path)element, base.TextProperties)
				};
				return result;
			case FixedDocumentElementType.Image:
				break;
			case FixedDocumentElementType.TextFragment:
			{
				TextFragment textFragment = (TextFragment)element;
				using (base.SaveProperties())
				{
					this.CopyPropertiesFrom(textFragment);
					layoutElements = TextFragmentLayoutElement.CreateTextFragmentLayoutElements(this, textFragment.Text).ToArray<LayoutElementBase>();
					return result;
				}
				break;
			}
			case FixedDocumentElementType.Form:
				layoutElements = new LayoutElementBase[]
				{
					new FormLayoutElement((Form)element, base.TextProperties)
				};
				return result;
			default:
				return false;
			}
			layoutElements = new LayoutElementBase[]
			{
				new ImageLayoutElement((Image)element, base.TextProperties)
			};
			return result;
		}

		void CopyPropertiesFrom(TextFragment textFragment)
		{
			base.GraphicProperties.CopyFrom(textFragment.TextProperties.GeometryProperties);
			base.TextProperties.CopyFrom(textFragment.TextProperties);
		}

		void CopyPropertiesTo(TextFragment textFragment)
		{
			base.GraphicProperties.CopyTo(textFragment.TextProperties.GeometryProperties);
			base.TextProperties.CopyTo(textFragment.TextProperties);
		}

		void InvalidateBullet()
		{
			this.isBulletInvalidated = true;
		}

		static double CalculateStartOffsetWhenCentering(double sizeToCenter, double sizeToFitIn)
		{
			return Block.CalculateStartOffsetWhenPositioningAtOpositeSide(sizeToCenter, sizeToFitIn) / 2.0;
		}

		static double CalculateStartOffsetWhenPositioningAtOpositeSide(double sizeToPosition, double sizeToFitIn)
		{
			return sizeToFitIn - sizeToPosition;
		}

		void AddToPendingElements(IEnumerable<LayoutElementBase> elements)
		{
			foreach (LayoutElementBase layoutElementBase in elements)
			{
				if (layoutElementBase.ShouldSplitToBlocks(this))
				{
					LayoutElementBase item;
					IEnumerable<LayoutElementBase> enumerable;
					IEnumerable<LayoutElementBase> items;
					layoutElementBase.SplitToBlocks(this, out item, out enumerable, out items);
					foreach (LayoutElementBase element in enumerable)
					{
						this.Insert(element);
					}
					this.pendingElements.InsertRange(0, items);
					this.pendingElements.Add(item);
				}
				else
				{
					this.pendingElements.Add(layoutElementBase);
				}
			}
		}

		void Arrange(FixedContentEditor editor, Rect boundingRect)
		{
			double num = (double.IsPositiveInfinity(boundingRect.Width) ? this.DesiredSize.Width : boundingRect.Width);
			double num2 = (double.IsPositiveInfinity(boundingRect.Height) ? this.DesiredSize.Height : boundingRect.Height);
			double num3 = boundingRect.Y + this.GetVerticalOffset(num2, this.DesiredSize.Height);
			if (this.HasBackground)
			{
				Telerik.Windows.Documents.Fixed.Model.Graphics.Path element = Block.CreateBackgroundPath(new Rect(boundingRect.X + this.LeftIndent, boundingRect.Y + this.SpacingBefore, num - this.RightIndent - this.LeftIndent, this.DesiredSize.Height - this.SpacingBefore - this.SpacingAfter), this.BackgroundColor);
				editor.Draw(element);
			}
			for (int i = 0; i < this.lines.Count; i++)
			{
				LineInfo lineInfo = this.lines[i];
				double num4 = boundingRect.X + this.GetHorizontalOffset(num, lineInfo, i);
				lineInfo.BoundingRect = new Rect(num4, num3, lineInfo.Width, this.GetActualLineHeight(lineInfo));
				Block.Draw(lineInfo.Elements, new DrawLayoutElementContext(editor, this, lineInfo, num4, num3, boundingRect));
				num3 += lineInfo.BoundingRect.Height;
			}
			this.ActualSize = new Size(num, num2);
		}

		void AddLine(LineInfo line)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			this.lines.Add(line);
		}

		LineInfo GetCurrentLine()
		{
			if (this.lines.Count == 0)
			{
				this.lines.Add(new LineInfo());
			}
			return this.lines.Last<LineInfo>();
		}

		double GetHorizontalOffset(double blockWidth, LineInfo line, int lineIndex)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			double num = this.LeftIndent + ((lineIndex == 0) ? this.GetFirstLineLeftOffset() : this.GetLeftOffset());
			switch (this.HorizontalAlignment)
			{
			case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left:
				return num;
			case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Right:
				return Block.CalculateStartOffsetWhenPositioningAtOpositeSide(line.TrimmedWidth, blockWidth) - this.RightIndent;
			case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center:
				return num + Block.CalculateStartOffsetWhenCentering(line.TrimmedWidth, blockWidth - num - this.RightIndent);
			default:
				return 0.0;
			}
		}

		double GetVerticalOffset(double availableHeight, double blockHeight)
		{
			double num = this.SpacingBefore;
			switch (this.VerticalAlignment)
			{
			case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Center:
				num += Block.CalculateStartOffsetWhenCentering(blockHeight, availableHeight);
				break;
			case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Bottom:
				num += Block.CalculateStartOffsetWhenPositioningAtOpositeSide(blockHeight, availableHeight);
				break;
			}
			return num;
		}

		double GetFirstLineLeftOffset()
		{
			if (this.FirstLineIndent < 0.0)
			{
				return 0.0;
			}
			return this.FirstLineIndent;
		}

		double GetLeftOffset()
		{
			if (this.FirstLineIndent < 0.0)
			{
				return -this.FirstLineIndent;
			}
			return 0.0;
		}

		void InsertHyperlinkStart(HyperlinkStartLayoutElement hyperlinkStart)
		{
			this.hyperlinkStarts.Push(hyperlinkStart);
			this.Insert(hyperlinkStart);
		}

		internal const double Epsilon = 0.1;

		readonly List<LineInfo> lines;

		readonly OrderedSet<LayoutElementBase> pendingElements;

		readonly Stack<HyperlinkStartLayoutElement> hyperlinkStarts;

		PositionContentElement bullet;

		double indentAfterBullet;

		bool isBulletInserted;

		bool isBulletInvalidated;
	}
}
