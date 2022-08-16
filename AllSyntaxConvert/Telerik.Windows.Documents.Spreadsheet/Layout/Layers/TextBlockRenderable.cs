using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class TextBlockRenderable : IRenderable
	{
		public Point TopLeft { get; set; }

		public string Text { get; set; }

		public double FontSize { get; set; }

		public Thickness Padding { get; set; }

		public FontFamily FontFamily { get; set; }

		public FontWeight FontWeight { get; set; }

		public FontStyle FontStyle { get; set; }

		public TextDecorationCollection TextDecorations { get; set; }

		public Color Foreground { get; set; }

		public Rect Clip { get; set; }

		public TextAlignment TextAlignment { get; set; }

		public TextWrapping TextWrapping { get; set; }

		public Size ScaleFactor { get; set; }

		internal bool ShouldFitToCellSize { get; set; }

		internal ICellValue CellValue { get; set; }

		internal CellValueFormat CellValueFormat { get; set; }

		internal CellLayoutBox CellBox { get; set; }

		internal FontProperties FontProperties
		{
			get
			{
				return new FontProperties(this.FontFamily, this.FontSize, this.FontWeight == FontWeights.Bold);
			}
		}

		internal IEnumerable<RunRenderable> Runs
		{
			get
			{
				return this.runs;
			}
		}

		internal int InlinesCount
		{
			get
			{
				return this.runs.Count;
			}
		}

		internal void AddRun(RunRenderable run)
		{
			this.runs.Add(run);
			this.InvalidateText();
		}

		internal void InsertRunAfter(RunRenderable previousRun, RunRenderable run)
		{
			int num = this.runs.IndexOf(previousRun);
			this.runs.Insert(num + 1, run);
			this.InvalidateText();
		}

		internal void ClearRuns()
		{
			this.runs.Clear();
			this.InvalidateText();
		}

		void InvalidateText()
		{
			this.Text = null;
		}

		public override string ToString()
		{
			return string.Format("TextBlockRenderable: CellBox={0}, Text={1}, InlinesCount={2}, FontSize={3}, FontFamily={4}, FontWeight={5}, FontStyle={6}, TextDecorations={7}, Foreground={8}, TextAlignment={9}, Wrapping={10}, Scale={11}, Format={12}", new object[]
			{
				this.CellBox,
				this.Text,
				this.InlinesCount,
				this.FontSize,
				this.FontFamily,
				this.FontWeight,
				this.FontStyle,
				this.TextDecorations,
				this.Foreground,
				this.TextAlignment,
				this.TextWrapping,
				this.ScaleFactor,
				this.CellValueFormat.FormatString
			});
		}

		readonly List<RunRenderable> runs = new List<RunRenderable>();
	}
}
