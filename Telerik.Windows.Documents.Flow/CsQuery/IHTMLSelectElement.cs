using System;

namespace CsQuery
{
	interface IHTMLSelectElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormSubmittableElement, IFormReassociateableElement, IFormAssociatedElement
	{
		IHTMLOptionsCollection Options { get; }

		int SelectedIndex { get; set; }

		IDomElement SelectedItem { get; set; }

		bool Multiple { get; set; }

		int Length { get; }
	}
}
