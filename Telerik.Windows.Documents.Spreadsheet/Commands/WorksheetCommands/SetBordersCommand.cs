using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetBordersCommand<TContext> : UndoableWorksheetCommandBase<TContext> where TContext : SetBordersCommandContextBase
	{
		protected override bool AffectsLayoutOverride(TContext context)
		{
			return CellPropertyDefinitions.LeftBorderProperty.AffectsLayout || CellPropertyDefinitions.TopBorderProperty.AffectsLayout || CellPropertyDefinitions.RightBorderProperty.AffectsLayout || CellPropertyDefinitions.BottomBorderProperty.AffectsLayout || CellPropertyDefinitions.DiagonalUpBorderProperty.AffectsLayout || CellPropertyDefinitions.DiagonalDownBorderProperty.AffectsLayout;
		}

		protected override void PreserveStateBeforeExecute(TContext context)
		{
			if (context.NewLeftBorderValues != null)
			{
				context.OldLeftBorderValues = context.GetBorderProperty(CellPropertyDefinitions.LeftBorderProperty, context.LeftBorderCellRange);
			}
			if (context.NewRightBorderValues != null)
			{
				context.OldRightBorderValues = context.GetBorderProperty(CellPropertyDefinitions.RightBorderProperty, context.RightBorderCellRange);
			}
			if (context.NewTopBorderValues != null)
			{
				context.OldTopBorderValues = context.GetBorderProperty(CellPropertyDefinitions.TopBorderProperty, context.TopBorderCellRange);
			}
			if (context.NewBottomBorderValues != null)
			{
				context.OldBottomBorderValues = context.GetBorderProperty(CellPropertyDefinitions.BottomBorderProperty, context.BottomBorderCellRange);
			}
			if (context.NewDiagonalUpBorderValues != null)
			{
				context.OldDiagonalUpBorderValues = context.GetBorderProperty(CellPropertyDefinitions.DiagonalUpBorderProperty, context.CellRange);
			}
			if (context.NewDiagonalDownBorderValues != null)
			{
				context.OldDiagonalDownBorderValues = context.GetBorderProperty(CellPropertyDefinitions.DiagonalDownBorderProperty, context.CellRange);
			}
		}

		protected override void ExecuteOverride(TContext context)
		{
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.LeftBorderProperty, context.CellRange, context.NewLeftBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.RightBorderProperty, context.CellRange, context.NewRightBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.TopBorderProperty, context.CellRange, context.NewTopBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.BottomBorderProperty, context.CellRange, context.NewBottomBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.DiagonalUpBorderProperty, context.CellRange, context.NewDiagonalUpBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.DiagonalDownBorderProperty, context.CellRange, context.NewDiagonalDownBorderValues);
		}

		protected override void UndoOverride(TContext context)
		{
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.LeftBorderProperty, context.CellRange, context.OldLeftBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.RightBorderProperty, context.CellRange, context.OldRightBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.BottomBorderProperty, context.CellRange, context.OldBottomBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.TopBorderProperty, context.CellRange, context.OldTopBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.DiagonalUpBorderProperty, context.CellRange, context.OldDiagonalUpBorderValues);
			SetBordersCommand<TContext>.SetPropertyValueIfNotNull(context, CellPropertyDefinitions.DiagonalDownBorderProperty, context.CellRange, context.OldDiagonalDownBorderValues);
		}

		static void SetPropertyValueIfNotNull(TContext context, IPropertyDefinition<CellBorder> property, CellRange cellRange, ICompressedList<CellBorder> values)
		{
			if (values != null)
			{
				context.SetBorderProperty(property, cellRange, values);
			}
		}
	}
}
