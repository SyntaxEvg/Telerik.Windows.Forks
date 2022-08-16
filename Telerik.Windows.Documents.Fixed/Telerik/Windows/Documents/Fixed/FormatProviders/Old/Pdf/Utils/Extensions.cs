using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	static class Extensions
	{
		public static IEnumerable<T> EnumerateChildrenOfType<T>(IEnumerable<IContentElement> collection) where T : IContentElement
		{
			foreach (IContentElement ce in collection)
			{
				if (ce is T)
				{
					yield return (T)((object)ce);
				}
				if (ce.HasChildren)
				{
					foreach (T t in Extensions.EnumerateChildrenOfType<T>(ce.Children))
					{
						IContentElement el = t;
						yield return (T)((object)el);
					}
				}
			}
			yield break;
		}

		public static bool GetFlag(int flags, int flag)
		{
			return (flags & (1 << flag - 1)) != 0;
		}

		public static object[] ToParams<T>(this T[] array)
		{
			object[] array2 = new object[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			return array2;
		}

		public static T[] ToParams<T>(this object[] array)
		{
			T[] array2 = new T[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (T)((object)array[i]);
			}
			return array2;
		}

		public static PdfArrayOld ToPdfArray(this string[] array, PdfContentManager contentManager)
		{
			PdfArrayOld pdfArrayOld = new PdfArrayOld(contentManager);
			foreach (string obj in array)
			{
				pdfArrayOld.Add(obj);
			}
			return pdfArrayOld;
		}

		public static double[] ToDoubleArray(this PdfArrayOld array)
		{
			return array.ToArray((object primitive) => primitive.ToReal());
		}

		public static int[] ToIntArray(this PdfArrayOld array)
		{
			return array.ToArray((object primitive) => (int)primitive.ToReal());
		}

		public static string[] ToStringArray(this PdfArrayOld array)
		{
			return array.ToArray((object primitive) => primitive.ToString());
		}

		static double ToReal(this object value)
		{
			double result;
			Helper.UnboxDouble(value, out result);
			return result;
		}

		static T[] ToArray<T>(this PdfArrayOld array, Func<object, T> primitiveConverter)
		{
			T[] array2 = new T[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = primitiveConverter(array[i]);
			}
			return array2;
		}
	}
}
