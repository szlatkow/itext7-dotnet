/*
This file is part of the iText (R) project.
Copyright (c) 1998-2025 Apryse Group NV
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
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;

namespace iText.Kernel.Pdf.Annot {
    public class PdfPrinterMarkAnnotation : PdfAnnotation {
        public PdfPrinterMarkAnnotation(Rectangle rect, PdfFormXObject appearanceStream)
            : base(rect) {
            SetNormalAppearance(appearanceStream.GetPdfObject());
            SetFlags(PdfAnnotation.PRINT | PdfAnnotation.READ_ONLY);
        }

        /// <summary>
        /// Instantiates a new
        /// <see cref="PdfPrinterMarkAnnotation"/>
        /// instance based on
        /// <see cref="iText.Kernel.Pdf.PdfDictionary"/>
        /// instance, that represents existing annotation object in the document.
        /// </summary>
        /// <param name="pdfObject">
        /// the
        /// <see cref="iText.Kernel.Pdf.PdfDictionary"/>
        /// representing annotation object
        /// </param>
        /// <seealso cref="PdfAnnotation.MakeAnnotation(iText.Kernel.Pdf.PdfObject)"/>
        protected internal PdfPrinterMarkAnnotation(PdfDictionary pdfObject)
            : base(pdfObject) {
        }

        public override PdfName GetSubtype() {
            return PdfName.PrinterMark;
        }

        public virtual PdfMarkupAnnotation SetArbitraryTypeName(PdfName arbitraryTypeName) {
            return (PdfMarkupAnnotation)Put(PdfName.MN, arbitraryTypeName);
        }

        public virtual PdfName GetArbitraryTypeName() {
            return GetPdfObject().GetAsName(PdfName.MN);
        }
    }
}
