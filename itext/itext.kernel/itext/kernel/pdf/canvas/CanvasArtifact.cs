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
using iText.Kernel.Pdf;

namespace iText.Kernel.Pdf.Canvas {
    /// <summary>
    /// A subclass of
    /// <see cref="CanvasTag"/>
    /// for Artifacts.
    /// </summary>
    /// <remarks>
    /// A subclass of
    /// <see cref="CanvasTag"/>
    /// for Artifacts.
    /// In Tagged PDF, an object can be marked as an Artifact in order to signify
    /// that it is more part of the document structure than of the document content.
    /// Examples are page headers, layout features, etc. Screen readers can choose to
    /// ignore Artifacts.
    /// </remarks>
    public class CanvasArtifact : CanvasTag {
        /// <summary>
        /// Creates a CanvasArtifact object, which is a
        /// <see cref="CanvasTag"/>
        /// with a role
        /// of
        /// <see cref="iText.Kernel.Pdf.PdfName.Artifact">Artifact</see>.
        /// </summary>
        public CanvasArtifact()
            : base(PdfName.Artifact) {
        }
    }
}
