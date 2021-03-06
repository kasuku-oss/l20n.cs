// Glen De Cauwsemaecker licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;

namespace L20n
{
	namespace FTL
	{
		namespace Parsers
		{
			/// <summary>
			/// The inerface for a L20n Parser.
			/// </summary>
			public interface IParser
			{	
				FTL.AST.Body Parse(StreamReader cs, Context ctx);
				string ParserName();
			}
		}
	}
}