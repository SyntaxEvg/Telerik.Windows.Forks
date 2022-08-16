using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class FieldCreationContext
	{
		public FieldCreationContext(IPdfImportContext importContext, FormFieldNode node)
		{
			this.importContext = importContext;
			this.node = node;
			this.flagReader = new FlagReader<FieldFlag>(this.node.FieldFlags.Value);
		}

		public string FullName
		{
			get
			{
				return this.node.FullName;
			}
		}

		public string UserInterfaceName
		{
			get
			{
				if (this.node.UserInterfaceName != null)
				{
					return this.node.UserInterfaceName.ToString();
				}
				return null;
			}
		}

		public string MappingName
		{
			get
			{
				if (this.node.MappingName != null)
				{
					return this.node.MappingName.ToString();
				}
				return null;
			}
		}

		public string FieldType
		{
			get
			{
				return this.node.FieldType.Value;
			}
		}

		public FlagReader<FieldFlag> FieldFlags
		{
			get
			{
				return this.flagReader;
			}
		}

		public int? MaxLen
		{
			get
			{
				if (this.node.MaxLengthOfInputCharacters != null)
				{
					return new int?(this.node.MaxLengthOfInputCharacters.Value);
				}
				return null;
			}
		}

		public bool HasWidgetsWithDifferentOnStateNames
		{
			get
			{
				string text = null;
				foreach (WidgetObject widgetObject in this.importContext.GetChildWidgets(this.node))
				{
					string text2;
					if (widgetObject.TryGetOnStateName(out text2))
					{
						if (text == null)
						{
							text = text2;
						}
						else if (text != text2)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		readonly IPdfImportContext importContext;

		readonly FormFieldNode node;

		readonly FlagReader<FieldFlag> flagReader;
	}
}
