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
using iText.IO.Source;

namespace iText.IO.Font.Otf {
    /// <summary>Parses an OpenTypeFont file and reads the Glyph Substitution Table.</summary>
    /// <remarks>
    /// Parses an OpenTypeFont file and reads the Glyph Substitution Table. This table governs how two or more Glyphs should be merged
    /// to a single Glyph. This is especially useful for Asian languages like Bangla, Hindi, etc.
    /// <para />
    /// This has been written according to the OPenTypeFont specifications. This may be found <a href="http://www.microsoft.com/typography/otspec/gsub.htm">here</a>.
    /// </remarks>
    public class GlyphSubstitutionTableReader : OpenTypeFontTableReader {
        public GlyphSubstitutionTableReader(RandomAccessFileOrArray rf, int gsubTableLocation, OpenTypeGdefTableReader
             gdef, IDictionary<int, Glyph> indexGlyphMap, int unitsPerEm)
            : base(rf, gsubTableLocation, gdef, indexGlyphMap, unitsPerEm) {
            StartReadingTable();
        }

        protected internal override OpenTableLookup ReadLookupTable(int lookupType, int lookupFlag, int[] subTableLocations
            ) {
            if (lookupType == 7) {
                for (int k = 0; k < subTableLocations.Length; ++k) {
                    int location = subTableLocations[k];
                    rf.Seek(location);
                    rf.ReadUnsignedShort();
                    lookupType = rf.ReadUnsignedShort();
                    location += rf.ReadInt();
                    subTableLocations[k] = location;
                }
            }
            switch (lookupType) {
                case 1: {
                    return new GsubLookupType1(this, lookupFlag, subTableLocations);
                }

                case 2: {
                    return new GsubLookupType2(this, lookupFlag, subTableLocations);
                }

                case 3: {
                    return new GsubLookupType3(this, lookupFlag, subTableLocations);
                }

                case 4: {
                    return new GsubLookupType4(this, lookupFlag, subTableLocations);
                }

                case 5: {
                    return new GsubLookupType5(this, lookupFlag, subTableLocations);
                }

                case 6: {
                    return new GsubLookupType6(this, lookupFlag, subTableLocations);
                }

                default: {
                    return null;
                }
            }
        }
    }
}
