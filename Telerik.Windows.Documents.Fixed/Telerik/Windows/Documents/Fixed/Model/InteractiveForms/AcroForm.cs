using System;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class AcroForm
	{
		internal AcroForm()
		{
			this.fields = new FormFieldCollection();
		}

		public FormFieldCollection FormFields
		{
			get
			{
				return this.fields;
			}
		}

		public bool ViewersShouldRecalculateWidgetAppearances { get; set; }

		readonly FormFieldCollection fields;
	}
}
