//=============================================================================
// WebFormRail.NET - .NET Web Application Framework 
//
// Copyright (c) 2008 Macrothinking
//
//=============================================================================

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WebFormRail
{
	public static class WikiFormatter
	{
		public static string _classImageComment = "";
		public static string _classTable = "";
		public static string _classBoxTitle = "";

		private static readonly Dictionary<char,string> _charMap = null;
		private static readonly char[] _charList = null;
        private static readonly Dictionary<string, string> _formatterCache = new Dictionary<string, string>(StringComparer.InvariantCulture);

		static WikiFormatter()
		{
            if (_charMap == null)
            {
                _charMap = new Dictionary<char, string>();
                _charMap['\''] = "&rsquo;";
                _charMap['<'] = "&lt;";
                _charMap['>'] = "&gt;";
                _charMap['\x00a1'] = "&iexcl;";
                _charMap['\x00a2'] = "&cent;";
                _charMap['\x00a3'] = "&pound;";
                _charMap['€'] = "&euro;";
                _charMap['\x00a5'] = "&yen;";
                _charMap['\x00a7'] = "&sect;";
                _charMap['\x00a8'] = "&uml;";
                _charMap['\x00aa'] = "&ordf;";
                _charMap['\x00ab'] = "&laquo;";
                _charMap['\x00ac'] = "&not";
                _charMap['\x00ad'] = "&shy;";
                _charMap['\x00b0'] = "&deg;";
                _charMap['\x00b1'] = "&plusmn;";
                _charMap['\x00b2'] = "&sup2;";
                _charMap['\x00b3'] = "&sup3;";
                _charMap['\x00b4'] = "&acute;";
                _charMap['\x00b5'] = "&micro;";
                _charMap['\x00b6'] = "&para;";
                _charMap['\x00b7'] = "&middot;";
                _charMap['\x00b8'] = "&cedil;";
                _charMap['\x00b9'] = "&sup1;";
                _charMap['\x00ba'] = "&ordm;";
                _charMap['\x00bb'] = "&raquo;";
                _charMap['\x00bc'] = "&frac14;";
                _charMap['\x00bd'] = "&frac12;";
                _charMap['\x00be'] = "&frac34;";
                _charMap['\x00bf'] = "&iquest;";
                _charMap['\x00c0'] = "&Agrave;";
                _charMap['\x00c1'] = "&Aacute;";
                _charMap['\x00c2'] = "&Acirc;";
                _charMap['\x00c3'] = "&Atilde;";
                _charMap['\x00c4'] = "&Auml;";
                _charMap['\x00c5'] = "&Aring;";
                _charMap['\x00c6'] = "&AElig;";
                _charMap['\x00c7'] = "&Ccedil;";
                _charMap['\x00c8'] = "&Egrave;";
                _charMap['\x00c9'] = "&Eacute;";
                _charMap['\x00ca'] = "&Ecirc;";
                _charMap['\x00cb'] = "&Euml;";
                _charMap['\x00cc'] = "&Igrave;";
                _charMap['\x00cd'] = "&Iacute;";
                _charMap['\x00ce'] = "&Icirc;";
                _charMap['\x00cf'] = "&Iuml;";
                _charMap['\x00d0'] = "&ETH;";
                _charMap['\x00d1'] = "&Ntilde;";
                _charMap['\x00d2'] = "&Ograve;";
                _charMap['\x00d3'] = "&Oacute;";
                _charMap['\x00d4'] = "&Ocirc;";
                _charMap['\x00d5'] = "&Otilde;";
                _charMap['\x00d6'] = "&Ouml;";
                _charMap['\x00d7'] = "&times;";
                _charMap['\x00d8'] = "&Oslash;";
                _charMap['\x00d9'] = "&Ugrave;";
                _charMap['\x00da'] = "&Uacute;";
                _charMap['\x00db'] = "&Ucirc;";
                _charMap['\x00dc'] = "&Uuml;";
                _charMap['\x00dd'] = "&Yacute;";
                _charMap['\x00de'] = "&THORN;";
                _charMap['\x00df'] = "&szlig;";
                _charMap['\x00e0'] = "&agrave;";
                _charMap['\x00e1'] = "&aacute;";
                _charMap['\x00e2'] = "&acirc;";
                _charMap['\x00e3'] = "&atilde;";
                _charMap['\x00e4'] = "&auml;";
                _charMap['\x00e5'] = "&aring;";
                _charMap['\x00e6'] = "&aelig;";
                _charMap['\x00e7'] = "&ccedil;";
                _charMap['\x00e8'] = "&egrave;";
                _charMap['\x00e9'] = "&eacute;";
                _charMap['\x00ea'] = "&ecirc;";
                _charMap['\x00eb'] = "&euml;";
                _charMap['\x00ec'] = "&igrave;";
                _charMap['\x00ed'] = "&iacute;";
                _charMap['\x00ee'] = "&icirc;";
                _charMap['\x00ef'] = "&iuml;";
                _charMap['\x00f0'] = "&eth;";
                _charMap['\x00f1'] = "&ntilde;";
                _charMap['\x00f2'] = "&ograve;";
                _charMap['\x00f3'] = "&oacute;";
                _charMap['\x00f4'] = "&ocirc;";
                _charMap['\x00f5'] = "&otilde;";
                _charMap['\x00f6'] = "&ouml;";
                _charMap['\x00f7'] = "&divide;";
                _charMap['\x00f8'] = "&oslash;";
                _charMap['\x00f9'] = "&ugrave;";
                _charMap['\x00fa'] = "&uacute;";
                _charMap['\x00fb'] = "&ucirc;";
                _charMap['\x00fc'] = "&uuml;";
                _charMap['\x00fd'] = "&yacute;";
                _charMap['\x00fe'] = "&thorn;";
                _charMap['\x00ff'] = "&yuml;";
                _charList = new char[_charMap.Count];
                int num = 0;
                foreach (char ch in _charMap.Keys)
                {
                    _charList[num++] = ch;
                }
            }

		}

		public static string ReplaceSymbols(string inputString)
		{
			if (inputString.IndexOfAny(_charList) < 0)
				return inputString;

			StringBuilder translatedString = new StringBuilder(inputString.Length);

			foreach (char c in inputString)
			{
				string tag;

				if (_charMap.TryGetValue(c, out tag))
					translatedString.Append(tag);
				else
					translatedString.Append(c);
			}

			return translatedString.ToString();
		}


		public static string FormatText(string inputString)
		{
			string cacheKey = inputString;

			lock (_formatterCache)
			{
				if (_formatterCache.ContainsKey(cacheKey))
					return _formatterCache[cacheKey];
			}

			StringBuilder outputString = new StringBuilder();

			inputString = inputString.Replace("\r","");
			inputString = inputString.Replace("<br>","");

			inputString = Regex.Replace(inputString,"<[^>]+>","",RegexOptions.Compiled);

			inputString = ReplaceSymbols(inputString);

			//inputString = Regex.Replace(inputString,"\n\\s*\n\\s*\n","\n\n",RegexOptions.Compiled);

			bool inBulletList = false;
			bool inTable = false;
			bool isFirstTableRow = false;
			char cBullet = '\0';

			int tablePadding = 1;
			bool firstRowIsHeader = false;
			char[] columnAlignments = new char[100];

			foreach (string textLine in inputString.Split('\n'))
			{
				Match match;
				bool doBreak = true;

				string newTextLine = textLine;

				// Bold : *text*
				// Italic : #text#
				// Underline : _text_

				if (newTextLine.IndexOf('*') >= 0)
					newTextLine = Regex.Replace(newTextLine, @"\*(?<text>(?<! )[^\*]+(?<! ))\*", "<b>${text}</b>", RegexOptions.Compiled);

				if (newTextLine.IndexOf('#') >= 0) 
					newTextLine = Regex.Replace(newTextLine, @"\#(?<text>(?<! )[^\#]+(?<! ))\#", "<i>${text}</i>", RegexOptions.Compiled);

				if (newTextLine.IndexOf('_') >= 0) 
					newTextLine = Regex.Replace(newTextLine, @"_(?<text>(?<! )[^_]+(?<! ))_", "<u>${text}</u>", RegexOptions.Compiled);

				if (newTextLine.IndexOf("[[") >= 0) 
					newTextLine = Regex.Replace(newTextLine, @"\[\[(?<text>(?<! )[^\]]+(?<! ))\]\]", "<table width='100%' border=1 cellpadding=2 cellspacing=0" + ((_classBoxTitle.Length == 0) ? ("") : (" class='" + _classBoxTitle + "'")) + "><tr><td>${text}</td></tr></table>", RegexOptions.Compiled);


				// Bulleted lists
				// * item1
				// * item2
				// * item3
				// 
				// or
				// - item1
				// - item2
				// - item3
				// 
				// or (for numbered lists)
				//
				// # item1
				// # item2
				// # item3

				
				match = Regex.Match(newTextLine , @"^\s*(?<bullet>-|\*|\+|#)\s(?<item>.*)",RegexOptions.Compiled);

				if (match.Success)
				{
					cBullet = match.Groups["bullet"].Value[0];

					if (!inBulletList)
					{
						outputString.Append(cBullet == '#' ? "<ol>":"<ul>");

						inBulletList = true;
					}

					newTextLine = "<li>" + match.Groups["item"].Value + "</li>";
					doBreak = false;
				}
				else
				{
					if (inBulletList)
					{
						outputString.Append(cBullet == '#' ? "</ol>":"</ul>");

						if (newTextLine.Trim().Length == 0)
							doBreak = false;
					}

					inBulletList = false;
				}

				// Process tables
				//
				// [TABLE H P3 LCR]
				// |cell1|cell2|cell3|
				// |mergedcell4||cell5|
				// |cell6|mergedcell7||

				if (newTextLine.IndexOf("[TABLE", StringComparison.InvariantCultureIgnoreCase) >= 0)
				{
					match = Regex.Match(newTextLine, @"\[TABLE(\s+(?<option>H|P\d+))*(\s+(?<cols>[LRC]+))*\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

					if (match.Success)
					{
						firstRowIsHeader = false;

						if (match.Groups["option"].Success)
						{
							foreach (Capture optionCapture in match.Groups["option"].Captures)
							{
								if (optionCapture.Value.ToUpper() == "H")
									firstRowIsHeader = true;

								if (optionCapture.Value.ToUpper().StartsWith("P"))
								{
									tablePadding = WebFormRailUtil.ConvertString<int>(optionCapture.Value.Substring(1));
								}
							}
						}

						if (match.Groups["cols"].Success)
						{
							string columnSpecs = match.Groups["cols"].Value.ToUpper();

							int i;

							for (i = 0; i < columnSpecs.Length; i++)
								columnAlignments[i] = columnSpecs[i];

							for (; i < 100; i++)
								columnAlignments[i] = 'L';
						}

						newTextLine = "";
						doBreak = false;
					}
				}

				match = Regex.Match(newTextLine , @"^\|(?<cell>[^\|]*\|)+(?<full>\+)?",RegexOptions.Compiled);

				if (match.Success)
				{
					if (!inTable)
					{
						string strExtend = "";

						if (match.Groups["full"].Success)
							strExtend = " width=\"100%\"";

						if (_classTable != null && _classTable.Length > 0)
							outputString.Append("<table cellspacing=\"0\" cellpadding=\"" + tablePadding + "\" class=\"" + _classTable + "\"" + strExtend + ">");
						else
							outputString.Append("<table cellspacing=\"0\" cellpadding=\"" + tablePadding + "\" border=\"1\"" + strExtend + ">");

						inTable = true;
						isFirstTableRow = true;
					}

					Group g = match.Groups["cell"];

					int span = 1;

					newTextLine = "";

					for (int i = g.Captures.Count - 1 ; i >= 0 ; i--)
					{
						Capture capture = g.Captures[i];

						if (capture.Value.Length == 1)
							span++;
						else
						{
							string column;
							string cellTag = "td";
							string strAlign = "left";

							switch (columnAlignments[i])
							{
								case 'L': strAlign = "left"; break;
								case 'R': strAlign = "right"; break;
								case 'C': strAlign = "center"; break;
							}

							if (isFirstTableRow && firstRowIsHeader)
								cellTag = "th";

							if (span > 1)
								column = "<" + cellTag + " align='" + strAlign + "' colspan='" + span + "'>";
							else
								column = "<" + cellTag + " align='" + strAlign + "'>";
							
							column += capture.Value.Substring(0,capture.Value.Length - 1);
								
							column += "</" + cellTag + ">";

							newTextLine = column + newTextLine;

							span = 1;
						}
					}

					newTextLine = "<tr>" + newTextLine + "</tr>";

					doBreak = false;

					isFirstTableRow = false;
				}
				else
				{
					if (inTable)
					{
						outputString.Append("</table>");

						for (int i = 0 ; i < 100; i++)
							columnAlignments[i] = 'L';

						firstRowIsHeader = false;
						tablePadding = 1;
					}

					inTable = false;
				}


				// [IMG=ImageURL,ImageLinkURL (caption)]
				// [IMG/L=ImageURL,ImageLinkURL (caption)]
				// [IMG/R=ImageURL,ImageLinkURL (caption)]
				//
				// Inserts an image in the text. "ImageURL" is required. ImageLinkURL and caption are optional

				if (newTextLine.IndexOf("[IMG",StringComparison.InvariantCultureIgnoreCase) >= 0)
				{
					match = Regex.Match(newTextLine, @"\[IMG(/(?<align>L|R))?=(?<url>[\w\.\?\&\:/@]+)(,(?<link>[^\s\(]+))?\s*(\((?<comment>.*)\))?\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

					if (match.Success)
					{
						string strAlign = "";
						string strComment = "";
						string strLink = "";

						if (match.Groups["align"].Success)
							strAlign = match.Groups["align"].Value;

						if (match.Groups["comment"].Success)
							strComment = match.Groups["comment"].Value;

						if (match.Groups["link"].Success)
							strLink = match.Groups["link"].Value;

						string strUrl = match.Groups["url"].Value;

						if (strAlign.ToUpper() == "L")
							strAlign = " align='left'";
						if (strAlign.ToUpper() == "R")
							strAlign = " align='right'";

						if (strComment.Length > 0)
						{
							if (_classImageComment != null && _classImageComment.Length > 0)
								strComment = "<font class='" + _classImageComment + "'>" + strComment + "</font>";

							strComment = "<tr><td" + strAlign + ">" + strComment + "</td></tr>";
						}

						string strImg = "<img border=0 src=\"" + strUrl + "\">";

						if (strLink.Length > 0)
							strImg = "<a href='" + strLink + "'>" + strImg + "</a>";

						string strRepl = "<table border='0' cellpadding='0' cellspacing='0'" + strAlign + ">"
						                 + "<tr><td>" + strImg + "</td></tr>"
						                 + strComment
						                 + "</table>";

						newTextLine = newTextLine.Substring(0, match.Index) + strRepl + newTextLine.Substring(match.Index + match.Length);
					}
				}

				// (c) or (r) becomes relevant symbol

				if (newTextLine.IndexOf('(') >= 0)
				{
					newTextLine = Regex.Replace(newTextLine, @"\((c|C)\)", "&copy;", RegexOptions.Compiled);
					newTextLine = Regex.Replace(newTextLine, @"\((r|R)\)", "&reg;", RegexOptions.Compiled);
				}

				// Named link:  "Link Text"="Link URL"

				if (newTextLine.IndexOf('=') >= 0) 
					newTextLine = Regex.Replace(newTextLine, @"""(?<name>[^""]+)""=""(?<url>[^""]+)""", "<a href=\"${url}\" target=\"_blank\">${name}</a>", RegexOptions.Compiled);

				// Text in form www.xxx.xxx are converted to a link with target "_blank"

				if (newTextLine.IndexOf("www") >= 0)
					newTextLine = Regex.Replace(newTextLine, @"(?<!href=""http://)www\.[\-\w\.\?\&\:/@]+", "<a href=\"http://$&\" target=\"_blank\">$&</a>", RegexOptions.Compiled);


				// E-mail adresses are converted to a mailto: URL link

				if (newTextLine.IndexOf('@') >= 0)
					newTextLine = Regex.Replace(newTextLine, @"[a-zA-Z\._0-9\-]{1,}@[A-Za-z0-9\.\-]+\.[A-Za-z0-9]{2,4}", "<a href=\"mailto:$&\">$&</a>", RegexOptions.Compiled);

				// !Title1
				// !!Title2
				// !!!Title3
				if (newTextLine.StartsWith("!"))
				{
					newTextLine = Regex.Replace(newTextLine, @"^!!!(?<title>.+)", "<h3>${title}</h3>", RegexOptions.Compiled);
					newTextLine = Regex.Replace(newTextLine, @"^!!(?<title>.+)", "<h2>${title}</h2>", RegexOptions.Compiled);
					newTextLine = Regex.Replace(newTextLine, @"^!(?<title>.+)", "<h1>${title}</h1>", RegexOptions.Compiled);
				}

				// --- at the start of a line is converted to a horizontal rule

				if (newTextLine.StartsWith("---"))
				{
					if (Regex.Match(newTextLine, @"^-{3,}").Success)
					{
						newTextLine = Regex.Replace(newTextLine, @"^-{3,}", "<hr>");

						doBreak = false;
					}
				}

				// {color:Text} is used to draw text in the specified color (named colors or HTML colors)

				if (newTextLine.IndexOf('{') >= 0)
					newTextLine = Regex.Replace(newTextLine , @"\{(?<color>[a-z#0-9]+):(?<text>[^\}]+)\}" , "<font color=\"${color}\">${text}</font>",RegexOptions.Compiled);

				outputString.Append(newTextLine);
				
				if (doBreak) 
					outputString.Append("<br>\r\n");
			}

			string finalString = outputString.ToString();

			if (finalString.EndsWith("<br>\r\n"))
				finalString = finalString.Substring(0,finalString.Length-6);

			_formatterCache[cacheKey] = finalString;

			return finalString;
		}
	}
}
