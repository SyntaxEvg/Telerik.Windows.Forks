using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class NameCollection : IEnumerable<ISpreadsheetName>, IEnumerable
	{
		internal SpreadsheetNameCollectionScope Owner
		{
			get
			{
				return this.owner;
			}
		}

		internal NameCollection(SpreadsheetNameCollectionScope owner)
		{
			Guard.ThrowExceptionIfNull<SpreadsheetNameCollectionScope>(owner, "owner");
			this.owner = owner;
			this.innerList = new Dictionary<string, ISpreadsheetName>();
		}

		public ISpreadsheetName this[string name]
		{
			get
			{
				return this.innerList[name.ToLowerInvariant()];
			}
		}

		public ISpreadsheetName Add(string name, string refersTo, int rowIndex, int columnIndex, string comment = null, bool isVisible = true)
		{
			if (string.IsNullOrEmpty(name) || !DefinedName.IsNameValid(name))
			{
				throw new SpreadsheetNameException("The name you have entered is not valid", "Spreadsheet_ErrorExpressions_InvalidName");
			}
			if (string.IsNullOrEmpty(refersTo))
			{
				throw new SpreadsheetNameException("The value that you entered is not valid.", "Spreadsheet_ErrorExpressions_InvalidValue");
			}
			if (this.innerList.ContainsKey(name.ToLowerInvariant()))
			{
				throw new SpreadsheetNameException("The name entered already exists. Enter a unique name.", "Spreadsheet_ErrorExpressions_NameExists");
			}
			this.EnsureNonEmptyOwner();
			FormulaCellValue formulaCellValue = this.CreateFormulaCellValue(refersTo, rowIndex, columnIndex);
			DefinedName definedName = DefinedName.Create(name, this.owner, formulaCellValue, comment, isVisible);
			AddRemoveSpreadsheetNameCommandContext context = new AddRemoveSpreadsheetNameCommandContext(this.owner, definedName);
			this.owner.CurrentWorksheet.ExecuteCommand<AddRemoveSpreadsheetNameCommandContext>(WorkbookCommands.AddSpreadsheetName, context);
			return definedName;
		}

		public ISpreadsheetName Add(string name, string refersTo, CellIndex cellIndex, string comment = null, bool isVisible = true)
		{
			if (cellIndex == null)
			{
				throw new SpreadsheetNameException("The cell index that you provided is not valid", "Spreadsheet_ErrorExpressions_InvalidCellIndex");
			}
			return this.Add(name, refersTo, cellIndex.RowIndex, cellIndex.ColumnIndex, comment, isVisible);
		}

		public void Remove(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			ISpreadsheetName name2 = this[name];
			AddRemoveSpreadsheetNameCommandContext context = new AddRemoveSpreadsheetNameCommandContext(this.owner, name2);
			this.owner.CurrentWorksheet.ExecuteCommand<AddRemoveSpreadsheetNameCommandContext>(WorkbookCommands.RemoveSpreadsheetName, context);
		}

		public bool Contains(string name)
		{
			return this.innerList.ContainsKey(name.ToLowerInvariant());
		}

		public bool TryGetSpreadsheetName(string name, out ISpreadsheetName spreadsheetName)
		{
			return this.innerList.TryGetValue(name.ToLowerInvariant(), out spreadsheetName);
		}

		internal void AddInternal(ISpreadsheetName name)
		{
			Guard.ThrowExceptionIfNull<ISpreadsheetName>(name, "name");
			this.OnChanging(new NameCollectionChangingEventArgs(null, null, name.Name, name.RefersTo));
			this.innerList.Add(name.Name.ToLowerInvariant(), name);
			this.OnChanged();
		}

		internal void RemoveInternal(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.OnChanging(new NameCollectionChangingEventArgs(name, null, null, null));
			this.innerList.Remove(name.ToLowerInvariant());
			this.OnChanged();
		}

		internal void RemoveInternal(ISpreadsheetName name)
		{
			Guard.ThrowExceptionIfNull<ISpreadsheetName>(name, "name");
			this.RemoveInternal(name.Name);
		}

		internal void Update(ISpreadsheetName oldName, string name, string refersTo, CellIndex cellIndex, string comment = null)
		{
			Guard.ThrowExceptionIfNull<ISpreadsheetName>(oldName, "oldName");
			this.OnChanging(new NameCollectionChangingEventArgs(oldName.Name, oldName.RefersTo, name, refersTo));
			this.SuspendChangeEvents();
			this.Owner.Workbook.History.BeginUndoGroup();
			try
			{
				this.Remove(oldName.Name);
				this.Add(name, refersTo, cellIndex, comment, true);
				this.Owner.Workbook.History.EndUndoGroup();
			}
			catch
			{
				this.Owner.Workbook.History.CancelUndoGroup();
				throw;
			}
			finally
			{
				this.ResumeChangeEvents();
				this.OnChanged();
			}
		}

		internal IEnumerable<ISpreadsheetName> GetNameBoxDefinedNames()
		{
			foreach (ISpreadsheetName name in this.innerList.Values)
			{
				DefinedName definedName = name as DefinedName;
				if (definedName != null && definedName.CellReferenceRangeExpression != null)
				{
					yield return definedName;
				}
			}
			yield break;
		}

		void UpdateDependentNames(Action<DefinedName> action)
		{
			foreach (ISpreadsheetName spreadsheetName in this.innerList.Values)
			{
				DefinedName definedName = spreadsheetName as DefinedName;
				if (definedName != null)
				{
					action(definedName);
				}
			}
		}

		internal void UpdateSheetNameDependentNames(Worksheet renamedWorksheet)
		{
			this.UpdateDependentNames(delegate(DefinedName name)
			{
				if (name.FormulaCellValue.IsSheetNameDependent)
				{
					name.FormulaCellValue.Translate(renamedWorksheet);
				}
			});
		}

		internal void UpdateWorkbookNameDependentNames(Workbook renamedWorkbook)
		{
			this.UpdateDependentNames(delegate(DefinedName name)
			{
				if (name.FormulaCellValue.IsWorkbookNameDependent)
				{
					name.FormulaCellValue.Translate(renamedWorkbook);
				}
			});
		}

		internal void UpdateSpreadsheetNameDependentNames(string oldName, string newName)
		{
			this.UpdateDependentNames(delegate(DefinedName name)
			{
				if (name.FormulaCellValue.ContainsSpreadsheetName)
				{
					name.FormulaCellValue.Translate(oldName, newName);
				}
			});
		}

		internal void CopyFrom(NameCollection fromNameCollection, CopyContext context)
		{
			foreach (KeyValuePair<string, ISpreadsheetName> keyValuePair in fromNameCollection.innerList)
			{
				ISpreadsheetName spreadsheetName = ((ICopyable<ISpreadsheetName>)keyValuePair.Value).Copy(context);
				if (!context.SpreadsheetNameExistsInTargetWorkbook)
				{
					this.innerList.Add(keyValuePair.Key, spreadsheetName);
				}
				else
				{
					context.TargetWorksheet.Names.AddInternal(spreadsheetName);
				}
			}
		}

		internal void Clear()
		{
			this.innerList.Clear();
		}

		void EnsureNonEmptyOwner()
		{
			if (this.owner.CurrentWorksheet == null)
			{
				throw new SpreadsheetNameException("Workbook should contain at least one worksheet", "Spreadsheet_NameCollection_EmptyWorkbookError");
			}
		}

		FormulaCellValue CreateFormulaCellValue(string refersTo, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNullOrEmpty(refersTo, "refersTo");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			ParseResult parseResult = ParseResult.Unsuccessful;
			RadExpression value = null;
			InputStringCollection translatableString = null;
			if (SpreadsheetCultureHelper.IsCharEqualTo(refersTo[0], new string[] { "=" }) && refersTo.Length > 1)
			{
				parseResult = this.TryParseRadExpression(refersTo, rowIndex, columnIndex, out value, out translatableString);
			}
			else
			{
				if (!char.IsLetter(refersTo[0]))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("=");
					stringBuilder.Append(refersTo);
					parseResult = this.TryParseRadExpression(stringBuilder.ToString(), rowIndex, columnIndex, out value, out translatableString);
				}
				if (parseResult != ParseResult.Successful)
				{
					parseResult = this.TryParseRadExpression(NameCollection.GetEscapedFormulaString(refersTo), rowIndex, columnIndex, out value, out translatableString);
				}
			}
			if (parseResult == ParseResult.Error)
			{
				throw new ParseException(SpreadsheetStrings.GeneralErrorMessage);
			}
			return new FormulaCellValue(translatableString, value, rowIndex, columnIndex);
		}

		static string GetEscapedFormulaString(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("=");
			stringBuilder.Append(TextHelper.EncodeValue(value));
			return stringBuilder.ToString();
		}

		ParseResult TryParseRadExpression(string value, int rowIndex, int columnIndex, out RadExpression expression, out InputStringCollection stringExpressionCollection)
		{
			return RadExpression.TryParse(value, this.owner.CurrentWorksheet, rowIndex, columnIndex, out expression, out stringExpressionCollection, this.owner.IsGlobal);
		}

		void SuspendChangeEvents()
		{
			this.suspendChangeEventsCount++;
		}

		void ResumeChangeEvents()
		{
			if (this.suspendChangeEventsCount == 0)
			{
				throw new SpreadsheetNameException("There is no active suspend to resume.", new InvalidOperationException("There is no active suspend to resume."), "Spreadsheet_ErrorExpressions_NoActiveSuspendToResume");
			}
			if (this.suspendChangeEventsCount > 0)
			{
				this.suspendChangeEventsCount--;
			}
		}

		bool AreChangeEventsSuspended()
		{
			return this.suspendChangeEventsCount > 0;
		}

		internal event EventHandler<NameCollectionChangingEventArgs> Changing;

		void OnChanging(NameCollectionChangingEventArgs args)
		{
			if (this.AreChangeEventsSuspended())
			{
				return;
			}
			if (this.Changing != null)
			{
				this.Changing(this, args);
			}
		}

		internal event EventHandler Changed;

		void OnChanged()
		{
			if (this.AreChangeEventsSuspended())
			{
				return;
			}
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ISpreadsheetName>)this).GetEnumerator();
		}

		public IEnumerator<ISpreadsheetName> GetEnumerator()
		{
			return this.innerList.Values.GetEnumerator();
		}

		readonly Dictionary<string, ISpreadsheetName> innerList;

		readonly SpreadsheetNameCollectionScope owner;

		int suspendChangeEventsCount;
	}
}
