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
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using iText.Commons;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Logs;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Layer;

namespace iText.Kernel.Pdf {
//\cond DO_NOT_DOCUMENT
    internal sealed class OcgPropertiesCopier {
        private static readonly ILogger LOGGER = ITextLogManager.GetLogger(typeof(iText.Kernel.Pdf.OcgPropertiesCopier
            ));

        private OcgPropertiesCopier() {
        }

        // Empty constructor
        /// <summary>Copy unique page OCGs stored inside annotations/xobjects/resources from source pages to destination pages.
        ///     </summary>
        /// <param name="sourceDocument">document from which OCGs should be copied</param>
        /// <param name="destinationDocument">document to which OCGs should be copied</param>
        /// <param name="sourceToDestPageMapping">page mapping, linking source pages to destination ones</param>
        public static void CopyOCGProperties(PdfDocument sourceDocument, PdfDocument destinationDocument, IDictionary
            <PdfPage, PdfPage> sourceToDestPageMapping) {
            try {
                // Configs are not copied
                PdfDictionary toOcProperties = destinationDocument.GetCatalog().GetPdfObject().GetAsDictionary(PdfName.OCProperties
                    );
                ICollection<PdfIndirectReference> ocgsToCopy = iText.Kernel.Pdf.OcgPropertiesCopier.GetAllUsedNonFlushedOCGs
                    (sourceToDestPageMapping, toOcProperties);
                if (ocgsToCopy.IsEmpty()) {
                    return;
                }
                // Reset ocProperties field in order to create it a new at the
                // method end using the new (merged) OCProperties dictionary
                toOcProperties = destinationDocument.GetCatalog().FillAndGetOcPropertiesDictionary();
                PdfDictionary fromOcProperties = sourceDocument.GetCatalog().GetPdfObject().GetAsDictionary(PdfName.OCProperties
                    );
                iText.Kernel.Pdf.OcgPropertiesCopier.CopyOCGs(ocgsToCopy, toOcProperties, destinationDocument);
                iText.Kernel.Pdf.OcgPropertiesCopier.CopyDDictionary(ocgsToCopy, fromOcProperties.GetAsDictionary(PdfName.
                    D), toOcProperties, destinationDocument);
            }
            catch (Exception e) {
                LOGGER.LogError(MessageFormatUtil.Format(iText.IO.Logs.IoLogMessageConstant.OCG_COPYING_ERROR, e.ToString(
                    )));
            }
        }

//\cond DO_NOT_DOCUMENT
        /// <summary>Get all OCGs from a given page annotations/xobjects/resources, including ones already stored in catalog
        ///     </summary>
        /// <param name="page">where to search for OCGs.</param>
        /// <returns>set of indirect references pointing to found OCGs.</returns>
        internal static ICollection<PdfIndirectReference> GetOCGsFromPage(PdfPage page) {
            //Using linked hash set for elements order consistency (e.g. in tests)
            ICollection<PdfIndirectReference> ocgs = new LinkedHashSet<PdfIndirectReference>();
            IList<PdfAnnotation> annotations = page.GetAnnotations();
            foreach (PdfAnnotation annotation in annotations) {
                //Pass null instead of catalog OCProperties value, to include ocg clashing with catalog
                GetUsedNonFlushedOCGsFromAnnotation(annotation, annotation, ocgs, null);
            }
            PdfDictionary resources = page.GetPdfObject().GetAsDictionary(PdfName.Resources);
            iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromResources(resources, resources, ocgs, null, 
                new HashSet<PdfObject>());
            return ocgs;
        }
//\endcond

