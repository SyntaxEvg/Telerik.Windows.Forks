using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts
{
	abstract class AnnotationContextBase<T>
	{
		public AnnotationContextBase(RadFlowDocument document)
		{
			this.document = document;
			this.idToAnnotationImportDictionary = new Dictionary<int, T>();
			this.annotationToIdExportDictionary = new Dictionary<T, int>();
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public void RegisterAnnotationToImport(int id, T annotation)
		{
			this.idToAnnotationImportDictionary.Add(id, annotation);
		}

		public T PopRegisteredAnnotationById(int id)
		{
			T result = this.idToAnnotationImportDictionary[id];
			this.idToAnnotationImportDictionary.Remove(id);
			return result;
		}

		public void RegisterAnnotationToExport(T annotation, int id)
		{
			this.annotationToIdExportDictionary.Add(annotation, id);
		}

		public int PopIdByRegisteredAnnotation(T annotation)
		{
			int next;
			if (this.annotationToIdExportDictionary.TryGetValue(annotation, out next))
			{
				this.annotationToIdExportDictionary.Remove(annotation);
			}
			else
			{
				next = AnnotationIdGenerator.GetNext();
			}
			return next;
		}

		public int GetIdByRegisteredAnnotation(T annotation)
		{
			return this.annotationToIdExportDictionary[annotation];
		}

		readonly RadFlowDocument document;

		readonly Dictionary<int, T> idToAnnotationImportDictionary;

		readonly Dictionary<T, int> annotationToIdExportDictionary;
	}
}
