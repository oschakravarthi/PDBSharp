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
using System.Text;

namespace Smx.PDBSharp.Leaves
{
	class LF_INDEX : ILeaf
	{
		public readonly ILeafContainer Referenced;

		public LF_INDEX(PDBFile pdb, Stream stream)  {
			TypeDataReader r = new TypeDataReader(pdb, stream);
			Referenced = r.ReadIndexedTypeLazy();
		}

		public void Write(PDBFile pdb, Stream stream) {
			TypeDataWriter w = new TypeDataWriter(pdb, stream, LeafType.LF_INDEX);
			w.WriteIndexedType(Referenced);
			w.WriteLeafHeader();
		}
	}
}