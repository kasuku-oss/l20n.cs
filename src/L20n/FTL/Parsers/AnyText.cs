// Glen De Cauwsemaecker licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;

using L20n.IO;

namespace L20n
{
	namespace FTL
	{
		namespace Parsers
		{
			/// <summary>
			/// Parser used to parse all types of text
			/// </summary>
			public static class AnyText
			{
				public static bool PeekUnquoted(CharStream cs)
				{
					char next = cs.PeekNext();
					return !CharStream.IsEOF(next) && !CharStream.IsNL(next) &&
						next != '{' && next != '}';
				}

				// ([^{}] | '\{' | '\}' )+
				public static FTL.AST.StringPrimitive ParseUnquoted(CharStream cs)
				{
					s_Buffer.Clear();

					bool allowCB = false;
					char next = cs.PeekNext();
					while(!CharStream.IsEOF(next) && !CharStream.IsNL(next) &&
					      (allowCB || (next != '{' && next != '}'))) {
						s_Buffer.Add(next);
						cs.SkipNext();
						allowCB = !allowCB && next == '\\';
						next = cs.PeekNext();
					}

					if(s_Buffer.Count == 0) {
						throw cs.CreateException(
							"no unquoted text could be parsed, while this was expected", null);
					}

					return new FTL.AST.StringPrimitive(new string(s_Buffer.ToArray()));
				}

				// ([^{"] | '\{' | '\"')+
				public static FTL.AST.QuotedText ParseQuoted(CharStream cs)
				{
					s_Buffer.Clear();
					WhiteSpace.Parse(cs);
					
					bool allowSC = false;
					char next = cs.PeekNext();
					while(!CharStream.IsEOF(next) && !CharStream.IsNL(next) &&
					      (allowSC || (next != '{' && next != '"'))) {
						s_Buffer.Add(next);
						cs.SkipNext();
						allowSC = !allowSC && next == '\\';
						next = cs.PeekNext();
					}
					
					if(s_Buffer.Count == 0) {
						throw cs.CreateException(
							"no quoted text could be parsed, while this was expected", null);
					}
					
					return new FTL.AST.QuotedText(new string(s_Buffer.ToArray()));
				}

				public static bool PeekBlockText(CharStream cs)
				{
					char next = cs.PeekNext();
					// if next char isn't a NewLine Character, we know for sure we
					// are not dealing with a block-text
					if(CharStream.IsEOF(next) || !CharStream.IsNL(next))
						return false;
					
					// from here on out, we're still not sure if we're dealing with a block-text
					// thus we start buffering so we can go back in time
					// here we check if we have the following pattern: `NL __ '|'`
					int bufferPos = cs.Position;
					NewLine.Parse(cs);
					WhiteSpace.Parse(cs);
					
					// if the next unbuffered char is not '|' we're not dealing with a block-text
					// and can return;
					next = cs.PeekNext();
					cs.Rewind(bufferPos);
					return next == '|';
				}

				// NL __ '|' __ (unquoted-text | placeable)+
				public static bool PeekAndParseBlock(CharStream cs, out FTL.AST.INode result)
				{
					char next = cs.PeekNext();
					// if next char isn't a NewLine Character, we know for sure we
					// are not dealing with a block-text
					if(CharStream.IsEOF(next) || !CharStream.IsNL(next)) {
						result = null;
						return false;
					}

					// from here on out, we're still not sure if we're dealing with a block-text
					// thus we start buffering so we can go back in time
					// here we check if we have the following pattern: `NL __ '|'`
					int bufferPos = cs.Position;
					NewLine.Parse(cs);
					WhiteSpace.Parse(cs);

					// if the next unbuffered char is not '|' we're not dealing with a block-text
					// and can return;
					if(cs.PeekNext() != '|') {
						cs.Rewind(bufferPos);
						result = null;
						return false;
					}

					// we know for sure we're dealing with a block-text,
					// buffer can be flushed and we can start checking for more lines as well;
					FTL.AST.INode line;

					FTL.AST.BlockText blockText = new FTL.AST.BlockText();
					do {
						cs.SkipNext(); // skip '|'

						WhiteSpace.Parse(cs);

						if(!Placeable.PeekAndParse(cs, out line)) {
							// it's not a placeable, so it must be unquoted-text
							line = ParseUnquoted(cs);
						}

						// add line
						blockText.AddLine(line);

						// peek if next char is a newline char
						// otherwise we can stop early with trying
						next = cs.PeekNext();
						if(CharStream.IsEOF(next) || !CharStream.IsNL(next))
							break;

						// check if we have more lines
						bufferPos = cs.Position;
						NewLine.Parse(cs);
						WhiteSpace.Parse(cs);

						
						// as long as the next char is '|'
						// we'll keep looping
						if(cs.PeekNext() != '|') {
							cs.Rewind(bufferPos);
							break;
						}
					} while(true);

					result = blockText;
					return true;
				}

				private static List<char> s_Buffer = new List<char>(80);
			}
		}
	}
}