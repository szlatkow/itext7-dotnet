/*
This file is part of the iText (R) project.
Copyright (c) 1998-2023 Apryse Group NV
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
using iText.Test;

namespace iText.Kernel.Utils.Objectpathitems {
    [NUnit.Framework.Category("UnitTest")]
    public class OffsetPathItemTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void EqualsAndHashCodeTest() {
            int offset = 1;
            OffsetPathItem offsetPathItem1 = new OffsetPathItem(offset);
            OffsetPathItem offsetPathItem2 = new OffsetPathItem(offset);
            bool result = offsetPathItem1.Equals(offsetPathItem2);
            NUnit.Framework.Assert.IsTrue(result);
            NUnit.Framework.Assert.AreEqual(offsetPathItem1.GetHashCode(), offsetPathItem2.GetHashCode());
        }

        [NUnit.Framework.Test]
        public virtual void NotEqualsAndHashCodeTest() {
            OffsetPathItem offsetPathItem1 = new OffsetPathItem(1);
            OffsetPathItem offsetPathItem2 = new OffsetPathItem(2);
            bool result = offsetPathItem1.Equals(offsetPathItem2);
            NUnit.Framework.Assert.IsFalse(result);
            NUnit.Framework.Assert.AreNotEqual(offsetPathItem1.GetHashCode(), offsetPathItem2.GetHashCode());
        }

        [NUnit.Framework.Test]
        public virtual void GetIndexTest() {
            int offset = 1;
            OffsetPathItem offsetPathItem = new OffsetPathItem(offset);
            NUnit.Framework.Assert.AreEqual(offset, offsetPathItem.GetOffset());
        }
    }
}
