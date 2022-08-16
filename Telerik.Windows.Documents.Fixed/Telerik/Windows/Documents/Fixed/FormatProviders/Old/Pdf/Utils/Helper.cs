using System;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	static class Helper
	{
		public static Dispatcher Dispatcher
		{
			get
			{
				if (Application.Current == null)
				{
					return null;
				}
				return Application.Current.Dispatcher;
			}
		}

		public static Matrix CalculateTextMatrix(Matrix m, GlyphOld glyph)
		{
			double width = glyph.Width;
			double offsetX = (width * glyph.FontSize + glyph.CharSpacing + glyph.WordSpacing) * (glyph.HorizontalScaling / 100.0);
			return new Matrix(1.0, 0.0, 0.0, 1.0, offsetX, 0.0) * m;
		}

		public static object[] BoxDoubleParameters(double[] doubleParameters)
		{
			object[] array = new object[doubleParameters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = doubleParameters[i];
			}
			return array;
		}

		public static bool UnboxDouble(object obj, out double res)
		{
			res = 0.0;
			if (obj == null)
			{
				return false;
			}
			if (obj is byte)
			{
				res = (double)((byte)obj);
				return true;
			}
			if (obj is int)
			{
				res = (double)((int)obj);
				return true;
			}
			if (obj is double)
			{
				res = (double)obj;
				return true;
			}
			PdfIntOld pdfIntOld = obj as PdfIntOld;
			if (pdfIntOld != null)
			{
				pdfIntOld.Load();
				res = (double)pdfIntOld.Value;
				return true;
			}
			PdfRealOld pdfRealOld = obj as PdfRealOld;
			if (pdfRealOld != null)
			{
				pdfRealOld.Load();
				res = pdfRealOld.Value;
				return true;
			}
			return false;
		}

		public static bool GetBit(int n, byte bit)
		{
			return (n & (1 << (int)bit)) != 0;
		}

		public static bool UnboxBool(object obj, out bool res)
		{
			res = false;
			if (obj == null)
			{
				return false;
			}
			if (obj is bool)
			{
				res = (bool)obj;
				return true;
			}
			PdfBoolOld pdfBoolOld = obj as PdfBoolOld;
			if (pdfBoolOld != null)
			{
				pdfBoolOld.Load();
				res = pdfBoolOld.Value;
				return true;
			}
			return false;
		}

		public static bool UnboxInt(object obj, out int res)
		{
			res = 0;
			if (obj == null)
			{
				return false;
			}
			if (obj is byte)
			{
				res = (int)((byte)obj);
				return true;
			}
			if (obj is int)
			{
				res = (int)obj;
				return true;
			}
			if (obj is double)
			{
				res = (int)((double)obj);
				return true;
			}
			PdfIntOld pdfIntOld = obj as PdfIntOld;
			if (pdfIntOld != null)
			{
				pdfIntOld.Load();
				res = pdfIntOld.Value;
				return true;
			}
			PdfRealOld pdfRealOld = obj as PdfRealOld;
			if (pdfRealOld != null)
			{
				pdfRealOld.Load();
				res = (int)pdfRealOld.Value;
				return true;
			}
			return false;
		}

		public static bool EnumTryParse<TEnum>(string valueAsString, out TEnum value) where TEnum : struct
		{
			return Helper.EnumTryParse<TEnum>(valueAsString, out value, false);
		}

		public static bool EnumTryParse<TEnum>(string valueAsString, out TEnum value, bool ignoreCase) where TEnum : struct
		{
			bool result;
			try
			{
				value = (TEnum)((object)Enum.Parse(typeof(TEnum), valueAsString, ignoreCase));
				result = true;
			}
			catch
			{
				value = default(TEnum);
				result = false;
			}
			return result;
		}

		public static Rect GetBoundingRect(Rect rect, Matrix matrix)
		{
			if (matrix.IsIdentity())
			{
				return rect;
			}
			Point[] array = new Point[]
			{
				new Point(rect.Left, rect.Top),
				new Point(rect.Right, rect.Top),
				new Point(rect.Right, rect.Bottom),
				new Point(rect.Left, rect.Bottom)
			};
			Helper.TransformPoints(matrix, array);
			double x = System.Math.Min(Math.Min(array[0].X, array[1].X), Math.Min(array[2].X, array[3].X));
			double x2 = Math.Max(Math.Max(array[0].X, array[1].X), Math.Max(array[2].X, array[3].X));
			double y = System.Math.Min(Math.Min(array[0].Y, array[1].Y), Math.Min(array[2].Y, array[3].Y));
			double y2 = Math.Max(Math.Max(array[0].Y, array[1].Y), Math.Max(array[2].Y, array[3].Y));
			return new Rect(new Point(x, y), new Point(x2, y2));
		}

		static void TransformPoints(Matrix matrix, Point[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Point point = matrix.Transform(points[i]);
				points[i].X = point.X;
				points[i].Y = point.Y;
			}
		}
	}
}
