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
using System.Collections.Generic;
using System.Text;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Utils;
using iText.IO.Util;
using iText.Kernel.Crypto;

namespace iText.Signatures.Validation.Extensions {
    /// <summary>Class representing "Key Usage" extenstion.</summary>
    public class KeyUsageExtension : CertificateExtension {
        private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();

        public const String EXPECTED_VALUE = "Key usage expected: ({0})";

        public const String ACTUAL_VALUE = "\nbut found {0}";

        public const String MISSING_VALUE = "\nbut nothing found.";

        private readonly int keyUsage;

        private readonly bool resultOnMissingExtension;

        private String messagePreAmble;

        private String message;

        /// <summary>
        /// Create new
        /// <see cref="KeyUsageExtension"/>
        /// instance using provided
        /// <c>int</c>
        /// flag.
        /// </summary>
        /// <param name="keyUsage">
        /// 
        /// <c>int</c>
        /// flag which represents bit values for key usage value
        /// bit strings are stored with the big-endian byte order and padding on the end,
        /// the big endian notation causes a shift in actual integer values for
        /// bits 1-8 becoming 0-7 and bit 1
        /// the 7 bits padding makes for bit 0 to become bit 7 of the first byte
        /// </param>
        public KeyUsageExtension(int keyUsage)
            : this(keyUsage, false) {
        }

        /// <summary>
        /// Create new
        /// <see cref="KeyUsageExtension"/>
        /// instance using provided
        /// <c>int</c>
        /// flag.
        /// </summary>
        /// <param name="keyUsage">
        /// 
        /// <c>int</c>
        /// flag which represents bit values for key usage value
        /// bit strings are stored with the big-endian byte order and padding on the end,
        /// the big endian notation causes a shift in actual integer values for bits 1-8
        /// becoming 0-7 and bit 1
        /// the 7 bits padding makes for bit 0 to become bit 7 of the first byte
        /// </param>
        /// <param name="resultOnMissingExtension">
        /// parameter which represents return value for
        /// <see cref="ExistsInCertificate(iText.Commons.Bouncycastle.Cert.IX509Certificate)"/>
        /// method in case of the extension not being present in a certificate
        /// </param>
        public KeyUsageExtension(int keyUsage, bool resultOnMissingExtension)
            : base(OID.X509Extensions.KEY_USAGE, FACTORY.CreateKeyUsage(keyUsage).ToASN1Primitive()) {
            this.keyUsage = keyUsage;
            this.resultOnMissingExtension = resultOnMissingExtension;
            messagePreAmble = MessageFormatUtil.Format(EXPECTED_VALUE, ConvertKeyUsageMaskToString(keyUsage));
            message = messagePreAmble;
        }

        /// <summary>
        /// Create new
        /// <see cref="KeyUsageExtension"/>
        /// instance using provided key usage enum list.
        /// </summary>
        /// <param name="keyUsages">
        /// key usages
        /// <see cref="System.Collections.IList{E}"/>
        /// which represents key usage values
        /// </param>
        public KeyUsageExtension(IList<KeyUsage> keyUsages)
            : this(keyUsages, false) {
        }

        /// <summary>
        /// Create new
        /// <see cref="KeyUsageExtension"/>
        /// instance using provided key usage enum list.
        /// </summary>
        /// <param name="keyUsages">
        /// key usages
        /// <see cref="System.Collections.IList{E}"/>
        /// which represents key usage values
        /// </param>
        /// <param name="resultOnMissingExtension">
        /// parameter which represents return value for
        /// <see cref="ExistsInCertificate(iText.Commons.Bouncycastle.Cert.IX509Certificate)"/>
        /// method in case of the extension not being present in a certificate
        /// </param>
        public KeyUsageExtension(IList<KeyUsage> keyUsages, bool resultOnMissingExtension)
            : this(ConvertKeyUsageSetToInt(keyUsages), resultOnMissingExtension) {
        }

        /// <summary>
        /// Create new
        /// <see cref="KeyUsageExtension"/>
        /// instance using provided single key usage enum value.
        /// </summary>
        /// <param name="keyUsageValue">
        /// 
        /// <see cref="KeyUsage"/>
        /// which represents single key usage enum value
        /// </param>
        public KeyUsageExtension(KeyUsage keyUsageValue)
            : this(JavaCollectionsUtil.SingletonList(keyUsageValue), false) {
        }

