using System;
using System.Collections;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter.Configuration
{
	public class TransformFactory
	{
		public void AddTransform(string transformType)
		{
			this.AddTransform((IRewriteTransform)TypeHelper.Activate(transformType, null));
		}

		public void AddTransform(IRewriteTransform transform)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			this._transforms.Add(transform.Name, transform);
		}

		public IRewriteTransform GetTransform(string name)
		{
			return this._transforms[name] as IRewriteTransform;
		}

		Hashtable _transforms = new Hashtable();
	}
}
