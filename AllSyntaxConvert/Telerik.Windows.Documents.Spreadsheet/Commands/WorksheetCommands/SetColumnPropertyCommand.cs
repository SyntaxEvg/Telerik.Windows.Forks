﻿using System;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetColumnPropertyCommand<T> : UndoableWorksheetCommandBase<SetRowColumnPropertyCommandContext<T>>
	{
		protected override bool AffectsLayoutOverride(SetRowColumnPropertyCommandContext<T> context)
		{
			return context.Property.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(SetRowColumnPropertyCommandContext<T> context)
		{
			context.OldValues = context.Worksheet.Columns.PropertyBag.GetPropertyValue<T>(context.Property, context.FromIndex, context.ToIndex);
		}

		protected override void ExecuteOverride(SetRowColumnPropertyCommandContext<T> context)
		{
			if (context.NewValue == null)
			{
				context.Worksheet.Columns.PropertyBag.ClearPropertyValue<T>(context.Property, context.FromIndex, context.ToIndex);
				return;
			}
			context.Worksheet.Columns.PropertyBag.SetPropertyValue<T>(context.Property, context.FromIndex, context.ToIndex, context.NewValue.Value);
		}

		protected override void UndoOverride(SetRowColumnPropertyCommandContext<T> context)
		{
			context.Worksheet.Columns.PropertyBag.SetPropertyValue<T>(context.Property, context.OldValues);
		}
	}
}
