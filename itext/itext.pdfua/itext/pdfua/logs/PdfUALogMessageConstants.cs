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
using System;

namespace iText.Pdfua.Logs {
    /// <summary>Class containing the log message constants.</summary>
    public sealed class PdfUALogMessageConstants {
        public const String PAGE_FLUSHING_DISABLED = "Page flushing is disabled in PDF/UA mode to allow UA checks "
             + "to be applied. Page will only be flushed on closing.";

        public const String PDF_TO_PDF_UA_CONVERSION_IS_NOT_SUPPORTED = "PDF to PDF/UA conversion is not supported.";

        private PdfUALogMessageConstants() {
        }
        // empty constructor
    }
}
