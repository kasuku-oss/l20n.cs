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
			/// The AST representation for a select-expression.
			/// </summary>
			public sealed class SelectExpression : INode
			{	
				/// <summary>
				/// Initializes a new instance of the <see cref="L20n.FTL.AST.SelectExpression"/> class.
				/// </summary>
				public SelectExpression(INode expression, MemberList memberList)
				{
					m_Expression = expression;
					m_MemberList = memberList;
				}
				
				/// <summary>
				/// TODO
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
					m_Expression.Serialize(writer);
					writer.Write(" ->");
					m_MemberList.Serialize(writer);
				}

				private readonly INode m_Expression;
				private readonly MemberList m_MemberList;
			}
		}
	}
}