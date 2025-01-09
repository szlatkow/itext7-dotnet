/*
This file is part of the iText (R) project.
Copyright (c) 1998-2025 Apryse Group NV
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
using iText.Kernel.Pdf;
using iText.Test;

namespace iText.Kernel.Pdf.Collection {
    [NUnit.Framework.Category("UnitTest")]
    public class PdfCollectionSchemaTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void FieldAddingTest() {
            String fieldName = "fieldName";
            PdfDictionary pdfObject = new PdfDictionary();
            pdfObject.Put(PdfName.Subtype, PdfName.S);
            PdfCollectionField field = new PdfCollectionField(pdfObject);
            PdfCollectionSchema schema = new PdfCollectionSchema();
            schema.AddField(fieldName, field);
            // should be the same instance
            NUnit.Framework.Assert.AreEqual(schema.GetField(fieldName).GetPdfObject(), pdfObject);
        }

        [NUnit.Framework.Test]
        public virtual void IsWrappedObjectMustBeIndirectTest() {
            NUnit.Framework.Assert.IsFalse(new PdfCollectionSchema().IsWrappedObjectMustBeIndirect());
        }
    }
}
