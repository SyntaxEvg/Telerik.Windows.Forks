using System;

namespace CsQuery
{
	enum NodeType : byte
	{
		ELEMENT_NODE = 1,
		TEXT_NODE = 3,
		CDATA_SECTION_NODE,
		COMMENT_NODE = 8,
		DOCUMENT_NODE,
		DOCUMENT_TYPE_NODE,
		DOCUMENT_FRAGMENT_NODE
	}
}
