using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.FieldCode
{
	static class FieldFactory
	{
		static FieldFactory()
		{
			FieldFactory.RegisterFieldType("HYPERLINK", (RadFlowDocument doc) => new Hyperlink(doc));
			FieldFactory.RegisterFieldType("DATE", (RadFlowDocument doc) => new DateField(doc));
			FieldFactory.RegisterFieldType("TIME", (RadFlowDocument doc) => new TimeField(doc));
			FieldFactory.RegisterFieldType("IF", (RadFlowDocument doc) => new IfField(doc));
			FieldFactory.RegisterFieldType("COMPARE", (RadFlowDocument doc) => new CompareField(doc));
			FieldFactory.RegisterFieldType("DOCVARIABLE", (RadFlowDocument doc) => new DocumentVariableField(doc));
			FieldFactory.RegisterFieldType(MergeField.Type, (RadFlowDocument doc) => new MergeField(doc));
		}

		public static void RegisterFieldType(string fieldType, Func<RadFlowDocument, Field> createFieldFunc)
		{
			FieldFactory.RegisterdFields[fieldType.ToUpperInvariant()] = createFieldFunc;
		}

		internal static Field CreateField(string fieldType, RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNullOrEmpty(fieldType, "fieldType");
			Func<RadFlowDocument, Field> func = null;
			Field result;
			if (FieldFactory.RegisterdFields.TryGetValue(fieldType.ToUpperInvariant(), out func))
			{
				result = func(document);
			}
			else
			{
				result = new CustomCodeField(document);
			}
			return result;
		}

		static readonly Dictionary<string, Func<RadFlowDocument, Field>> RegisterdFields = new Dictionary<string, Func<RadFlowDocument, Field>>();
	}
}
