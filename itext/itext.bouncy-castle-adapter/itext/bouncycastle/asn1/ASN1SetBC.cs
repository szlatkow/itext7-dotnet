using System.Collections;
using Org.BouncyCastle.Asn1;
using iText.Commons.Bouncycastle.Asn1;

namespace iText.Bouncycastle.Asn1 {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
    /// </summary>
    public class ASN1SetBC : ASN1PrimitiveBC, IASN1Set {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
        /// </summary>
        /// <param name="set">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>
        /// to be wrapped
        /// </param>
        public ASN1SetBC(Asn1Set set)
            : base(set) {
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
        /// </summary>
        /// <param name="taggedObject">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Asn1TaggedObject"/>
        /// to create
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>
        /// to be wrapped
        /// </param>
        /// <param name="b">
        /// boolean to create
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>
        /// to be wrapped
        /// </param>
        public ASN1SetBC(Asn1TaggedObject taggedObject, bool b)
            : base(Asn1Set.GetInstance(taggedObject, b)) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
        /// </returns>
        public virtual Asn1Set GetASN1Set() {
            return (Asn1Set)GetPrimitive();
        }

        /// <summary><inheritDoc/></summary>
        public virtual IEnumerator GetObjects() {
            return GetASN1Set().GetEnumerator();
        }

        /// <summary><inheritDoc/></summary>
        public virtual int Size() {
            return GetASN1Set().Count;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1Encodable GetObjectAt(int index) {
            return new ASN1EncodableBC(GetASN1Set()[index]);
        }

        /// <summary><inheritDoc/></summary>
        public virtual IASN1Encodable[] ToArray() {
            Asn1Encodable[] encodables = GetASN1Set().ToArray();
            ASN1EncodableBC[] encodablesBC = new ASN1EncodableBC[encodables.Length];
            for (int i = 0; i < encodables.Length; ++i) {
                encodablesBC[i] = new ASN1EncodableBC(encodables[i]);
            }
            return encodablesBC;
        }
    }
}
