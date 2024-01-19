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
using Org.BouncyCastle.Cms;
using iText.Commons.Bouncycastle.Cms;
using iText.Commons.Utils;

namespace iText.Bouncycastlefips.Cms {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Cms.CmsEnvelopedData"/>.
    /// </summary>
    public class CmsEnvelopedDataBCFips : ICmsEnvelopedData {
        private readonly CmsEnvelopedData cmsEnvelopedData;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Cms.CmsEnvelopedData"/>.
        /// </summary>
        /// <param name="cmsEnvelopedData">
        /// 
        /// <see cref="Org.BouncyCastle.Cms.CmsEnvelopedData"/>
        /// to be wrapped
        /// </param>
        public CmsEnvelopedDataBCFips(CmsEnvelopedData cmsEnvelopedData) {
            this.cmsEnvelopedData = cmsEnvelopedData;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Cms.CmsEnvelopedData"/>.
        /// </returns>
        public virtual CmsEnvelopedData GetCmsEnvelopedData() {
            return cmsEnvelopedData;
        }

        /// <summary><inheritDoc/></summary>
        public virtual IRecipientInformationStore GetRecipientInfos() {
            return new RecipientInformationStoreBCFips(cmsEnvelopedData.GetRecipientInfos());
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
            iText.Bouncycastlefips.Cms.CmsEnvelopedDataBCFips that = (iText.Bouncycastlefips.Cms.CmsEnvelopedDataBCFips
                )o;
            return Object.Equals(cmsEnvelopedData, that.cmsEnvelopedData);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(cmsEnvelopedData);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return cmsEnvelopedData.ToString();
        }
    }
}
