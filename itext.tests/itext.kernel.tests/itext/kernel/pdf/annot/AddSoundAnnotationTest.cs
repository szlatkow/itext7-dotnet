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
using System.IO;
using System.Text;
using iText.Commons.Utils;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Test;

namespace iText.Kernel.Pdf.Annot {
    [NUnit.Framework.Category("IntegrationTest")]
    public class AddSoundAnnotationTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/pdf/annot/AddSoundAnnotationTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/pdf/annot/AddSoundAnnotationTest/";

        private const String RIFF_TAG = "RIFF";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.OneTimeTearDown]
        public static void AfterClass() {
            CompareTool.Cleanup(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void SoundTestAif() {
            String filename = destinationFolder + "soundAnnotation02.pdf";
            String audioFile = sourceFolder + "sample.aif";
            String cmp = sourceFolder + "cmp_soundAnnotation02.pdf";
            using (Stream @is = PrepareAudioFileStream(audioFile)) {
                using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(filename))) {
                    PdfStream sound1 = new PdfStream(pdfDoc, @is);
                    sound1.Put(PdfName.R, new PdfNumber(32117));
                    sound1.Put(PdfName.E, PdfName.Signed);
                    sound1.Put(PdfName.B, new PdfNumber(16));
                    sound1.Put(PdfName.C, new PdfNumber(1));
                    pdfDoc.AddNewPage().AddAnnotation(new PdfSoundAnnotation(new Rectangle(100, 100, 100, 100), sound1));
                }
            }
            String errorMessage = new CompareTool().CompareByContent(filename, cmp, destinationFolder);
            if (errorMessage != null) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void SoundTestAiff() {
            String filename = destinationFolder + "soundAnnotation03.pdf";
            String audioFile = sourceFolder + "sample.aiff";
            String cmpPdf = sourceFolder + "cmp_soundAnnotation03.pdf";
            using (Stream @is = PrepareAudioFileStream(audioFile)) {
                using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(filename))) {
                    PdfStream sound1 = new PdfStream(pdfDoc, @is);
                    sound1.Put(PdfName.R, new PdfNumber(44100));
                    sound1.Put(PdfName.E, PdfName.Signed);
                    sound1.Put(PdfName.B, new PdfNumber(16));
                    sound1.Put(PdfName.C, new PdfNumber(1));
                    pdfDoc.AddNewPage().AddAnnotation(new PdfSoundAnnotation(new Rectangle(100, 100, 100, 100), sound1));
                }
            }
            String errorMessage = new CompareTool().CompareByContent(filename, cmpPdf, destinationFolder);
            if (errorMessage != null) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void SoundTestWav01() {
            String filename = destinationFolder + "soundAnnotation05.pdf";
            String audioFile = sourceFolder + "sample.wav";
            String cmpPdf = sourceFolder + "cmp_soundAnnotation05.pdf";
            using (Stream @is = PrepareAudioFileStream(audioFile)) {
                using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(filename))) {
                    PdfStream soundStream = new PdfStream(pdfDoc, @is);
                    soundStream.Put(PdfName.R, new PdfNumber(48000));
                    soundStream.Put(PdfName.E, PdfName.Signed);
                    soundStream.Put(PdfName.B, new PdfNumber(16));
                    soundStream.Put(PdfName.C, new PdfNumber(2));
                    PdfSoundAnnotation sound = new PdfSoundAnnotation(new Rectangle(100, 100, 100, 100), soundStream);
                    pdfDoc.AddNewPage().AddAnnotation(sound);
                }
            }
            String errorMessage = new CompareTool().CompareByContent(filename, cmpPdf, destinationFolder);
            if (errorMessage != null) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void SoundTestSnd() {
            String filename = destinationFolder + "soundAnnotation04.pdf";
            String audioFile = sourceFolder + "sample.snd";
            String cmpPdf = sourceFolder + "cmp_soundAnnotation04.pdf";
            using (Stream @is = new FileStream(audioFile, FileMode.Open, FileAccess.Read)) {
                using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(filename))) {
                    Rectangle rect = new Rectangle(100, 100, 100, 100);
                    PdfSoundAnnotation sound = new PdfSoundAnnotation(pdfDoc, rect, @is, 44100, PdfName.Signed, 2, 16);
                    pdfDoc.AddNewPage().AddAnnotation(sound);
                }
            }
            String errorMessage = new CompareTool().CompareByContent(filename, cmpPdf, destinationFolder);
            if (errorMessage != null) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void SoundTestWav() {
            String filename = destinationFolder + "soundAnnotation01.pdf";
            String audioFile = sourceFolder + "sample.wav";
            String cmpPdf = sourceFolder + "cmp_soundAnnotation01.pdf";
            using (Stream @is = new FileStream(audioFile, FileMode.Open, FileAccess.Read)) {
                using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(filename))) {
                    Rectangle rect = new Rectangle(100, 100, 100, 100);
                    PdfSoundAnnotation sound = new PdfSoundAnnotation(pdfDoc, rect, @is, 48000, PdfName.Signed, 2, 16);
                    pdfDoc.AddNewPage().AddAnnotation(sound);
                }
            }
            String errorMessage = new CompareTool().CompareByContent(filename, cmpPdf, destinationFolder);
            if (errorMessage != null) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }

        private Stream PrepareAudioFileStream(String audioFile) {
            StringBuilder sb = new StringBuilder();
            using (Stream @is = FileUtil.GetInputStreamForFile(audioFile)) {
                for (int i = 0; i < 4; i++) {
                    sb.Append((char)@is.Read());
                }
            }
            bool skipFirstByte = sb.ToString().Equals(RIFF_TAG);
            Stream stream = FileUtil.GetInputStreamForFile(audioFile);
            if (skipFirstByte) {
                stream.Read();
            }
            return stream;
        }
    }
}
