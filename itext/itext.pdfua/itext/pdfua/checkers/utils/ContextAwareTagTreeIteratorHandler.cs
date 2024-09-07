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
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;

namespace iText.Pdfua.Checkers.Utils {
    /// <summary>Class that holds the validation context while iterating the tag tree structure.</summary>
    public abstract class ContextAwareTagTreeIteratorHandler : ITagTreeIteratorHandler {
        protected internal readonly PdfUAValidationContext context;

        /// <summary>
        /// Creates a new instance of the
        /// <see cref="ContextAwareTagTreeIteratorHandler"/>.
        /// </summary>
        /// <param name="context">The validation context.</param>
        protected internal ContextAwareTagTreeIteratorHandler(PdfUAValidationContext context) {
            this.context = context;
        }

        public abstract bool Accept(IStructureNode arg1);

        public abstract void ProcessElement(IStructureNode arg1);
    }
}
