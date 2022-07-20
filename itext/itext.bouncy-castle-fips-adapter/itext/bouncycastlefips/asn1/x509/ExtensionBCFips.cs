using Org.BouncyCastle.Asn1.X509;
using iText.Bouncycastlefips.Asn1;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Asn1.X509;

namespace iText.Bouncycastlefips.Asn1.X509 {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.X509.X509Extensions"/>.
    /// </summary>
    public class ExtensionBCFips : ASN1EncodableBCFips, IExtension {
        private static readonly iText.Bouncycastlefips.Asn1.X509.ExtensionBCFips INSTANCE = new iText.Bouncycastlefips.Asn1.X509.ExtensionBCFips
            (null);

        private static readonly ASN1ObjectIdentifierBCFips C_RL_DISTRIBUTION_POINTS = new ASN1ObjectIdentifierBCFips
            (X509Extensions.CrlDistributionPoints);

        private static readonly ASN1ObjectIdentifierBCFips AUTHORITY_INFO_ACCESS = new ASN1ObjectIdentifierBCFips(
            X509Extensions.AuthorityInfoAccess);

        private static readonly ASN1ObjectIdentifierBCFips BASIC_CONSTRAINTS = new ASN1ObjectIdentifierBCFips(X509Extensions
            .basicConstraints);

        private static readonly ASN1ObjectIdentifierBCFips KEY_USAGE = new ASN1ObjectIdentifierBCFips(X509Extensions
            .keyUsage);

        private static readonly ASN1ObjectIdentifierBCFips EXTENDED_KEY_USAGE = new ASN1ObjectIdentifierBCFips(X509Extensions
            .extendedKeyUsage);

        private static readonly ASN1ObjectIdentifierBCFips AUTHORITY_KEY_IDENTIFIER = new ASN1ObjectIdentifierBCFips
            (X509Extensions.authorityKeyIdentifier);

        private static readonly ASN1ObjectIdentifierBCFips SUBJECT_KEY_IDENTIFIER = new ASN1ObjectIdentifierBCFips
            (X509Extensions.subjectKeyIdentifier);

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.X509.X509Extensions"/>.
        /// </summary>
        /// <param name="extension">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.X509.X509Extensions"/>
        /// to be wrapped
        /// </param>
        public ExtensionBCFips(X509Extensions extension)
            : base(extension) {
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.X509.X509Extensions"/>.
        /// </summary>
        /// <param name="objectIdentifier">ASN1ObjectIdentifier wrapper</param>
        /// <param name="critical">boolean</param>
        /// <param name="octetString">ASN1OctetString wrapper</param>
        public ExtensionBCFips(IASN1ObjectIdentifier objectIdentifier, bool critical, IASN1OctetString octetString
            )
            : base(new X509Extensions(((ASN1ObjectIdentifierBCFips)objectIdentifier).GetASN1ObjectIdentifier(), critical
                , ((ASN1OctetStringBCFips)octetString).GetOctetString())) {
        }

        /// <summary>Gets wrapper instance.</summary>
        /// <returns>
        /// 
        /// <see cref="ExtensionBCFips"/>
        /// instance.
        /// </returns>
        public static iText.Bouncycastlefips.Asn1.X509.ExtensionBCFips GetInstance() {
            return INSTANCE;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.X509.X509Extensions"/>.
        /// </returns>
        public virtual X509Extensions GetExtension() {
            return (X509Extensions)GetEncodable();
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetCRlDistributionPoints() {
            return C_RL_DISTRIBUTION_POINTS;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetAuthorityInfoAccess() {
            return AUTHORITY_INFO_ACCESS;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetBasicConstraints() {
            return BASIC_CONSTRAINTS;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetKeyUsage() {
            return KEY_USAGE;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetExtendedKeyUsage() {
            return EXTENDED_KEY_USAGE;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetAuthorityKeyIdentifier() {
            return AUTHORITY_KEY_IDENTIFIER;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1ObjectIdentifier GetSubjectKeyIdentifier() {
            return SUBJECT_KEY_IDENTIFIER;
        }
    }
}
