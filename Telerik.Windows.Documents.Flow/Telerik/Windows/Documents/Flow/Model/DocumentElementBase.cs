using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public abstract class DocumentElementBase
	{
		internal string GetDebuggerDisplayText()
		{
			string childrenDebuggerDisplayText = this.GetChildrenDebuggerDisplayText();
			string text = this.GetPropertiesDebuggerDisplayText();
			if (!string.IsNullOrEmpty(text))
			{
				text = " " + text;
			}
			string arg = DocumentElementBase.DebugVisualizer.EscapeXmlContent(this.Type);
			if (childrenDebuggerDisplayText == null)
			{
				return string.Format("<{0}{1}/>", arg, text);
			}
			return string.Format("<{0}{1}>{2}</{0}>", arg, text, childrenDebuggerDisplayText);
		}

		internal virtual string GetChildrenDebuggerDisplayText()
		{
			if (!this.Children.Any<DocumentElementBase>())
			{
				return null;
			}
			return string.Join(string.Empty, from element in this.Children
				select element.GetDebuggerDisplayText());
		}

		internal virtual string GetPropertiesDebuggerDisplayText()
		{
			IElementWithProperties elementWithProperties = this as IElementWithProperties;
			if (elementWithProperties == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStyleProperty styleProperty in from property in elementWithProperties.Properties.StyleProperties
				where property.HasLocalValue
				select property)
			{
				stringBuilder.AppendFormat("{0}=\"{1}\" ", styleProperty.PropertyDefinition.PropertyName, DocumentElementBase.DebugVisualizer.EscapeXmlContent(styleProperty.GetLocalValueAsObject()));
			}
			return stringBuilder.ToString();
		}

		internal DocumentElementBase()
		{
			if (base.GetType() != typeof(RadFlowDocument))
			{
				throw new InvalidOperationException("This constructor should be used for RadFlowDocument only.");
			}
		}

		internal DocumentElementBase(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
		}

		public virtual RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public DocumentElementBase Parent
		{
			get
			{
				return this.parent;
			}
		}

		internal abstract IEnumerable<DocumentElementBase> Children { get; }

		internal virtual IEnumerable<DocumentElementBase> ContentChildren
		{
			get
			{
				return this.Children;
			}
		}

		internal abstract DocumentElementType Type { get; }

		public IEnumerable<T> EnumerateChildrenOfType<T>() where T : DocumentElementBase
		{
			return this.EnumerateChildrenOfTypeInternal<T>(this.Children);
		}

		internal IEnumerable<T> EnumerateContentChildrenOfType<T>() where T : DocumentElementBase
		{
			return this.EnumerateChildrenOfTypeInternal<T>(this.ContentChildren);
		}

		internal void SetParent(DocumentElementBase newParent)
		{
			if (this.Parent == newParent)
			{
				return;
			}
			DocumentElementBase documentElementBase = this.Parent;
			this.parent = newParent;
			if (documentElementBase != null)
			{
				documentElementBase.OnChildRemoved(this);
			}
			if (newParent != null)
			{
				newParent.OnChildAdded(this);
			}
		}

		internal IEnumerable<T> EnumerateParentsOfType<T>() where T : DocumentElementBase
		{
			for (DocumentElementBase parent = this; parent != null; parent = parent.Parent)
			{
				if (parent is T)
				{
					yield return (T)((object)parent);
				}
			}
			yield break;
		}

		internal abstract DocumentElementBase CloneCore(CloneContext cloneContext);

		internal virtual void OnAfterCloneCore(CloneContext cloneContext, DocumentElementBase clonedElement)
		{
		}

		protected virtual void OnChildAdded(DocumentElementBase child)
		{
		}

		protected virtual void OnChildRemoved(DocumentElementBase child)
		{
		}

		static IEnumerable<T> EnumerateChildrenOrSelfOfType<T>(DocumentElementBase element) where T : DocumentElementBase
		{
			if (element is T)
			{
				yield return (T)((object)element);
			}
			if (DocumentElementBase.CanHaveChildOfType(element.GetType(), typeof(T)))
			{
				foreach (DocumentElementBase child in element.Children)
				{
					foreach (T subChild in DocumentElementBase.EnumerateChildrenOrSelfOfType<T>(child))
					{
						yield return subChild;
					}
				}
			}
			yield break;
		}

		static bool CanHaveChildOfType(Type parentType, Type childType)
		{
			return !DocumentElementBase.ParagraphType.IsAssignableFrom(parentType) || DocumentElementBase.InlineType.IsAssignableFrom(childType) || childType.IsAssignableFrom(DocumentElementBase.InlineType);
		}

		IEnumerable<T> EnumerateChildrenOfTypeInternal<T>(IEnumerable<DocumentElementBase> children) where T : DocumentElementBase
		{
			if (DocumentElementBase.CanHaveChildOfType(base.GetType(), typeof(T)))
			{
				foreach (DocumentElementBase child in children)
				{
					foreach (T subChild in DocumentElementBase.EnumerateChildrenOrSelfOfType<T>(child))
					{
						yield return subChild;
					}
				}
			}
			yield break;
		}

		static readonly Type ParagraphType = typeof(Paragraph);

		static readonly Type InlineType = typeof(InlineBase);

		readonly RadFlowDocument document;

		DocumentElementBase parent;

		protected static class DebugVisualizer
		{
			public static string EscapeXmlContent(object stringContent)
			{
				if (stringContent == null)
				{
					return null;
				}
				return stringContent.ToString().Replace("&", "&amp;").Replace("<", "&lt;")
					.Replace(">", "&gt;")
					.Replace("\"", "&quot;")
					.Replace("'", "&apos;")
					.ToString();
			}
		}
	}
}
