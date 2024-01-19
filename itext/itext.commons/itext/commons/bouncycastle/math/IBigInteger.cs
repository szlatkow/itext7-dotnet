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
namespace iText.Commons.Bouncycastle.Math {
    /// <summary>
    /// This interface represents the wrapper for BigInteger that provides the ability
    /// to switch between bouncy-castle and bouncy-castle FIPS implementations.
    /// </summary>
    public interface IBigInteger {
        /// <summary>
        /// Gets integer value for the wrapped BigInteger.
        /// </summary>
        int GetIntValue();

        /// <summary>
        /// Calls toString with radix for the wrapped BigInteger.
        /// </summary>
        string ToString(int radix);

        /// <summary>
        /// Calls ValueOf method for the wrapped BigInteger.
        /// </summary>
        IBigInteger ValueOf(long value);

        /// <summary>
        /// Calls Remainder method for the wrapped BigInteger.
        /// </summary>
        IBigInteger Remainder(IBigInteger n);
    }
}
