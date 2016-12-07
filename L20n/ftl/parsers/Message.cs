// Glen De Cauwsemaecker licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

using L20n.IO;

namespace L20n
{
	namespace FTL
	{
		namespace Parsers
		{
			/// <summary>
			/// The combinator parser used to parse an entity (similar to L10n's entity).
			/// </summary>
			public static class Message
			{
				public static bool PeekAndParse(CharStream cs, Context ctx, out L20n.FTL.AST.INode entity)
				{
					if(Identifier.PeekAndParse(cs, out entity)) {
						entity = Parse(cs, ctx, entity as FTL.AST.StringPrimitive);
						return true;
					}

					return false;
				}

				private static FTL.AST.Entity Parse(CharStream cs, Context ctx, FTL.AST.StringPrimitive identifier)
				{
					WhiteSpace.Parse(cs);
					cs.SkipCharacter('=');
					WhiteSpace.Parse(cs);


					FTL.AST.Pattern pattern = null;
					// check if we have a Pattern available
					if(!CharStream.IsNL(cs.PeekNext())) {
						pattern = Pattern.Parse(cs);
					}

					WhiteSpace.Parse(cs);

					NewLine.Parse(cs);
					FTL.AST.MemberList memberList;
					bool parsedMemberList = MemberList.PeekAndParse(cs, out memberList);
					if(!parsedMemberList && pattern == null) {
						cs.CreateException(
							"member-list was epcted, as no pattern was found", null);
					}

					return new FTL.AST.Entity(identifier, pattern, memberList);
				}
			}
		}
	}
}