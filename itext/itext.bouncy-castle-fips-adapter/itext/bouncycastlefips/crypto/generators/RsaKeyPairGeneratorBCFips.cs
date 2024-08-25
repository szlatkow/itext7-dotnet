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
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Crypto.Generators;
using iText.Commons.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Fips;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace iText.Bouncycastlefips.Crypto.Generators {
    /// <summary>
    /// Wrapper class for RSA KeyPairGenerator.
    /// </summary>
    public class RsaKeyPairGeneratorBCFips : IRsaKeyPairGenerator {
        private readonly FipsRsa.KeyPairGenerator generator;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="FipsRsa.KeyPairGenerator"/>.
        /// </summary>
        public RsaKeyPairGeneratorBCFips() {
            byte[] personalizationString = Strings.ToUtf8ByteArray("some personalization string");
            SecureRandom entropySource = new SecureRandom();
            SecureRandom secureRandomForGenerator = CryptoServicesRegistrar.CreateService(FipsDrbg.Sha512)
                .FromEntropySource(entropySource,true)
                .SetPersonalizationString(personalizationString).Build(
                    entropySource.GenerateSeed(256 / (2 * 8)), true, 
                    Strings.ToByteArray("number only used once"));
            this.generator = CryptoServicesRegistrar.CreateGenerator(
                new FipsRsa.KeyGenerationParameters(BigInteger.ValueOf(0x10001), 2048), 
                secureRandomForGenerator);
        }
        
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="FipsRsa.KeyPairGenerator"/>.
        /// </summary>
        /// <param name="generator">
        /// <see cref="FipsRsa.KeyPairGenerator"/> to be wrapped
        /// </param>
        public RsaKeyPairGeneratorBCFips(FipsRsa.KeyPairGenerator generator) {
            this.generator = generator;
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped FipsRsa.KeyPairGenerator<IBlockResult>.
        /// </returns>
        public FipsRsa.KeyPairGenerator GetGenerator() {
            return generator;
        }

        /// <summary><inheritDoc/></summary>
        public IAsymmetricCipherKeyPair GenerateKeyPair() {
            return new AsymmetricCipherKeyPairBCFips(generator.GenerateKeyPair());
        }
        
        /// <summary>Indicates whether some other object is "equal to" this one. Compares wrapped objects.</summary>
        public override bool Equals(Object o) {
            if (this == o) {
                return true;
            }
            if (o == null || GetType() != o.GetType()) {
                return false;
            }
            RsaKeyPairGeneratorBCFips that = (RsaKeyPairGeneratorBCFips)o;
            return Object.Equals(generator, that.generator);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(generator);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return generator.ToString();
        }
    }
}
