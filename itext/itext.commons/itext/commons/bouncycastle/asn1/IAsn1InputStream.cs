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

namespace iText.Commons.Bouncycastle.Asn1 {
    /// <summary>
    /// This interface represents the wrapper for ASN1InputStream that provides the ability
    /// to switch between bouncy-castle and bouncy-castle FIPS implementations.
    /// </summary>
    public interface IAsn1InputStream : IDisposable {
        /// <summary>
        /// Calls actual
        /// <c>readObject</c>
        /// method for the wrapped ASN1InputStream object.
        /// </summary>
        /// <returns>
        /// 
        /// <see cref="IAsn1Object"/>
        /// wrapped ASN1Primitive object.
        /// </returns>
        IAsn1Object ReadObject();

        /// <summary>
        /// Delegates
        /// <c>close</c>
        /// method call to the wrapped stream.
        /// </summary>
        void Dispose();
    }
}
