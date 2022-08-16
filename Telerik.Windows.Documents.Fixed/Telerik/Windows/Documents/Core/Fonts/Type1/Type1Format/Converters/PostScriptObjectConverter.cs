using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters
{
	class PostScriptObjectConverter : IConverter
	{
		public object Convert(Type resultType, object value)
		{
			PostScriptDictionary postScriptDictionary = value as PostScriptDictionary;
			if (postScriptDictionary == null)
			{
				return null;
			}
			PostScriptObject postScriptObject = Activator.CreateInstance(resultType) as PostScriptObject;
			if (postScriptObject == null)
			{
				return null;
			}
			postScriptObject.Load(postScriptDictionary);
			return postScriptObject;
		}
	}
}
