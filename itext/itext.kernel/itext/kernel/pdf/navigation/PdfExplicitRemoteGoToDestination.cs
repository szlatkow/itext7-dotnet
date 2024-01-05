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

namespace iText.Kernel.Pdf.Navigation {
    /// <summary>
    /// This class shall be used for creation of destinations, associated Remote Go-To and Embedded Go-To actions only,
    /// i.e. the destination point is in another PDF.
    /// </summary>
    /// <remarks>
    /// This class shall be used for creation of destinations, associated Remote Go-To and Embedded Go-To actions only,
    /// i.e. the destination point is in another PDF.
    /// If you need to create a destination, associated with an object inside current PDF, you should use
    /// <see cref="PdfExplicitDestination"/>
    /// class instead.
    /// </remarks>
    public class PdfExplicitRemoteGoToDestination : PdfDestination {
        public PdfExplicitRemoteGoToDestination()
            : this(new PdfArray()) {
        }

        public PdfExplicitRemoteGoToDestination(PdfArray pdfObject)
            : base(pdfObject) {
        }

        public override PdfObject GetDestinationPage(IPdfNameTreeAccess names) {
            return ((PdfArray)GetPdfObject()).Get(0);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified by the factor zoom and positioned at the upper-left corner of the window.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <param name="left">the X coordinate of the left edge of the destination rectangle</param>
        /// <param name="top">the Y coordinate of the upper edge of the destination rectangle</param>
        /// <param name="zoom">zoom factor</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateXYZ(int pageNum, float left
            , float top, float zoom) {
            return Create(pageNum, PdfName.XYZ, left, float.NaN, float.NaN, top, zoom);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified just enough to fit the entire page within the window both horizontally and vertically.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFit(int pageNum) {
            return Create(pageNum, PdfName.Fit, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified just enough to fit the entire width of the page within the window.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <param name="top">the Y coordinate of the upper edge of the destination rectangle</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFitH(int pageNum, float top
            ) {
            return Create(pageNum, PdfName.FitH, float.NaN, float.NaN, float.NaN, top, float.NaN);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified just enough to fit the entire height of the page within the window.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <param name="left">the X coordinate of the left edge of the destination rectangle</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFitV(int pageNum, float left
            ) {
            return Create(pageNum, PdfName.FitV, left, float.NaN, float.NaN, float.NaN, float.NaN);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// .  The designated page will be displayed with its contents
        /// magnified just enough to fit the rectangle specified by the coordinates left, bottom, right, and top
        /// entirely within the window both horizontally and vertically.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <param name="left">the X coordinate of the left edge of the destination rectangle</param>
        /// <param name="bottom">the Y coordinate of the lower edge of the destination rectangle</param>
        /// <param name="right">the X coordinate of the right edge of the destination rectangle</param>
        /// <param name="top">the Y coordinate of the upper edge of the destination rectangle</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFitR(int pageNum, float left
            , float bottom, float right, float top) {
            return Create(pageNum, PdfName.FitR, left, bottom, right, top, float.NaN);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified just enough to fit its bounding box entirely within the window both horizontally and vertically.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFitB(int pageNum) {
            return Create(pageNum, PdfName.FitB, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified just enough to fit the entire width of its bounding box within the window.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <param name="top">the Y coordinate of the upper edge of the destination rectangle</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFitBH(int pageNum, float 
            top) {
            return Create(pageNum, PdfName.FitBH, float.NaN, float.NaN, float.NaN, top, float.NaN);
        }

        /// <summary>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>.
        /// </summary>
        /// <remarks>
        /// Creates
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// . The designated page will be displayed with its contents
        /// magnified just enough to fit the entire height of its bounding box within the window.
        /// </remarks>
        /// <param name="pageNum">the destination page</param>
        /// <param name="left">the X coordinate of the left edge of the destination rectangle</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination CreateFitBV(int pageNum, float 
            left) {
            return Create(pageNum, PdfName.FitBH, left, float.NaN, float.NaN, float.NaN, float.NaN);
        }

        /// <summary>
        /// Creates a
        /// <see cref="PdfExplicitRemoteGoToDestination"/>
        /// associated with an object in another PDF document.
        /// </summary>
        /// <param name="pageNum">the destination page</param>
        /// <param name="type">
        /// a
        /// <see cref="iText.Kernel.Pdf.PdfName"/>
        /// specifying one of the possible ways to define the area to be displayed.
        /// See ISO 32000-1, section 12.3.2.2 "Explicit Destinations", Table 151 – Destination syntax
        /// </param>
        /// <param name="left">the X coordinate of the left edge of the destination rectangle</param>
        /// <param name="bottom">the Y coordinate of the lower edge of the destination rectangle</param>
        /// <param name="right">the X coordinate of the right edge of the destination rectangle</param>
        /// <param name="top">the Y coordinate of the upper edge of the destination rectangle</param>
        /// <param name="zoom">zoom factor</param>
        /// <returns>
        /// newly created
        /// <see cref="PdfExplicitDestination"/>
        /// </returns>
        public static iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination Create(int pageNum, PdfName type
            , float left, float bottom, float right, float top, float zoom) {
            return new iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination().Add(--pageNum).Add(type).Add(left
                ).Add(bottom).Add(right).Add(top).Add(zoom);
        }

        protected internal override bool IsWrappedObjectMustBeIndirect() {
            return false;
        }

        private iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination Add(float value) {
            if (!float.IsNaN(value)) {
                ((PdfArray)GetPdfObject()).Add(new PdfNumber(value));
            }
            return this;
        }

        private iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination Add(int value) {
            ((PdfArray)GetPdfObject()).Add(new PdfNumber(value));
            return this;
        }

        private iText.Kernel.Pdf.Navigation.PdfExplicitRemoteGoToDestination Add(PdfName type) {
            ((PdfArray)GetPdfObject()).Add(type);
            return this;
        }
    }
}
