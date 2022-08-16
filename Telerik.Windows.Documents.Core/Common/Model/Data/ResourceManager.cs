using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Common.Model.Data
{
	class ResourceManager : IEnumerable<IResource>, IEnumerable
	{
		public ResourceManager()
		{
			this.resourceInstances = new Dictionary<IResource, int>();
			this.nextId = 1;
		}

		public void RegisterResource(IResource resource)
		{
			Guard.ThrowExceptionIfNull<IResource>(resource, "resource");
			if (resource.Owner != null && resource.Owner != this)
			{
				throw new ArgumentException("The resource is associated with another owner.", "resource");
			}
			if (!this.resourceInstances.ContainsKey(resource))
			{
				this.resourceInstances[resource] = 1;
				resource.Id = this.nextId++;
				resource.Owner = this;
				return;
			}
			Dictionary<IResource, int> dictionary;
			(dictionary = this.resourceInstances)[resource] = dictionary[resource] + 1;
		}

		public void ReleaseResource(IResource resource)
		{
			Guard.ThrowExceptionIfNull<IResource>(resource, "resource");
			Dictionary<IResource, int> dictionary;
			if (this.resourceInstances.ContainsKey(resource) && ((dictionary = this.resourceInstances)[resource] = dictionary[resource] - 1) == 0)
			{
				this.resourceInstances.Remove(resource);
				resource.Owner = null;
			}
		}

		public IEnumerator<IResource> GetEnumerator()
		{
			return this.resourceInstances.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.resourceInstances.Keys.GetEnumerator();
		}

		readonly Dictionary<IResource, int> resourceInstances;

		int nextId;
	}
}