        private static ICollection<PdfIndirectReference> GetAllUsedNonFlushedOCGs(IDictionary<PdfPage, PdfPage> page2page
            , PdfDictionary toOcProperties) {
            // NOTE: the PDF is considered to be valid and therefore the presence of OСG in OCProperties.OCGs is not checked
            ICollection<PdfIndirectReference> fromUsedOcgs = new LinkedHashSet<PdfIndirectReference>();
            // Visit the pages in parallel to find non-flush OSGs
            PdfPage[] fromPages = page2page.Keys.ToArray(new PdfPage[0]);
            PdfPage[] toPages = page2page.Values.ToArray(new PdfPage[0]);
            for (int i = 0; i < toPages.Length; i++) {
                PdfPage fromPage = fromPages[i];
                PdfPage toPage = toPages[i];
                // Copy OCGs from annotations
                IList<PdfAnnotation> toAnnotations = toPage.GetAnnotations();
                IList<PdfAnnotation> fromAnnotations = fromPage.GetAnnotations();
                for (int j = 0; j < toAnnotations.Count; j++) {
                    if (!toAnnotations[j].IsFlushed()) {
                        PdfAnnotation toAnnot = toAnnotations[j];
                        PdfAnnotation fromAnnot = fromAnnotations[j];
                        if (!toAnnot.GetPdfObject().IsFlushed()) {
                            GetUsedNonFlushedOCGsFromAnnotation(toAnnot, fromAnnot, fromUsedOcgs, toOcProperties);
                        }
                    }
                }
                PdfDictionary toResources = toPage.GetPdfObject().GetAsDictionary(PdfName.Resources);
                PdfDictionary fromResources = fromPage.GetPdfObject().GetAsDictionary(PdfName.Resources);
                iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromResources(toResources, fromResources, fromUsedOcgs
                    , toOcProperties, new HashSet<PdfObject>());
            }
            return fromUsedOcgs;
        }

        private static void GetUsedNonFlushedOCGsFromAnnotation(PdfAnnotation toAnnot, PdfAnnotation fromAnnot, ICollection
            <PdfIndirectReference> fromUsedOcgs, PdfDictionary toOcProperties) {
            iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromOcDict(toAnnot.GetPdfObject().GetAsDictionary
                (PdfName.OC), fromAnnot.GetPdfObject().GetAsDictionary(PdfName.OC), fromUsedOcgs, toOcProperties);
            iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromXObject(toAnnot.GetNormalAppearanceObject(), 
                fromAnnot.GetNormalAppearanceObject(), fromUsedOcgs, toOcProperties, new HashSet<PdfObject>());
            iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromXObject(toAnnot.GetRolloverAppearanceObject(
                ), fromAnnot.GetRolloverAppearanceObject(), fromUsedOcgs, toOcProperties, new HashSet<PdfObject>());
            iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromXObject(toAnnot.GetDownAppearanceObject(), fromAnnot
                .GetDownAppearanceObject(), fromUsedOcgs, toOcProperties, new HashSet<PdfObject>());
        }

        private static void GetUsedNonFlushedOCGsFromResources(PdfDictionary toResources, PdfDictionary fromResources
            , ICollection<PdfIndirectReference> fromUsedOcgs, PdfDictionary toOcProperties, ICollection<PdfObject>
             visitedObjects) {
            if (toResources != null && !toResources.IsFlushed()) {
                // Copy OCGs from properties
                PdfDictionary toProperties = toResources.GetAsDictionary(PdfName.Properties);
                PdfDictionary fromProperties = fromResources.GetAsDictionary(PdfName.Properties);
                if (toProperties != null && !toProperties.IsFlushed()) {
                    foreach (PdfName name in toProperties.KeySet()) {
                        PdfObject toCurrObj = toProperties.Get(name);
                        PdfObject fromCurrObj = fromProperties.Get(name);
                        iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromOcDict(toCurrObj, fromCurrObj, fromUsedOcgs, 
                            toOcProperties);
                    }
                }
                // Copy OCGs from xObject
                PdfDictionary toXObject = toResources.GetAsDictionary(PdfName.XObject);
                PdfDictionary fromXObject = fromResources.GetAsDictionary(PdfName.XObject);
                iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromXObject(toXObject, fromXObject, fromUsedOcgs
                    , toOcProperties, visitedObjects);
            }
        }

