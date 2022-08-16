using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	static class PdfObjectsExtensions
	{
		public static P ToPrimitive<P, T>(this PdfProperty<T> pdfProperty, Func<T, P> convertToPrimitive, Func<P> getDefaultValue = null) where P : IPdfSharedObject
		{
			P result = default(P);
			if (pdfProperty.HasValue)
			{
				result = convertToPrimitive(pdfProperty.Value);
			}
			else if (getDefaultValue != null)
			{
				result = getDefaultValue();
			}
			return result;
		}

		public static void CopyToProperty<P, T>(this P primitive, PdfProperty<T> pdfProperty, Func<P, T> getValueFromPrimitive) where P : IPdfSharedObject
		{
			if (primitive != null)
			{
				pdfProperty.Value = getValueFromPrimitive(primitive);
			}
		}

		public static V GetInheritableProperty<T, V>(this ITreeNode<T> node, Func<T, V> getPropertyFromNodeValue, V defautValue = default(V)) where V : class
		{
			ITreeNode<T> treeNode = node;
			V v = default(V);
			while (treeNode != null && v == null)
			{
				v = getPropertyFromNodeValue(treeNode.NodeValue);
				treeNode = treeNode.ParentNode;
			}
			V result;
			if ((result = v) == null)
			{
				result = defautValue;
			}
			return result;
		}

		public static PdfArray ToPdfArray(this IEnumerable<int> array)
		{
			if (array == null)
			{
				return null;
			}
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (int defaultValue in array)
			{
				pdfArray.Add(new PdfInt(defaultValue));
			}
			return pdfArray;
		}

		public static PdfArray ToPdfArray(this IEnumerable<double> array)
		{
			if (array == null)
			{
				return null;
			}
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (double num in array)
			{
				double d = num;
				pdfArray.Add(d.ToPdfReal());
			}
			return pdfArray;
		}

		public static string ToText(this PrimitiveWrapper textWrapper)
		{
			string result = null;
			if (textWrapper != null && (textWrapper.Type == PdfElementType.String || textWrapper.Type == PdfElementType.PdfName))
			{
				result = textWrapper.Primitive.ToString();
			}
			return result;
		}

		public static V[] ToArray<P, V>(this PdfArray array, PostScriptReader reader, IPdfImportContext context) where P : PdfSimpleType<V>
		{
			if (array == null)
			{
				return null;
			}
			V[] array2 = new V[array.Count];
			for (int i = 0; i < array2.Length; i++)
			{
				P p;
				array.TryGetElement<P>(reader, context, i, out p);
				if (p != null)
				{
					array2[i] = p.Value;
				}
			}
			return array2;
		}

		public static double[] ToDoubleArray(this PdfArray array)
		{
			return array.ToArray((PdfPrimitive primitive) => primitive.ToReal());
		}

		public static int[] ToIntArray(this PdfArray array)
		{
			return array.ToArray((PdfPrimitive primitive) => (int)primitive.ToReal());
		}

		public static double ToReal(this IPdfSharedObject value)
		{
			double result;
			if (value.IsOldSchema)
			{
				Helper.UnboxDouble(value, out result);
			}
			else
			{
				result = PdfObjectsExtensions.ToReal(value as PdfPrimitive);
			}
			return result;
		}

		static double ToReal(PdfPrimitive value)
		{
			PdfReal pdfReal = value as PdfReal;
			if (pdfReal != null)
			{
				return pdfReal.Value;
			}
			PdfInt pdfInt = value as PdfInt;
			if (pdfInt != null)
			{
				return (double)pdfInt.Value;
			}
			PdfBool pdfBool = value as PdfBool;
			if (pdfBool == null)
			{
				throw new ArgumentException(string.Format("The {0} type cannot be converted to a real numeric value.", value.GetType().Name));
			}
			if (!pdfBool.Value)
			{
				return 0.0;
			}
			return 1.0;
		}

		public static string[] ToStringArray(this PdfArray array)
		{
			return array.ToArray((PdfPrimitive primitive) => primitive.ToString());
		}

		static T[] ToArray<T>(this PdfArray array, Func<PdfPrimitive, T> primitiveConverter)
		{
			T[] array2 = new T[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = primitiveConverter(array[i]);
			}
			return array2;
		}

		public static PdfArray ToPdfArray(this Rect rect)
		{
			return new PdfArray(new PdfPrimitive[0])
			{
				rect.X.ToPdfReal(),
				rect.Y.ToPdfReal(),
				(rect.X + rect.Width).ToPdfReal(),
				(rect.Y + rect.Height).ToPdfReal()
			};
		}

		public static PdfArray ToPointPdfArray(this Rect rect)
		{
			return new PdfArray(new PdfPrimitive[0])
			{
				Unit.DipToPoint(rect.X).ToPdfReal(),
				Unit.DipToPoint(rect.Y).ToPdfReal(),
				Unit.DipToPoint(rect.Right).ToPdfReal(),
				Unit.DipToPoint(rect.Bottom).ToPdfReal()
			};
		}

		public static Rect ToDipRect(this PdfArray array, PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfArray>(array, "array");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfReal pdfReal;
			array.TryGetElement<PdfReal>(reader, context, 0, out pdfReal);
			PdfReal pdfReal2;
			array.TryGetElement<PdfReal>(reader, context, 1, out pdfReal2);
			PdfReal pdfReal3;
			array.TryGetElement<PdfReal>(reader, context, 2, out pdfReal3);
			PdfReal pdfReal4;
			array.TryGetElement<PdfReal>(reader, context, 3, out pdfReal4);
			return new Rect(new Point(Unit.PointToDip(pdfReal.Value), Unit.PointToDip(pdfReal2.Value)), new Point(Unit.PointToDip(pdfReal3.Value), Unit.PointToDip(pdfReal4.Value)));
		}

		public static Matrix ToMatrix(this PdfArray array, PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfArray>(array, "array");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfReal pdfReal;
			array.TryGetElement<PdfReal>(reader, context, 0, out pdfReal);
			PdfReal pdfReal2;
			array.TryGetElement<PdfReal>(reader, context, 1, out pdfReal2);
			PdfReal pdfReal3;
			array.TryGetElement<PdfReal>(reader, context, 2, out pdfReal3);
			PdfReal pdfReal4;
			array.TryGetElement<PdfReal>(reader, context, 3, out pdfReal4);
			PdfReal pdfReal5;
			array.TryGetElement<PdfReal>(reader, context, 4, out pdfReal5);
			PdfReal pdfReal6;
			array.TryGetElement<PdfReal>(reader, context, 5, out pdfReal6);
			return new Matrix
			{
				M11 = pdfReal.Value,
				M12 = pdfReal2.Value,
				M21 = pdfReal3.Value,
				M22 = pdfReal4.Value,
				OffsetX = pdfReal5.Value,
				OffsetY = pdfReal6.Value
			};
		}

		public static Rect ToRect(this PdfArray array, PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfArray>(array, "array");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			PdfReal x;
			array.TryGetElement<PdfReal>(reader, context, 0, out x);
			PdfReal y;
			array.TryGetElement<PdfReal>(reader, context, 1, out y);
			PdfReal x2;
			array.TryGetElement<PdfReal>(reader, context, 2, out x2);
			PdfReal y2;
			array.TryGetElement<PdfReal>(reader, context, 3, out y2);
			return PdfObjectsExtensions.CreateRectFromArrayValues(x, y, x2, y2);
		}

		public static Rect CreateRectFromArrayValues(PdfReal x1, PdfReal y1, PdfReal x2, PdfReal y2)
		{
			return new Rect(new Point(x1.Value, y1.Value), new Point(x2.Value, y2.Value));
		}

		public static PdfArray ToPdfArray(this Matrix matrix)
		{
			return new PdfArray(new PdfPrimitive[0])
			{
				matrix.M11.ToPdfReal(),
				matrix.M12.ToPdfReal(),
				matrix.M21.ToPdfReal(),
				matrix.M22.ToPdfReal(),
				matrix.OffsetX.ToPdfReal(),
				matrix.OffsetY.ToPdfReal()
			};
		}

		public static PdfArray ToPdfArray(this string[] array)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (string str in array)
			{
				pdfArray.Add(str.ToPdfName());
			}
			return pdfArray;
		}

		public static PdfString ToPdfString(this string text)
		{
			if (text == null)
			{
				return null;
			}
			bool flag = true;
			foreach (char c in text)
			{
				if (c > '\u007f')
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return text.ToPdfLiteralString(StringType.ASCII);
			}
			return text.ToPdfHexString();
		}

		public static PdfHexString ToPdfHexString(this string text)
		{
			if (text == null)
			{
				return null;
			}
			byte[] bytes = PdfEncoding.Encoding.GetBytes(text);
			return new PdfHexString(bytes);
		}

		public static PdfHexString ToPdfHexString(this byte[] array)
		{
			return new PdfHexString(array);
		}

		public static PdfName ToPdfName(this string str)
		{
			return new PdfName(str);
		}

		public static PdfLiteralString ToPdfLiteralString(this string str, StringType type)
		{
			if (str == null)
			{
				return null;
			}
			return new PdfLiteralString(str, type);
		}

		public static PdfBool ToPdfBool(this bool value)
		{
			return new PdfBool(value);
		}

		public static PdfInt ToPdfInt(this int i)
		{
			return new PdfInt(i);
		}

		public static PdfReal ToPdfReal(this double? d)
		{
			if (d != null)
			{
				return d.Value.ToPdfReal();
			}
			return null;
		}

		public static PdfReal ToPdfReal(this double d)
		{
			return new PdfReal(Math.Round(d, 8));
		}

		public static PdfReal ToPdfReal(this int i)
		{
			return new PdfReal((double)i);
		}

		public static ColorObjectBase ToColor(this ColorBase color, IPdfContentExportContext context)
		{
			string name;
			if ((name = color.ColorSpace.Name) != null)
			{
				if (name == "DeviceRGB")
				{
					return ((RgbColor)color).ToRgbColor(context);
				}
				if (name == "Pattern")
				{
					return ((PatternColor)color).ToPatternColor(context);
				}
			}
			throw new NotSupportedColorSpaceException(color.ColorSpace.Name);
		}

		public static RgbColorObject ToRgbColor(this RgbColor color, IPdfContentExportContext context)
		{
			RgbColorObject rgbColorObject = new RgbColorObject();
			rgbColorObject.CopyPropertiesFrom(context, color);
			return rgbColorObject;
		}

		public static PatternColorObject ToPatternColor(this PatternColor color, IPdfContentExportContext context)
		{
			PatternColorObject patternColorObject = PatternColorObject.CreateInstance((int)color.PatternType);
			patternColorObject.CopyPropertiesFrom(context, color);
			return patternColorObject;
		}

		public static Matrix GetScaledPosition(this Form form, bool applyTopLeftTransformation)
		{
			double m = ((form.FormSource != null && form.FormSource.Size.Width != 0.0) ? (form.Width / form.FormSource.Size.Width) : 1.0);
			double m2 = ((form.FormSource != null && form.FormSource.Size.Height != 0.0) ? (form.Height / form.FormSource.Size.Height) : 1.0);
			Matrix matrix = new Matrix(m, 0.0, 0.0, m2, 0.0, 0.0);
			Matrix m4;
			if (applyTopLeftTransformation)
			{
				Matrix m3 = PdfObjectsExtensions.CalculateTopLeftCoordinateMatrix(form.Height);
				m4 = m3.MultiplyBy(form.Position.Matrix);
			}
			else
			{
				m4 = form.Position.Matrix;
			}
			matrix = matrix.MultiplyBy(m4);
			return matrix;
		}

		public static Matrix ToTopLeftCoordinateSystem(this IContentRootElement root, Matrix matrix)
		{
			Guard.ThrowExceptionIfNull<IContentRootElement>(root, "root");
			double rootHeight = PdfObjectsExtensions.GetRootHeight(root);
			Matrix m = PdfObjectsExtensions.CalculateTopLeftCoordinateMatrix(rootHeight);
			return matrix.MultiplyBy(m);
		}

		public static Rect ToTopLeftCoordinateSystem(this Rect rect, double rootHeightInPoints)
		{
			double rootHeightInDip = Unit.PointToDip(rootHeightInPoints);
			Matrix matrix = PdfObjectsExtensions.CalculateTopLeftCoordinateMatrix(rootHeightInDip);
			return matrix.Transform(rect);
		}

		public static PdfArray ToBottomLeftCoordinateSystem(this IContentRootElement root, Rect rect)
		{
			Guard.ThrowExceptionIfNull<IContentRootElement>(root, "root");
			double rootHeight = PdfObjectsExtensions.GetRootHeight(root);
			double rootHeightInPoint = Unit.DipToPoint(rootHeight);
			Matrix matrix = PdfObjectsExtensions.CalculateBottomLeftCoordinateMatrix(rootHeightInPoint);
			Rect rect2 = matrix.Transform(rect);
			return rect2.ToPdfArray();
		}

		public static PdfArray ToBottomLeftCoordinateSystem(this Rect rect, double rootHeightInDip)
		{
			double rootHeightInPoint = Unit.DipToPoint(rootHeightInDip);
			Matrix matrix = PdfObjectsExtensions.CalculateBottomLeftCoordinateMatrix(rootHeightInPoint);
			Rect rect2 = matrix.Transform(rect);
			return rect2.ToPdfArray();
		}

		static Matrix CalculateTopLeftCoordinateMatrix(double rootHeightInDip)
		{
			Matrix result = new Matrix(PdfObjectsExtensions.PointToDipConversion, 0.0, 0.0, -PdfObjectsExtensions.PointToDipConversion, 0.0, rootHeightInDip);
			return result;
		}

		static Matrix CalculateBottomLeftCoordinateMatrix(double rootHeightInPoint)
		{
			Matrix result = new Matrix(PdfObjectsExtensions.DipToPointConversion, 0.0, 0.0, -PdfObjectsExtensions.DipToPointConversion, 0.0, rootHeightInPoint);
			return result;
		}

		static double GetRootHeight(IContentRootElement root)
		{
			bool flag = root is RadFixedPage || root is FormSource;
			if (flag)
			{
				return root.Size.Height;
			}
			return 0.0;
		}

		static readonly double PointToDipConversion = Unit.PointToDip(1.0);

		static readonly double DipToPointConversion = Unit.DipToPoint(1.0);
	}
}
