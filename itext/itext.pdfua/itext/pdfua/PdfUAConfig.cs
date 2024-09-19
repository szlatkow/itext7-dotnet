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
using iText.Kernel.Pdf;

namespace iText.Pdfua {
    /// <summary>Class that holds the configuration for the PDF/UA document.</summary>
    public class PdfUAConfig {
        private readonly PdfUAConformance conformance;

        private readonly String title;

        private readonly String language;

        /// <summary>Creates a new PdfUAConfig instance.</summary>
        /// <param name="conformance">the conformance of the PDF/UA document</param>
        /// <param name="title">the title of the PDF/UA document</param>
        /// <param name="language">the language of the PDF/UA document</param>
        public PdfUAConfig(PdfUAConformance conformance, String title, String language) {
            this.conformance = conformance;
            this.title = title;
            this.language = language;
        }

        /// <summary>Gets the UA conformance.</summary>
        /// <returns>
        /// The
        /// <see cref="iText.Kernel.Pdf.PdfUAConformance"/>.
        /// </returns>
        public virtual PdfUAConformance GetConformance() {
            return conformance;
        }

        /// <summary>Gets the title.</summary>
        /// <returns>The title.</returns>
        public virtual String GetTitle() {
            return title;
        }

        /// <summary>Gets the language.</summary>
        /// <returns>The language.</returns>
        public virtual String GetLanguage() {
            return language;
        }
    }
}
