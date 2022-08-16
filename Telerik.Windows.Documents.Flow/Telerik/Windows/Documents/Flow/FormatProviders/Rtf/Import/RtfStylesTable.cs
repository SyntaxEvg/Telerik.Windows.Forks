using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class RtfStylesTable : RtfElementIteratorBase
	{
		public void ReadTable(RtfGroup group, RtfImportContext context)
		{
			if (this.isInitialized)
			{
				return;
			}
			this.context = context;
			this.dictStyles = new Dictionary<int, RtfStyleDefinitionInfo>();
			base.VisitGroupChildren(group, false);
			this.InitializeRelatedStyles();
			this.AddStylesToDocumentRepositiory();
			this.FillStylePropertiesFormInfos();
			this.isInitialized = true;
		}

		public void ImportDefaultSpanStyle(RtfGroup group, RtfImportContext currentContext)
		{
			RtfImportContext rtfImportContext = new RtfImportContext(currentContext);
			RtfStyleImporter rtfStyleImporter = new RtfStyleImporter(rtfImportContext, StyleType.Character);
			RtfStyleDefinitionInfo rtfStyleDefinitionInfo = rtfStyleImporter.ImportStyleGroup(group);
			if (currentContext.DefaultFont != null && !rtfStyleDefinitionInfo.ImportedSpanProperties.Properties.FontFamily.HasLocalValue)
			{
				rtfStyleDefinitionInfo.ImportedSpanProperties.FontFamily = new ThemableFontFamily(currentContext.DefaultFont);
			}
			RtfStylesTable.CopyPropertiesIfNotSame(rtfStyleDefinitionInfo.ImportedSpanProperties.Properties, currentContext.Document.DefaultStyle.CharacterProperties);
		}

		public void ImportDefaultParagraphStyle(RtfGroup group, RtfImportContext currentContext)
		{
			RtfImportContext rtfImportContext = new RtfImportContext(currentContext);
			RtfStyleImporter rtfStyleImporter = new RtfStyleImporter(rtfImportContext, StyleType.Paragraph);
			RtfStyleDefinitionInfo rtfStyleDefinitionInfo = rtfStyleImporter.ImportStyleGroup(group);
			RtfStylesTable.CopyParagraphPropertiesIfNotSame(rtfStyleDefinitionInfo.ImportedParagraphProperties.Properties, currentContext.Document.DefaultStyle.ParagraphProperties);
		}

		public Style GetStyleById(int styleId)
		{
			RtfStyleDefinitionInfo rtfStyleDefinitionInfo;
			if (this.dictStyles.TryGetValue(styleId, out rtfStyleDefinitionInfo))
			{
				return rtfStyleDefinitionInfo.CurrentStyle;
			}
			return null;
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			if ((destination = group.Destination) != null)
			{
				if (destination == "s")
				{
					this.ImportStyle(group, StyleType.Paragraph, false);
					return;
				}
				if (destination == "cs")
				{
					this.ImportStyle(group, StyleType.Character, false);
					return;
				}
				if (destination == "ts")
				{
					this.ImportStyle(group, StyleType.Table, false);
					return;
				}
				if (destination == "ds")
				{
					return;
				}
			}
			this.ImportStyle(group, StyleType.Paragraph, true);
		}

		static void CopyParagraphPropertiesIfNotSame(ParagraphProperties fromProperties, ParagraphProperties toProperties)
		{
			RtfStylesTable.CopyPropertiesIfNotSame(fromProperties, toProperties);
		}

		static void CopyPropertiesIfNotSame(DocumentElementPropertiesBase fromProperties, DocumentElementPropertiesBase toProperties)
		{
			if (fromProperties == null || fromProperties.GetType() != toProperties.GetType())
			{
				return;
			}
			foreach (IStyleProperty styleProperty in fromProperties.StyleProperties)
			{
				if (styleProperty.HasLocalValue)
				{
					IStyleProperty styleProperty2 = toProperties.GetStyleProperty(styleProperty.PropertyDefinition);
					object actualValueAsObject = styleProperty.GetActualValueAsObject();
					object actualValueAsObject2 = styleProperty2.GetActualValueAsObject();
					if (actualValueAsObject != null && !actualValueAsObject.Equals(actualValueAsObject2))
					{
						styleProperty2.SetValueAsObject(actualValueAsObject);
					}
				}
			}
		}

		static void FillStylePropertiesFromInfo(RtfStyleDefinitionInfo styleInfo)
		{
			RtfStylesTable.CopyPropertiesIfNotSame(styleInfo.ImportedSpanProperties.Properties, styleInfo.CurrentStyle.CharacterProperties);
			if (styleInfo.CurrentStyle.StyleType == StyleType.Paragraph || styleInfo.CurrentStyle.StyleType == StyleType.Table)
			{
				RtfStylesTable.CopyParagraphPropertiesIfNotSame(styleInfo.ImportedParagraphProperties.Properties, styleInfo.CurrentStyle.ParagraphProperties);
			}
			if (styleInfo.CurrentStyle.StyleType == StyleType.Table)
			{
				RtfStylesTable.CopyPropertiesIfNotSame(styleInfo.ImportedTableProperties.Properties, styleInfo.CurrentStyle.TableProperties);
				RtfStylesTable.CopyPropertiesIfNotSame(styleInfo.ImportedTableRowProperties.Properties, styleInfo.CurrentStyle.TableRowProperties);
				RtfStylesTable.CopyPropertiesIfNotSame(styleInfo.ImportedTableCellProperties.Properties, styleInfo.CurrentStyle.TableCellProperties);
				styleInfo.CurrentStyle.TableProperties.LayoutType.LocalValue = new TableLayoutType?(DocumentDefaultStyleSettings.TableLayoutType);
			}
		}

		void InitializeRelatedStyles()
		{
			foreach (RtfStyleDefinitionInfo rtfStyleDefinitionInfo in this.dictStyles.Values)
			{
				RtfStyleDefinitionInfo rtfStyleDefinitionInfo2;
				if (rtfStyleDefinitionInfo.BasedOnStyleId != null && this.dictStyles.TryGetValue(rtfStyleDefinitionInfo.BasedOnStyleId.Value, out rtfStyleDefinitionInfo2))
				{
					rtfStyleDefinitionInfo.CurrentStyle.BasedOnStyleId = rtfStyleDefinitionInfo2.CurrentStyle.Id;
				}
				RtfStyleDefinitionInfo rtfStyleDefinitionInfo3;
				if (rtfStyleDefinitionInfo.LinkedStyleId != null && this.dictStyles.TryGetValue(rtfStyleDefinitionInfo.LinkedStyleId.Value, out rtfStyleDefinitionInfo3))
				{
					rtfStyleDefinitionInfo.CurrentStyle.LinkedStyleId = rtfStyleDefinitionInfo3.CurrentStyle.Id;
				}
				RtfStyleDefinitionInfo rtfStyleDefinitionInfo4;
				if (rtfStyleDefinitionInfo.NextStyleId != null && this.dictStyles.TryGetValue(rtfStyleDefinitionInfo.NextStyleId.Value, out rtfStyleDefinitionInfo4))
				{
					rtfStyleDefinitionInfo.CurrentStyle.NextStyleId = rtfStyleDefinitionInfo4.CurrentStyle.Id;
				}
			}
		}

		void AddStylesToDocumentRepositiory()
		{
			foreach (RtfStyleDefinitionInfo rtfStyleDefinitionInfo in this.dictStyles.Values)
			{
				Style style = this.context.Document.StyleRepository.GetStyle(rtfStyleDefinitionInfo.CurrentStyle.Id);
				if (style != null)
				{
					rtfStyleDefinitionInfo.CurrentStyle.IsDefault = style.IsDefault;
					rtfStyleDefinitionInfo.CurrentStyle.IsCustom = style.IsCustom;
				}
				this.context.Document.StyleRepository.Add(rtfStyleDefinitionInfo.CurrentStyle);
			}
		}

		void FillStylePropertiesFormInfos()
		{
			using (IEnumerator<Style> enumerator = this.context.Document.StyleRepository.GetSortedTopologicallyStyles().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Style style = enumerator.Current;
					RtfStyleDefinitionInfo rtfStyleDefinitionInfo = this.dictStyles.Values.FirstOrDefault((RtfStyleDefinitionInfo info) => info.CurrentStyle == style);
					if (rtfStyleDefinitionInfo != null)
					{
						RtfStylesTable.FillStylePropertiesFromInfo(rtfStyleDefinitionInfo);
					}
				}
			}
		}

		void ImportStyle(RtfGroup group, StyleType type, bool isDefault = false)
		{
			RtfImportContext rtfImportContext = new RtfImportContext(this.context);
			RtfStyleImporter rtfStyleImporter = new RtfStyleImporter(rtfImportContext, type);
			RtfStyleDefinitionInfo rtfStyleDefinitionInfo = rtfStyleImporter.ImportStyleGroup(group);
			if (isDefault)
			{
				rtfStyleDefinitionInfo.StyleId = new int?(0);
			}
			if (rtfStyleDefinitionInfo.CurrentStyle.Id == RtfStylesTable.TableNormalDisplayName)
			{
				rtfStyleDefinitionInfo.CurrentStyle.Id = "TableNormal";
			}
			if (!this.dictStyles.ContainsKey(rtfStyleDefinitionInfo.StyleId.Value))
			{
				this.dictStyles[rtfStyleDefinitionInfo.StyleId.Value] = rtfStyleDefinitionInfo;
			}
		}

		static readonly string TableNormalDisplayName = "Normal Table";

		bool isInitialized;

		RtfImportContext context;

		Dictionary<int, RtfStyleDefinitionInfo> dictStyles;
	}
}
