using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public class StyleRepository
	{
		internal StyleRepository(RadFlowDocument document)
		{
			this.Document = document;
		}

		public RadFlowDocument Document { get; set; }

		public IEnumerable<Style> Styles
		{
			get
			{
				return this.stylesDictionary.Values;
			}
		}

		public void Add(Style style)
		{
			this.EnsureStyleRegistered(style);
		}

		public void Remove(Style style)
		{
			Guard.ThrowExceptionIfNull<Style>(style, "style");
			if (style.Document != this.Document)
			{
				throw new InvalidOperationException("Style should be from the same document repository.");
			}
			this.Remove(style.Id);
		}

		public void Remove(string styleId)
		{
			Style valueOrNull = this.stylesDictionary.GetValueOrNull(styleId);
			if (valueOrNull == null)
			{
				return;
			}
			this.stylesDictionary.Remove(styleId);
			valueOrNull.Document = null;
			if (valueOrNull.IsDefault)
			{
				Style valueOrNull2 = this.defaultStyles.GetValueOrNull(valueOrNull.StyleType);
				if (valueOrNull == valueOrNull2)
				{
					this.defaultStyles.Remove(valueOrNull.StyleType);
				}
			}
		}

		public Style GetStyle(string styleId)
		{
			return this.stylesDictionary.GetValueOrNull(styleId);
		}

		public Style GetDefaultStyle(StyleType type)
		{
			return this.defaultStyles.GetValueOrNull(type);
		}

		public bool Contains(string styleId)
		{
			return this.stylesDictionary.ContainsKey(styleId);
		}

		public void Clear()
		{
			foreach (string styleId in this.stylesDictionary.Keys.ToList<string>())
			{
				this.Remove(styleId);
			}
		}

		public Style AddBuiltInStyle(string styleId)
		{
			if (!BuiltInStyles.IsBuiltInStyle(styleId))
			{
				throw new ArgumentException("The style with ID '{0}' is not built-in style.", styleId);
			}
			Style style = this.GetStyle(styleId);
			if (style != null)
			{
				return style;
			}
			style = BuiltInStyles.GetStyle(styleId);
			if (style != null)
			{
				this.Add(style);
			}
			return style;
		}

		internal void EnsureStyleRegistered(Style style)
		{
			if (style.Document != null && style.Document != this.Document)
			{
				throw new InvalidOperationException("Style from another document added to repository.");
			}
			Style valueOrNull = this.stylesDictionary.GetValueOrNull(style.Id);
			if (valueOrNull == null)
			{
				this.AddInternal(style);
				return;
			}
			if (valueOrNull != style)
			{
				this.Remove(valueOrNull.Id);
				this.AddInternal(style);
			}
		}

		internal IEnumerable<Style> GetSortedTopologicallyStyles()
		{
			HashSet<Style> visited = new HashSet<Style>();
			List<Style> result = new List<Style>();
			foreach (Style style in this.Styles)
			{
				StyleRepository.TopologicalSortVisit(style, this, visited, result);
			}
			return result;
		}

		internal void AddClonedStylesFrom(StyleRepository fromStyleCollection)
		{
			Guard.ThrowExceptionIfNull<StyleRepository>(fromStyleCollection, "fromStyleCollection");
			foreach (Style style in fromStyleCollection.Styles)
			{
				this.Add(style.Clone());
			}
		}

		internal void Merge(StyleRepository withStyleRepository, CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<StyleRepository>(withStyleRepository, "withStyleRepository");
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			StyleRepositoryMerger styleRepositoryMerger = new StyleRepositoryMerger(this, withStyleRepository);
			styleRepositoryMerger.Merge(cloneContext);
		}

		static void TopologicalSortVisit(Style style, StyleRepository styleCollection, HashSet<Style> visited, List<Style> result)
		{
			if (!visited.Contains(style))
			{
				visited.Add(style);
				if (!string.IsNullOrEmpty(style.BasedOnStyleId))
				{
					Style style2 = styleCollection.GetStyle(style.BasedOnStyleId);
					if (style2 != null)
					{
						StyleRepository.TopologicalSortVisit(style2, styleCollection, visited, result);
					}
				}
				if (style.StyleType == StyleType.Character && !string.IsNullOrEmpty(style.LinkedStyleId))
				{
					Style style3 = styleCollection.GetStyle(style.LinkedStyleId);
					if (style3 != null)
					{
						StyleRepository.TopologicalSortVisit(style3, styleCollection, visited, result);
					}
				}
				result.Add(style);
			}
		}

		void AddInternal(Style style)
		{
			if (this.stylesDictionary.ContainsKey(style.Id))
			{
				throw new InvalidOperationException(string.Format("Style with name {0} is already registered", style.Name));
			}
			this.stylesDictionary.Add(style.Id, style);
			if (style.IsDefault)
			{
				this.defaultStyles[style.StyleType] = style;
			}
			style.Document = this.Document;
		}

		readonly Dictionary<StyleType, Style> defaultStyles = new Dictionary<StyleType, Style>();

		readonly Dictionary<string, Style> stylesDictionary = new Dictionary<string, Style>();
	}
}