        private static void GetUsedNonFlushedOCGsFromXObject(PdfDictionary toXObject, PdfDictionary fromXObject, ICollection
            <PdfIndirectReference> fromUsedOcgs, PdfDictionary toOcProperties, ICollection<PdfObject> visitedObjects
            ) {
            //Resolving cycled properties, by memorizing the visited objects
            if (visitedObjects.Contains(fromXObject)) {
                return;
            }
            visitedObjects.Add(fromXObject);
            if (toXObject != null && !toXObject.IsFlushed()) {
                if (toXObject.IsStream() && !toXObject.IsFlushed()) {
                    PdfStream toStream = (PdfStream)toXObject;
                    PdfStream fromStream = (PdfStream)fromXObject;
                    iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromOcDict(toStream.GetAsDictionary(PdfName.OC), 
                        fromStream.GetAsDictionary(PdfName.OC), fromUsedOcgs, toOcProperties);
                    iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromResources(toStream.GetAsDictionary(PdfName.Resources
                        ), fromStream.GetAsDictionary(PdfName.Resources), fromUsedOcgs, toOcProperties, visitedObjects);
                }
                else {
                    foreach (PdfName name in toXObject.KeySet()) {
                        PdfObject toCurrObj = toXObject.Get(name);
                        PdfObject fromCurrObj = fromXObject.Get(name);
                        if (toCurrObj.IsStream() && !toCurrObj.IsFlushed()) {
                            PdfStream toStream = (PdfStream)toCurrObj;
                            PdfStream fromStream = (PdfStream)fromCurrObj;
                            iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromXObject(toStream, fromStream, fromUsedOcgs, 
                                toOcProperties, visitedObjects);
                        }
                    }
                }
            }
        }

        private static void GetUsedNonFlushedOCGsFromOcDict(PdfObject toObj, PdfObject fromObj, ICollection<PdfIndirectReference
            > fromUsedOcgs, PdfDictionary toOcProperties) {
            if (toObj != null && toObj.IsDictionary() && !toObj.IsFlushed()) {
                PdfDictionary toCurrDict = (PdfDictionary)toObj;
                PdfDictionary fromCurrDict = (PdfDictionary)fromObj;
                PdfName typeName = toCurrDict.GetAsName(PdfName.Type);
                if (PdfName.OCG.Equals(typeName) && !iText.Kernel.Pdf.OcgPropertiesCopier.OcgAlreadyInOCGs(toCurrDict.GetIndirectReference
                    (), toOcProperties)) {
                    fromUsedOcgs.Add(fromCurrDict.GetIndirectReference());
                }
                else {
                    if (PdfName.OCMD.Equals(typeName)) {
                        PdfArray toOcgs = null;
                        PdfArray fromOcgs = null;
                        if (toCurrDict.GetAsDictionary(PdfName.OCGs) != null) {
                            toOcgs = new PdfArray();
                            toOcgs.Add(toCurrDict.GetAsDictionary(PdfName.OCGs));
                            fromOcgs = new PdfArray();
                            fromOcgs.Add(fromCurrDict.GetAsDictionary(PdfName.OCGs));
                        }
                        else {
                            if (toCurrDict.GetAsArray(PdfName.OCGs) != null) {
                                toOcgs = toCurrDict.GetAsArray(PdfName.OCGs);
                                fromOcgs = fromCurrDict.GetAsArray(PdfName.OCGs);
                            }
                        }
                        if (toOcgs != null && !toOcgs.IsFlushed()) {
                            for (int i = 0; i < toOcgs.Size(); i++) {
                                iText.Kernel.Pdf.OcgPropertiesCopier.GetUsedNonFlushedOCGsFromOcDict(toOcgs.Get(i), fromOcgs.Get(i), fromUsedOcgs
                                    , toOcProperties);
                            }
                        }
                    }
                }
            }
        }

