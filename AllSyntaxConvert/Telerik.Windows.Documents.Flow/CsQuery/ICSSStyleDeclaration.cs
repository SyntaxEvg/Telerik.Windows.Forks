using System;
using CsQuery.Implementation;

namespace CsQuery
{
	interface ICSSStyleDeclaration
	{
		int Length { get; }

		string CssText { get; set; }

		ICSSRule ParentRule { get; }

		event EventHandler<CSSStyleChangedArgs> OnHasStylesChanged;

		bool HasStyle(string styleName);

		void SetStyles(string styles);

		void SetStyles(string styles, bool strict);

		void SetStyle(string name, string value);

		void SetStyle(string name, string value, bool strict);

		string GetStyle(string name);

		bool RemoveStyle(string name);
	}
}
