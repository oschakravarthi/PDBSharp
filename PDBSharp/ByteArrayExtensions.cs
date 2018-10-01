#region License
/*
 * Copyright (C) 2018 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smx.PDBSharp
{
	public static class ByteArrayExtensions
	{
		public static void HexDump(this byte[] bytes, int? optBytesLength = null) {
			if(bytes == null) return;
			int bytesPerLine = 16;

			int bytesLength;
			if(optBytesLength == null)
				bytesLength = bytes.Length;
			else
				bytesLength = optBytesLength.Value;

			char[] HexChars = "0123456789ABCDEF".ToCharArray();

			int firstHexColumn =
				  8                   // 8 characters for the address
				+ 3;                  // 3 spaces

			int firstCharColumn = firstHexColumn
				+ bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
				+ (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
				+ 2;                  // 2 spaces 

			int lineLength = firstCharColumn
				+ bytesPerLine           // - characters to show the ascii value
				+ Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

			char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
			int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
			StringBuilder result = new StringBuilder(expectedLines * lineLength);

			for(int i = 0; i < bytesLength; i += bytesPerLine) {
				line[0] = HexChars[(i >> 28) & 0xF];
				line[1] = HexChars[(i >> 24) & 0xF];
				line[2] = HexChars[(i >> 20) & 0xF];
				line[3] = HexChars[(i >> 16) & 0xF];
				line[4] = HexChars[(i >> 12) & 0xF];
				line[5] = HexChars[(i >> 8) & 0xF];
				line[6] = HexChars[(i >> 4) & 0xF];
				line[7] = HexChars[(i >> 0) & 0xF];

				int hexColumn = firstHexColumn;
				int charColumn = firstCharColumn;

				for(int j = 0; j < bytesPerLine; j++) {
					if(j > 0 && (j & 7) == 0) hexColumn++;
					if(i + j >= bytesLength) {
						line[hexColumn] = ' ';
						line[hexColumn + 1] = ' ';
						line[charColumn] = ' ';
					} else {
						byte b = bytes[i + j];
						line[hexColumn] = HexChars[(b >> 4) & 0xF];
						line[hexColumn + 1] = HexChars[b & 0xF];
						line[charColumn] = (b < 32 ? '·' : (char)b);
					}
					hexColumn += 3;
					charColumn++;
				}
				result.Append(line);
			}
			Console.WriteLine(result.ToString());
		}

		public static void HexDump2(this byte[] data, int size) {
			StringBuilder final = new StringBuilder();

			int offset = 0;

			int indent = 1;

			while(size > 0) {
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat(" >                                                      {0:X8}", offset);

				int outLen = size;
				if(outLen > 16)
					outLen = 16;


				int outLen2 = outLen;

				int relPos = 0;
				int index = 1 + indent;
				int index2 = 53 - 15 + indent;

				IEnumerable<byte> tmp = data.Skip(offset);

				for(; outLen2 > 0; outLen2--, index += 2, index2++) {
					var ucTmp = tmp.Take(1).First();

					string bStr = string.Format("{0:X2}", ucTmp);
					sb.Remove(index, bStr.Length);
					sb.Insert(index, bStr);

					char ch = Convert.ToChar(ucTmp);
					if(!char.IsLetterOrDigit(ch) && !char.IsPunctuation(ch) && !char.IsSymbol(ch) && ch != ' ') {
						ch = '.';
					}
					sb.Remove(index2, 1);
					sb.Insert(index2, ch);

					if((++relPos % 4) == 0) {
						index++;
						sb.Remove(index + 2, 1);
						sb.Insert(index + 2, ' ');
					}

					tmp = tmp.Skip(1);
				}

				if((relPos % 4) == 0) {
					index--;
				}

				sb.Remove(index, 1);
				sb.Insert(index, '<');

				sb.Remove(index + 1, 1);
				sb.Insert(index + 1, ' ');

				sb.Append(Environment.NewLine);
				final.Append(sb.ToString());

				offset += outLen;
				size -= outLen;
			}

			Console.Write(final.ToString());
		}
	}
}