        /// <summary>
        /// Create new
        /// <see cref="KeyUsageExtension"/>
        /// instance using provided single key usage enum value.
        /// </summary>
        /// <param name="keyUsageValue">
        /// 
        /// <see cref="KeyUsage"/>
        /// which represents single key usage enum value
        /// </param>
        /// <param name="resultOnMissingExtension">
        /// parameter which represents return value for
        /// <see cref="ExistsInCertificate(iText.Commons.Bouncycastle.Cert.IX509Certificate)"/>
        /// method in case of the extension not being present in a certificate
        /// </param>
        public KeyUsageExtension(KeyUsage keyUsageValue, bool resultOnMissingExtension)
            : this(JavaCollectionsUtil.SingletonList(keyUsageValue), resultOnMissingExtension) {
        }

        /// <summary>Check if this extension is present in the provided certificate.</summary>
        /// <remarks>
        /// Check if this extension is present in the provided certificate. In case of
        /// <see cref="KeyUsageExtension"/>
        /// ,
        /// check if this key usage bit values are present in certificate. Other values may be present as well.
        /// </remarks>
        /// <param name="certificate">
        /// 
        /// <see cref="iText.Commons.Bouncycastle.Cert.IX509Certificate"/>
        /// in which this extension shall be present
        /// </param>
        /// <returns>
        /// 
        /// <see langword="true"/>
        /// if this key usage bit values are present in certificate,
        /// <see langword="false"/>
        /// otherwise
        /// </returns>
        public override bool ExistsInCertificate(IX509Certificate certificate) {
            bool[] providedKeyUsageFlags = certificate.GetKeyUsage();
            if (providedKeyUsageFlags == null) {
                // By default, we want to return true if extension is not specified. Configurable.
                message = messagePreAmble + MISSING_VALUE;
                return resultOnMissingExtension;
            }
            int bitmap = 0;
            // bit strings are stored with the big-endian byte order and padding on the end,
            // the big endian notation causes a shift in actual integer values for bits 1-8 becoming 0-7 and bit 1
            // the 7 bits padding makes for bit 0 to become bit 7 of the first byte
            for (int i = 0; i < providedKeyUsageFlags.Length - 1; ++i) {
                if (providedKeyUsageFlags[i]) {
                    bitmap += 1 << (8 - i - 1);
                }
            }
            if (providedKeyUsageFlags[8]) {
                bitmap += 0x8000;
            }
            if ((bitmap & keyUsage) != keyUsage) {
                message = new StringBuilder(messagePreAmble).Append(MessageFormatUtil.Format(ACTUAL_VALUE, ConvertKeyUsageMaskToString
                    (bitmap))).ToString();
                return false;
            }
            return true;
        }

        public override String GetMessage() {
            return message;
        }

        private static String ConvertKeyUsageMaskToString(int keyUsageMask) {
            StringBuilder result = new StringBuilder();
            String separator = "";
            // bit strings are stored with the big-endian byte order and padding on the end,
            // the big endian notation causes a shift in actual integer values for bits 1-8 becoming 0-7 and bit 1
            // the 7 bits padding makes for bit 0 to become bit 7 of the first byte
            foreach (KeyUsage usage in EnumUtil.GetAllValuesOfEnum<KeyUsage>()) {
                if (((1 << (8 - (int)(usage) - 1)) & keyUsageMask) > 0 || (usage == KeyUsage.DECIPHER_ONLY && (keyUsageMask
                     & 0x8000) == 0x8000)) {
                    result.Append(separator).Append(usage);
                    separator = ", ";
                }
            }
            return result.ToString();
        }

        private static int ConvertKeyUsageSetToInt(IEnumerable<KeyUsage> keyUsages) {
            int keyUsageMask = 0;
            // bit strings are stored with the big-endian byte order and padding on the end,
            // the big endian notation causes a shift in actual integer values for bits 1-8 becoming 0-7 and bit 1
            // the 7 bits padding makes for bit 0 to become bit 7 of the first byte
            foreach (KeyUsage usage in keyUsages) {
                if (usage == KeyUsage.DECIPHER_ONLY) {
                    keyUsageMask += 0x8000;
                    continue;
                }
                keyUsageMask += 1 << (8 - (int)(usage) - 1);
            }
            return keyUsageMask;
        }
    }
}
