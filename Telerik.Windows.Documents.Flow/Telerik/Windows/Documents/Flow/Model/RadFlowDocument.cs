using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Collections;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class RadFlowDocument : DocumentElementBase, IElementWithProperties
	{
		public RadFlowDocument()
			: this(true)
		{
		}

		internal RadFlowDocument(bool setInitialDocumentDefaultValues)
		{
			this.DefaultStyle = new DocumentDefaultStyle(this);
			this.sections = new SectionCollection(this);
			this.styleRepository = new StyleRepository(this);
			this.resources = new ResourceManager();
			this.properties = new DocumentProperties(this);
			this.lists = new ListCollection(this);
			this.comments = new CommentCollection(this);
			this.documentVariables = new DocumentVariableCollection();
			this.ProtectionSettings = new ProtectionSettings();
			if (setInitialDocumentDefaultValues)
			{
				this.EnsureInitialDocumentDefaultValues();
			}
			this.RegisterDefaultStyles();
		}

		public DocumentDefaultStyle DefaultStyle { get; set; }

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public bool HasDifferentEvenOddPageHeadersFooters
		{
			get
			{
				return this.Properties.HasDifferentEvenOddPageHeadersFooters.GetActualValue().Value;
			}
			set
			{
				this.Properties.HasDifferentEvenOddPageHeadersFooters.LocalValue = new bool?(value);
			}
		}

		public DocumentViewType ViewType
		{
			get
			{
				return this.Properties.ViewType.GetActualValue().Value;
			}
			set
			{
				this.Properties.ViewType.LocalValue = new DocumentViewType?(value);
			}
		}

		public double DefaultTabStopWidth
		{
			get
			{
				return this.Properties.DefaultTabStopWidth.GetActualValue().Value;
			}
			set
			{
				this.Properties.DefaultTabStopWidth.LocalValue = new double?(value);
			}
		}

		public override RadFlowDocument Document
		{
			get
			{
				return this;
			}
		}

		public DocumentProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public SectionCollection Sections
		{
			get
			{
				return this.sections;
			}
		}

		public StyleRepository StyleRepository
		{
			get
			{
				return this.styleRepository;
			}
		}

		public ListCollection Lists
		{
			get
			{
				return this.lists;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.theme;
			}
			set
			{
				this.theme = value;
			}
		}

		public CommentCollection Comments
		{
			get
			{
				return this.comments;
			}
		}

		public DocumentVariableCollection DocumentVariables
		{
			get
			{
				return this.documentVariables;
			}
		}

		public ProtectionSettings ProtectionSettings { get; set; }

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return this.Sections.Concat(this.Comments);
			}
		}

		internal override IEnumerable<DocumentElementBase> ContentChildren
		{
			get
			{
				return this.Sections;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Document;
			}
		}

		internal ResourceManager Resources
		{
			get
			{
				return this.resources;
			}
		}

		public RadFlowDocument Clone()
		{
			return (RadFlowDocument)this.CloneCore(new CloneContext(new RadFlowDocument(false)));
		}

		public void Merge(RadFlowDocument sourceDocument)
		{
			this.Merge(sourceDocument, null);
		}

		public void Merge(RadFlowDocument sourceDocument, MergeOptions mergeOptions)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(sourceDocument, "sourceDocument");
			CloneContext cloneContext = new CloneContext(this)
			{
				MergeOptions = (mergeOptions ?? new MergeOptions())
			};
			this.MergeWithoutChildren(sourceDocument, cloneContext);
			this.Sections.AddClonedChildrenFrom(sourceDocument.Sections, cloneContext);
		}

		public void UpdateFields()
		{
			IEnumerable<FieldCharacter> fields = base.EnumerateChildrenOfType<FieldCharacter>();
			List<FieldCharacter> orderedListOfFieldToUpdate = FieldUpdateScheduler.GetOrderedListOfFieldToUpdate(fields);
			foreach (FieldCharacter fieldCharacter in orderedListOfFieldToUpdate)
			{
				FieldUpdateContext context = new FieldUpdateContext(fieldCharacter.FieldInfo);
				fieldCharacter.FieldInfo.UpdateFieldCore(context);
			}
		}

		public RadFlowDocument MailMerge(IEnumerable collection)
		{
			return MailMergeProcessor.Execute(this, collection);
		}

		internal void MergeWithoutChildren(RadFlowDocument sourceDocument, CloneContext cloneContext)
		{
			this.StyleRepository.Merge(sourceDocument.StyleRepository, cloneContext);
			this.Lists.Merge(sourceDocument.Lists, cloneContext);
		}

		internal Bookmark GetBookmarkByName(string name)
		{
			BookmarkRangeStart bookmarkRangeStart = base.EnumerateChildrenOfType<BookmarkRangeStart>().FirstOrDefault((BookmarkRangeStart b) => b.Bookmark.Name == name);
			if (bookmarkRangeStart == null)
			{
				return null;
			}
			return bookmarkRangeStart.Bookmark;
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			RadFlowDocument document = cloneContext.Document;
			document.DefaultStyle.CharacterProperties.CopyPropertiesFrom(this.DefaultStyle.CharacterProperties);
			document.DefaultStyle.ParagraphProperties.CopyPropertiesFrom(this.DefaultStyle.ParagraphProperties);
			document.Properties.CopyPropertiesFrom(this.Properties);
			document.Sections.AddClonedChildrenFrom(this.Sections, cloneContext);
			document.StyleRepository.AddClonedStylesFrom(this.StyleRepository);
			document.Lists.AddClonedListsFrom(this.Lists);
			document.DocumentVariables.AddClonedDocumentVariablesFrom(this.DocumentVariables);
			document.ProtectionSettings = this.ProtectionSettings.Clone();
			document.Theme = this.Theme.Clone();
			return document;
		}

		void EnsureInitialDocumentDefaultValues()
		{
			this.DefaultStyle.CharacterProperties.FontSize.LocalValue = new double?(Unit.PointToDip(11.0));
			this.DefaultStyle.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Minor);
			this.DefaultStyle.ParagraphProperties.SpacingAfter.LocalValue = new double?(12.0);
			this.DefaultStyle.ParagraphProperties.LineSpacing.LocalValue = new double?(1.15);
			this.DefaultStyle.ParagraphProperties.LineSpacingType.LocalValue = new HeightType?(HeightType.Auto);
		}

		void RegisterDefaultStyles()
		{
			this.StyleRepository.AddBuiltInStyle("Normal");
			this.StyleRepository.AddBuiltInStyle("TableNormal");
		}

		static RadFlowDocument()
		{
			RadFlowDocument.DefaultTabStopWidthPropertyDefinition.Validation.AddRule(new ValidationRule<double>((double value) => value >= 0.0));
		}

		readonly StyleRepository styleRepository;

		readonly SectionCollection sections;

		readonly ResourceManager resources;

		readonly DocumentProperties properties;

		readonly ListCollection lists;

		readonly CommentCollection comments;

		readonly DocumentVariableCollection documentVariables;

		DocumentTheme theme = PredefinedThemeSchemes.DefaultTheme;

		public static readonly StylePropertyDefinition<bool?> HasDifferentEvenOddPageHeadersFootersPropertyDefinition = new StylePropertyDefinition<bool?>("HasDifferentEvenOddPageHeadersFooters", new bool?(DocumentDefaultStyleSettings.HasDifferentEvenOddPageHeadersFooters), StylePropertyType.Document);

		public static readonly StylePropertyDefinition<DocumentViewType?> ViewTypePropertyDefinition = new StylePropertyDefinition<DocumentViewType?>("ViewType", new DocumentViewType?(DocumentDefaultStyleSettings.ViewType), StylePropertyType.Document);

		public static readonly StylePropertyDefinition<double?> DefaultTabStopWidthPropertyDefinition = new StylePropertyDefinition<double?>("DefaultTabStopWidth", new double?(DocumentDefaultStyleSettings.DefaultTabStopWidth), StylePropertyType.Document);
	}
}
