using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class NameManager
	{
		public NameCollection WorkbookNameCollection
		{
			get
			{
				return this.workbookCollection;
			}
		}

		public SpreadsheetNameCollectionScope WorkbookScope
		{
			get
			{
				return this.workbookCollection.Owner;
			}
		}

		public Dictionary<int, IEnumerable<ISpreadsheetName>> NameWithWorksheetIdCollections
		{
			get
			{
				Dictionary<int, IEnumerable<ISpreadsheetName>> dictionary = new Dictionary<int, IEnumerable<ISpreadsheetName>>();
				foreach (KeyValuePair<Worksheet, NameCollection> keyValuePair in this.worksheetToCollection)
				{
					dictionary.Add(keyValuePair.Key.Workbook.Worksheets.IndexOf(keyValuePair.Key), keyValuePair.Value);
				}
				dictionary.Add(-1, this.workbookCollection);
				return dictionary;
			}
		}

		public IEnumerable<ISpreadsheetName> Names
		{
			get
			{
				IEnumerable<ISpreadsheetName> enumerable = this.workbookCollection;
				foreach (NameCollection second in this.worksheetToCollection.Values)
				{
					enumerable = enumerable.Concat(second);
				}
				return enumerable;
			}
		}

		public IEnumerable<SpreadsheetNameCollectionScope> Scopes
		{
			get
			{
				yield return this.WorkbookScope;
				foreach (SpreadsheetNameCollectionScope owner in this.ownerToWorksheet.Keys)
				{
					yield return owner;
				}
				yield break;
			}
		}

		public NameManager(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.worksheetToCollection = new Dictionary<Worksheet, NameCollection>();
			this.ownerToWorksheet = new Dictionary<SpreadsheetNameCollectionScope, Worksheet>();
			this.workbookCollection = new NameCollection(new SpreadsheetNameCollectionScope(workbook));
			this.workbookCollection.Changing += this.NameCollectionChanging;
			this.workbookCollection.Changed += this.NameCollectionChanged;
		}

		ISpreadsheetName GetGlobalName(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			ISpreadsheetName result = null;
			this.workbookCollection.TryGetSpreadsheetName(name, out result);
			return result;
		}

		ISpreadsheetName GetLocalName(Worksheet worksheet, string name)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			ISpreadsheetName result = null;
			if (this.worksheetToCollection.ContainsKey(worksheet))
			{
				this.worksheetToCollection[worksheet].TryGetSpreadsheetName(name, out result);
			}
			return result;
		}

		public IEnumerable<ISpreadsheetName> GetOrderedNameBoxDefinedNames(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfFalse(this.worksheetToCollection.ContainsKey(worksheet), "WorksheetToCollection does not contain worksheet");
			IEnumerable<ISpreadsheetName> nameBoxDefinedNames = this.GetNameBoxDefinedNames(worksheet);
			return from name in nameBoxDefinedNames
				orderby name.Name
				select name;
		}

		IEnumerable<ISpreadsheetName> GetNameBoxDefinedNames(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfFalse(this.worksheetToCollection.ContainsKey(worksheet), "WorksheetToCollection does not contain worksheet");
			NameCollection nameCollection;
			if (this.worksheetToCollection.TryGetValue(worksheet, out nameCollection))
			{
				IEnumerable<ISpreadsheetName> localNames = nameCollection.GetNameBoxDefinedNames();
				foreach (ISpreadsheetName name in localNames)
				{
					yield return name;
				}
			}
			IEnumerable<ISpreadsheetName> globalNames = this.workbookCollection.GetNameBoxDefinedNames();
			foreach (ISpreadsheetName globalName in globalNames)
			{
				if (!nameCollection.Contains(globalName.Name))
				{
					yield return globalName;
				}
			}
			yield break;
		}

		public ISpreadsheetName Add(SpreadsheetNameCollectionScope scope, string name, string refersTo, CellIndex cellIndex, string comment = null)
		{
			NameCollection nameCollection = this.FindNameCollection(scope);
			return nameCollection.Add(name, refersTo, cellIndex, comment, true);
		}

		public void Remove(ISpreadsheetName name)
		{
			Guard.ThrowExceptionIfNull<ISpreadsheetName>(name, "name");
			NameCollection nameCollection = this.FindNameCollection(name.Scope);
			nameCollection.Remove(name.Name);
		}

		public void Update(ISpreadsheetName oldName, string name, string refersTo, CellIndex cellIndex, string comment = null)
		{
			NameCollection nameCollection = this.FindNameCollection(oldName.Scope);
			nameCollection.Update(oldName, name, refersTo, cellIndex, comment);
		}

		NameCollection FindNameCollection(SpreadsheetNameCollectionScope scope)
		{
			if (scope == this.WorkbookScope)
			{
				return this.workbookCollection;
			}
			Worksheet key;
			NameCollection result;
			if (this.ownerToWorksheet.TryGetValue(scope, out key) && this.worksheetToCollection.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}

		public NameCollection CreateCollection(Worksheet worksheet)
		{
			SpreadsheetNameCollectionScope spreadsheetNameCollectionScope = new SpreadsheetNameCollectionScope(worksheet);
			NameCollection nameCollection = new NameCollection(spreadsheetNameCollectionScope);
			this.worksheetToCollection.Add(worksheet, nameCollection);
			this.ownerToWorksheet.Add(spreadsheetNameCollectionScope, worksheet);
			nameCollection.Changing += this.NameCollectionChanging;
			nameCollection.Changed += this.NameCollectionChanged;
			return nameCollection;
		}

		public void RemoveOwner(Worksheet worksheet)
		{
			NameCollection nameCollection;
			if (this.worksheetToCollection.TryGetValue(worksheet, out nameCollection))
			{
				nameCollection.Changing -= this.NameCollectionChanging;
				nameCollection.Changed -= this.NameCollectionChanged;
			}
			this.worksheetToCollection.Remove(worksheet);
		}

		void NameCollectionChanging(object sender, NameCollectionChangingEventArgs args)
		{
			this.OnChanging(args);
		}

		void NameCollectionChanged(object sender, EventArgs e)
		{
			this.OnChanged();
		}

		public ISpreadsheetName FindSpreadsheetName(string qualifier, string name, Worksheet currentWorksheet, Workbook workbook)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<Worksheet>(currentWorksheet, "currentWorksheet");
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			if (string.IsNullOrEmpty(qualifier))
			{
				ISpreadsheetName spreadsheetName = this.GetLocalName(currentWorksheet, name);
				if (spreadsheetName == null)
				{
					spreadsheetName = workbook.NameManager.GetGlobalName(name);
				}
				return spreadsheetName;
			}
			string text = TextHelper.DecodeWorksheetName(qualifier);
			if (text.Equals(workbook.Name, StringComparison.CurrentCultureIgnoreCase))
			{
				return this.GetGlobalName(name);
			}
			Worksheet worksheetByName = SpreadsheetHelper.GetWorksheetByName(workbook, text);
			if (worksheetByName != null)
			{
				return this.GetLocalName(worksheetByName, name);
			}
			return null;
		}

		internal event EventHandler<NameCollectionChangingEventArgs> Changing;

		void OnChanging(NameCollectionChangingEventArgs args)
		{
			if (this.Changing != null)
			{
				this.Changing(this, args);
			}
		}

		public event EventHandler Changed;

		void OnChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		readonly Dictionary<Worksheet, NameCollection> worksheetToCollection;

		readonly Dictionary<SpreadsheetNameCollectionScope, Worksheet> ownerToWorksheet;

		readonly NameCollection workbookCollection;
	}
}
