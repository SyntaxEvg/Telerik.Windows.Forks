using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class TypeAwareConverter<T> : Converter where T : PdfPrimitive
	{
		public TypeAwareConverter(Func<T, object> factoryFunction, string typeProperty = "Type")
		{
			Guard.ThrowExceptionIfNull<Func<T, object>>(factoryFunction, "factoryFunction");
			Guard.ThrowExceptionIfNullOrEmpty(typeProperty, "typeProperty");
			this.factoryFunction = factoryFunction;
			this.typeProperty = typeProperty;
		}

		protected override object CreateInstance(Type type, PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			T arg;
			if (dictionary.TryGetElement<T>(reader, context, this.typeProperty, out arg))
			{
				return this.factoryFunction(arg);
			}
			return base.CreateInstance(type, reader, context, dictionary);
		}

		readonly Func<T, object> factoryFunction;

		readonly string typeProperty;
	}
}
