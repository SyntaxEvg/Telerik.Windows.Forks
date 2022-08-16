using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class TypeAwareConverterWithParameter<T, P> : Converter where T : PdfPrimitive where P : PdfPrimitive
	{
		public TypeAwareConverterWithParameter(Func<T, P, object> factoryFunction, string typeProperty, string parameterProperty)
		{
			Guard.ThrowExceptionIfNull<Func<T, P, object>>(factoryFunction, "factoryFunction");
			Guard.ThrowExceptionIfNullOrEmpty(typeProperty, "typeProperty");
			Guard.ThrowExceptionIfNullOrEmpty(parameterProperty, "parameterProperty");
			this.factoryFunction = factoryFunction;
			this.typeProperty = typeProperty;
			this.parameterProperty = parameterProperty;
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
				P arg2;
				dictionary.TryGetElement<P>(reader, context, this.parameterProperty, out arg2);
				return this.factoryFunction(arg, arg2);
			}
			return base.CreateInstance(type, reader, context, dictionary);
		}

		readonly Func<T, P, object> factoryFunction;

		readonly string typeProperty;

		readonly string parameterProperty;
	}
}
