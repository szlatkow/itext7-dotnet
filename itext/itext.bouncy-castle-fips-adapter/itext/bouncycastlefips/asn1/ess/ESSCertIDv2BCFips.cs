using Org.BouncyCastle.Asn1.Ess;
using iText.Bouncycastlefips.Asn1;
using iText.Bouncycastlefips.Asn1.X509;
using iText.Commons.Bouncycastle.Asn1.Ess;
using iText.Commons.Bouncycastle.Asn1.X509;

namespace iText.Bouncycastlefips.Asn1.Ess {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Ess.EssCertIDv2"/>.
    /// </summary>
    public class ESSCertIDv2BCFips : ASN1EncodableBCFips, IESSCertIDv2 {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Ess.EssCertIDv2"/>.
        /// </summary>
        /// <param name="essCertIDv2">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Ess.EssCertIDv2"/>
        /// to be wrapped
        /// </param>
        public ESSCertIDv2BCFips(EssCertIDv2 essCertIDv2)
            : base(essCertIDv2) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Ess.EssCertIDv2"/>.
        /// </returns>
        public virtual EssCertIDv2 GetEssCertIDv2() {
            return (EssCertIDv2)GetEncodable();
        }

        /// <summary><inheritDoc/></summary>
        public virtual IAlgorithmIdentifier GetHashAlgorithm() {
            return new AlgorithmIdentifierBCFips(GetEssCertIDv2().HashAlgorithm);
        }

        /// <summary><inheritDoc/></summary>
        public virtual byte[] GetCertHash() {
            return GetEssCertIDv2().GetCertHash();
        }
    }
}
