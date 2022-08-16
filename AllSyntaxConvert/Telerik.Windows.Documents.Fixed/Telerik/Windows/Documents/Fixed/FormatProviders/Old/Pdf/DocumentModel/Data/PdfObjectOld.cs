using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfObjectOld : IPdfSharedObject
	{
		public PdfObjectOld(PdfContentManager contentManager)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			this.contentManager = contentManager;
			this.properties = new Dictionary<string, IPdfProperty>();
		}

		public PdfContentManager ContentManager
		{
			get
			{
				return this.contentManager;
			}
		}

		public IndirectReferenceOld Reference { get; set; }

		public bool IsOldSchema
		{
			get
			{
				return true;
			}
		}

		public bool IsLoaded { get; protected set; }

		public void Load()
		{
			this.ContentManager.LoadIndirectObject<PdfObjectOld>(this);
		}

		public virtual void Load(PdfDictionaryOld dictionary)
		{
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				IPdfProperty pdfProperty;
				if (this.properties.TryGetValue(keyValuePair.Key, out pdfProperty))
				{
					pdfProperty.SetValue(keyValuePair.Value);
				}
			}
			this.IsLoaded = true;
		}

		public virtual void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			if (this.Reference == null)
			{
				this.Reference = new IndirectReferenceOld
				{
					GenerationNumber = indirectObject.GenerationNumber,
					ObjectNumber = indirectObject.ObjectNumber
				};
			}
			if (indirectObject.Value is PdfDictionaryOld)
			{
				this.Load(indirectObject.Value as PdfDictionaryOld);
			}
		}

		public PdfDictionaryOld ToPdfDictionary()
		{
			PdfDictionaryOld pdfDictionaryOld = new PdfDictionaryOld(this.ContentManager);
			PropertyInfo[] array = base.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in array)
			{
				PdfPropertyAttribute pdfPropertyAttribute = propertyInfo.GetCustomAttributes(typeof(PdfPropertyAttribute), false).FirstOrDefault<object>() as PdfPropertyAttribute;
				if (pdfPropertyAttribute != null)
				{
					PdfObjectOld pdfObjectOld = propertyInfo.GetValue(this, null) as PdfObjectOld;
					if (pdfObjectOld != null)
					{
						string key = (string.IsNullOrEmpty(pdfPropertyAttribute.Name) ? propertyInfo.Name : pdfPropertyAttribute.Name);
						pdfDictionaryOld[key] = pdfObjectOld;
					}
				}
			}
			return pdfDictionaryOld;
		}

		public override bool Equals(object obj)
		{
			PdfObjectOld pdfObjectOld = obj as PdfObjectOld;
			return pdfObjectOld != null && this.Reference == pdfObjectOld.Reference;
		}

		public override int GetHashCode()
		{
			if (this.Reference != null)
			{
				return this.Reference.GetHashCode();
			}
			return base.GetHashCode();
		}

		protected InstantLoadProperty<T> CreateInstantLoadProperty<T>(PdfPropertyDescriptor descriptor) where T : PdfObjectOld
		{
			InstantLoadProperty<T> instantLoadProperty = new InstantLoadProperty<T>(this.ContentManager, descriptor);
			this.RegisterPdfProperty(instantLoadProperty);
			return instantLoadProperty;
		}

		protected InstantLoadProperty<T> CreateInstantLoadProperty<T>(PdfPropertyDescriptor descriptor, T initialValue) where T : PdfObjectOld
		{
			InstantLoadProperty<T> instantLoadProperty = new InstantLoadProperty<T>(this.ContentManager, descriptor, initialValue);
			this.RegisterPdfProperty(instantLoadProperty);
			return instantLoadProperty;
		}

		protected InstantLoadProperty<T> CreateInstantLoadProperty<T>(PdfPropertyDescriptor descriptor, IConverter converter) where T : PdfObjectOld
		{
			InstantLoadProperty<T> instantLoadProperty = new InstantLoadProperty<T>(this.ContentManager, descriptor, converter);
			this.RegisterPdfProperty(instantLoadProperty);
			return instantLoadProperty;
		}

		protected InstantLoadProperty<T> CreateInstantLoadProperty<T>(PdfPropertyDescriptor descriptor, T initialValue, IConverter converter) where T : PdfObjectOld
		{
			InstantLoadProperty<T> instantLoadProperty = new InstantLoadProperty<T>(this.ContentManager, descriptor, initialValue, converter);
			this.RegisterPdfProperty(instantLoadProperty);
			return instantLoadProperty;
		}

		protected LoadOnDemandProperty<T> CreateLoadOnDemandProperty<T>(PdfPropertyDescriptor descriptor) where T : PdfObjectOld
		{
			LoadOnDemandProperty<T> loadOnDemandProperty = new LoadOnDemandProperty<T>(this.ContentManager, descriptor);
			this.RegisterPdfProperty(loadOnDemandProperty);
			return loadOnDemandProperty;
		}

		protected LoadOnDemandProperty<T> CreateLoadOnDemandProperty<T>(PdfPropertyDescriptor descriptor, T initialValue) where T : PdfObjectOld
		{
			LoadOnDemandProperty<T> loadOnDemandProperty = new LoadOnDemandProperty<T>(this.ContentManager, descriptor, initialValue);
			this.RegisterPdfProperty(loadOnDemandProperty);
			return loadOnDemandProperty;
		}

		protected LoadOnDemandProperty<T> CreateLoadOnDemandProperty<T>(PdfPropertyDescriptor descriptor, IConverter converter) where T : PdfObjectOld
		{
			LoadOnDemandProperty<T> loadOnDemandProperty = new LoadOnDemandProperty<T>(this.ContentManager, descriptor, converter);
			this.RegisterPdfProperty(loadOnDemandProperty);
			return loadOnDemandProperty;
		}

		protected LoadOnDemandProperty<T> CreateLoadOnDemandProperty<T>(PdfPropertyDescriptor descriptor, T initialValue, IConverter converter) where T : PdfObjectOld
		{
			LoadOnDemandProperty<T> loadOnDemandProperty = new LoadOnDemandProperty<T>(this.ContentManager, descriptor, initialValue, converter);
			this.RegisterPdfProperty(loadOnDemandProperty);
			return loadOnDemandProperty;
		}

		void RegisterPdfProperty(string name, IPdfProperty property)
		{
			this.properties[name] = property;
		}

		void RegisterPdfProperty(IPdfProperty property)
		{
			if (property.Descriptor.IsMultipleProperties)
			{
				string[] array = property.Descriptor.Name.Split(new char[] { '|' });
				foreach (string name in array)
				{
					this.RegisterPdfProperty(name, property);
				}
				return;
			}
			this.RegisterPdfProperty(property.Descriptor.Name, property);
		}

		readonly Dictionary<string, IPdfProperty> properties;

		readonly PdfContentManager contentManager;
	}
}