        private static void CopyOCGs(ICollection<PdfIndirectReference> fromOcgsToCopy, PdfDictionary toOcProperties
            , PdfDocument toDocument) {
            ICollection<String> layerNames = new HashSet<String>();
            if (toOcProperties.GetAsArray(PdfName.OCGs) != null) {
                PdfArray toOcgs = toOcProperties.GetAsArray(PdfName.OCGs);
                foreach (PdfObject toOcgObj in toOcgs) {
                    if (toOcgObj.IsDictionary()) {
                        layerNames.Add(((PdfDictionary)toOcgObj).GetAsString(PdfName.Name).ToUnicodeString());
                    }
                }
            }
            bool hasConflictingNames = false;
            foreach (PdfIndirectReference fromOcgRef in fromOcgsToCopy) {
                PdfDictionary toOcg = (PdfDictionary)fromOcgRef.GetRefersTo().CopyTo(toDocument, false);
                String currentLayerName = toOcg.GetAsString(PdfName.Name).ToUnicodeString();
                // Here we check on existed layer names only in destination document but not in source document.
                // That is why there is no something like layerNames.add(currentLayerName); after this if statement
                if (layerNames.Contains(currentLayerName)) {
                    hasConflictingNames = true;
                    int i = 0;
                    while (layerNames.Contains(currentLayerName + "_" + i)) {
                        i++;
                    }
                    currentLayerName += "_" + i;
                    toOcg.Put(PdfName.Name, new PdfString(currentLayerName, PdfEncodings.UNICODE_BIG));
                }
                if (toOcProperties.GetAsArray(PdfName.OCGs) == null) {
                    toOcProperties.Put(PdfName.OCGs, new PdfArray());
                }
                toOcProperties.GetAsArray(PdfName.OCGs).Add(toOcg);
            }
            if (hasConflictingNames) {
                LOGGER.LogWarning(iText.IO.Logs.IoLogMessageConstant.DOCUMENT_HAS_CONFLICTING_OCG_NAMES);
            }
        }

