using System;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;
using iText.Test;

namespace iText.Kernel.Pdf.Annot {
    [NUnit.Framework.Category("IntegrationTest")]
    public class AddTextMarkupAnnotationTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/pdf/annot/AddTextMarkupAnnotationTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/pdf/annot/AddTextMarkupAnnotationTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void TextMarkupTest01() {
            String filename = destinationFolder + "textMarkupAnnotation01.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filename));
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            //Initialize canvas and write text to it
            canvas.SaveState().BeginText().MoveText(36, 750).SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA
                ), 16).ShowText("Underline!").EndText().RestoreState();
            float[] points = new float[] { 36, 765, 109, 765, 36, 746, 109, 746 };
            PdfTextMarkupAnnotation markup = PdfTextMarkupAnnotation.CreateUnderline(PageSize.A4, points);
            markup.SetContents(new PdfString("TextMarkup"));
            float[] rgb = new float[] { 1, 0, 0 };
            PdfArray colors = new PdfArray(rgb);
            markup.SetColor(colors);
            page1.AddAnnotation(markup);
            page1.Flush();
            pdfDoc.Close();
            CompareTool compareTool = new CompareTool();
            String errorMessage = compareTool.CompareByContent(filename, sourceFolder + "cmp_textMarkupAnnotation01.pdf"
                , destinationFolder, "diff_");
            if (errorMessage != null) {
                NUnit.Framework.Assert.IsNull(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void TextMarkupTest02() {
            String filename = destinationFolder + "textMarkupAnnotation02.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filename));
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            //Initialize canvas and write text to it
            canvas.SaveState().BeginText().MoveText(36, 750).SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA
                ), 16).ShowText("Highlight!").EndText().RestoreState();
            float[] points = new float[] { 36, 765, 109, 765, 36, 746, 109, 746 };
            PdfTextMarkupAnnotation markup = PdfTextMarkupAnnotation.CreateHighLight(PageSize.A4, points);
            markup.SetContents(new PdfString("TextMarkup"));
            float[] rgb = new float[] { 1, 0, 0 };
            PdfArray colors = new PdfArray(rgb);
            markup.SetColor(colors);
            page1.AddAnnotation(markup);
            page1.Flush();
            pdfDoc.Close();
            CompareTool compareTool = new CompareTool();
            String errorMessage = compareTool.CompareByContent(filename, sourceFolder + "cmp_textMarkupAnnotation02.pdf"
                , destinationFolder, "diff_");
            if (errorMessage != null) {
                NUnit.Framework.Assert.IsNull(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void TextMarkupTest03() {
            String filename = destinationFolder + "textMarkupAnnotation03.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filename));
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            //Initialize canvas and write text to it
            canvas.SaveState().BeginText().MoveText(36, 750).SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA
                ), 16).ShowText("Squiggly!").EndText().RestoreState();
            float[] points = new float[] { 36, 765, 109, 765, 36, 746, 109, 746 };
            PdfTextMarkupAnnotation markup = PdfTextMarkupAnnotation.CreateSquiggly(PageSize.A4, points);
            markup.SetContents(new PdfString("TextMarkup"));
            float[] rgb = new float[] { 1, 0, 0 };
            PdfArray colors = new PdfArray(rgb);
            markup.SetColor(colors);
            page1.AddAnnotation(markup);
            page1.Flush();
            pdfDoc.Close();
            CompareTool compareTool = new CompareTool();
            String errorMessage = compareTool.CompareByContent(filename, sourceFolder + "cmp_textMarkupAnnotation03.pdf"
                , destinationFolder, "diff_");
            if (errorMessage != null) {
                NUnit.Framework.Assert.IsNull(errorMessage);
            }
        }

        [NUnit.Framework.Test]
        public virtual void TextMarkupTest04() {
            String filename = destinationFolder + "textMarkupAnnotation04.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filename));
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            //Initialize canvas and write text to it
            canvas.SaveState().BeginText().MoveText(36, 750).SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA
                ), 16).ShowText("Strikeout!").EndText().RestoreState();
            float[] points = new float[] { 36, 765, 109, 765, 36, 746, 109, 746 };
            PdfTextMarkupAnnotation markup = PdfTextMarkupAnnotation.CreateStrikeout(PageSize.A4, points);
            markup.SetContents(new PdfString("TextMarkup"));
            float[] rgb = new float[] { 1, 0, 0 };
            PdfArray colors = new PdfArray(rgb);
            markup.SetColor(colors);
            page1.AddAnnotation(markup);
            page1.Flush();
            pdfDoc.Close();
            CompareTool compareTool = new CompareTool();
            String errorMessage = compareTool.CompareByContent(filename, sourceFolder + "cmp_textMarkupAnnotation04.pdf"
                , destinationFolder, "diff_");
            if (errorMessage != null) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }
    }
}
