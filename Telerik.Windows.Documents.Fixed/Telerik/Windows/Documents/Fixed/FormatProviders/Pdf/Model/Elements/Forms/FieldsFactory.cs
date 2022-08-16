using System;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class FieldsFactory
	{
		public static FormField CreateField(FieldCreationContext creationContext)
		{
			string fieldType;
			if ((fieldType = creationContext.FieldType) != null)
			{
				FormField formField;
				if (!(fieldType == "Btn"))
				{
					if (!(fieldType == "Ch"))
					{
						if (!(fieldType == "Tx"))
						{
							if (!(fieldType == "Sig"))
							{
								goto IL_69;
							}
							formField = new SignatureField(creationContext.FullName);
						}
						else
						{
							formField = FieldsFactory.CreateTextField(creationContext);
						}
					}
					else
					{
						formField = FieldsFactory.CreateChoiceField(creationContext);
					}
				}
				else
				{
					formField = FieldsFactory.CreateButtonField(creationContext);
				}
				formField.IsReadOnly = creationContext.FieldFlags.IsSet(FieldFlag.ReadOnly);
				formField.IsRequired = creationContext.FieldFlags.IsSet(FieldFlag.Required);
				formField.ShouldBeSkipped = creationContext.FieldFlags.IsSet(FieldFlag.NoExport);
				formField.UserInterfaceName = creationContext.UserInterfaceName;
				formField.MappingName = creationContext.MappingName;
				return formField;
			}
			IL_69:
			throw new NotSupportedException(string.Format("Not supported field type: {0}", creationContext.FieldType));
		}

		static FormField CreateButtonField(FieldCreationContext creationContext)
		{
			FormField result;
			if (creationContext.FieldFlags.IsSet(FieldFlag.PushButton))
			{
				result = new PushButtonField(creationContext.FullName);
			}
			else if (creationContext.FieldFlags.IsSet(FieldFlag.Radio) || creationContext.HasWidgetsWithDifferentOnStateNames)
			{
				result = new RadioButtonField(creationContext.FullName)
				{
					AllowToggleOff = !creationContext.FieldFlags.IsSet(FieldFlag.NoToggleToOff),
					ShouldUpdateRadiosInUnison = creationContext.FieldFlags.IsSet(FieldFlag.RadiosInUnison)
				};
			}
			else
			{
				result = new CheckBoxField(creationContext.FullName);
			}
			return result;
		}

		static FormField CreateChoiceField(FieldCreationContext creationContext)
		{
			ChoiceField choiceField;
			if (creationContext.FieldFlags.IsSet(FieldFlag.Combo))
			{
				choiceField = new ComboBoxField(creationContext.FullName)
				{
					HasEditableTextBox = creationContext.FieldFlags.IsSet(FieldFlag.Edit),
					ShouldSpellCheck = !creationContext.FieldFlags.IsSet(FieldFlag.DoNotSpellCheck)
				};
			}
			else
			{
				choiceField = new ListBoxField(creationContext.FullName)
				{
					AllowMultiSelection = creationContext.FieldFlags.IsSet(FieldFlag.MultiSelect)
				};
			}
			choiceField.ShouldCommitOnSelectionChange = creationContext.FieldFlags.IsSet(FieldFlag.CommitOnSelChange);
			return choiceField;
		}

		static FormField CreateTextField(FieldCreationContext creationContext)
		{
			bool flag = creationContext.FieldFlags.IsSet(FieldFlag.Comb);
			bool flag2 = creationContext.FieldFlags.IsSet(FieldFlag.Multiline);
			bool flag3 = creationContext.FieldFlags.IsSet(FieldFlag.Password);
			bool flag4 = creationContext.FieldFlags.IsSet(FieldFlag.FileSelect);
			TextField result;
			if (flag && creationContext.MaxLen != null && !flag2 && !flag3 && !flag4)
			{
				result = new CombTextBoxField(creationContext.FullName)
				{
					MaxLengthOfInputCharacters = creationContext.MaxLen.Value
				};
			}
			else
			{
				result = new TextBoxField(creationContext.FullName)
				{
					MaxLengthOfInputCharacters = creationContext.MaxLen,
					IsMultiline = flag2,
					IsPassword = flag3,
					IsFileSelect = flag4,
					ShouldSpellCheck = !creationContext.FieldFlags.IsSet(FieldFlag.DoNotSpellCheck),
					AllowScroll = !creationContext.FieldFlags.IsSet(FieldFlag.DoNotScroll)
				};
			}
			return result;
		}
	}
}
