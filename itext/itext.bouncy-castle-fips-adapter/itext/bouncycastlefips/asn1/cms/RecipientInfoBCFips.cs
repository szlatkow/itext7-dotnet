/*
This file is part of the iText (R) project.
Copyright (c) 1998-2022 iText Group NV
Authors: iText Software.

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
using Org.BouncyCastle.Asn1.Cms;
using iText.Bouncycastlefips.Asn1;
using iText.Commons.Bouncycastle.Asn1.Cms;

namespace iText.Bouncycastlefips.Asn1.Cms {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Cms.RecipientInfo"/>.
    /// </summary>
    public class RecipientInfoBCFips : ASN1EncodableBCFips, IRecipientInfo {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Cms.RecipientInfo"/>.
        /// </summary>
        /// <param name="recipientInfo">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Cms.RecipientInfo"/>
        /// to be wrapped
        /// </param>
        public RecipientInfoBCFips(RecipientInfo recipientInfo)
            : base(recipientInfo) {
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Cms.RecipientInfo"/>.
        /// </summary>
        /// <param name="keyTransRecipientInfo">
        /// KeyTransRecipientInfo to create
        /// <see cref="Org.BouncyCastle.Asn1.Cms.RecipientInfo"/>
        /// </param>
        public RecipientInfoBCFips(IKeyTransRecipientInfo keyTransRecipientInfo)
            : base(new RecipientInfo(((KeyTransRecipientInfoBCFips)keyTransRecipientInfo).GetKeyTransRecipientInfo())) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Cms.RecipientInfo"/>.
        /// </returns>
        public virtual RecipientInfo GetRecipientInfo() {
            return (RecipientInfo)GetEncodable();
        }
    }
}
