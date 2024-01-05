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
    public class ActualTextIteratorTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void TestActualTestParts() {
            Glyph glyph = new Glyph(200, 200, '\u002d');
            GlyphLine glyphLine = new GlyphLine(JavaUtil.ArraysAsList(glyph));
            glyphLine.SetActualText(0, 1, "\u002d");
            ActualTextIterator actualTextIterator = new ActualTextIterator(glyphLine);
            GlyphLine.GlyphLinePart part = actualTextIterator.Next();
            // When actual text is the same as the result by text extraction, we should omit redundant actual text in the content stream
            NUnit.Framework.Assert.IsNull(part.actualText);
        }
    }
}
