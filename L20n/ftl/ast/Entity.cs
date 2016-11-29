// Glen De Cauwsemaecker licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;

namespace L20n
{
	namespace FTL
	{
		namespace AST
		{
			/// <summary>
			/// represents the AST element for an entity, AKA <message>
			/// </summary>
			public sealed class Entity : INode
			{	
				/// <summary>
				/// Returns the most optimized form of itself.
				/// </summary>
				public INode Optimize()
				{
					throw new NotImplementedException();
				}

				/// <summary>
				/// Writes its content and the content of its children.
				/// </summary>
				public void Serialize(TextWriter writer)
				{
					// write the comment if one is attached to this message
					if(m_Comment != null)
						m_Comment.Serialize(writer);

					// write the actual content
					// <ADD MESSAGE CONTENT HERE>
					writer.Write('\n');
					throw new NotImplementedException();
				}

				/// <summary>
				/// Attaches the given comment to this message.
				/// </summary>
				public void AttachComment(Comment comment)
				{
					m_Comment = comment;
				}

				private Comment m_Comment;
			}
		}
	}
}