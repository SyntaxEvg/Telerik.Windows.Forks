using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public abstract class DocumentElementPropertiesBase : IElementWithStyle
	{
		internal DocumentElementPropertiesBase(Style style)
		{
			this.SetOwnerStyle(style);
			this.InitProperties();
		}

		internal DocumentElementPropertiesBase(DocumentElementBase documentElement, bool suppressStylePropertyEvaluation)
		{
			this.suppressStylePropertyEvaluation = suppressStylePropertyEvaluation;
			this.SetOwnerDocumentElement(documentElement);
			this.InitProperties();
		}

		internal DocumentElementPropertiesBase(ListLevel listLevel, bool suppressStylePropertyEvaluation)
		{
			this.suppressStylePropertyEvaluation = suppressStylePropertyEvaluation;
			this.SetOwnerListLevel(listLevel);
			this.InitProperties();
		}

		public IEnumerable<IStyleProperty> StyleProperties
		{
			get
			{
				foreach (IStyleProperty styleProperty in this.EnumerateStyleProperties())
				{
					yield return styleProperty;
				}
				yield return this.styleIdProperty;
				yield break;
			}
		}

		public string StyleId
		{
			get
			{
				return this.styleIdProperty.GetActualValue();
			}
			set
			{
				this.styleIdProperty.LocalValue = value;
			}
		}

		internal DocumentElementBase OwnerDocumentElement
		{
			get
			{
				return this.ownerDocumentElement;
			}
		}

		internal bool SuppressStylePropertyEvaluation
		{
			get
			{
				return this.suppressStylePropertyEvaluation;
			}
		}

		internal Style OwnerStyle
		{
			get
			{
				return this.ownerStyle;
			}
		}

		internal ListLevel OwnerListLevel
		{
			get
			{
				return this.ownerListLevel;
			}
		}

		internal RadFlowDocument Document
		{
			get
			{
				if (this.OwnerDocumentElement != null)
				{
					return this.OwnerDocumentElement.Document;
				}
				if (this.OwnerStyle != null)
				{
					return this.OwnerStyle.Document;
				}
				if (this.OwnerListLevel != null)
				{
					return this.OwnerListLevel.Document;
				}
				return null;
			}
		}

		protected IStyleProperty StyleIdProperty
		{
			get
			{
				return this.styleIdProperty;
			}
		}

		public IStyleProperty GetStyleProperty(IStylePropertyDefinition propertyDefinition)
		{
			return this.GetStylePropertyOverride(propertyDefinition);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public IStyleProperty GetStyleProperty(string propertyName)
		{
			return this.StyleProperties.FirstOrDefault((IStyleProperty p) => p.PropertyDefinition.PropertyName == propertyName);
		}

		public virtual void CopyPropertiesFrom(DocumentElementPropertiesBase fromProperties)
		{
			if (fromProperties == null || fromProperties.GetType() != base.GetType())
			{
				return;
			}
			foreach (IStyleProperty styleProperty in fromProperties.StyleProperties)
			{
				if (styleProperty.HasLocalValue)
				{
					IStyleProperty styleProperty2 = this.GetStyleProperty(styleProperty.PropertyDefinition);
					styleProperty2.SetValueAsObject(styleProperty.GetLocalValueAsObject());
				}
			}
		}

		public void ClearLocalValues()
		{
			foreach (IStyleProperty styleProperty in this.StyleProperties)
			{
				if (styleProperty.HasLocalValue)
				{
					styleProperty.ClearValue();
				}
			}
		}

		public bool HasLocalValues()
		{
			return this.StyleProperties.Any((IStyleProperty p) => p.HasLocalValue);
		}

		protected abstract IEnumerable<IStyleProperty> EnumerateStyleProperties();

		protected abstract IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition);

		void InitProperties()
		{
			this.styleIdProperty = new LocalProperty<string>(DocumentElementPropertiesBase.StyleIdPropertyDefinition, this);
		}

		void SetOwnerStyle(Style style)
		{
			if (this.ownerStyle != null || this.ownerDocumentElement != null || this.ownerListLevel != null)
			{
				throw new InvalidOperationException("Owner cannot be set more than once.");
			}
			Guard.ThrowExceptionIfNull<Style>(style, "style");
			this.ownerStyle = style;
		}

		void SetOwnerDocumentElement(DocumentElementBase documentElement)
		{
			if (this.ownerStyle != null || this.ownerDocumentElement != null || this.ownerListLevel != null)
			{
				throw new InvalidOperationException("Owner cannot be set more than once.");
			}
			Guard.ThrowExceptionIfNull<DocumentElementBase>(documentElement, "document element");
			this.ownerDocumentElement = documentElement;
		}

		void SetOwnerListLevel(ListLevel listLevel)
		{
			if (this.ownerStyle != null || this.ownerDocumentElement != null || this.ownerListLevel != null)
			{
				throw new InvalidOperationException("Owner cannot be set more than once.");
			}
			Guard.ThrowExceptionIfNull<ListLevel>(listLevel, "list level");
			this.ownerListLevel = listLevel;
		}

		protected static readonly StylePropertyDefinition<string> StyleIdPropertyDefinition = new StylePropertyDefinition<string>("StyleID", string.Empty, StylePropertyType.DocumentElement);

		readonly bool suppressStylePropertyEvaluation;

		Style ownerStyle;

		DocumentElementBase ownerDocumentElement;

		ListLevel ownerListLevel;

		LocalProperty<string> styleIdProperty;
	}
}
