/*
This file is part of the iText (R) project.
Copyright (c) 1998-2024 Apryse Group NV
Authors: Apryse Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System.Collections.Generic;
using iText.IO.Util;
using iText.Test;

namespace iText.IO.Font.Cmap {
    [NUnit.Framework.Category("UnitTest")]
    public class CMapCidToCodepointTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void AddCharAndLookupTest() {
            CMapCidToCodepoint cidToCode = new CMapCidToCodepoint();
            NUnit.Framework.Assert.AreEqual(new byte[0], cidToCode.Lookup(14));
            cidToCode.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 17 }), new CMapObject(CMapObject
                .NUMBER, 14));
            cidToCode.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 19 }), new CMapObject(CMapObject
                .STRING, "some text"));
            NUnit.Framework.Assert.AreEqual(new byte[] { 32, 17 }, cidToCode.Lookup(14));
            NUnit.Framework.Assert.AreEqual(new byte[0], cidToCode.Lookup(1));
        }

        [NUnit.Framework.Test]
        public virtual void GetReverseMapTest() {
            CMapCidToCodepoint cidToCode = new CMapCidToCodepoint();
            cidToCode.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 17 }), new CMapObject(CMapObject
                .NUMBER, 14));
            cidToCode.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 18 }), new CMapObject(CMapObject
                .NUMBER, 15));
            IntHashtable table = cidToCode.GetReversMap();
            NUnit.Framework.Assert.AreEqual(2, table.Size());
            NUnit.Framework.Assert.AreEqual(14, table.Get(8209));
            NUnit.Framework.Assert.AreEqual(15, table.Get(8210));
        }

        [NUnit.Framework.Test]
        public virtual void AddAndGetCodeSpaceRangeTest() {
            CMapCidToCodepoint cidToCode = new CMapCidToCodepoint();
            NUnit.Framework.Assert.IsTrue(cidToCode.GetCodeSpaceRanges().IsEmpty());
            cidToCode.AddCodeSpaceRange(new byte[] { 11 }, new byte[] { 12, 13 });
            cidToCode.AddCodeSpaceRange(null, new byte[] {  });
            IList<byte[]> codeSpaceRanges = cidToCode.GetCodeSpaceRanges();
            NUnit.Framework.Assert.AreEqual(4, codeSpaceRanges.Count);
            NUnit.Framework.Assert.AreEqual(new byte[] { 11 }, codeSpaceRanges[0]);
            NUnit.Framework.Assert.AreEqual(new byte[] { 12, 13 }, codeSpaceRanges[1]);
            NUnit.Framework.Assert.IsNull(codeSpaceRanges[2]);
            NUnit.Framework.Assert.AreEqual(new byte[] {  }, codeSpaceRanges[3]);
        }
    }
}
