using System;

namespace Telerik.Windows.Documents.Fixed.Model
{
	class Marker
	{
		public Marker(string name)
		{
			this.name = name;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		readonly string name;
	}
}
