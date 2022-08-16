using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public abstract class FormField : IInstanceIdOwner
	{
		internal FormField(string fieldName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fieldName, "fieldName");
			this.name = fieldName;
			this.id = InstanceIdGenerator.GetNextId();
		}

		public abstract FormFieldType FieldType { get; }

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public VariableTextProperties TextProperties
		{
			get
			{
				if (this.textProperties == null)
				{
					this.textProperties = this.GetDefaultTextProperties();
				}
				return this.textProperties;
			}
			set
			{
				Guard.ThrowExceptionIfNull<VariableTextProperties>(value, "value");
				this.textProperties = value;
			}
		}

		public string UserInterfaceName { get; set; }

		public string MappingName { get; set; }

		public bool IsReadOnly { get; set; }

		public bool IsRequired { get; set; }

		public bool ShouldBeSkipped { get; set; }

		public IEnumerable<Widget> Widgets
		{
			get
			{
				return this.GetWidgets();
			}
		}

		internal abstract IEnumerable<Widget> GetWidgets();

		internal abstract Widget AddWidget();

		internal FormField GetClonedInstanceWithoutWidgets(RadFixedDocumentCloneContext cloneContext)
		{
			FormField clonedInstanceWithoutWidgetsOverride = this.GetClonedInstanceWithoutWidgetsOverride(cloneContext);
			clonedInstanceWithoutWidgetsOverride.UserInterfaceName = this.UserInterfaceName;
			clonedInstanceWithoutWidgetsOverride.MappingName = this.MappingName;
			clonedInstanceWithoutWidgetsOverride.IsReadOnly = this.IsReadOnly;
			clonedInstanceWithoutWidgetsOverride.IsRequired = this.IsRequired;
			clonedInstanceWithoutWidgetsOverride.ShouldBeSkipped = this.ShouldBeSkipped;
			clonedInstanceWithoutWidgetsOverride.TextProperties = new VariableTextProperties(this.TextProperties);
			return clonedInstanceWithoutWidgetsOverride;
		}

		internal virtual VariableTextProperties GetDefaultTextProperties()
		{
			return new VariableTextProperties();
		}

		internal abstract FormField GetClonedInstanceWithoutWidgetsOverride(RadFixedDocumentCloneContext cloneContext);

		internal abstract void AddClonedWidget(Widget clonedWidget);

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		internal void InvalidateWidgetAppearances()
		{
			foreach (Widget widget in this.Widgets)
			{
				widget.InvalidateAppearance();
			}
		}

		internal abstract void PrepareWidgetAppearancesForExport();

		VariableTextProperties textProperties;

		readonly string name;

		readonly int id;
	}
}
