using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class PatternFillRenderable : IRenderable
	{
		public PatternFillRenderable()
		{
			this.visibleBoxes = new List<Rect>();
		}

		public PatternFill PatternFill { get; set; }

		public ThemeColorScheme ColorScheme { get; set; }

		public IEnumerable<Rect> VisibleBoxes
		{
			get
			{
				return this.visibleBoxes;
			}
		}

		public void AddVisibleBox(Rect rectangle)
		{
			this.visibleBoxes.Add(rectangle);
		}

		public void AddVisibleBoxes(IEnumerable<Rect> rectangles)
		{
			this.visibleBoxes.AddRange(rectangles);
		}

		public void ClearVisibleBoxes()
		{
			this.visibleBoxes.Clear();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Rect rect in this.VisibleBoxes)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("({0})", rect);
			}
			return string.Format("PatternFillRenderable: PatternType={0}, VisibleBoxes:{1}", this.PatternFill.PatternType, stringBuilder);
		}

		readonly List<Rect> visibleBoxes;
	}
}