        private static bool OcgAlreadyInOCGs(PdfIndirectReference toOcgRef, PdfDictionary toOcProperties) {
            if (toOcProperties == null) {
                return false;
            }
            PdfArray toOcgs = toOcProperties.GetAsArray(PdfName.OCGs);
            if (toOcgs != null) {
                foreach (PdfObject toOcg in toOcgs) {
                    if (toOcgRef.Equals(toOcg.GetIndirectReference())) {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void CopyDDictionary(ICollection<PdfIndirectReference> fromOcgsToCopy, PdfDictionary fromDDict
            , PdfDictionary toOcProperties, PdfDocument toDocument) {
            if (toOcProperties.GetAsDictionary(PdfName.D) == null) {
                toOcProperties.Put(PdfName.D, new PdfDictionary());
            }
            PdfDictionary toDDict = toOcProperties.GetAsDictionary(PdfName.D);
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDStringField(PdfName.Name, fromDDict, toDDict);
            // Delete the Creator field because the D dictionary are changing
            toDDict.Remove(PdfName.Creator);
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDNameField(PdfName.BaseState, fromDDict, toDDict);
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDArrayField(PdfName.ON, fromOcgsToCopy, fromDDict, toDDict, toDocument
                );
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDArrayField(PdfName.OFF, fromOcgsToCopy, fromDDict, toDDict, toDocument
                );
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDNameField(PdfName.Intent, fromDDict, toDDict);
            // The AS field is not copied because it will be given when flushing the PdfOCProperties
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDArrayField(PdfName.Order, fromOcgsToCopy, fromDDict, toDDict, toDocument
                );
            // The ListMode field is copied, but it only affects the visual presentation of the layers
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDNameField(PdfName.ListMode, fromDDict, toDDict);
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDArrayField(PdfName.RBGroups, fromOcgsToCopy, fromDDict, toDDict, 
                toDocument);
            iText.Kernel.Pdf.OcgPropertiesCopier.CopyDArrayField(PdfName.Locked, fromOcgsToCopy, fromDDict, toDDict, toDocument
                );
        }

        private static void AttemptToAddObjectToArray(ICollection<PdfIndirectReference> fromOcgsToCopy, PdfObject 
            fromObj, PdfArray toArray, PdfDocument toDocument) {
            PdfIndirectReference fromObjRef = fromObj.GetIndirectReference();
            if (fromObjRef != null && fromOcgsToCopy.Contains(fromObjRef)) {
                toArray.Add(fromObj.CopyTo(toDocument, false));
            }
        }

        private static void CopyDNameField(PdfName fieldToCopy, PdfDictionary fromDict, PdfDictionary toDict) {
            PdfName fromName = fromDict.GetAsName(fieldToCopy);
            if (fromName == null || toDict.GetAsName(fieldToCopy) != null) {
                return;
            }
            if (PdfOCProperties.CheckDDictonaryFieldValue(fieldToCopy, fromName)) {
                toDict.Put(fieldToCopy, fromName);
            }
            else {
                ILogger logger = ITextLogManager.GetLogger(typeof(iText.Kernel.Pdf.OcgPropertiesCopier));
                String warnText = MessageFormatUtil.Format(KernelLogMessageConstant.INVALID_DDICTIONARY_FIELD_VALUE, fieldToCopy
                    , fromName);
                logger.LogWarning(warnText);
            }
        }

        private static void CopyDStringField(PdfName fieldToCopy, PdfDictionary fromDict, PdfDictionary toDict) {
            PdfString fromString = fromDict.GetAsString(fieldToCopy);
            if (fromString == null || toDict.GetAsString(fieldToCopy) != null) {
                return;
            }
            if (PdfOCProperties.CheckDDictonaryFieldValue(fieldToCopy, fromString)) {
                toDict.Put(fieldToCopy, fromString);
            }
            else {
                ILogger logger = ITextLogManager.GetLogger(typeof(iText.Kernel.Pdf.OcgPropertiesCopier));
                String warnText = MessageFormatUtil.Format(KernelLogMessageConstant.INVALID_DDICTIONARY_FIELD_VALUE, fieldToCopy
                    , fromString);
                logger.LogWarning(warnText);
            }
        }

        private static void CopyDArrayField(PdfName fieldToCopy, ICollection<PdfIndirectReference> fromOcgsToCopy, 
            PdfDictionary fromDict, PdfDictionary toDict, PdfDocument toDocument) {
            if (fromDict.GetAsArray(fieldToCopy) == null) {
                return;
            }
            PdfArray fromArray = fromDict.GetAsArray(fieldToCopy);
            if (toDict.GetAsArray(fieldToCopy) == null) {
                toDict.Put(fieldToCopy, new PdfArray());
            }
            PdfArray toArray = toDict.GetAsArray(fieldToCopy);
            ICollection<PdfIndirectReference> toOcgsToCopy = new HashSet<PdfIndirectReference>();
            foreach (PdfIndirectReference fromRef in fromOcgsToCopy) {
                toOcgsToCopy.Add(fromRef.GetRefersTo().CopyTo(toDocument, false).GetIndirectReference());
            }
            if (PdfName.Order.Equals(fieldToCopy)) {
                // Stage 1: delete all Order the entire branches from the output document in which the copied OCGs were
                IList<int> removeIndex = new List<int>();
                for (int i = 0; i < toArray.Size(); i++) {
                    PdfObject toOrderItem = toArray.Get(i);
                    if (iText.Kernel.Pdf.OcgPropertiesCopier.OrderBranchContainsSetElements(toOrderItem, toArray, i, toOcgsToCopy
                        , null, null)) {
                        removeIndex.Add(i);
                    }
                }
                for (int i = removeIndex.Count - 1; i > -1; i--) {
                    toArray.Remove(removeIndex[i]);
                }
                PdfArray toOcgs = toDocument.GetCatalog().GetPdfObject().GetAsDictionary(PdfName.OCProperties).GetAsArray(
                    PdfName.OCGs);
                // Stage 2: copy all the Order the entire branches in which the copied OСGs were
                for (int i = 0; i < fromArray.Size(); i++) {
                    PdfObject fromOrderItem = fromArray.Get(i);
                    if (iText.Kernel.Pdf.OcgPropertiesCopier.OrderBranchContainsSetElements(fromOrderItem, fromArray, i, fromOcgsToCopy
                        , toOcgs, toDocument)) {
                        toArray.Add(fromOrderItem.CopyTo(toDocument, false));
                    }
                }
            }
            else {
                // Stage 3: remove from Order OCGs not presented in the output document. When forming
                // the Order dictionary in the PdfOcProperties constructor, only those OCGs that are
                // in the OCProperties/OCGs array will be taken into account
                if (PdfName.RBGroups.Equals(fieldToCopy)) {
                    // Stage 1: delete all RBGroups from the output document in which the copied OCGs were
                    for (int i = toArray.Size() - 1; i > -1; i--) {
                        PdfArray toRbGroup = (PdfArray)toArray.Get(i);
                        foreach (PdfObject toRbGroupItemObj in toRbGroup) {
                            if (toOcgsToCopy.Contains(toRbGroupItemObj.GetIndirectReference())) {
                                toArray.Remove(i);
                                break;
                            }
                        }
                    }
                    // Stage 2: copy all the RBGroups in which the copied OCGs were
                    foreach (PdfObject fromRbGroupObj in fromArray) {
                        PdfArray fromRbGroup = (PdfArray)fromRbGroupObj;
                        foreach (PdfObject fromRbGroupItemObj in fromRbGroup) {
                            if (fromOcgsToCopy.Contains(fromRbGroupItemObj.GetIndirectReference())) {
                                toArray.Add(fromRbGroup.CopyTo(toDocument, false));
                                break;
                            }
                        }
                    }
                }
                else {
                    // Stage 3: remove from RBGroups OCGs not presented in the output
                    // document (is in the PdfOcProperties#fillDictionary method)
                    foreach (PdfObject fromObj in fromArray) {
                        iText.Kernel.Pdf.OcgPropertiesCopier.AttemptToAddObjectToArray(fromOcgsToCopy, fromObj, toArray, toDocument
                            );
                    }
                }
            }
            if (toArray.IsEmpty()) {
                toDict.Remove(fieldToCopy);
            }
        }

        private static bool OrderBranchContainsSetElements(PdfObject arrayObj, PdfArray array, int currentIndex, ICollection
            <PdfIndirectReference> ocgs, PdfArray toOcgs, PdfDocument toDocument) {
            if (arrayObj.IsDictionary()) {
                if (ocgs.Contains(arrayObj.GetIndirectReference())) {
                    return true;
                }
                else {
                    if (currentIndex < (array.Size() - 1) && array.Get(currentIndex + 1).IsArray()) {
                        PdfArray nextArray = array.GetAsArray(currentIndex + 1);
                        if (!nextArray.Get(0).IsString()) {
                            bool result = iText.Kernel.Pdf.OcgPropertiesCopier.OrderBranchContainsSetElements(nextArray, array, currentIndex
                                 + 1, ocgs, toOcgs, toDocument);
                            if (result && toOcgs != null && !ocgs.Contains(arrayObj.GetIndirectReference())) {
                                // Add the OCG to the OCGs array to register the OCG in document, since it is not used
                                // directly in the document, but is used as a parent for the order group. If it is not added
                                // to the OCGs array, then the OCG will be deleted at the 3rd stage of the /Order entry coping.
                                toOcgs.Add(arrayObj.CopyTo(toDocument, false));
                            }
                            return result;
                        }
                    }
                }
            }
            else {
                if (arrayObj.IsArray()) {
                    PdfArray arrayItem = (PdfArray)arrayObj;
                    for (int i = 0; i < arrayItem.Size(); i++) {
                        PdfObject obj = arrayItem.Get(i);
                        if (iText.Kernel.Pdf.OcgPropertiesCopier.OrderBranchContainsSetElements(obj, arrayItem, i, ocgs, toOcgs, toDocument
                            )) {
                            return true;
                        }
                    }
                    if (!arrayItem.IsEmpty() && !arrayItem.Get(0).IsString()) {
                        if (currentIndex > 0 && array.Get(currentIndex - 1).IsDictionary()) {
                            PdfDictionary previousDict = (PdfDictionary)array.Get(currentIndex - 1);
                            return ocgs.Contains(previousDict.GetIndirectReference());
                        }
                    }
                }
            }
            return false;
        }
    }
//\endcond
}
