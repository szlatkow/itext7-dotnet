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
using iText.Commons.Bouncycastle.Cert.Ocsp;
using iText.Commons.Utils;
using Org.BouncyCastle.Asn1.Ocsp;

namespace iText.Bouncycastlefips.Cert.Ocsp {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Ocsp.CertStatus"/>.
    /// </summary>
    public class CertStatusBCFips : ICertStatus {
        private static readonly CertStatusBCFips INSTANCE = new CertStatusBCFips(null);

        private static readonly CertStatusBCFips GOOD = new CertStatusBCFips(new CertStatus());

        private readonly CertStatus certificateStatus;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Ocsp.CertStatus"/>.
        /// </summary>
        /// <param name="certificateStatus">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Ocsp.CertStatus"/>
        /// to be wrapped
        /// </param>
        public CertStatusBCFips(CertStatus certificateStatus) {
            this.certificateStatus = certificateStatus;
        }

        /// <summary>Gets wrapper instance.</summary>
        /// <returns>
        /// 
        /// <see cref="CertStatusBCFips"/>
        /// instance.
        /// </returns>
        public static iText.Bouncycastlefips.Cert.Ocsp.CertStatusBCFips GetInstance() {
            return INSTANCE;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Ocsp.CertStatus"/>.
        /// </returns>
        public virtual CertStatus GetCertStatus() {
            return certificateStatus;
        }

        /// <summary><inheritDoc/></summary>
        public virtual ICertStatus GetGood() {
            return GOOD;
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
            iText.Bouncycastlefips.Cert.Ocsp.CertStatusBCFips that = (iText.Bouncycastlefips.Cert.Ocsp.CertStatusBCFips
                )o;
            return Object.Equals(certificateStatus, that.certificateStatus);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(certificateStatus);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return certificateStatus.ToString();
        }
    }
}
