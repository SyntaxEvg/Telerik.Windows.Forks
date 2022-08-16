using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Editing.Lists;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Collections
{
	public class ListCollection
	{
		internal ListCollection()
		{
			this.listIdGenerator = new IdGenerator();
			this.idToList = new Dictionary<int, List>();
		}

		internal List this[int id]
		{
			get
			{
				return this.idToList[id];
			}
		}

		internal bool TryGetList(int id, out List list)
		{
			return this.idToList.TryGetValue(id, out list);
		}

		public List AddList()
		{
			return this.AddList(new List());
		}

		public List AddList(ListTemplateType listTemplateType)
		{
			return this.AddList(new List(listTemplateType));
		}

		List AddList(List list)
		{
			list.Id = this.listIdGenerator.GetNext();
			this.idToList.Add(list.Id, list);
			return list;
		}

		readonly IdGenerator listIdGenerator;

		readonly Dictionary<int, List> idToList;
	}
}
