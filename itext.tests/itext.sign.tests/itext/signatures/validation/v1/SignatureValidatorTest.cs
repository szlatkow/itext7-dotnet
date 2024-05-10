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
using System.Collections.Generic;
using System.Linq;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Security;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Signatures.Testutils;
using iText.Signatures.Testutils.Builder;
using iText.Signatures.Testutils.Client;
using iText.Signatures.Validation.V1.Context;
using iText.Signatures.Validation.V1.Report;
using iText.Test;

namespace iText.Signatures.Validation.V1 {
    [NUnit.Framework.Category("BouncyCastleUnitTest")]
    public class SignatureValidatorTest : ExtendedITextTest {
        private static readonly String CERTS_SRC = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/validation/v1/SignatureValidatorTest/certs/";

        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/validation/v1/SignatureValidatorTest/";

        private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();

        private static readonly char[] PASSWORD = "testpassphrase".ToCharArray();

        private SignatureValidationProperties parameters;

        private MockIssuingCertificateRetriever mockCertificateRetriever;

        private ValidatorChainBuilder builder;

        private MockChainValidator mockCertificateChainValidator;

        [NUnit.Framework.OneTimeSetUp]
        public static void Before() {
        }

        [NUnit.Framework.SetUp]
        public virtual void SetUp() {
            mockCertificateChainValidator = new MockChainValidator();
            parameters = new SignatureValidationProperties();
            mockCertificateRetriever = new MockIssuingCertificateRetriever();
            builder = new ValidatorChainBuilder().WithIssuingCertificateRetriever(mockCertificateRetriever).WithSignatureValidationProperties
                (parameters).WithCertificateChainValidator(mockCertificateChainValidator).WithRevocationDataValidator(
                new MockRevocationDataValidator());
        }

