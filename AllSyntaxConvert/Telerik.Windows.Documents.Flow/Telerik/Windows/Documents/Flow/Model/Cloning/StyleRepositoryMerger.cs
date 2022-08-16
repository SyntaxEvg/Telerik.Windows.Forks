using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.Model.Cloning
{
	class StyleRepositoryMerger
	{
		public StyleRepositoryMerger(StyleRepository target, StyleRepository source)
		{
			this.targetStyleRepository = target;
			this.sourceStyleRepository = source;
			this.styleComparer = new StyleMergeEqualityComparer();
		}

		public void Merge(CloneContext cloneContext)
		{
			this.addedStyles = new List<Style>();
			foreach (Style style in this.sourceStyleRepository.Styles)
			{
				StyleRepositoryMerger.AddStyleAction addStyleAction = this.ComputeAddStyleAction(style, cloneContext.MergeOptions.ConflictingStylesResolutionMode);
				Style style2 = null;
				switch (addStyleAction)
				{
				case StyleRepositoryMerger.AddStyleAction.Add:
					style2 = style.Clone();
					this.AddStyle(style2);
					break;
				case StyleRepositoryMerger.AddStyleAction.RenameAndAdd:
					style2 = style.Clone();
					style2.IsCustom = true;
					this.RenameStyle(style2, cloneContext);
					this.AddStyle(style2);
					break;
				}
				if (style2 != null && (style2.StyleType == StyleType.Numbering || style2.StyleType == StyleType.Paragraph))
				{
					StyleRepositoryMerger.AssignOldListIdToNewStyleId(style2, cloneContext);
				}
			}
			this.UpdateStyleLinks(cloneContext);
		}

		static void AssignOldListIdToNewStyleId(Style style, CloneContext cloneContext)
		{
			if (style.ParagraphProperties.ListId.HasLocalValue)
			{
				if (cloneContext.OldListsToStyles.ContainsKey(style.ParagraphProperties.ListId.LocalValue.Value))
				{
					cloneContext.OldListsToStyles[style.ParagraphProperties.ListId.LocalValue.Value].Add(style.Id);
					return;
				}
				List<string> list = new List<string>();
				list.Add(style.Id);
				cloneContext.OldListsToStyles.Add(style.ParagraphProperties.ListId.LocalValue.Value, list);
			}
		}

		void UpdateStyleLinks(CloneContext cloneContext)
		{
			foreach (Style style in this.addedStyles)
			{
				if (style.BasedOnStyleId != null && cloneContext.RenamedStyles.ContainsKey(style.BasedOnStyleId))
				{
					style.BasedOnStyleId = cloneContext.RenamedStyles[style.BasedOnStyleId];
				}
				if (style.LinkedStyleId != null && cloneContext.RenamedStyles.ContainsKey(style.LinkedStyleId))
				{
					style.LinkedStyleId = cloneContext.RenamedStyles[style.LinkedStyleId];
				}
				if (style.NextStyleId != null && cloneContext.RenamedStyles.ContainsKey(style.NextStyleId))
				{
					style.NextStyleId = cloneContext.RenamedStyles[style.NextStyleId];
				}
			}
		}

		void AddStyle(Style style)
		{
			if (style.IsDefault)
			{
				style.IsDefault = false;
			}
			this.targetStyleRepository.Add(style);
			this.addedStyles.Add(style);
		}

		void RenameStyle(Style style, CloneContext cloneContext)
		{
			string id = style.Id;
			string text = this.ChooseRenamedStyledId(id);
			style.Id = text;
			cloneContext.RenamedStyles.Add(id, text);
		}

		string ChooseRenamedStyledId(string oldId)
		{
			string text;
			do
			{
				text = oldId + "_1";
				oldId = text;
			}
			while (this.targetStyleRepository.Contains(text));
			return text;
		}

		StyleRepositoryMerger.AddStyleAction ComputeAddStyleAction(Style style, ConflictingStylesResolutionMode conflictStyleResolutionMode)
		{
			if (!this.targetStyleRepository.Contains(style.Id))
			{
				return StyleRepositoryMerger.AddStyleAction.Add;
			}
			Style style2 = this.targetStyleRepository.GetStyle(style.Id);
			switch (conflictStyleResolutionMode)
			{
			case ConflictingStylesResolutionMode.UseTargetStyle:
				if (style2.StyleType == style.StyleType)
				{
					return StyleRepositoryMerger.AddStyleAction.None;
				}
				return StyleRepositoryMerger.AddStyleAction.RenameAndAdd;
			case ConflictingStylesResolutionMode.RenameSourceStyle:
				if (this.styleComparer.Equals(style2, style))
				{
					return StyleRepositoryMerger.AddStyleAction.None;
				}
				return StyleRepositoryMerger.AddStyleAction.RenameAndAdd;
			default:
				throw new ArgumentException("Unsupported ConflictStyleResolutionMode.");
			}
		}

		const string StyleNameMergeSuffix = "_1";

		readonly StyleRepository targetStyleRepository;

		readonly StyleRepository sourceStyleRepository;

		readonly StyleMergeEqualityComparer styleComparer;

		List<Style> addedStyles;

		enum AddStyleAction
		{
			None,
			Add,
			RenameAndAdd
		}
	}
}
