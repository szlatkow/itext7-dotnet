/*
This file is part of the iText (R) project.
Copyright (c) 1998-2023 Apryse Group NV
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

namespace iText.Pdfa.Exceptions {
    /// <summary>Class that bundles all the error message templates as constants.</summary>
    public sealed class PdfaExceptionMessageConstant {
        public const String CANNOT_PARSE_CONTENT_STREAM = "Cannot parse content stream.";

        public const String INVALID_STREAM_FILTER_USAGE = "Filters that are not listed in ISO 32000-2:—, 7.4, Table 6 shall not be used.";

        public const String INVALID_INLINE_IMAGE_FILTER_USAGE = "Filters that are not listed in ISO 32000-2:—, 8.9.7, Table 92 or an array containing any such value shall not be used.";

        public const String DOCUMENT_INFO_DICTIONARY_SHALL_ONLY_CONTAIN_MOD_DATE = "If a document information dictionary is present, it shall only contain a ModDate entry.";

        public const String DOCUMENT_SHALL_NOT_CONTAIN_INFO_UNLESS_THERE_IS_PIECE_INFO = "The Info key shall not be present in the trailer dictionary of PDF/A-4 conforming files unless there exists a PieceInfo entry in the document catalog dictionary.";

        public const String NAME_DICTIONARY_SHALL_CONTAIN_EMBEDDED_FILES_KEY = "Conforming file shall contain an EmbeddedFiles key in the name dictionary of the document catalog dictionary.";

        public const String THE_FILE_HEADER_SHALL_CONTAIN_RIGHT_PDF_VERSION = "The file header shall begin at byte zero and shall consist of “%PDF-{0}.n”";

        public const String THE_CATALOG_VERSION_SHALL_CONTAIN_RIGHT_PDF_VERSION = "The catalog version key shall begin at byte zero and shall consist of “%PDF-{0}.n”";

        public const String CANNOT_FIND_PDFA_CHECKER_FOR_SPECIFIED_NAME = "Can't find an appropriate checker for a specified name.";

        public const String FILE_SPECIFICATION_DICTIONARY_SHALL_CONTAIN_AFRELATIONSHIP_KEY = "Each embedded file’s file specification dictionary shall contain an AFRelationship key.";

        public const String WIDGET_ANNOTATION_DICTIONARY_OR_FIELD_DICTIONARY_SHALL_NOT_INCLUDE_A_ENTRY = "Widget annotation dictionary or field dictionary shall not include a entry";

        public const String ANNOTATION_AA_DICTIONARY_SHALL_CONTAIN_ONLY_ALLOWED_KEYS = "If annotation dictionary (other than a Widget "
             + "annotation dictionary) includes an AA entry, its value (which is an additional-actions " + "dictionary) shall only contain keys from the following list: E, X, D, U, Fo, and B";

        public const String CATALOG_AA_DICTIONARY_SHALL_CONTAIN_ONLY_ALLOWED_KEYS = "If a document catalog dictionary includes an AA entry, its value (which is an additional-actions "
             + "dictionary) shall only contain keys from the following list: E, X, D, U, Fo, and B";

        public const String PAGE_AA_DICTIONARY_SHALL_CONTAIN_ONLY_ALLOWED_KEYS = "If page dictionary includes an AA entry, its value (which is an additional-actions "
             + "dictionary) shall only contain keys from the following list: E, X, D, U, Fo, and B";

        public const String THE_DOCUMENT_AND_THE_PAGE_DO_NOT_CONTAIN_A_PDFA_OUTPUTINTENT_BUT_PAGE_CONTAINS_TRANSPARENCY_AND_DOES_NOT_CONTAIN_BLENDING_COLOR_SPACE
             = "If the document does not contain a PDF/A output intent, then all pages that contain transparency shall"
             + " either have a page-level PDF/A output intent or the page dictionary shall include the" + " Group key, and the attribute dictionary that forms the value of that Group key shall include"
             + " a CS entry whose value shall be used as the default blending colour space.";

        public const String ONLY_STANDARD_BLEND_MODES_SHALL_BE_USED_FOR_THE_VALUE_OF_THE_BM_KEY_IN_A_GRAPHIC_STATE_AND_ANNOTATION_DICTIONARY
             = "Only blend modes that are specified in ISO 32000-2:2020 shall be used for the value of the BM key in a"
             + " graphic state dictionary or an annotation dictionary.";

        private PdfaExceptionMessageConstant() {
        }
    }
}
