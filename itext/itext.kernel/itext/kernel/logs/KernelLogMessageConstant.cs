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

namespace iText.Kernel.Logs {
    /// <summary>Class that bundles all the error message templates as constants.</summary>
    public sealed class KernelLogMessageConstant {
        public const String CORRUPTED_OUTLINE_DICTIONARY_HAS_INFINITE_LOOP = "Document outline dictionary is corrupted: some outline (PDF object: \"{0}\") has wrong first/next link "
             + "entry. Next outlines in this dictionary will be unprocessed.";

        public const String DCTDECODE_FILTER_DECODING = "DCTDecode filter decoding into the bit map is not supported. The stream data would be left in JPEG "
             + "baseline format";

        public const String ERROR_WHILE_FINALIZING_AES_CIPHER = "Exception finalizing AES cipher.";

        public const String FEATURE_IS_NOT_SUPPORTED = "Exception was thrown: {0}. The feature {1} is probably not supported by your XML processor.";

        public const String FULL_COMPRESSION_APPEND_MODE_XREF_TABLE_INCONSISTENCY = "Full compression mode requested in append mode but the original document has cross-reference table, "
             + "not cross-reference stream. " + "Falling back to cross-reference table in appended document and switching full compression off";

        public const String FULL_COMPRESSION_APPEND_MODE_XREF_STREAM_INCONSISTENCY = "Full compression mode was requested to be switched off in append mode but the original document has "
             + "cross-reference stream, not cross-reference table. Falling back to cross-reference stream in " + "appended document and switching full compression on";

        public const String JPXDECODE_FILTER_DECODING = "JPXDecode filter decoding into the bit map is not supported. The stream data would be left in JPEG2000 "
             + "format";

        public const String MD5_IS_NOT_FIPS_COMPLIANT = "MD5 hash algorithm is not FIPS compliant. However we still use this algorithm "
             + "since it is required according to the PDF specification.";

        public const String UNABLE_TO_PARSE_COLOR_WITHIN_COLORSPACE = "Unable to parse color {0} within {1} color space";

        public const String CANNOT_MERGE_ENTRY = "Cannot merge entry {0}, entry with such key already exists.";

        /// <summary>
        /// Message warns about unexpected product name which was mentioned as involved into PDF
        /// processing.
        /// </summary>
        /// <remarks>
        /// Message warns about unexpected product name which was mentioned as involved into PDF
        /// processing. List of params:
        /// <list type="bullet">
        /// <item><description>0th is a name of unknown product;
        /// </description></item>
        /// </list>
        /// </remarks>
        public const String UNKNOWN_PRODUCT_INVOLVED = "Unknown product {0} was involved into PDF processing. It will be ignored";

        /// <summary>Message warns that some event was reported but wasn't confirmed.</summary>
        /// <remarks>
        /// Message warns that some event was reported but wasn't confirmed. Probably some processing has failed.
        /// List of params:
        /// <list type="bullet">
        /// <item><description>0th is a name of product for which event was reported;
        /// </description></item>
        /// <item><description>1st is an event type;
        /// </description></item>
        /// </list>
        /// </remarks>
        public const String UNCONFIRMED_EVENT = "Event for the product {0} with type {1} was reported but was not confirmed. Probably appropriate process "
             + "fail";

        public const String FLATTENING_IS_NOT_YET_SUPPORTED = "Flattening annotation type {0} is not yet supported, it will not be removed from the page";

        public const String FORMFIELD_ANNOTATION_WILL_NOT_BE_FLATTENED = "Form field annotation flattening is not "
             + "supported. Use the PdfAcroForm#flattenFields() method instead.";

        private KernelLogMessageConstant() {
        }
        //Private constructor will prevent the instantiation of this class directly
    }
}
