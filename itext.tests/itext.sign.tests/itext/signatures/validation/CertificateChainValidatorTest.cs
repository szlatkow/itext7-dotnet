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
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Security;
using iText.Commons.Utils;
using iText.Signatures;
using iText.Signatures.Testutils;
using iText.Signatures.Validation.Extensions;
using iText.Test;

namespace iText.Signatures.Validation {
    [NUnit.Framework.Category("BouncyCastleUnitTest")]
    public class CertificateChainValidatorTest : ExtendedITextTest {
        private static readonly String certsSrc = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/signatures/validation/CertificateChainValidatorTest/";

        [NUnit.Framework.Test]
        public virtual void ValidChainTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.VALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetLogs().Count);
            NUnit.Framework.Assert.IsTrue(report.GetFailures().IsEmpty());
            CertificateValidationReport.ReportItem item = report.GetLogs()[0];
            NUnit.Framework.Assert.AreEqual(rootCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), item.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void SeveralFailuresWithProceedAfterFailTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            validator.SetGlobalRequiredExtensions(JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension
                (KeyUsage.DIGITAL_SIGNATURE)));
            validator.ProceedValidationAfterFail(true);
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension(KeyUsage.DECIPHER_ONLY
                )));
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INVALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(3, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(4, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[1], report.GetLogs()[1]);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[2], report.GetLogs()[2]);
            CertificateValidationReport.ReportItem log = report.GetLogs()[3];
            NUnit.Framework.Assert.AreEqual(rootCert, log.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", log.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), log.GetMessage());
            CertificateValidationReport.ReportItem failure1 = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(signingCert, failure1.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure1.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure1.GetMessage());
            CertificateValidationReport.ReportItem failure2 = report.GetFailures()[1];
            NUnit.Framework.Assert.AreEqual(intermediateCert, failure2.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure2.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Globally required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure2.GetMessage());
            CertificateValidationReport.ReportItem failure3 = report.GetFailures()[2];
            NUnit.Framework.Assert.AreEqual(rootCert, failure3.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure3.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Globally required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure3.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void SeveralFailuresWithoutProceedAfterFailTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            validator.SetGlobalRequiredExtensions(JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension
                (KeyUsage.DIGITAL_SIGNATURE)));
            validator.ProceedValidationAfterFail(false);
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension(KeyUsage.DECIPHER_ONLY
                )));
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INVALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(1, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            CertificateValidationReport.ReportItem failure1 = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(signingCert, failure1.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure1.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure1.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void IntermediateCertTrustedTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.VALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetLogs().Count);
            NUnit.Framework.Assert.IsTrue(report.GetFailures().IsEmpty());
            CertificateValidationReport.ReportItem item = report.GetLogs()[0];
            NUnit.Framework.Assert.AreEqual(intermediateCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , intermediateCert.GetSubjectDN()), item.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void ValidChainRequiredExtensionPositiveTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension(KeyUsage.DIGITAL_SIGNATURE
                )));
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.VALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetLogs().Count);
            NUnit.Framework.Assert.IsTrue(report.GetFailures().IsEmpty());
            CertificateValidationReport.ReportItem item = report.GetLogs()[0];
            NUnit.Framework.Assert.AreEqual(rootCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), item.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void ValidChainGloballyRequiredExtensionPositiveTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            validator.SetGlobalRequiredExtensions(JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension
                (0)));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.VALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetLogs().Count);
            NUnit.Framework.Assert.IsTrue(report.GetFailures().IsEmpty());
            CertificateValidationReport.ReportItem item = report.GetLogs()[0];
            NUnit.Framework.Assert.AreEqual(rootCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), item.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void ValidChainRequiredExtensionNegativeTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension(KeyUsage.KEY_CERT_SIGN
                )));
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INVALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(2, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            CertificateValidationReport.ReportItem log = report.GetLogs()[1];
            NUnit.Framework.Assert.AreEqual(rootCert, log.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", log.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), log.GetMessage());
            CertificateValidationReport.ReportItem failure = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(signingCert, failure.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void ValidChainGloballyRequiredExtensionNegativeTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            validator.SetGlobalRequiredExtensions(JavaCollectionsUtil.SingletonList<CertificateExtension>(new KeyUsageExtension
                (KeyUsage.DIGITAL_SIGNATURE)));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INVALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(2, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(3, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[1], report.GetLogs()[1]);
            CertificateValidationReport.ReportItem log = report.GetLogs()[2];
            NUnit.Framework.Assert.AreEqual(rootCert, log.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", log.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), log.GetMessage());
            CertificateValidationReport.ReportItem failure1 = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(intermediateCert, failure1.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure1.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Globally required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure1.GetMessage());
            CertificateValidationReport.ReportItem failure2 = report.GetFailures()[1];
            NUnit.Framework.Assert.AreEqual(rootCert, failure2.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Required certificate extensions check.", failure2.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Globally required extension {0} is missing or incorrect."
                , OID.X509Extensions.KEY_USAGE), failure2.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void ValidChainTrustedRootIsnSetTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaUtil.ArraysAsList(intermediateCert, rootCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INDETERMINATE, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(1, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            CertificateValidationReport.ReportItem item = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(rootCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} isn't trusted and issuer certificate isn't provided."
                , rootCert.GetSubjectDN()), item.GetMessage());
        }

        [NUnit.Framework.Test]
        public virtual void IntermediateCertIsNotYetValidTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            String intermediateCertName = certsSrc + "notYetValidIntermediateCert.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)PemFileHelper.ReadFirstChain(intermediateCertName)[0
                ];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INVALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(2, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            CertificateValidationReport.ReportItem log = report.GetLogs()[1];
            NUnit.Framework.Assert.AreEqual(rootCert, log.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", log.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), log.GetMessage());
            CertificateValidationReport.ReportItem item = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(intermediateCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate validity period check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is not yet valid.", intermediateCert
                .GetSubjectDN()), item.GetMessage());
            Exception exception = item.GetExceptionCause();
            NUnit.Framework.Assert.IsTrue(exception is AbstractCertificateNotYetValidException);
        }

        [NUnit.Framework.Test]
        public virtual void IntermediateCertIsExpiredTest() {
            String chainName = certsSrc + "validCertsChain.pem";
            String intermediateCertName = certsSrc + "expiredIntermediateCert.pem";
            IX509Certificate[] certificateChain = PemFileHelper.ReadFirstChain(chainName);
            IX509Certificate signingCert = (IX509Certificate)certificateChain[0];
            IX509Certificate intermediateCert = (IX509Certificate)PemFileHelper.ReadFirstChain(intermediateCertName)[0
                ];
            IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
            CertificateChainValidator validator = new CertificateChainValidator();
            validator.SetKnownCertificates(JavaCollectionsUtil.SingletonList(intermediateCert));
            validator.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            CertificateValidationReport report = validator.ValidateCertificate(signingCert, DateTimeUtil.GetCurrentUtcTime
                (), null);
            NUnit.Framework.Assert.AreEqual(CertificateValidationReport.ValidationResult.INVALID, report.GetValidationResult
                ());
            NUnit.Framework.Assert.AreEqual(1, report.GetFailures().Count);
            NUnit.Framework.Assert.AreEqual(2, report.GetLogs().Count);
            NUnit.Framework.Assert.AreEqual(report.GetFailures()[0], report.GetLogs()[0]);
            CertificateValidationReport.ReportItem log = report.GetLogs()[1];
            NUnit.Framework.Assert.AreEqual(rootCert, log.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate check.", log.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is trusted, revocation data checks are not required."
                , rootCert.GetSubjectDN()), log.GetMessage());
            CertificateValidationReport.ReportItem item = report.GetFailures()[0];
            NUnit.Framework.Assert.AreEqual(intermediateCert, item.GetCertificate());
            NUnit.Framework.Assert.AreEqual("Certificate validity period check.", item.GetCheckName());
            NUnit.Framework.Assert.AreEqual(MessageFormatUtil.Format("Certificate {0} is expired.", intermediateCert.GetSubjectDN
                ()), item.GetMessage());
            Exception exception = item.GetExceptionCause();
            NUnit.Framework.Assert.IsTrue(exception is AbstractCertificateExpiredException);
        }
    }
}
