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
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using iText.Commons;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Colorspace;
using iText.Pdfa.Exceptions;
using iText.Pdfa.Logs;

namespace iText.Pdfa.Checker {
    /// <summary>
    /// PdfA4Checker defines the requirements of the PDF/A-4 standard and contains a
    /// number of methods that override the implementations of its superclass
    /// <see cref="PdfA3Checker"/>.
    /// </summary>
    /// <remarks>
    /// PdfA4Checker defines the requirements of the PDF/A-4 standard and contains a
    /// number of methods that override the implementations of its superclass
    /// <see cref="PdfA3Checker"/>.
    /// <para />
    /// The specification implemented by this class is ISO 19005-4
    /// </remarks>
    public class PdfA4Checker : PdfA3Checker {
        private static readonly ICollection<PdfName> forbiddenAnnotations4 = JavaCollectionsUtil.UnmodifiableSet(new 
            HashSet<PdfName>(JavaUtil.ArraysAsList(PdfName._3D, PdfName.RichMedia, PdfName.FileAttachment, PdfName
            .Sound, PdfName.Screen, PdfName.Movie)));

        private static readonly ICollection<PdfName> forbiddenAnnotations4E = JavaCollectionsUtil.UnmodifiableSet(
            new HashSet<PdfName>(JavaUtil.ArraysAsList(PdfName.FileAttachment, PdfName.Sound, PdfName.Screen, PdfName
            .Movie)));

        private static readonly ICollection<PdfName> forbiddenAnnotations4F = JavaCollectionsUtil.UnmodifiableSet(
            new HashSet<PdfName>(JavaUtil.ArraysAsList(PdfName._3D, PdfName.RichMedia, PdfName.Sound, PdfName.Screen
            , PdfName.Movie)));

        private static readonly ICollection<PdfName> apLessAnnotations = JavaCollectionsUtil.UnmodifiableSet(new HashSet
            <PdfName>(JavaUtil.ArraysAsList(PdfName.Popup, PdfName.Link, PdfName.Projection)));

        private static readonly ICollection<PdfName> allowedBlendModes4 = JavaCollectionsUtil.UnmodifiableSet(new 
            HashSet<PdfName>(JavaUtil.ArraysAsList(PdfName.Normal, PdfName.Multiply, PdfName.Screen, PdfName.Overlay
            , PdfName.Darken, PdfName.Lighten, PdfName.ColorDodge, PdfName.ColorBurn, PdfName.HardLight, PdfName.SoftLight
            , PdfName.Difference, PdfName.Exclusion, PdfName.Hue, PdfName.Saturation, PdfName.Color, PdfName.Luminosity
            )));

        private const String TRANSPARENCY_ERROR_MESSAGE = PdfaExceptionMessageConstant.THE_DOCUMENT_AND_THE_PAGE_DO_NOT_CONTAIN_A_PDFA_OUTPUTINTENT_BUT_PAGE_CONTAINS_TRANSPARENCY_AND_DOES_NOT_CONTAIN_BLENDING_COLOR_SPACE;

        private static readonly ILogger LOGGER = ITextLogManager.GetLogger(typeof(PdfAChecker));

