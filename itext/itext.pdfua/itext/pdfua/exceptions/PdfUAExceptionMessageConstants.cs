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

namespace iText.Pdfua.Exceptions {
    /// <summary>Class that bundles all the error message templates as constants.</summary>
    public sealed class PdfUAExceptionMessageConstants {
        public const String ONE_OR_MORE_STANDARD_ROLE_REMAPPED = "One or more standard types are remapped.";

        public const String TAG_HASNT_BEEN_ADDED_BEFORE_CONTENT_ADDING = "Tag hasn't been added before adding content to the canvas.";

        public const String CONTENT_IS_NOT_REAL_CONTENT_AND_NOT_ARTIFACT = "Content is neither marked as Artifact nor tagged as real content.";

        public const String ARTIFACT_CANT_BE_INSIDE_REAL_CONTENT = "Content marked as artifact may only reside in Artifact content.";

        public const String REAL_CONTENT_CANT_BE_INSIDE_ARTIFACT = "Content marked as content may not reside in Artifact content.";

        public const String CONTENT_WITH_MCID_BUT_MCID_NOT_FOUND_IN_STRUCT_TREE_ROOT = "Content with MCID, but MCID wasn't found in StructTreeRoot.";

        public const String FONT_SHOULD_BE_EMBEDDED = "Following font(s) are not embedded: {0}";

        public const String METADATA_SHALL_BE_PRESENT_IN_THE_CATALOG_DICTIONARY = "Metadata shall be present in the catalog dictionary";

        public const String REAL_CONTENT_INSIDE_ARTIFACT_OR_VICE_VERSA = "Tagged content is present inside content marked as Artifact or vice versa.";

        public const String SUSPECTS_ENTRY_IN_MARK_INFO_DICTIONARY_SHALL_NOT_HAVE_A_VALUE_OF_TRUE = "Suspects entry in mark info dictionary shall not have a value of true.";

        public const String TAG_MAPPING_DOESNT_TERMINATE_WITH_STANDARD_TYPE = "\"{0}\" tag mapping does not terminate with a standard type.";

        public const String IMAGE_SHALL_HAVE_ALT = "Figure tags shall include an alternative representation or " +
             "replacement text. call com.itextpdf.kernel.pdf.tagutils.AccessibilityProperties#setActualText or com"
             + ".itextpdf.kernel.pdf.tagutils.AccessibilityProperties#setAlternateDescription to be PDF/UA compliant.";

        private PdfUAExceptionMessageConstants() {
        }
        // Empty constructor
    }
}
