using Org.BouncyCastle.Asn1.Cmp;
using iText.Bouncycastlefips.Asn1;
using iText.Commons.Bouncycastle.Asn1.Cmp;

namespace iText.Bouncycastlefips.Asn1.Cmp {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Cmp.PkiFailureInfo"/>.
    /// </summary>
    public class PKIFailureInfoBCFips : ASN1PrimitiveBCFips, IPKIFailureInfo {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Cmp.PkiFailureInfo"/>.
        /// </summary>
        /// <param name="pkiFailureInfo">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Cmp.PkiFailureInfo"/>
        /// to be wrapped
        /// </param>
        public PKIFailureInfoBCFips(PkiFailureInfo pkiFailureInfo)
            : base(pkiFailureInfo) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Cmp.PkiFailureInfo"/>.
        /// </returns>
        public virtual PkiFailureInfo GetPkiFailureInfo() {
            return (PkiFailureInfo)GetEncodable();
        }

        /// <summary><inheritDoc/></summary>
        public virtual int IntValue() {
            return GetPkiFailureInfo().IntValue;
        }
    }
}
