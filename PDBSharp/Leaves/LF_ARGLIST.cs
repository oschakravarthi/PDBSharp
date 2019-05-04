#region License
/*
 * Copyright (C) 2018 Stefano Moioli <smxdev4@gmail.com>
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Smx.PDBSharp.Leaves
{
	[LeafReader(LeafType.LF_ARGLIST)]
	public class LF_ARGLIST : TypeDataReader
	{
		public UInt16 NumberOfArguments;
		public UInt32[] ArgumentTypeIndices;

		public LF_ARGLIST(Stream stream) : base(stream) {
			NumberOfArguments = ReadUInt16();
			ArgumentTypeIndices = Enumerable.Range(1, NumberOfArguments)
											.Select(_ => ReadUInt32())
											.ToArray();
		}
	}
}