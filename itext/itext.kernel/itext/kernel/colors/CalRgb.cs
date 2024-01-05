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
using iText.Kernel.Pdf.Colorspace;

namespace iText.Kernel.Colors {
    public class CalRgb : Color {
        public CalRgb(PdfCieBasedCs.CalRgb cs)
            : this(cs, new float[cs.GetNumberOfComponents()]) {
        }

        public CalRgb(PdfCieBasedCs.CalRgb cs, float[] value)
            : base(cs, value) {
        }

        public CalRgb(float[] whitePoint, float[] value)
            : base(new PdfCieBasedCs.CalRgb(whitePoint), value) {
        }

        public CalRgb(float[] whitePoint, float[] blackPoint, float[] gamma, float[] matrix, float[] value)
            : this(new PdfCieBasedCs.CalRgb(whitePoint, blackPoint, gamma, matrix), value) {
        }
    }
}
