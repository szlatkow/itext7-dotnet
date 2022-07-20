using Org.BouncyCastle.Asn1;
using iText.Commons.Bouncycastle.Asn1;

namespace iText.Bouncycastle.Asn1 {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.DerUtcTime"/>.
    /// </summary>
    public class ASN1UTCTimeBC : ASN1PrimitiveBC, IASN1UTCTime {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.DerUtcTime"/>.
        /// </summary>
        /// <param name="asn1UTCTime">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.DerUtcTime"/>
        /// to be wrapped
        /// </param>
        public ASN1UTCTimeBC(DerUtcTime asn1UTCTime)
            : base(asn1UTCTime) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.DerUtcTime"/>.
        /// </returns>
        public virtual DerUtcTime GetASN1UTCTime() {
            return (DerUtcTime)GetEncodable();
        }
    }
}
