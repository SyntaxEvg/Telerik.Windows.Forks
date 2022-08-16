using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	class TabStopCollectionStyleProperty : StyleProperty<TabStopCollection>
	{
		public TabStopCollectionStyleProperty(StylePropertyDefinition<TabStopCollection> propertyDefinition, DocumentElementPropertiesBase propertyContainer)
			: base(propertyDefinition, propertyContainer)
		{
		}

		public override TabStopCollection GetActualValue()
		{
			TabStopCollection tabStopCollection = base.GetActualValueInternal();
			if (this.HasLocalValue)
			{
				tabStopCollection = tabStopCollection.Merge(base.LocalValue);
			}
			return tabStopCollection;
		}
	}
}