        /// <summary>Creates a PdfA4Checker with the required conformance level</summary>
        /// <param name="conformanceLevel">the required conformance level</param>
        public PdfA4Checker(PdfAConformanceLevel conformanceLevel)
            : base(conformanceLevel) {
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckTrailer(PdfDictionary trailer) {
            base.CheckTrailer(trailer);
            if (trailer.Get(PdfName.Info) != null) {
                PdfDictionary info = trailer.GetAsDictionary(PdfName.Info);
                if (info.Size() != 1 || info.Get(PdfName.ModDate) == null) {
                    throw new PdfAConformanceException(PdfaExceptionMessageConstant.DOCUMENT_INFO_DICTIONARY_SHALL_ONLY_CONTAIN_MOD_DATE
                        );
                }
            }
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckCatalog(PdfCatalog catalog) {
            if ('2' != catalog.GetDocument().GetPdfVersion().ToString()[4]) {
                throw new PdfAConformanceException(MessageFormatUtil.Format(PdfaExceptionMessageConstant.THE_FILE_HEADER_SHALL_CONTAIN_RIGHT_PDF_VERSION
                    , "2"));
            }
            PdfDictionary trailer = catalog.GetDocument().GetTrailer();
            if (trailer.Get(PdfName.Info) != null) {
                if (catalog.GetPdfObject().Get(PdfName.PieceInfo) == null) {
                    throw new PdfAConformanceException(PdfaExceptionMessageConstant.DOCUMENT_SHALL_NOT_CONTAIN_INFO_UNLESS_THERE_IS_PIECE_INFO
                        );
                }
            }
            if ("F".Equals(conformanceLevel.GetConformance())) {
                if (!catalog.NameTreeContainsKey(PdfName.EmbeddedFiles)) {
                    throw new PdfAConformanceException(PdfaExceptionMessageConstant.NAME_DICTIONARY_SHALL_CONTAIN_EMBEDDED_FILES_KEY
                        );
                }
            }
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckCatalogValidEntries(PdfDictionary catalogDict) {
            base.CheckCatalogValidEntries(catalogDict);
            PdfString version = catalogDict.GetAsString(PdfName.Version);
            if (version != null && (version.ToString()[0] != '2' || version.ToString()[1] != '.' || !char.IsDigit(version
                .ToString()[2]))) {
                throw new PdfAConformanceException(MessageFormatUtil.Format(PdfaExceptionMessageConstant.THE_CATALOG_VERSION_SHALL_CONTAIN_RIGHT_PDF_VERSION
                    , "2"));
            }
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckFileSpec(PdfDictionary fileSpec) {
            if (fileSpec.GetAsName(PdfName.AFRelationship) == null) {
                throw new PdfAConformanceException(PdfaExceptionMessageConstant.FILE_SPECIFICATION_DICTIONARY_SHALL_CONTAIN_AFRELATIONSHIP_KEY
                    );
            }
            if (!fileSpec.ContainsKey(PdfName.F) || !fileSpec.ContainsKey(PdfName.UF)) {
                throw new PdfAConformanceException(PdfAConformanceException.FILE_SPECIFICATION_DICTIONARY_SHALL_CONTAIN_F_KEY_AND_UF_KEY
                    );
            }
            if (!fileSpec.ContainsKey(PdfName.Desc)) {
                LOGGER.LogWarning(PdfAConformanceLogMessageConstant.FILE_SPECIFICATION_DICTIONARY_SHOULD_CONTAIN_DESC_KEY);
            }
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckPageTransparency(PdfDictionary pageDict, PdfDictionary pageResources
            ) {
            // Get page pdf/a output intent
            PdfDictionary pdfAPageOutputIntent = null;
            PdfArray outputIntents = pageDict.GetAsArray(PdfName.OutputIntents);
            if (outputIntents != null) {
                pdfAPageOutputIntent = GetPdfAOutputIntent(outputIntents);
            }
            if (pdfAOutputIntentColorSpace == null && pdfAPageOutputIntent == null && transparencyObjects.Count > 0 &&
                 (pageDict.GetAsDictionary(PdfName.Group) == null || pageDict.GetAsDictionary(PdfName.Group).Get(PdfName
                .CS) == null)) {
                CheckContentsForTransparency(pageDict);
                CheckAnnotationsForTransparency(pageDict.GetAsArray(PdfName.Annots));
                CheckResourcesForTransparency(pageResources, new HashSet<PdfObject>());
            }
        }

        //There are no limits for numbers in pdf-a/4
        /// <summary><inheritDoc/></summary>
        protected internal override void CheckPdfNumber(PdfNumber number) {
        }

        //There is no limit for canvas stack in pdf-a/4
        /// <summary><inheritDoc/></summary>
        public override void CheckCanvasStack(char stackOperation) {
        }

        //There is no limit for String length in pdf-a/4
        /// <summary><inheritDoc/></summary>
        protected internal override int GetMaxStringLength() {
            return int.MaxValue;
        }

        //There is no limit for DeviceN components count in pdf-a/4
        /// <summary><inheritDoc/></summary>
        protected internal override void CheckNumberOfDeviceNComponents(PdfSpecialCs.DeviceN deviceN) {
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckAnnotation(PdfDictionary annotDic) {
            base.CheckAnnotation(annotDic);
            // Extra check for blending mode
            PdfName blendMode = annotDic.GetAsName(PdfName.BM);
            if (blendMode != null && !allowedBlendModes4.Contains(blendMode)) {
                throw new PdfAConformanceException(PdfaExceptionMessageConstant.ONLY_STANDARD_BLEND_MODES_SHALL_BE_USED_FOR_THE_VALUE_OF_THE_BM_KEY_IN_A_GRAPHIC_STATE_AND_ANNOTATION_DICTIONARY
                    );
            }
            // And then treat the annotation as an object with transparency
            if (blendMode != null && !PdfName.Normal.Equals(blendMode)) {
                transparencyObjects.Add(annotDic);
            }
        }

        /// <summary><inheritDoc/></summary>
        protected internal override ICollection<PdfName> GetForbiddenAnnotations() {
            if ("E".Equals(conformanceLevel.GetConformance())) {
                return forbiddenAnnotations4E;
            }
            else {
                if ("F".Equals(conformanceLevel.GetConformance())) {
                    return forbiddenAnnotations4F;
                }
            }
            return forbiddenAnnotations4;
        }

        /// <summary><inheritDoc/></summary>
        protected internal override ICollection<PdfName> GetAppearanceLessAnnotations() {
            return apLessAnnotations;
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckAnnotationAgainstActions(PdfDictionary annotDic) {
            if (PdfName.Widget.Equals(annotDic.GetAsName(PdfName.Subtype)) && annotDic.ContainsKey(PdfName.A)) {
                throw new PdfAConformanceException(PdfaExceptionMessageConstant.WIDGET_ANNOTATION_DICTIONARY_OR_FIELD_DICTIONARY_SHALL_NOT_INCLUDE_A_ENTRY
                    );
            }
            if (!PdfName.Widget.Equals(annotDic.GetAsName(PdfName.Subtype)) && annotDic.ContainsKey(PdfName.AA)) {
                throw new PdfAConformanceException(PdfAConformanceException.AN_ANNOTATION_DICTIONARY_SHALL_NOT_CONTAIN_AA_KEY
                    );
            }
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckContentConfigurationDictAgainstAsKey(PdfDictionary config) {
        }

        // Do nothing because in PDF/A-4 AS key may appear in any optional content configuration dictionary.
        /// <summary><inheritDoc/></summary>
        protected internal override String GetTransparencyErrorMessage() {
            return TRANSPARENCY_ERROR_MESSAGE;
        }

        /// <summary><inheritDoc/></summary>
        protected internal override void CheckBlendMode(PdfName blendMode) {
            if (!allowedBlendModes4.Contains(blendMode)) {
                throw new PdfAConformanceException(PdfAConformanceException.ONLY_STANDARD_BLEND_MODES_SHALL_BE_USED_FOR_THE_VALUE_OF_THE_BM_KEY_IN_AN_EXTENDED_GRAPHIC_STATE_DICTIONARY
                    );
            }
        }
    }
}
