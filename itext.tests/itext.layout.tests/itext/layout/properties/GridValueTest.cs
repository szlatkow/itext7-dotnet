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
using iText.Test;

namespace iText.Layout.Properties {
    [NUnit.Framework.Category("UnitTest")]
    public class GridValueTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void SizingValueTest() {
            GridValue value = GridValue.CreateSizeValue(SizingValue.CreateUnitValue(UnitValue.CreatePointValue(1.3f)));
            NUnit.Framework.Assert.AreEqual(GridValue.GridValueType.SIZING, value.GetType());
            NUnit.Framework.Assert.AreEqual(1.3f, (float)value.GetAbsoluteValue(), 0.00001);
            value = GridValue.CreateSizeValue(SizingValue.CreateUnitValue(UnitValue.CreatePercentValue(30)));
            NUnit.Framework.Assert.AreEqual(GridValue.GridValueType.SIZING, value.GetType());
            NUnit.Framework.Assert.IsNull(value.GetAbsoluteValue());
            NUnit.Framework.Assert.AreEqual(30, value.GetSizingValue().GetUnitValue().GetValue(), 0.00001);
        }

        [NUnit.Framework.Test]
        public virtual void UnitValueTest() {
            GridValue value = GridValue.CreateUnitValue(UnitValue.CreatePointValue(1.3f));
            NUnit.Framework.Assert.AreEqual(GridValue.GridValueType.SIZING, value.GetType());
            NUnit.Framework.Assert.AreEqual(1.3f, (float)value.GetAbsoluteValue(), 0.00001);
            value = GridValue.CreateUnitValue(UnitValue.CreatePercentValue(30));
            NUnit.Framework.Assert.AreEqual(GridValue.GridValueType.SIZING, value.GetType());
            NUnit.Framework.Assert.IsNull(value.GetAbsoluteValue());
            NUnit.Framework.Assert.AreEqual(30, value.GetSizingValue().GetUnitValue().GetValue(), 0.00001);
        }

        [NUnit.Framework.Test]
        public virtual void FlexValueTest() {
            GridValue value = GridValue.CreateFlexValue(1.5f);
            NUnit.Framework.Assert.AreEqual(GridValue.GridValueType.FLEX, value.GetType());
            NUnit.Framework.Assert.IsNull(value.GetAbsoluteValue());
            NUnit.Framework.Assert.AreEqual(1.5f, (float)value.GetFlexValue(), 0.00001);
        }
    }
}
