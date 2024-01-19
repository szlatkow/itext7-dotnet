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
using iText.Bouncycastle.Asn1;
using iText.Bouncycastle.Asn1.X509;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.Math;
using iText.Bouncycastle.Operator;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Asn1.X500;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Math;
using iText.Commons.Bouncycastle.Operator;
using iText.Commons.Utils;
using Org.BouncyCastle.X509;

namespace iText.Bouncycastle.X509 {
    /// <summary>
    /// Wrapper class for
    /// <see cref="X509V3CertificateGenerator"/>.
    /// </summary>
    public class X509V3CertificateGeneratorBC : IX509V3CertificateGenerator {
        private readonly X509V3CertificateGenerator certificateBuilder;

        /// <summary>
        /// Creates new wrapper instance for <see cref="X509V3CertificateGenerator"/>.
        /// </summary>
        /// <param name="certificateBuilder">
        /// <see cref="X509V3CertificateGenerator"/>
        /// to be wrapped
        /// </param>
        public X509V3CertificateGeneratorBC(X509V3CertificateGenerator certificateBuilder) {
            this.certificateBuilder = certificateBuilder;
        }

        /// <summary>
        /// Creates new wrapper instance for <see cref="X509V3CertificateGenerator"/>.
        /// </summary>
        /// <param name="signingCert">to get issuerDN to set</param>
        /// <param name="number">certificate serial number to set</param>
        /// <param name="startDate">to set</param>
        /// <param name="endDate">to set</param>
        /// <param name="subjectDn">to set</param>
        /// <param name="publicKey">to set</param>
        public X509V3CertificateGeneratorBC(IX509Certificate signingCert, IBigInteger number, DateTime startDate, 
            DateTime endDate, IX500Name subjectDn, IPublicKey publicKey) {
            certificateBuilder = new X509V3CertificateGenerator();
            certificateBuilder.SetIssuerDN(((X509NameBC) signingCert.GetIssuerDN()).GetX509Name());
            certificateBuilder.SetSerialNumber(((BigIntegerBC) number).GetBigInteger());
            certificateBuilder.SetNotBefore(startDate);
            certificateBuilder.SetNotAfter(endDate);
            certificateBuilder.SetSubjectDN(((X509NameBC) subjectDn).GetX509Name());
            certificateBuilder.SetPublicKey(((PublicKeyBC) publicKey).GetPublicKey());
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped <see cref="X509V3CertificateGenerator"/>.
        /// </returns>
        public virtual X509V3CertificateGenerator GetCertificateBuilder() {
            return certificateBuilder;
        }

        /// <summary><inheritDoc/></summary>
        public IX509V3CertificateGenerator AddExtension(IDerObjectIdentifier extensionOid, bool critical, IAsn1Encodable extensionValue) {
            certificateBuilder.AddExtension(((DerObjectIdentifierBC) extensionOid).GetDerObjectIdentifier(),
                critical, ((Asn1EncodableBC) extensionValue).GetEncodable());
            return this;
        }

        /// <summary><inheritDoc/></summary>
        public IX509Certificate Build(IContentSigner contentSigner) {
            return new X509CertificateBC(certificateBuilder.Generate(
                ((ContentSignerBC)contentSigner).GetContentSigner()));
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
            X509V3CertificateGeneratorBC that = (X509V3CertificateGeneratorBC
                )o;
            return Object.Equals(certificateBuilder, that.certificateBuilder);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(certificateBuilder);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return certificateBuilder.ToString();
        }
    }
}
