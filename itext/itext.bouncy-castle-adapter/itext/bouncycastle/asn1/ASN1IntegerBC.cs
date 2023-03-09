using iText.Bouncycastle.Math;
using Org.BouncyCastle.Asn1;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Math;

namespace iText.Bouncycastle.Asn1 {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>.
    /// </summary>
    public class ASN1IntegerBC : ASN1PrimitiveBC, IASN1Integer {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>.
        /// </summary>
        /// <param name="i">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>
        /// to be wrapped
        /// </param>
        public ASN1IntegerBC(DerInteger i)
            : base(i) {
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>.
        /// </summary>
        /// <param name="i">
        /// int value to create
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>
        /// to be wrapped
        /// </param>
        public ASN1IntegerBC(int i)
            : base(new DerInteger(i)) {
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>.
        /// </summary>
        /// <param name="i">
        /// BigInteger value to create
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>
        /// to be wrapped
        /// </param>
        public ASN1IntegerBC(IBigInteger i)
            : base(new DerInteger(((BigIntegerBC) i).GetBigInteger())) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.DerInteger"/>.
        /// </returns>
        public virtual DerInteger GetASN1Integer() {
            return (DerInteger)GetPrimitive();
        }

        /// <summary><inheritDoc/></summary>
        public virtual IBigInteger GetValue() {
            return new BigIntegerBC(GetASN1Integer().Value);
        }

        public int GetIntValue()
        {
            return GetASN1Integer().Value.IntValue;
        }
    }
}
