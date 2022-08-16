using System;

namespace Telerik.Windows.Documents.Common.Model.Data
{
	interface IResource
	{
		int Id { get; set; }

		string Name { get; }

		byte[] Data { get; }

		ResourceManager Owner { get; set; }

		string Extension { get; }
	}
}
