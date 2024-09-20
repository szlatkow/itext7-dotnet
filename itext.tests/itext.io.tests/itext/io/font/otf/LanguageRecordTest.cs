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

namespace iText.IO.Font.Otf {
    [NUnit.Framework.Category("UnitTest")]
    public class LanguageRecordTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void FeaturesRequiredTest() {
            LanguageRecord languageRecord = new LanguageRecord();
            languageRecord.SetFeatureRequired(1);
            NUnit.Framework.Assert.AreEqual(1, languageRecord.GetFeatureRequired());
        }

        [NUnit.Framework.Test]
        public virtual void TaggingTest() {
            LanguageRecord languageRecord = new LanguageRecord();
            languageRecord.SetTag("tagname");
            NUnit.Framework.Assert.AreEqual("tagname", languageRecord.GetTag());
        }

        [NUnit.Framework.Test]
        public virtual void FeaturesTest() {
            LanguageRecord languageRecord = new LanguageRecord();
            int[] features = new int[2];
            languageRecord.SetFeatures(features);
            NUnit.Framework.Assert.AreEqual(2, languageRecord.GetFeatures().Length);
        }
    }
}
