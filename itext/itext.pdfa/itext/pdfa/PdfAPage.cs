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
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Pdfa.Checker;

namespace iText.Pdfa {
//\cond DO_NOT_DOCUMENT
    internal class PdfAPage : PdfPage {
        private readonly PdfAChecker checker;

//\cond DO_NOT_DOCUMENT
        internal PdfAPage(PdfDocument pdfDocument, PageSize pageSize, PdfAChecker checker)
            : base(pdfDocument, pageSize) {
            this.checker = checker;
        }
//\endcond

//\cond DO_NOT_DOCUMENT
        internal PdfAPage(PdfDictionary pdfObject, PdfAChecker checker)
            : base(pdfObject) {
            this.checker = checker;
        }
//\endcond

        public override void Flush(bool flushResourcesContentStreams) {
            // We check in advance whether this PdfAPage can be flushed and call the flush method only if it is.
            // This avoids processing actions that are invoked during flushing (for example, sending the END_PAGE event)
            // if the page is not actually flushed.
            if (flushResourcesContentStreams || GetDocument().IsClosing() || checker.IsPdfObjectReadyToFlush(this.GetPdfObject
                ())) {
                base.Flush(flushResourcesContentStreams);
            }
        }
    }
//\endcond
}
