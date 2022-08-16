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
		public static P ToPrimitive<P, T>(this global::Telerik.Windows.Documents.Fixed.Model.Common.PdfProperty<T> pdfProperty, global::System.Func<T, P> convertToPrimitive, global::System.Func<P> getDefaultValue = null) where P : global::Telerik.Windows.Documents.Fixed.FormatProviders.IPdfSharedObject
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

		public static void CopyToProperty<P, T>(this P primitive, global::Telerik.Windows.Documents.Fixed.Model.Common.PdfProperty<T> pdfProperty, global::System.Func<P, T> getValueFromPrimitive) where P : global::Telerik.Windows.Documents.Fixed.FormatProviders.IPdfSharedObject
		{
			if (primitive != null)
			{
				pdfProperty.Value = getValueFromPrimitive(primitive);
			}
		}

		public static V GetInheritableProperty<T, V>(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.ITreeNode<T> node, global::System.Func<T, V> getPropertyFromNodeValue, V defautValue = default(V)) where V : class
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.ITreeNode<T> treeNode = node;
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

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToPdfArray(this global::System.Collections.Generic.IEnumerable<int> array)
		{
			if (array == null)
			{
				return null;
			}
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray pdfArray = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[0]);
			foreach (int defaultValue in array)
			{
				pdfArray.Add(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt(defaultValue));
			}
			return pdfArray;
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToPdfArray(this global::System.Collections.Generic.IEnumerable<double> array)
		{
			if (array == null)
			{
				return null;
			}
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray pdfArray = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[0]);
			foreach (double num in array)
			{
				double d = num;
				pdfArray.Add(d.ToPdfReal());
			}
			return pdfArray;
		}

		public static string ToText(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PrimitiveWrapper textWrapper)
		{
			string result = null;
			if (textWrapper != null && (textWrapper.Type == global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType.String || textWrapper.Type == global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType.PdfName))
			{
				result = textWrapper.Primitive.ToString();
			}
			return result;
		}

		public static V[] ToArray<P, V>(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context) where P : global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfSimpleType<V>
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

		public static double[] ToDoubleArray(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array)
		{
			return array.ToArray((global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive primitive) => primitive.ToReal());
		}

		public static int[] ToIntArray(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array)
		{
			return array.ToArray((global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive primitive) => (int)primitive.ToReal());
		}

		public static double ToReal(this global::Telerik.Windows.Documents.Fixed.FormatProviders.IPdfSharedObject value)
		{
			double result;
			if (value.IsOldSchema)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils.Helper.UnboxDouble(value, out result);
			}
			else
			{
				result = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.ToReal(value as global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive);
			}
			return result;
		}

		private static double ToReal(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive value)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal = value as global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal;
			if (pdfReal != null)
			{
				return pdfReal.Value;
			}
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt pdfInt = value as global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt;
			if (pdfInt != null)
			{
				return (double)pdfInt.Value;
			}
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfBool pdfBool = value as global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfBool;
			if (pdfBool == null)
			{
				throw new global::System.ArgumentException(string.Format("The {0} type cannot be converted to a real numeric value.", value.GetType().Name));
			}
			if (!pdfBool.Value)
			{
				return 0.0;
			}
			return 1.0;
		}

		public static string[] ToStringArray(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array)
		{
			return array.ToArray((global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive primitive) => primitive.ToString());
		}

		private static T[] ToArray<T>(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array, global::System.Func<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive, T> primitiveConverter)
		{
			T[] array2 = new T[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = primitiveConverter(array[i]);
			}
			return array2;
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToPdfArray(this global::System.Windows.Rect rect)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[0])
			{
				rect.X.ToPdfReal(),
				rect.Y.ToPdfReal(),
				(rect.X + rect.Width).ToPdfReal(),
				(rect.Y + rect.Height).ToPdfReal()
			};
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToPointPdfArray(this global::System.Windows.Rect rect)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[0])
			{
				global::Telerik.Windows.Documents.Media.Unit.DipToPoint(rect.X).ToPdfReal(),
				global::Telerik.Windows.Documents.Media.Unit.DipToPoint(rect.Y).ToPdfReal(),
				global::Telerik.Windows.Documents.Media.Unit.DipToPoint(rect.Right).ToPdfReal(),
				global::Telerik.Windows.Documents.Media.Unit.DipToPoint(rect.Bottom).ToPdfReal()
			};
		}

		public static global::System.Windows.Rect ToDipRect(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray>(array, "array");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader>(reader, "reader");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext>(context, "context");
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 0, out pdfReal);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal2;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 1, out pdfReal2);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal3;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 2, out pdfReal3);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal4;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 3, out pdfReal4);
			return new global::System.Windows.Rect(new global::System.Windows.Point(global::Telerik.Windows.Documents.Media.Unit.PointToDip(pdfReal.Value), global::Telerik.Windows.Documents.Media.Unit.PointToDip(pdfReal2.Value)), new global::System.Windows.Point(global::Telerik.Windows.Documents.Media.Unit.PointToDip(pdfReal3.Value), global::Telerik.Windows.Documents.Media.Unit.PointToDip(pdfReal4.Value)));
		}

		public static global::System.Windows.Media.Matrix ToMatrix(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray>(array, "array");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader>(reader, "reader");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext>(context, "context");
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 0, out pdfReal);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal2;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 1, out pdfReal2);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal3;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 2, out pdfReal3);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal4;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 3, out pdfReal4);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal5;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 4, out pdfReal5);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal pdfReal6;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 5, out pdfReal6);
			return new global::System.Windows.Media.Matrix
			{
				M11 = pdfReal.Value,
				M12 = pdfReal2.Value,
				M21 = pdfReal3.Value,
				M22 = pdfReal4.Value,
				OffsetX = pdfReal5.Value,
				OffsetY = pdfReal6.Value
			};
		}

		public static global::System.Windows.Rect ToRect(this global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray array, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray>(array, "array");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader>(reader, "reader");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext>(context, "context");
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal x;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 0, out x);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal y;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 1, out y);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal x2;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 2, out x2);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal y2;
			array.TryGetElement<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal>(reader, context, 3, out y2);
			return global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.CreateRectFromArrayValues(x, y, x2, y2);
		}

		public static global::System.Windows.Rect CreateRectFromArrayValues(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal x1, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal y1, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal x2, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal y2)
		{
			return new global::System.Windows.Rect(new global::System.Windows.Point(x1.Value, y1.Value), new global::System.Windows.Point(x2.Value, y2.Value));
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToPdfArray(this global::System.Windows.Media.Matrix matrix)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[0])
			{
				matrix.M11.ToPdfReal(),
				matrix.M12.ToPdfReal(),
				matrix.M21.ToPdfReal(),
				matrix.M22.ToPdfReal(),
				matrix.OffsetX.ToPdfReal(),
				matrix.OffsetY.ToPdfReal()
			};
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToPdfArray(this string[] array)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray pdfArray = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[0]);
			foreach (string str in array)
			{
				pdfArray.Add(str.ToPdfName());
			}
			return pdfArray;
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfString ToPdfString(this string text)
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
				return text.ToPdfLiteralString(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.StringType.ASCII);
			}
			return text.ToPdfHexString();
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfHexString ToPdfHexString(this string text)
		{
			if (text == null)
			{
				return null;
			}
			byte[] bytes = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfEncoding.Encoding.GetBytes(text);
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfHexString(bytes);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfHexString ToPdfHexString(this byte[] array)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfHexString(array);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfName ToPdfName(this string str)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfName(str);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfLiteralString ToPdfLiteralString(this string str, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.StringType type)
		{
			if (str == null)
			{
				return null;
			}
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfLiteralString(str, type);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfBool ToPdfBool(this bool value)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfBool(value);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt ToPdfInt(this int i)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt(i);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal ToPdfReal(this double? d)
		{
			if (d != null)
			{
				return d.Value.ToPdfReal();
			}
			return null;
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal ToPdfReal(this double d)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal(global::System.Math.Round(d, 8));
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal ToPdfReal(this int i)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfReal((double)i);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces.ColorObjectBase ToColor(this global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase color, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext context)
		{
			string name;
			if ((name = color.ColorSpace.Name) != null)
			{
				if (name == "DeviceRGB")
				{
					return ((global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor)color).ToRgbColor(context);
				}
				if (name == "Pattern")
				{
					return ((global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.PatternColor)color).ToPatternColor(context);
				}
			}
			throw new global::Telerik.Windows.Documents.Fixed.Exceptions.NotSupportedColorSpaceException(color.ColorSpace.Name);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces.RgbColorObject ToRgbColor(this global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor color, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext context)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces.RgbColorObject rgbColorObject = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces.RgbColorObject();
			rgbColorObject.CopyPropertiesFrom(context, color);
			return rgbColorObject;
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.PatternColorObject ToPatternColor(this global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.PatternColor color, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext context)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.PatternColorObject patternColorObject = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns.PatternColorObject.CreateInstance((int)color.PatternType);
			patternColorObject.CopyPropertiesFrom(context, color);
			return patternColorObject;
		}

		public static global::System.Windows.Media.Matrix GetScaledPosition(this global::Telerik.Windows.Documents.Fixed.Model.Objects.Form form, bool applyTopLeftTransformation)
		{
			double m = ((form.FormSource != null && form.FormSource.Size.Width != 0.0) ? (form.Width / form.FormSource.Size.Width) : 1.0);
			double m2 = ((form.FormSource != null && form.FormSource.Size.Height != 0.0) ? (form.Height / form.FormSource.Size.Height) : 1.0);
			global::System.Windows.Media.Matrix matrix = new global::System.Windows.Media.Matrix(m, 0.0, 0.0, m2, 0.0, 0.0);
			global::System.Windows.Media.Matrix m4;
			if (applyTopLeftTransformation)
			{
				global::System.Windows.Media.Matrix m3 = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.CalculateTopLeftCoordinateMatrix(form.Height);
				m4 = m3.MultiplyBy(form.Position.Matrix);
			}
			else
			{
				m4 = form.Position.Matrix;
			}
			matrix = matrix.MultiplyBy(m4);
			return matrix;
		}

		public static global::System.Windows.Media.Matrix ToTopLeftCoordinateSystem(this global::Telerik.Windows.Documents.Fixed.Model.Common.IContentRootElement root, global::System.Windows.Media.Matrix matrix)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Common.IContentRootElement>(root, "root");
			double rootHeight = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.GetRootHeight(root);
			global::System.Windows.Media.Matrix m = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.CalculateTopLeftCoordinateMatrix(rootHeight);
			return matrix.MultiplyBy(m);
		}

		public static global::System.Windows.Rect ToTopLeftCoordinateSystem(this global::System.Windows.Rect rect, double rootHeightInPoints)
		{
			double rootHeightInDip = global::Telerik.Windows.Documents.Media.Unit.PointToDip(rootHeightInPoints);
			global::System.Windows.Media.Matrix matrix = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.CalculateTopLeftCoordinateMatrix(rootHeightInDip);
			return matrix.Transform(rect);
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToBottomLeftCoordinateSystem(this global::Telerik.Windows.Documents.Fixed.Model.Common.IContentRootElement root, global::System.Windows.Rect rect)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Common.IContentRootElement>(root, "root");
			double rootHeight = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.GetRootHeight(root);
			double rootHeightInPoint = global::Telerik.Windows.Documents.Media.Unit.DipToPoint(rootHeight);
			global::System.Windows.Media.Matrix matrix = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.CalculateBottomLeftCoordinateMatrix(rootHeightInPoint);
			global::System.Windows.Rect rect2 = matrix.Transform(rect);
			return rect2.ToPdfArray();
		}

		public static global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray ToBottomLeftCoordinateSystem(this global::System.Windows.Rect rect, double rootHeightInDip)
		{
			double rootHeightInPoint = global::Telerik.Windows.Documents.Media.Unit.DipToPoint(rootHeightInDip);
			global::System.Windows.Media.Matrix matrix = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.CalculateBottomLeftCoordinateMatrix(rootHeightInPoint);
			global::System.Windows.Rect rect2 = matrix.Transform(rect);
			return rect2.ToPdfArray();
		}

		private static global::System.Windows.Media.Matrix CalculateTopLeftCoordinateMatrix(double rootHeightInDip)
		{
			global::System.Windows.Media.Matrix result = new global::System.Windows.Media.Matrix(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.PointToDipConversion, 0.0, 0.0, -global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.PointToDipConversion, 0.0, rootHeightInDip);
			return result;
		}

		private static global::System.Windows.Media.Matrix CalculateBottomLeftCoordinateMatrix(double rootHeightInPoint)
		{
			global::System.Windows.Media.Matrix result = new global::System.Windows.Media.Matrix(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.DipToPointConversion, 0.0, 0.0, -global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.PdfObjectsExtensions.DipToPointConversion, 0.0, rootHeightInPoint);
			return result;
		}

		private static double GetRootHeight(global::Telerik.Windows.Documents.Fixed.Model.Common.IContentRootElement root)
		{
			bool flag = root is global::Telerik.Windows.Documents.Fixed.Model.RadFixedPage || root is global::Telerik.Windows.Documents.Fixed.Model.Resources.FormSource;
			if (flag)
			{
				return root.Size.Height;
			}
			return 0.0;
		}

		private static readonly double PointToDipConversion = global::Telerik.Windows.Documents.Media.Unit.PointToDip(1.0);

		private static readonly double DipToPointConversion = global::Telerik.Windows.Documents.Media.Unit.DipToPoint(1.0);
	}
}
