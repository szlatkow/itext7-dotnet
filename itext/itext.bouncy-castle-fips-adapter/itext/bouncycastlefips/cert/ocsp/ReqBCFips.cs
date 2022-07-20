using System;
using Org.BouncyCastle.Cert.Ocsp;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using iText.Commons.Utils;

namespace iText.Bouncycastlefips.Cert.Ocsp {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Cert.Ocsp.Req"/>.
    /// </summary>
    public class ReqBCFips : IReq {
        private readonly Req req;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Cert.Ocsp.Req"/>.
        /// </summary>
        /// <param name="req">
        /// 
        /// <see cref="Org.BouncyCastle.Cert.Ocsp.Req"/>
        /// to be wrapped
        /// </param>
        public ReqBCFips(Req req) {
            this.req = req;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Cert.Ocsp.Req"/>.
        /// </returns>
        public virtual Req GetReq() {
            return req;
        }

        /// <summary><inheritDoc/></summary>
        public virtual ICertificateID GetCertID() {
            return new CertificateIDBCFips(req.GetCertID());
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
            iText.Bouncycastlefips.Cert.Ocsp.ReqBCFips reqBCFips = (iText.Bouncycastlefips.Cert.Ocsp.ReqBCFips)o;
            return Object.Equals(req, reqBCFips.req);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(req);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return req.ToString();
        }
    }
}
