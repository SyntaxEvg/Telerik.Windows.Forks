using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;

namespace Telerik.Windows.Documents.Flow.Model.Cloning
{
	class StyleMergeEqualityComparer : IEqualityComparer<Style>
	{
		public bool Equals(Style first, Style second)
		{
			return !object.ReferenceEquals(null, second) && (object.ReferenceEquals(this, second) || (first.StyleType == second.StyleType && first.Id == second.Id && first.BasedOnStyleId == second.BasedOnStyleId && first.LinkedStyleId == second.LinkedStyleId && first.NextStyleId == second.NextStyleId && first.Name == second.Name && first.IsCustom == second.IsCustom && first.IsDefault == second.IsDefault && first.IsPrimary == second.IsPrimary && first.UIPriority == second.UIPriority && StyleMergeEqualityComparer.PropertiesAreEqual(first.CharacterProperties, second.CharacterProperties) && StyleMergeEqualityComparer.PropertiesAreEqual(first.ParagraphProperties, second.ParagraphProperties) && StyleMergeEqualityComparer.PropertiesAreEqual(first.TableProperties, second.TableProperties) && StyleMergeEqualityComparer.PropertiesAreEqual(first.TableRowProperties, second.TableRowProperties) && StyleMergeEqualityComparer.PropertiesAreEqual(first.TableCellProperties, second.TableCellProperties)));
		}

		public int GetHashCode(Style obj)
		{
			throw new NotImplementedException();
		}

		internal static bool PropertiesAreEqual(DocumentElementPropertiesBase firstProperties, DocumentElementPropertiesBase secondProperties)
		{
			if ((firstProperties == null && secondProperties == null) || firstProperties == secondProperties)
			{
				return true;
			}
			if ((firstProperties != null && secondProperties == null) || (firstProperties == null && secondProperties != null))
			{
				return false;
			}
			if (firstProperties.StyleId != secondProperties.StyleId)
			{
				return false;
			}
			foreach (IStyleProperty styleProperty in firstProperties.StyleProperties)
			{
				IStyleProperty styleProperty2 = secondProperties.GetStyleProperty(styleProperty.PropertyDefinition);
				if (styleProperty.HasLocalValue != styleProperty2.HasLocalValue)
				{
					return false;
				}
				if (styleProperty.PropertyDefinition == Paragraph.ListIdPropertyDefinition && styleProperty.HasLocalValue && styleProperty2.HasLocalValue)
				{
					if (!firstProperties.GetStyleProperty(Paragraph.ListLevelPropertyDefinition).HasLocalValue && !secondProperties.GetStyleProperty(Paragraph.ListLevelPropertyDefinition).HasLocalValue)
					{
						return false;
					}
				}
				else if (styleProperty.HasLocalValue || styleProperty2.HasLocalValue)
				{
					object actualValueAsObject = styleProperty.GetActualValueAsObject();
					object actualValueAsObject2 = styleProperty2.GetActualValueAsObject();
					if (!object.Equals(actualValueAsObject, actualValueAsObject2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
