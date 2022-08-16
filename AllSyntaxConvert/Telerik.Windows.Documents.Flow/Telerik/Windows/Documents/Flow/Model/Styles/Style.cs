using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	[DebuggerDisplay("Id = {Id}, Type = {StyleType}")]
	public class Style
	{
		static Style()
		{
			Dictionary<StylePropertyType, Func<Style, DocumentElementPropertiesBase>> dictionary = new Dictionary<StylePropertyType, Func<Style, DocumentElementPropertiesBase>>();
			dictionary.Add(StylePropertyType.Character, (Style s) => s.CharacterProperties);
			dictionary.Add(StylePropertyType.Paragraph, (Style s) => s.ParagraphProperties);
			dictionary.Add(StylePropertyType.Table, (Style s) => s.TableProperties);
			dictionary.Add(StylePropertyType.TableRow, (Style s) => s.TableRowProperties);
			dictionary.Add(StylePropertyType.TableCell, (Style s) => s.TableCellProperties);
			dictionary.Add(StylePropertyType.DocumentElement, (Style s) => null);
			Style.StylePropertyTypeToPropertiesContainerEvaluator = dictionary;
		}

		public Style(string id, StyleType type)
		{
			this.Id = id;
			this.styleType = type;
			this.UIPriority = DocumentDefaultStyleSettings.UIPriority;
		}

		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				if (this.Document != null)
				{
					throw new InvalidOperationException("Cannot change the id of a style that is already added to a document.");
				}
				Guard.ThrowExceptionIfNullOrEmpty(value, "Id");
				this.id = value;
			}
		}

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(this.name))
				{
					return this.Id;
				}
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public RadFlowDocument Document { get; internal set; }

		public StyleType StyleType
		{
			get
			{
				return this.styleType;
			}
		}

		public string BasedOnStyleId
		{
			get
			{
				return this.basedOnStyleId;
			}
			set
			{
				this.CheckIsValidBasedOnStyle(value);
				this.basedOnStyleId = value;
			}
		}

		public string NextStyleId { get; set; }

		public string LinkedStyleId
		{
			get
			{
				return this.linkedStyleId;
			}
			set
			{
				this.linkedStyleId = value;
			}
		}

		public bool IsDefault
		{
			get
			{
				return this.isDefault;
			}
			set
			{
				if (this.Document != null)
				{
					throw new InvalidOperationException("Cannot change the IsDefault property of a style that is already added to a document.");
				}
				this.isDefault = value;
			}
		}

		public bool IsCustom
		{
			get
			{
				return this.isCustom;
			}
			set
			{
				this.isCustom = value;
			}
		}

		public bool IsPrimary { get; set; }

		public int UIPriority { get; set; }

		public CharacterProperties CharacterProperties
		{
			get
			{
				if (this.characterProperties == null)
				{
					this.characterProperties = new CharacterProperties(this);
				}
				return this.characterProperties;
			}
		}

		public ParagraphProperties ParagraphProperties
		{
			get
			{
				if (this.paragraphProperties == null && (this.StyleType != StyleType.Character || this.StyleType != StyleType.Numbering))
				{
					this.paragraphProperties = new ParagraphProperties(this);
				}
				return this.paragraphProperties;
			}
		}

		public TableProperties TableProperties
		{
			get
			{
				if (this.tableProperties == null && this.StyleType == StyleType.Table)
				{
					this.tableProperties = new TableProperties(this);
				}
				return this.tableProperties;
			}
		}

		public TableRowProperties TableRowProperties
		{
			get
			{
				if (this.tableRowProperties == null && this.StyleType == StyleType.Table)
				{
					this.tableRowProperties = new TableRowProperties(this);
				}
				return this.tableRowProperties;
			}
		}

		public TableCellProperties TableCellProperties
		{
			get
			{
				if (this.tableCellProperties == null && this.StyleType == StyleType.Table)
				{
					this.tableCellProperties = new TableCellProperties(this);
				}
				return this.tableCellProperties;
			}
		}

		public object GetPropertyValue(IStylePropertyDefinition stylePropertyDefinition)
		{
			IStyleProperty styleProperty = this.GetStyleProperty(stylePropertyDefinition);
			if (styleProperty != null)
			{
				return styleProperty.GetActualValueAsObject();
			}
			return null;
		}

		public Style Clone()
		{
			Style style = new Style(this.Id, this.StyleType);
			style.Name = this.Name;
			style.BasedOnStyleId = this.BasedOnStyleId;
			style.NextStyleId = this.NextStyleId;
			style.LinkedStyleId = this.LinkedStyleId;
			style.IsDefault = this.IsDefault;
			style.IsCustom = this.IsCustom;
			style.IsPrimary = this.IsPrimary;
			style.UIPriority = this.UIPriority;
			style.CopyStylePropertiesFrom(this);
			return style;
		}

		internal IStyleProperty GetStyleProperty(IStylePropertyDefinition stylePropertyDefinition)
		{
			Func<Style, DocumentElementPropertiesBase> valueOrNull = Style.StylePropertyTypeToPropertiesContainerEvaluator.GetValueOrNull(stylePropertyDefinition.StylePropertyType);
			if (valueOrNull != null)
			{
				DocumentElementPropertiesBase documentElementPropertiesBase = valueOrNull(this);
				if (documentElementPropertiesBase != null)
				{
					return documentElementPropertiesBase.GetStyleProperty(stylePropertyDefinition);
				}
			}
			return null;
		}

		internal void CopyStylePropertiesFrom(Style fromStyle)
		{
			if (this.CharacterProperties != null)
			{
				this.CharacterProperties.CopyPropertiesFrom(fromStyle.CharacterProperties);
			}
			if (this.ParagraphProperties != null)
			{
				this.ParagraphProperties.CopyPropertiesFrom(fromStyle.ParagraphProperties);
			}
			if (this.TableProperties != null)
			{
				this.TableProperties.CopyPropertiesFrom(fromStyle.TableProperties);
			}
			if (this.TableRowProperties != null)
			{
				this.TableRowProperties.CopyPropertiesFrom(fromStyle.TableRowProperties);
			}
			if (this.TableCellProperties != null)
			{
				this.TableCellProperties.CopyPropertiesFrom(fromStyle.TableCellProperties);
			}
		}

		void CheckIsValidBasedOnStyle(string value)
		{
			if (string.IsNullOrEmpty(value) || this.Document == null)
			{
				return;
			}
			Style style = this.Document.StyleRepository.GetStyle(value);
			if (style == null)
			{
				return;
			}
			if (this.StyleType != style.StyleType)
			{
				throw new ArgumentException(string.Format("Style of type {0} can not be based on style of type {1}.", this.StyleType, style.StyleType));
			}
			if (this.HasCycles(style))
			{
				throw new InvalidOperationException("Cyclic style inheritance is not allowed.");
			}
		}

		bool HasCycles(Style newBasedOn)
		{
			if (newBasedOn == null)
			{
				return false;
			}
			string text;
			for (Style style = newBasedOn; style != null; style = this.Document.StyleRepository.GetStyle(text))
			{
				if (style == this)
				{
					return true;
				}
				text = style.BasedOnStyleId;
				if (string.IsNullOrEmpty(text))
				{
					return false;
				}
			}
			return false;
		}

		static readonly Dictionary<StylePropertyType, Func<Style, DocumentElementPropertiesBase>> StylePropertyTypeToPropertiesContainerEvaluator;

		readonly StyleType styleType;

		string id;

		string name;

		string basedOnStyleId;

		string linkedStyleId;

		bool isCustom;

		bool isDefault;

		CharacterProperties characterProperties;

		ParagraphProperties paragraphProperties;

		TableProperties tableProperties;

		TableRowProperties tableRowProperties;

		TableCellProperties tableCellProperties;
	}
}