        [NUnit.Framework.Test]
        public virtual void LatestSignatureIsTimestampTest() {
            String chainName = CERTS_SRC + "validCertsChain.pem";
            String privateKeyName = CERTS_SRC + "rootCertKey.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            IPrivateKey rootPrivateKey = PemFileHelper.ReadFirstKey(privateKeyName, PASSWORD);
            IX509Certificate timeStampCert = (IX509Certificate)PemFileHelper.ReadFirstChain(CERTS_SRC + "timestamp.pem"
                )[0];
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "timestampSignatureDoc.pdf"))) {
                mockCertificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
                TestOcspResponseBuilder ocspBuilder = new TestOcspResponseBuilder(rootCert, rootPrivateKey);
                DateTime currentDate = DateTimeUtil.GetCurrentUtcTime();
                ocspBuilder.SetProducedAt(currentDate);
                ocspBuilder.SetThisUpdate(DateTimeUtil.GetCalendar(currentDate.AddDays(3)));
                ocspBuilder.SetNextUpdate(DateTimeUtil.GetCalendar(currentDate.AddDays(30)));
                TestOcspClient ocspClient = new TestOcspClient().AddBuilderForCertIssuer(rootCert, ocspBuilder);
                builder.GetRevocationDataValidator().AddOcspClient(ocspClient);
                parameters.SetRevocationOnlineFetching(ValidatorContexts.All(), CertificateSources.All(), TimeBasedContexts
                    .All(), SignatureValidationProperties.OnlineFetching.NEVER_FETCH).SetFreshness(ValidatorContexts.All()
                    , CertificateSources.All(), TimeBasedContexts.All(), TimeSpan.FromDays(-2));
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                report = signatureValidator.ValidateLatestSignature();
            }
            NUnit.Framework.Assert.AreEqual(1, mockCertificateChainValidator.verificationCalls.Count);
            MockChainValidator.ValidationCallBack call = mockCertificateChainValidator.verificationCalls[0];
            NUnit.Framework.Assert.AreEqual(CertificateSource.TIMESTAMP, call.context.GetCertificateSource());
            NUnit.Framework.Assert.AreEqual(ValidatorContext.SIGNATURE_VALIDATOR, call.context.GetValidatorContext());
            NUnit.Framework.Assert.AreEqual(timeStampCert.GetSubjectDN(), call.certificate.GetSubjectDN());
        }

        [NUnit.Framework.Test]
        public virtual void LatestSignatureWithBrokenTimestampTest() {
            String chainName = CERTS_SRC + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "docWithBrokenTimestamp.pdf"))
                ) {
                mockCertificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                report = signatureValidator.ValidateLatestSignature();
            }
            AssertValidationReport.AssertThat(report, (a) => a.HasStatus(ValidationReport.ValidationResult.INVALID).HasLogItem
                ((al) => al.WithCheckName(SignatureValidator.TIMESTAMP_VERIFICATION).WithMessage(SignatureValidator.CANNOT_VERIFY_TIMESTAMP
                ).WithStatus(ReportItem.ReportItemStatus.INVALID)));
        }

        [NUnit.Framework.Test]
        public virtual void DocumentModifiedLatestSignatureTest() {
            String chainName = CERTS_SRC + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "modifiedDoc.pdf"))) {
                mockCertificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                report = signatureValidator.ValidateLatestSignature();
            }
            AssertValidationReport.AssertThat(report, (a) => a.HasStatus(ValidationReport.ValidationResult.INVALID).HasLogItem
                ((al) => al.WithCheckName(SignatureValidator.SIGNATURE_VERIFICATION).WithMessage(SignatureValidator.DOCUMENT_IS_NOT_COVERED
                , (i) => "Signature1")).HasLogItem((al) => al.WithCheckName(SignatureValidator.SIGNATURE_VERIFICATION)
                .WithMessage(SignatureValidator.CANNOT_VERIFY_SIGNATURE, (i) => "Signature1")));
        }

        [NUnit.Framework.Test]
        public virtual void LatestSignatureInvalidStopValidationTest() {
            String chainName = CERTS_SRC + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            ValidationReport report;
            parameters.SetContinueAfterFailure(ValidatorContexts.All(), CertificateSources.All(), false);
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "modifiedDoc.pdf"))) {
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                mockCertificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
                report = signatureValidator.ValidateLatestSignature();
            }
            AssertValidationReport.AssertThat(report, (a) => a.HasStatus(ValidationReport.ValidationResult.INVALID).HasLogItem
                ((al) => al.WithCheckName(SignatureValidator.SIGNATURE_VERIFICATION).WithMessage(SignatureValidator.DOCUMENT_IS_NOT_COVERED
                , (i) => "Signature1").WithStatus(ReportItem.ReportItemStatus.INVALID)).HasLogItem((al) => al.WithCheckName
                (SignatureValidator.SIGNATURE_VERIFICATION).WithMessage(SignatureValidator.CANNOT_VERIFY_SIGNATURE, (i
                ) => "Signature1").WithStatus(ReportItem.ReportItemStatus.INVALID)));
            // check that no requests are made after failure
            NUnit.Framework.Assert.AreEqual(0, mockCertificateChainValidator.verificationCalls.Count);
        }

        [NUnit.Framework.Test]
        public virtual void CertificatesNotInLatestSignatureButTakenFromDSSTest() {
            String chainName = CERTS_SRC + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate signCert = (IX509Certificate)certificateChain[0];
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "docWithDss.pdf"))) {
                mockCertificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                signatureValidator.ValidateLatestSignature();
            }
            NUnit.Framework.Assert.AreEqual(2, mockCertificateRetriever.addKnownCertificatesCalls.Count);
            ICollection<IX509Certificate> dssCall = mockCertificateRetriever.addKnownCertificatesCalls[0];
            NUnit.Framework.Assert.AreEqual(3, dssCall.Count);
            NUnit.Framework.Assert.AreEqual(1, dssCall.Where((c) => ((IX509Certificate)c).Equals(rootCert)).Count());
            NUnit.Framework.Assert.AreEqual(1, dssCall.Where((c) => ((IX509Certificate)c).Equals(intermediateCert)).Count
                ());
            NUnit.Framework.Assert.AreEqual(1, dssCall.Where((c) => ((IX509Certificate)c).Equals(signCert)).Count());
        }

        [NUnit.Framework.Test]
        public virtual void CertificatesNotInLatestSignatureButTakenFromDSSOneCertIsBrokenTest() {
            String chainName = CERTS_SRC + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "docWithBrokenDss.pdf"))) {
                mockCertificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                report = signatureValidator.ValidateLatestSignature();
            }
            AssertValidationReport.AssertThat(report, (a) => a.HasStatus(ValidationReport.ValidationResult.VALID).HasLogItem
                ((al) => al.WithCheckName(SignatureValidator.CERTS_FROM_DSS).WithExceptionCauseType(typeof(AbstractGeneralSecurityException
                ))));
        }

        [NUnit.Framework.Test]
        public virtual void IndeterminateChainValidationLeadsToIndeterminateResultTest() {
            mockCertificateChainValidator.OnCallDo((c) => c.report.AddReportItem(new ReportItem("test", "test", ReportItem.ReportItemStatus
                .INDETERMINATE)));
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "validDoc.pdf"))) {
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                report = signatureValidator.ValidateLatestSignature();
            }
            AssertValidationReport.AssertThat(report, (a) => a.HasStatus(ValidationReport.ValidationResult.INDETERMINATE
                ).HasNumberOfFailures(1).HasLogItem((al) => al.WithCheckName("test").WithMessage("test")));
        }

        [NUnit.Framework.Test]
        public virtual void InvalidChainValidationLeadsToInvalidResultTest() {
            mockCertificateChainValidator.OnCallDo((c) => c.report.AddReportItem(new ReportItem("test", "test", ReportItem.ReportItemStatus
                .INVALID)));
            ValidationReport report;
            using (PdfDocument document = new PdfDocument(new PdfReader(SOURCE_FOLDER + "validDoc.pdf"))) {
                SignatureValidator signatureValidator = builder.BuildSignatureValidator(document);
                report = signatureValidator.ValidateLatestSignature();
            }
            AssertValidationReport.AssertThat(report, (a) => a.HasStatus(ValidationReport.ValidationResult.INVALID).HasNumberOfFailures
                (1).HasLogItem((al) => al.WithCheckName("test").WithMessage("test")));
        }
    }
}
