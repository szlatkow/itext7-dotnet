using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Forms.Form.Element;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using iText.Signatures.Logs;
using iText.Signatures.Testutils;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Signatures.Sign {
    [NUnit.Framework.Category("BouncyCastleIntegrationTest")]
    [NUnit.Framework.TestFixtureSource("CreateParametersTestFixtureData")]
    public class PdfPadesWithMissingCertTest : ExtendedITextTest {
        private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();

        private static readonly String certsSrc = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/sign/PdfPadesWithMissingCertTest/certs/";

        private static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/sign/PdfPadesWithMissingCertTest/";

        private static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/signatures/sign/PdfPadesWithMissingCertTest/";

        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        private readonly String missingCertName1;

        private readonly String missingCertName2;

        [NUnit.Framework.OneTimeSetUp]
        public static void Before() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        public PdfPadesWithMissingCertTest(Object missingCertName1, Object missingCertName2) {
            this.missingCertName1 = (String)missingCertName1;
            this.missingCertName2 = (String)missingCertName2;
        }

        public PdfPadesWithMissingCertTest(Object[] array)
            : this(array[0], array[1]) {
        }

        public static IEnumerable<Object[]> CreateParameters() {
            return JavaUtil.ArraysAsList(new Object[] { "missing_cert1.cer", "missing_cert2.cer" }, new Object[] { "missing_cert1.crt"
                , "missing_cert2.crt" }, new Object[] { null, "missing_certs.p7b" }, new Object[] { "not_existing_file"
                , "not_existing_file" }, new Object[] { "missing_cert1.der", "missing_cert2.der" });
        }

        public static ICollection<NUnit.Framework.TestFixtureData> CreateParametersTestFixtureData() {
            return CreateParameters().Select(array => new NUnit.Framework.TestFixtureData(array)).ToList();
        }

        [NUnit.Framework.Test]
        [LogMessage(SignLogMessageConstant.UNABLE_TO_PARSE_AIA_CERT, Ignore = true)]
        public virtual void MissingCertTest() {
            String srcFileName = sourceFolder + "helloWorldDoc.pdf";
            String signCertFileName = certsSrc + "sign_cert.pem";
            String fistIntermediateCertFileName = certsSrc + "first_intermediate_cert.pem";
            String secondIntermediateCertFileName = certsSrc + "second_intermediate_cert.pem";
            String rootCertFileName = certsSrc + "root_cert.pem";
            String firstMissingCertFileName = certsSrc + missingCertName1;
            String secondMissingCertFileName = certsSrc + missingCertName2;
            IX509Certificate signCert = (IX509Certificate)PemFileHelper.ReadFirstChain(signCertFileName)[0];
            IX509Certificate fistIntermediateCert = (IX509Certificate)PemFileHelper.ReadFirstChain(fistIntermediateCertFileName
                )[0];
            IX509Certificate secondIntermediateCert = (IX509Certificate)PemFileHelper.ReadFirstChain(secondIntermediateCertFileName
                )[0];
            IX509Certificate rootCert = (IX509Certificate)PemFileHelper.ReadFirstChain(rootCertFileName)[0];
            IPrivateKey signPrivateKey = PemFileHelper.ReadFirstKey(signCertFileName, PASSWORD);
            SignerProperties signerProperties = CreateSignerProperties();
            MemoryStream outputStream = new MemoryStream();
            PdfPadesSigner padesSigner = CreatePdfPadesSigner(srcFileName, outputStream);
            IMissingCertificatesClient missingCertificatesClient = new _MissingCertificatesClient_96(firstMissingCertFileName
                , secondMissingCertFileName);
            padesSigner.SetMissingCertificatesClient(missingCertificatesClient);
            padesSigner.SignWithBaselineBProfile(signerProperties, new IX509Certificate[] { signCert, rootCert }, signPrivateKey
                );
            TestSignUtils.BasicCheckSignedDoc(new MemoryStream(outputStream.ToArray()), "Signature1");
            IList<IX509Certificate> expectedCerts;
            if ("not_existing_file".Equals(missingCertName1)) {
                expectedCerts = JavaUtil.ArraysAsList(signCert, rootCert);
            }
            else {
                expectedCerts = JavaUtil.ArraysAsList(signCert, fistIntermediateCert, secondIntermediateCert, rootCert);
            }
            TestSignUtils.SignedDocumentContainsCerts(new MemoryStream(outputStream.ToArray()), expectedCerts);
        }

        private sealed class _MissingCertificatesClient_96 : MissingCertificatesClient {
            public _MissingCertificatesClient_96(String firstMissingCertFileName, String secondMissingCertFileName) {
                this.firstMissingCertFileName = firstMissingCertFileName;
                this.secondMissingCertFileName = secondMissingCertFileName;
            }

            protected internal override Stream GetIssuerCertByURI(String uri) {
                if (uri.Contains("intermediate")) {
                    return FileUtil.GetInputStreamForFile(firstMissingCertFileName);
                }
                if (uri.Contains("leaf")) {
                    return FileUtil.GetInputStreamForFile(secondMissingCertFileName);
                }
                return null;
            }

            private readonly String firstMissingCertFileName;

            private readonly String secondMissingCertFileName;
        }

        private SignerProperties CreateSignerProperties() {
            SignerProperties signerProperties = new SignerProperties();
            signerProperties.SetFieldName("Signature1");
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(signerProperties.GetFieldName()).SetContent
                ("Approval test signature.\nCreated by iText.");
            signerProperties.SetPageRect(new Rectangle(50, 650, 200, 100)).SetSignatureAppearance(appearance);
            return signerProperties;
        }

        private PdfPadesSigner CreatePdfPadesSigner(String srcFileName, Stream outputStream) {
            return new PdfPadesSigner(new PdfReader(FileUtil.GetInputStreamForFile(srcFileName)), outputStream);
        }
    }
}