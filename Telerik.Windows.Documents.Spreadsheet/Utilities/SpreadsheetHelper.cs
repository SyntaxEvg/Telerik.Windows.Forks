using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class SpreadsheetHelper
	{
		static BitmapSource NoImagePlaceholder
		{
			get
			{
				if (SpreadsheetHelper.noImagePlaceholder == null)
				{
					using (Stream stream = Application.GetResourceStream(new Uri("/Telerik.Windows.Documents.Spreadsheet;component/Resources/imageUnavailablePlaceholder.png", UriKind.Relative)).Stream)
					{
						SpreadsheetHelper.noImagePlaceholder = SpreadsheetHelper.GetImageSource(stream);
					}
				}
				return SpreadsheetHelper.noImagePlaceholder;
			}
		}

		public static string GetSheetNamePrefixBySheetType(SheetType type)
		{
			if (type == SheetType.Worksheet)
			{
				return "Sheet";
			}
			throw new NotSupportedException();
		}

		public static SheetType GetSheetType<T>() where T : Sheet
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(Worksheet))
			{
				return SheetType.Worksheet;
			}
			throw new NotSupportedException();
		}

		public static double EnsureValidScaleFactor(double value)
		{
			value = Math.Max(SpreadsheetDefaultValues.MinScaleFactor, value);
			value = System.Math.Min(SpreadsheetDefaultValues.MaxScaleFactor, value);
			return value;
		}

		public static System.Windows.Size EnsureValidScaleFactor(System.Windows.Size value)
		{
			value.Width = SpreadsheetHelper.EnsureValidScaleFactor(value.Width);
			value.Height = SpreadsheetHelper.EnsureValidScaleFactor(value.Height);
			return value;
		}

		static double Min(params double[] arguments)
		{
			double num = arguments[0];
			for (int i = 1; i < arguments.Length; i++)
			{
				if (num > arguments[i])
				{
					num = arguments[i];
				}
			}
			return num;
		}

		static double Max(params double[] arguments)
		{
			double num = arguments[0];
			for (int i = 1; i < arguments.Length; i++)
			{
				if (num < arguments[i])
				{
					num = arguments[i];
				}
			}
			return num;
		}

		public static Rect CalculateShapeBoundingRect(System.Windows.Point position, FloatingShapeBase shape)
		{
			double x = position.X;
			double y = position.Y;
			double num = (shape.DoesRotationAngleRequireCellIndexChange() ? shape.Height : shape.Width);
			double num2 = (shape.DoesRotationAngleRequireCellIndexChange() ? shape.Width : shape.Height);
			double angleInDegrees = (shape.DoesRotationAngleRequireCellIndexChange() ? (shape.RotationAngle - 90.0) : shape.RotationAngle);
			Matrix m = Matrix.Identity;
			m = m.RotateMatrixAt(angleInDegrees, x + num / 2.0, y + num2 / 2.0);
			System.Windows.Point point = m.Transform(new System.Windows.Point(x, y));
			System.Windows.Point point2 = m.Transform(new System.Windows.Point(x + num, y));
			System.Windows.Point point3 = m.Transform(new System.Windows.Point(x + num, y + num2));
			System.Windows.Point point4 = m.Transform(new System.Windows.Point(x, y + num2));
			return new Rect(new System.Windows.Point(SpreadsheetHelper.Min(new double[] { point.X, point2.X, point3.X, point4.X }), SpreadsheetHelper.Min(new double[] { point.Y, point2.Y, point3.Y, point4.Y })), new System.Windows.Point(SpreadsheetHelper.Max(new double[] { point.X, point2.X, point3.X, point4.X }), SpreadsheetHelper.Max(new double[] { point.Y, point2.Y, point3.Y, point4.Y })));
		}

		public static bool TryParseMailToAddress(string address, out string emailAddress, out string emailSubject)
		{
			emailAddress = null;
			emailSubject = null;
			Match match = SpreadsheetHelper.mailtoRegex.Match(address);
			bool success = match.Success;
			if (success)
			{
				emailAddress = match.Groups["email"].ToString();
				if (match.Groups["subject"].Success)
				{
					emailSubject = match.Groups["subject"].ToString();
				}
			}
			return success;
		}

		public static bool TryParseHyperlinkAddress(string name, out string address, out string subAddress, out string emailAddress, out string emailSubject)
		{
			address = string.Empty;
			subAddress = string.Empty;
			emailAddress = string.Empty;
			emailSubject = string.Empty;
			bool flag = SpreadsheetHelper.TryParseMailToAddress(name, out emailAddress, out emailSubject);
			if (!flag)
			{
				string[] array = name.Split(new char[] { '!' });
				address = array[0];
				if (array.Length > 1)
				{
					subAddress = array[1];
				}
				flag = true;
			}
			return flag;
		}

		public static bool TryGetHyperLinkInfo(ICellValue cellValue, out HyperlinkInfo hyperlinkInfo)
		{
			hyperlinkInfo = null;
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (formulaCellValue != null)
			{
				FunctionExpression functionExpression = formulaCellValue.Value as FunctionExpression;
				if (functionExpression != null && functionExpression.IsValid)
				{
					HyperlinkFunction hyperlinkFunction = functionExpression.Function as HyperlinkFunction;
					if (hyperlinkFunction != null)
					{
						hyperlinkInfo = hyperlinkFunction.HyperlinkInfo;
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsHyperlinkFormula(ICellValue cellValue)
		{
			HyperlinkInfo hyperlinkInfo;
			return SpreadsheetHelper.TryGetHyperLinkInfo(cellValue, out hyperlinkInfo);
		}

		public static Worksheet GetWorksheetByName(Workbook workbook, string worksheetName)
		{
			Worksheet result = null;
			for (int i = 0; i < workbook.Worksheets.Count; i++)
			{
				if (string.Equals(workbook.Worksheets[i].Name, worksheetName, StringComparison.CurrentCultureIgnoreCase))
				{
					result = workbook.Worksheets[i];
					break;
				}
			}
			return result;
		}

		public static BitmapSource GetImageSourceOrNoImagePlaceHolder(Stream stream)
		{
			BitmapSource imageSource;
			try
			{
				imageSource = SpreadsheetHelper.GetImageSource(stream);
			}
			catch (SystemException)
			{
				imageSource = SpreadsheetHelper.NoImagePlaceholder;
			}
			return imageSource;
		}

		public static BitmapSource GetImageSource(Stream stream)
		{
			BitmapImage bitmapImage = new BitmapImage();
			try
			{
				SpreadsheetHelper.InitBitmapImage(bitmapImage, stream);
			}
			catch (NotSupportedException)
			{
				Image image = Image.FromStream(stream);
				MemoryStream memoryStream = new MemoryStream();
				image.Save(memoryStream, ImageFormat.Png);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				stream = memoryStream;
				bitmapImage = new BitmapImage();
				SpreadsheetHelper.InitBitmapImage(bitmapImage, stream);
			}
			return bitmapImage;
		}

		public static void InitBitmapImage(BitmapImage bitmapImage, Stream stream)
		{
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = stream;
			bitmapImage.EndInit();
		}

		public static Stream StreamFromBitmapSource(BitmapSource writeBmp)
		{
			Stream stream = new MemoryStream();
			new BmpBitmapEncoder
			{
				Frames = { BitmapFrame.Create(writeBmp) }
			}.Save(stream);
			return stream;
		}

		public static T GetNearestValidValue<T>(T value, T minValue, T maxValue) where T : IComparable<T>
		{
			if (value.CompareTo(minValue) < 0)
			{
				return minValue;
			}
			if (value.CompareTo(maxValue) > 0)
			{
				return maxValue;
			}
			return value;
		}

		public static int PercentFromScaleFactor(System.Windows.Size scaleFactor)
		{
			return SpreadsheetHelper.PercentFromScaleFactor(scaleFactor.Width);
		}

		public static int PercentFromScaleFactor(double scaleFactor)
		{
			return (int)(scaleFactor * 100.0);
		}

		public static System.Windows.Size ScaleFactorFromPercent(int? percent)
		{
			double num = (double)(percent ?? 0) / 100.0;
			return new System.Windows.Size(num, num);
		}

		public static double RestrictRotationAngle(double angle)
		{
			double num = angle % 360.0;
			if (num < 0.0)
			{
				num = 360.0 + num;
			}
			return num;
		}

		public static double Transform(GeneralTransform generalTransform, double value)
		{
			return generalTransform.Transform(new System.Windows.Point(value, 0.0)).X;
		}

		public static System.Windows.Size Transform(GeneralTransform generalTransform, System.Windows.Size value)
		{
			System.Windows.Point point = generalTransform.Transform(new System.Windows.Point(value.Width, value.Height));
			return new System.Windows.Size(point.X, point.Y);
		}

		public static List<CellRange> SplitIntoRangesAccordingToHidden(CellRange cellRange, Worksheet worksheet)
		{
			int[] array;
			int[] array2;
			return SpreadsheetHelper.SplitIntoRangesAccordingToHidden(cellRange, worksheet, out array, out array2);
		}

		public static List<CellRange> SplitIntoRangesAccordingToHidden(CellRange cellRange, Worksheet worksheet, out int[] hiddenRows, out int[] hiddenColumns)
		{
			List<CellRange> columnRanges = SpreadsheetHelper.SplitAccordingToHiddenColumns(cellRange, worksheet, out hiddenColumns);
			return SpreadsheetHelper.SplitAccordingToHiddenRows(columnRanges, cellRange, worksheet, out hiddenRows);
		}

		public static int CountRows(IEnumerable<CellRange> cellRanges)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (CellRange cellRange in cellRanges)
			{
				for (int i = cellRange.FromIndex.RowIndex; i <= cellRange.ToIndex.RowIndex; i++)
				{
					hashSet.Add(i);
				}
			}
			return hashSet.Count;
		}

		public static int CountColumns(IEnumerable<CellRange> cellRanges)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (CellRange cellRange in cellRanges)
			{
				for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
				{
					hashSet.Add(i);
				}
			}
			return hashSet.Count;
		}

		static List<CellRange> SplitAccordingToHiddenColumns(CellRange cellRange, Worksheet worksheet, out int[] hiddenColumns)
		{
			List<CellRange> list = new List<CellRange>();
			List<int> list2 = new List<int>();
			RangePropertyValue<bool> hidden = worksheet.Columns[cellRange].GetHidden();
			if (!hidden.IsIndeterminate && !hidden.Value)
			{
				hiddenColumns = new int[0];
				return new List<CellRange> { cellRange };
			}
			bool flag = false;
			int fromColumnIndex = 0;
			for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
			{
				if (worksheet.Columns[i].GetHidden().Value)
				{
					if (flag)
					{
						int toColumnIndex = i - 1;
						flag = false;
						list.Add(new CellRange(cellRange.FromIndex.RowIndex, fromColumnIndex, cellRange.ToIndex.RowIndex, toColumnIndex));
					}
					list2.Add(i);
				}
				else if (!flag)
				{
					fromColumnIndex = i;
					flag = true;
				}
			}
			if (flag)
			{
				list.Add(new CellRange(cellRange.FromIndex.RowIndex, fromColumnIndex, cellRange.ToIndex.RowIndex, cellRange.ToIndex.ColumnIndex));
			}
			hiddenColumns = list2.ToArray();
			return list;
		}

		static List<CellRange> SplitAccordingToHiddenRows(List<CellRange> columnRanges, CellRange cellRange, Worksheet worksheet, out int[] hiddenRows)
		{
			List<CellRange> list = new List<CellRange>();
			HashSet<int> hashSet = new HashSet<int>();
			RangePropertyValue<bool> hidden = worksheet.Rows[cellRange].GetHidden();
			if (!hidden.IsIndeterminate && !hidden.Value)
			{
				hiddenRows = new int[0];
				return columnRanges;
			}
			foreach (CellRange cellRange2 in columnRanges)
			{
				bool flag = false;
				int fromRowIndex = 0;
				for (int i = cellRange.FromIndex.RowIndex; i <= cellRange2.ToIndex.RowIndex; i++)
				{
					if (worksheet.Rows[i].GetHidden().Value)
					{
						if (flag)
						{
							int toRowIndex = i - 1;
							flag = false;
							list.Add(new CellRange(fromRowIndex, cellRange2.FromIndex.ColumnIndex, toRowIndex, cellRange2.ToIndex.ColumnIndex));
						}
						hashSet.Add(i);
					}
					else if (!flag)
					{
						fromRowIndex = i;
						flag = true;
					}
				}
				if (flag)
				{
					list.Add(new CellRange(fromRowIndex, cellRange2.FromIndex.ColumnIndex, cellRange2.ToIndex.RowIndex, cellRange2.ToIndex.ColumnIndex));
				}
			}
			hiddenRows = hashSet.ToArray<int>();
			return list;
		}

		const string EmailMatch = "([a-zA-Z0-9._%-]+)@([a-zA-Z0-9.-]+\\.[a-zA-Z]{2,6})";

		const string EmailGroupName = "email";

		const string SubjectGroupName = "subject";

		static readonly string mailtoMatch = string.Format("mailto:(?<{0}>{1})(\\?[Ss]ubject=(?<{2}>.+))?", "email", "([a-zA-Z0-9._%-]+)@([a-zA-Z0-9.-]+\\.[a-zA-Z]{2,6})", "subject");

		static readonly Regex mailtoRegex = new Regex(SpreadsheetHelper.mailtoMatch);

		internal static readonly Regex EmailRegex = new Regex("([a-zA-Z0-9._%-]+)@([a-zA-Z0-9.-]+\\.[a-zA-Z]{2,6})");

		static BitmapSource noImagePlaceholder;
	}
}
