using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public class DocumentVariableCollection : IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		internal DocumentVariableCollection()
		{
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			this.documentVariables = new Dictionary<string, string>(ordinalIgnoreCase);
		}

		public int Count
		{
			get
			{
				return this.documentVariables.Count;
			}
		}

		public string this[string name]
		{
			get
			{
				string result;
				if (!this.documentVariables.TryGetValue(name, out result))
				{
					throw new KeyNotFoundException("The collection does not contains the specified variable name.");
				}
				return result;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "value");
				this.documentVariables[name] = value;
			}
		}

		public void Add(string name, string value)
		{
			this.documentVariables[name] = value;
		}

		public bool Remove(string name)
		{
			return this.documentVariables.Remove(name);
		}

		public bool Contains(string name)
		{
			return this.documentVariables.ContainsKey(name);
		}

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this.documentVariables.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal bool TryGetValue(string name, out string value)
		{
			return this.documentVariables.TryGetValue(name, out value);
		}

		internal void AddClonedDocumentVariablesFrom(DocumentVariableCollection fromDocumentVariableCollection)
		{
			foreach (KeyValuePair<string, string> keyValuePair in fromDocumentVariableCollection)
			{
				this.documentVariables.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		readonly Dictionary<string, string> documentVariables;
	}
}
