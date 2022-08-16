using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class ListBoxField : ChoiceField
	{
		public ListBoxField(string fieldName)
			: base(fieldName)
		{
		}

		public sealed override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.ListBox;
			}
		}

		public bool AllowMultiSelection { get; set; }

		public ChoiceOption[] Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				base.InvalidateWidgetAppearances();
			}
		}

		public ChoiceOption[] DefaultValue { get; set; }

		public int TopIndex
		{
			get
			{
				return this.topIndex;
			}
			set
			{
				if (this.topIndex != value)
				{
					this.topIndex = value;
					base.InvalidateWidgetAppearances();
				}
			}
		}

		internal override ChoiceField GetClonedChoiceField(RadFixedDocumentCloneContext cloneContext)
		{
			return new ListBoxField(base.Name)
			{
				AllowMultiSelection = this.AllowMultiSelection,
				TopIndex = this.TopIndex,
				Value = ListBoxField.GetClonedValue(this.Value, cloneContext),
				DefaultValue = ListBoxField.GetClonedValue(this.DefaultValue, cloneContext)
			};
		}

		internal int[] GetSelectedIndicesSorted()
		{
			return (from i in this.GetIndices(this.Value)
				orderby i
				select i).ToArray<int>();
		}

		internal int[] GetDefaultSelectedIndicesSorted()
		{
			return (from i in this.GetIndices(this.DefaultValue)
				orderby i
				select i).ToArray<int>();
		}

		IEnumerable<int> GetIndices(ChoiceOption[] options)
		{
			if (options != null)
			{
				foreach (ChoiceOption option in options)
				{
					int index;
					if (base.Options.TryGetOptionIndex(option, out index))
					{
						yield return index;
					}
				}
			}
			yield break;
		}

		static ChoiceOption[] GetClonedValue(ChoiceOption[] value, RadFixedDocumentCloneContext cloneContext)
		{
			if (value == null)
			{
				return null;
			}
			ChoiceOption[] array = new ChoiceOption[value.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = cloneContext.GetClonedOption(value[i]);
			}
			return array;
		}

		ChoiceOption[] value;

		int topIndex;
	}
}
