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
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Openssl;
using iText.Commons.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace iText.Bouncycastle.Openssl {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Openssl.PEMParser"/>.
    /// </summary>
    public class PEMParserBC : IPemReader {
        private readonly PemReader parser;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.OpenSsl.OpenSslPemReader"/>.
        /// </summary>
        /// <param name="parser">
        /// 
        /// <see cref="Org.BouncyCastle.OpenSsl.OpenSslPemReader"/>
        /// to be wrapped
        /// </param>
        public PEMParserBC(PemReader parser) {
            this.parser = parser;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.OpenSsl.OpenSslPemReader"/>.
        /// </returns>
        public virtual PemReader GetParser() {
            return parser;
        }

        /// <summary><inheritDoc/></summary>
        public virtual Object ReadObject() {
            Object readObject;
            try {
                readObject = parser.ReadObject();
            }
            catch (PasswordException) {
                return new PrivateKeyBC(null);
            }
            if (readObject is X509Certificate) {
                return new X509CertificateBC((X509Certificate)readObject);
            }
            if (readObject is ICipherParameters) {
                return new PrivateKeyBC((ICipherParameters)readObject);
            }
            return readObject;
        }

        /// <summary>Indicates whether some other object is "equal to" this one.</summary>
        /// <remarks>Indicates whether some other object is "equal to" this one. Compares wrapped objects.</remarks>
        public override bool Equals(Object o) {
            if (this == o) {
                return true;
            }
            if (o == null || GetType() != o.GetType()) {
                return false;
            }
            iText.Bouncycastle.Openssl.PEMParserBC that = (iText.Bouncycastle.Openssl.PEMParserBC)o;
            return Object.Equals(parser, that.parser);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(parser);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return parser.ToString();
        }
    }
}
