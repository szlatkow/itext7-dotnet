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
using iText.Commons.Utils;
using iText.Test;

namespace iText.IO.Font.Otf {
    [NUnit.Framework.Category("UnitTest")]
    public class OpenTableLookupTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void IdxTest() {
            OpenTableLookup.GlyphIndexer glyphIndexer = new OpenTableLookup.GlyphIndexer();
            glyphIndexer.SetIdx(2);
            NUnit.Framework.Assert.AreEqual(2, glyphIndexer.GetIdx());
        }

        [NUnit.Framework.Test]
        public virtual void GlyphTest() {
            Glyph glyph = new Glyph(200, 200, 200);
            OpenTableLookup.GlyphIndexer glyphIndexer = new OpenTableLookup.GlyphIndexer();
            glyphIndexer.SetGlyph(glyph);
            NUnit.Framework.Assert.AreEqual(200, glyphIndexer.GetGlyph().GetWidth());
            NUnit.Framework.Assert.AreEqual(200, glyphIndexer.GetGlyph().GetCode());
            NUnit.Framework.Assert.AreEqual(200, glyphIndexer.GetGlyph().GetUnicode());
        }

        [NUnit.Framework.Test]
        public virtual void GlyphLineTest() {
            Glyph glyph = new Glyph(200, 200, 200);
            GlyphLine glyphLine = new GlyphLine(JavaCollectionsUtil.SingletonList(glyph));
            OpenTableLookup.GlyphIndexer glyphIndexer = new OpenTableLookup.GlyphIndexer();
            glyphIndexer.SetLine(glyphLine);
            NUnit.Framework.Assert.AreEqual(0, glyphIndexer.GetLine().GetIdx());
            NUnit.Framework.Assert.AreEqual(0, glyphIndexer.GetLine().GetStart());
            NUnit.Framework.Assert.AreEqual(1, glyphIndexer.GetLine().GetEnd());
        }
    }
}
