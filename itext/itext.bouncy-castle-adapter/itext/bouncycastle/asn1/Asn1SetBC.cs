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
using System.Collections;
using Org.BouncyCastle.Asn1;
using iText.Commons.Bouncycastle.Asn1;

namespace iText.Bouncycastle.Asn1 {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
    /// </summary>
    public class Asn1SetBC : Asn1ObjectBC, IAsn1Set {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
        /// </summary>
        /// <param name="set">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>
        /// to be wrapped
        /// </param>
        public Asn1SetBC(Asn1Set set)
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
        public Asn1SetBC(Asn1TaggedObject taggedObject, bool b)
            : base(Asn1Set.GetInstance(taggedObject, b)) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Asn1Set"/>.
        /// </returns>
        public virtual Asn1Set GetAsn1Set() {
            return (Asn1Set)GetPrimitive();
        }

        /// <summary><inheritDoc/></summary>
        public virtual IEnumerator GetObjects() {
            return GetAsn1Set().GetEnumerator();
        }

        /// <summary><inheritDoc/></summary>
        public virtual int Size() {
            return GetAsn1Set().Count;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IAsn1Encodable GetObjectAt(int index) {
            return new Asn1EncodableBC(GetAsn1Set()[index]);
        }

        /// <summary><inheritDoc/></summary>
        public virtual IAsn1Encodable[] ToArray() {
            Asn1Encodable[] encodables = GetAsn1Set().ToArray();
            Asn1EncodableBC[] encodablesBC = new Asn1EncodableBC[encodables.Length];
            for (int i = 0; i < encodables.Length; ++i) {
                encodablesBC[i] = new Asn1EncodableBC(encodables[i]);
            }
            return encodablesBC;
        }
    }
}
