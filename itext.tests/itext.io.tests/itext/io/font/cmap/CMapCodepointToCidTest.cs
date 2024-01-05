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
using iText.Test;

namespace iText.IO.Font.Cmap {
    [NUnit.Framework.Category("UnitTest")]
    public class CMapCodepointToCidTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void ReverseConstructorTest() {
            CMapCidToCodepoint cidToCode = new CMapCidToCodepoint();
            cidToCode.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 17 }), new CMapObject(CMapObject
                .NUMBER, 14));
            cidToCode.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 18 }), new CMapObject(CMapObject
                .NUMBER, 15));
            CMapCodepointToCid codeToCid = new CMapCodepointToCid(cidToCode);
            NUnit.Framework.Assert.AreEqual(14, codeToCid.Lookup(8209));
            NUnit.Framework.Assert.AreEqual(15, codeToCid.Lookup(8210));
        }

        [NUnit.Framework.Test]
        public virtual void AddCharAndLookupTest() {
            CMapCodepointToCid codeToCid = new CMapCodepointToCid();
            NUnit.Framework.Assert.AreEqual(0, codeToCid.Lookup(8209));
            codeToCid.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 17 }), new CMapObject(CMapObject
                .NUMBER, 14));
            codeToCid.AddChar(iText.Commons.Utils.JavaUtil.GetStringForBytes(new byte[] { 32, 19 }), new CMapObject(CMapObject
                .STRING, "some text"));
            NUnit.Framework.Assert.AreEqual(14, codeToCid.Lookup(8209));
            NUnit.Framework.Assert.AreEqual(0, codeToCid.Lookup(1));
        }
    }
}
