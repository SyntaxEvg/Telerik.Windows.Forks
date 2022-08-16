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
	public sealed class RadFlowDocument : global::Telerik.Windows.Documents.Flow.Model.DocumentElementBase, global::Telerik.Windows.Documents.Flow.Model.Styles.IElementWithProperties
	{
		public RadFlowDocument()
			: this(true)
		{
		}

		internal RadFlowDocument(bool setInitialDocumentDefaultValues)
		{
			this.DefaultStyle = new global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentDefaultStyle(this);
			this.sections = new global::Telerik.Windows.Documents.Flow.Model.Collections.SectionCollection(this);
			this.styleRepository = new global::Telerik.Windows.Documents.Flow.Model.Styles.StyleRepository(this);
			this.resources = new global::Telerik.Windows.Documents.Common.Model.Data.ResourceManager();
			this.properties = new global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentProperties(this);
			this.lists = new global::Telerik.Windows.Documents.Flow.Model.Lists.ListCollection(this);
			this.comments = new global::Telerik.Windows.Documents.Flow.Model.Collections.CommentCollection(this);
			this.documentVariables = new global::Telerik.Windows.Documents.Flow.Model.Collections.DocumentVariableCollection();
			this.ProtectionSettings = new global::Telerik.Windows.Documents.Flow.Model.Protection.ProtectionSettings();
			if (setInitialDocumentDefaultValues)
			{
				this.EnsureInitialDocumentDefaultValues();
			}
			this.RegisterDefaultStyles();
		}

		public global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentDefaultStyle DefaultStyle { get; private set; }

		global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentElementPropertiesBase global::Telerik.Windows.Documents.Flow.Model.Styles.IElementWithProperties.Properties
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

		public global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentViewType ViewType
		{
			get
			{
				return this.Properties.ViewType.GetActualValue().Value;
			}
			set
			{
				this.Properties.ViewType.LocalValue = new global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentViewType?(value);
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

		public override global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument Document
		{
			get
			{
				return this;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.Collections.SectionCollection Sections
		{
			get
			{
				return this.sections;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.Styles.StyleRepository StyleRepository
		{
			get
			{
				return this.styleRepository;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.Lists.ListCollection Lists
		{
			get
			{
				return this.lists;
			}
		}

		public global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme Theme
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

		public global::Telerik.Windows.Documents.Flow.Model.Collections.CommentCollection Comments
		{
			get
			{
				return this.comments;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.Collections.DocumentVariableCollection DocumentVariables
		{
			get
			{
				return this.documentVariables;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.Protection.ProtectionSettings ProtectionSettings { get; private set; }

		internal override global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Flow.Model.DocumentElementBase> Children
		{
			get
			{
				//var e =this.Sections;
				//if (e != null)
    //            {
				//	var t1 =e as IEnumerable;

				//	System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Flow.Model.DocumentElementBase> t = t1.Concat(this.Comments);
				//	return t;
    //            }
				return null;
			}
		}

		internal override global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Flow.Model.DocumentElementBase> ContentChildren
		{
			get
			{
				return this.Sections;
			}
		}

		internal override global::Telerik.Windows.Documents.Flow.Model.DocumentElementType Type
		{
			get
			{
				return global::Telerik.Windows.Documents.Flow.Model.DocumentElementType.Document;
			}
		}

		internal global::Telerik.Windows.Documents.Common.Model.Data.ResourceManager Resources
		{
			get
			{
				return this.resources;
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument Clone()
		{
			return (global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument)this.CloneCore(new global::Telerik.Windows.Documents.Flow.Model.Cloning.CloneContext(new global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument(false)));
		}

		public void Merge(global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument sourceDocument)
		{
			this.Merge(sourceDocument, null);
		}

		public void Merge(global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument sourceDocument, global::Telerik.Windows.Documents.Flow.Model.MergeOptions mergeOptions)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument>(sourceDocument, "sourceDocument");
			global::Telerik.Windows.Documents.Flow.Model.Cloning.CloneContext cloneContext = new global::Telerik.Windows.Documents.Flow.Model.Cloning.CloneContext(this)
			{
				MergeOptions = (mergeOptions ?? new global::Telerik.Windows.Documents.Flow.Model.MergeOptions())
			};
			this.MergeWithoutChildren(sourceDocument, cloneContext);
			this.Sections.AddClonedChildrenFrom(sourceDocument.Sections, cloneContext);
		}

		public void UpdateFields()
		{
			global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Flow.Model.Fields.FieldCharacter> fields = base.EnumerateChildrenOfType<global::Telerik.Windows.Documents.Flow.Model.Fields.FieldCharacter>();
			global::System.Collections.Generic.List<global::Telerik.Windows.Documents.Flow.Model.Fields.FieldCharacter> orderedListOfFieldToUpdate = global::Telerik.Windows.Documents.Flow.Model.Fields.FieldUpdateScheduler.GetOrderedListOfFieldToUpdate(fields);
			foreach (global::Telerik.Windows.Documents.Flow.Model.Fields.FieldCharacter fieldCharacter in orderedListOfFieldToUpdate)
			{
				global::Telerik.Windows.Documents.Flow.Model.Fields.FieldUpdateContext context = new global::Telerik.Windows.Documents.Flow.Model.Fields.FieldUpdateContext(fieldCharacter.FieldInfo);
				fieldCharacter.FieldInfo.UpdateFieldCore(context);
			}
		}

		public global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument MailMerge(global::System.Collections.IEnumerable collection)
		{
			return global::Telerik.Windows.Documents.Flow.Model.MailMergeProcessor.Execute(this, collection);
		}

		internal void MergeWithoutChildren(global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument sourceDocument, global::Telerik.Windows.Documents.Flow.Model.Cloning.CloneContext cloneContext)
		{
			this.StyleRepository.Merge(sourceDocument.StyleRepository, cloneContext);
			this.Lists.Merge(sourceDocument.Lists, cloneContext);
		}

		internal global::Telerik.Windows.Documents.Flow.Model.Bookmark GetBookmarkByName(string name)
		{
			global::Telerik.Windows.Documents.Flow.Model.BookmarkRangeStart bookmarkRangeStart = base.EnumerateChildrenOfType<global::Telerik.Windows.Documents.Flow.Model.BookmarkRangeStart>().FirstOrDefault((global::Telerik.Windows.Documents.Flow.Model.BookmarkRangeStart b) => b.Bookmark.Name == name);
			if (bookmarkRangeStart == null)
			{
				return null;
			}
			return bookmarkRangeStart.Bookmark;
		}

		internal override global::Telerik.Windows.Documents.Flow.Model.DocumentElementBase CloneCore(global::Telerik.Windows.Documents.Flow.Model.Cloning.CloneContext cloneContext)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Cloning.CloneContext>(cloneContext, "cloneContext");
			global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument document = cloneContext.Document;
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

		private void EnsureInitialDocumentDefaultValues()
		{
			this.DefaultStyle.CharacterProperties.FontSize.LocalValue = new double?(global::Telerik.Windows.Documents.Media.Unit.PointToDip(11.0));
			this.DefaultStyle.CharacterProperties.FontFamily.LocalValue = new global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableFontFamily(global::Telerik.Windows.Documents.Spreadsheet.Theming.ThemeFontType.Minor);
			this.DefaultStyle.ParagraphProperties.SpacingAfter.LocalValue = new double?(12.0);
			this.DefaultStyle.ParagraphProperties.LineSpacing.LocalValue = new double?(1.15);
			this.DefaultStyle.ParagraphProperties.LineSpacingType.LocalValue = new global::Telerik.Windows.Documents.Flow.Model.Styles.HeightType?(global::Telerik.Windows.Documents.Flow.Model.Styles.HeightType.Auto);
		}

		private void RegisterDefaultStyles()
		{
			this.StyleRepository.AddBuiltInStyle("Normal");
			this.StyleRepository.AddBuiltInStyle("TableNormal");
		}

		static RadFlowDocument()
		{
			global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument.DefaultTabStopWidthPropertyDefinition.Validation.AddRule(new global::Telerik.Windows.Documents.Flow.Model.Styles.Core.ValidationRule<double>((double value) => value >= 0.0));
		}

		private readonly global::Telerik.Windows.Documents.Flow.Model.Styles.StyleRepository styleRepository;

		private readonly global::Telerik.Windows.Documents.Flow.Model.Collections.SectionCollection sections;

		private readonly global::Telerik.Windows.Documents.Common.Model.Data.ResourceManager resources;

		private readonly global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentProperties properties;

		private readonly global::Telerik.Windows.Documents.Flow.Model.Lists.ListCollection lists;

		private readonly global::Telerik.Windows.Documents.Flow.Model.Collections.CommentCollection comments;

		private readonly global::Telerik.Windows.Documents.Flow.Model.Collections.DocumentVariableCollection documentVariables;

		private global::Telerik.Windows.Documents.Spreadsheet.Theming.DocumentTheme theme = global::Telerik.Windows.Documents.Spreadsheet.Theming.PredefinedThemeSchemes.DefaultTheme;

		public static readonly global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyDefinition<bool?> HasDifferentEvenOddPageHeadersFootersPropertyDefinition = new global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyDefinition<bool?>("HasDifferentEvenOddPageHeadersFooters", new bool?(global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentDefaultStyleSettings.HasDifferentEvenOddPageHeadersFooters), global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyType.Document);

		public static readonly global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyDefinition<global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentViewType?> ViewTypePropertyDefinition = new global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyDefinition<global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentViewType?>("ViewType", new global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentViewType?(global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentDefaultStyleSettings.ViewType), global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyType.Document);

		public static readonly global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyDefinition<double?> DefaultTabStopWidthPropertyDefinition = new global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyDefinition<double?>("DefaultTabStopWidth", new double?(global::Telerik.Windows.Documents.Flow.Model.Styles.DocumentDefaultStyleSettings.DefaultTabStopWidth), global::Telerik.Windows.Documents.Flow.Model.Styles.Core.StylePropertyType.Document);
	}
}
