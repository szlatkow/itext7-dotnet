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
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.IO.Util;
using iText.Kernel.Crypto;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Utils;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Kernel.Mac {
    [NUnit.Framework.Category("BouncyCastleIntegrationTest")]
    public class MacIntegrityProtectorTest : ExtendedITextTest {
        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/mac/MacIntegrityProtectorTest/";

        private static readonly String DESTINATION_FOLDER = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/mac/MacIntegrityProtectorTest/";

        private static readonly String CERTS_SRC = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/mac/MacIntegrityProtectorTest/certs/";

        private static readonly byte[] PASSWORD = "123".GetBytes();

        private static readonly String PROVIDER_NAME = BouncyCastleFactoryCreator.GetFactory().GetProviderName();

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(DESTINATION_FOLDER);
        }

        [NUnit.Framework.OneTimeTearDown]
        public static void AfterClass() {
            CompareTool.Cleanup(DESTINATION_FOLDER);
        }

        [NUnit.Framework.Test]
        public virtual void StandaloneMacStandardEncryptionTest() {
            String fileName = "standaloneMacStandardEncryptionTest.pdf";
            String outputFileName = DESTINATION_FOLDER + fileName;
            String cmpFileName = SOURCE_FOLDER + "cmp_" + fileName;
            MacProperties macProperties = new MacProperties(MacProperties.MacDigestAlgorithm.SHA_256, MacProperties.MacAlgorithm
                .HMAC_WITH_SHA_256, MacProperties.KeyWrappingAlgorithm.AES_256_NO_PADD);
            WriterProperties writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetStandardEncryption
                (PASSWORD, PASSWORD, 0, EncryptionConstants.ENCRYPTION_AES_256, macProperties);
            if ("BC".Equals(PROVIDER_NAME)) {
                using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                    ))) {
                    pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                }
                NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outputFileName, cmpFileName, DESTINATION_FOLDER
                    , "diff", PASSWORD, PASSWORD));
            }
            else {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(NotSupportedException), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("HKDF algorithm is not supported in bouncy-castle FIPS mode.", exceptionMessage
                    );
            }
        }

        [NUnit.Framework.Test]
        public virtual void StandaloneMacUnwritableStreamTest() {
            String fileName = "standaloneMacStandardEncryptionTest.pdf";
            String outputFileName = DESTINATION_FOLDER + fileName;
            MacProperties macProperties = new MacProperties(MacProperties.MacDigestAlgorithm.SHA_256, MacProperties.MacAlgorithm
                .HMAC_WITH_SHA_256, MacProperties.KeyWrappingAlgorithm.AES_256_NO_PADD);
            WriterProperties writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetStandardEncryption
                (PASSWORD, PASSWORD, 0, EncryptionConstants.ENCRYPTION_AES_256, macProperties);
            MemoryStream unwritableStream = new _MemoryStream_122();
            if ("BC".Equals(PROVIDER_NAME)) {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(Exception), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(new PdfWriter(unwritableStream, writerProperties))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("expected", exceptionMessage);
            }
            else {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(NotSupportedException), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("HKDF algorithm is not supported in bouncy-castle FIPS mode.", exceptionMessage
                    );
            }
            unwritableStream.Dispose();
        }

        private sealed class _MemoryStream_122 : MemoryStream {
            public _MemoryStream_122() {
            }

            public override void Write(byte[] b, int off, int len) {
                throw new Exception("expected");
            }
        }

        [NUnit.Framework.Test]
        public virtual void StandaloneMacWithAllHashAlgorithmsTest() {
            for (int i = 0; i < EnumUtil.GetAllValuesOfEnum<MacProperties.MacDigestAlgorithm>().Count; i++) {
                String fileName = "standaloneMacWithAllHashAlgorithmsTest" + (i + 1) + ".pdf";
                String outputFileName = DESTINATION_FOLDER + fileName;
                String cmpFileName = SOURCE_FOLDER + "cmp_" + fileName;
                MacProperties macProperties = new MacProperties(EnumUtil.GetAllValuesOfEnum<MacProperties.MacDigestAlgorithm
                    >()[i], MacProperties.MacAlgorithm.HMAC_WITH_SHA_256, MacProperties.KeyWrappingAlgorithm.AES_256_NO_PADD
                    );
                WriterProperties writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetStandardEncryption
                    (PASSWORD, PASSWORD, 0, EncryptionConstants.ENCRYPTION_AES_256, macProperties);
                if ("BC".Equals(PROVIDER_NAME)) {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                    NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outputFileName, cmpFileName, DESTINATION_FOLDER
                        , "diff", PASSWORD, PASSWORD));
                }
                else {
                    String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(NotSupportedException), () => {
                        using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                            ))) {
                            pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                        }
                    }
                    ).Message;
                    NUnit.Framework.Assert.AreEqual("HKDF algorithm is not supported in bouncy-castle FIPS mode.", exceptionMessage
                        );
                }
            }
        }

        [NUnit.Framework.Test]
        public virtual void StandaloneMacPdfVersionNotSetTest() {
            String fileName = "standaloneMacPdfVersionNotSetTest.pdf";
            String outputFileName = DESTINATION_FOLDER + fileName;
            MacProperties macProperties = new MacProperties(MacProperties.MacDigestAlgorithm.SHA_256, MacProperties.MacAlgorithm
                .HMAC_WITH_SHA_256, MacProperties.KeyWrappingAlgorithm.AES_256_NO_PADD);
            WriterProperties writerProperties = new WriterProperties().SetStandardEncryption(PASSWORD, PASSWORD, 0, EncryptionConstants
                .ENCRYPTION_AES_256, macProperties);
            if ("BC".Equals(PROVIDER_NAME)) {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(PdfException), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("MAC integrity protection is only supported for PDF 2.0 or higher.", exceptionMessage
                    );
            }
            else {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(NotSupportedException), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("HKDF algorithm is not supported in bouncy-castle FIPS mode.", exceptionMessage
                    );
            }
        }

        [NUnit.Framework.Test]
        public virtual void StandaloneMacOldEncryptionAlgorithmTest() {
            String fileName = "standaloneMacOldEncryptionAlgorithmTest.pdf";
            String outputFileName = DESTINATION_FOLDER + fileName;
            MacProperties macProperties = new MacProperties(MacProperties.MacDigestAlgorithm.SHA_256, MacProperties.MacAlgorithm
                .HMAC_WITH_SHA_256, MacProperties.KeyWrappingAlgorithm.AES_256_NO_PADD);
            WriterProperties writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetStandardEncryption
                (PASSWORD, PASSWORD, 0, EncryptionConstants.ENCRYPTION_AES_128, macProperties);
            if ("BC".Equals(PROVIDER_NAME)) {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(PdfException), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("MAC integrity protection is only supported for encryption algorithms of version 5 or higher."
                    , exceptionMessage);
            }
            else {
                String exceptionMessage = NUnit.Framework.Assert.Catch(typeof(NotSupportedException), () => {
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                ).Message;
                NUnit.Framework.Assert.AreEqual("HKDF algorithm is not supported in bouncy-castle FIPS mode.", exceptionMessage
                    );
            }
        }

        [NUnit.Framework.Test]
        [LogMessage(VersionConforming.DEPRECATED_AES256_REVISION, Ignore = true)]
        public virtual void StandaloneMacPublicKeyEncryptionTest() {
            String fileName = "standaloneMacPublicKeyEncryptionTest.pdf";
            String outputFileName = DESTINATION_FOLDER + fileName;
            String cmpFileName = SOURCE_FOLDER + "cmp_" + fileName;
            MacProperties macProperties = new MacProperties(MacProperties.MacDigestAlgorithm.SHA_256, MacProperties.MacAlgorithm
                .HMAC_WITH_SHA_256, MacProperties.KeyWrappingAlgorithm.AES_256_NO_PADD);
            IX509Certificate certificate = CryptoUtil.ReadPublicCertificate(FileUtil.GetInputStreamForFile(CERTS_SRC +
                 "SHA256withRSA.cer"));
            if ("BC".Equals(PROVIDER_NAME)) {
                WriterProperties writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetPublicKeyEncryption
                    (new IX509Certificate[] { certificate }, new int[] { -1 }, EncryptionConstants.ENCRYPTION_AES_256, macProperties
                    );
                using (PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outputFileName, writerProperties))) {
                    pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                }
                IPrivateKey privateKey = GetPrivateKey("SHA256withRSA.key");
                CompareTool compareTool = new CompareTool();
                compareTool.GetCmpReaderProperties().SetPublicKeySecurityParams(certificate, privateKey);
                compareTool.GetOutReaderProperties().SetPublicKeySecurityParams(certificate, privateKey);
                NUnit.Framework.Assert.IsNull(compareTool.CompareByContent(outputFileName, cmpFileName, DESTINATION_FOLDER
                    , "diff"));
            }
            else {
                NUnit.Framework.Assert.Catch(typeof(Exception), () => {
                    WriterProperties writerProperties = new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0).SetPublicKeyEncryption
                        (new IX509Certificate[] { certificate }, new int[] { -1 }, EncryptionConstants.ENCRYPTION_AES_256, macProperties
                        );
                    using (PdfDocument pdfDoc = new PdfDocument(CompareTool.CreateTestPdfWriter(outputFileName, writerProperties
                        ))) {
                        pdfDoc.AddNewPage().AddAnnotation(new PdfTextAnnotation(new Rectangle(100, 100, 100, 100)));
                    }
                }
                );
            }
        }

        public virtual IPrivateKey GetPrivateKey(String keyName) {
            return PemFileHelper.ReadPrivateKeyFromPemFile(FileUtil.GetInputStreamForFile(CERTS_SRC + keyName), "testpassphrase"
                .ToCharArray());
        }
    }
}
