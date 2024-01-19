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
using iText.Commons.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Fips;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Utilities.IO;
using ICipher = iText.Commons.Bouncycastle.Crypto.ICipher;

namespace iText.Bouncycastlefips.Crypto {
    /// <summary>
    /// Wrapper class for ICipher.
    /// </summary>
    public class CipherBCFips : ICipher {
        private readonly Org.BouncyCastle.Crypto.ICipher cipher;
        private MemoryOutputStream memoryStream = new MemoryOutputStream();
        private long lastPos = 0;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Crypto.ICipher"/>.
        /// </summary>
        /// <param name="cipher">
        /// <see cref="Org.BouncyCastle.Crypto.ICipher"/> to be wrapped
        /// </param>
        public CipherBCFips(Org.BouncyCastle.Crypto.ICipher cipher) {
            this.cipher = cipher;
        }
        
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Crypto.ICipher"/>.
        /// </summary>
        /// <param name="forEncryption">boolean value</param>
        /// <param name="key">byte array</param>
        /// <param name="iv">init vector</param>
        public CipherBCFips(bool forEncryption, byte[] key, byte[] iv) {
            FipsAes.Key aesKey = new FipsAes.Key(key);
            IBlockCipherService provider = CryptoServicesRegistrar.CreateService(aesKey);
            IBlockCipherBuilder<IParameters<Algorithm>> cipherBuilder = forEncryption ?
                provider.CreateBlockEncryptorBuilder(FipsAes.Cbc.WithIV(iv)) : provider.CreateBlockDecryptorBuilder(FipsAes.Cbc.WithIV(iv));
            cipher = cipherBuilder.BuildPaddedCipher(memoryStream, new Pkcs7Padding());
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped ICipher<IBlockResult>.
        /// </returns>
        public virtual Org.BouncyCastle.Crypto.ICipher GetICipher() {
            return cipher;
        }

        /// <summary><inheritDoc/></summary>
        public byte[] Update(byte[] inp, int inpOff, int inpLen) {
            cipher.Stream.Write(inp, inpOff, inpLen);
            return GetBytes();
        }

        /// <summary><inheritDoc/></summary>
        public byte[] DoFinal() {
            int neededLen = cipher.GetMaxOutputSize(0);
            byte[] outp = new byte[neededLen];
            try {
                cipher.Stream.Close();
            } catch (Exception) {
                return outp;
            }
            return GetBytes();
        }
        
        /// <summary>Indicates whether some other object is "equal to" this one. Compares wrapped objects.</summary>
        public override bool Equals(Object o) {
            if (this == o) {
                return true;
            }
            if (o == null || GetType() != o.GetType()) {
                return false;
            }
            CipherBCFips that = (CipherBCFips)o;
            return Object.Equals(cipher, that.cipher) &&
                   Object.Equals(memoryStream, that.memoryStream);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode<object>(cipher, memoryStream);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return cipher.ToString();
        }

        private byte[] GetBytes() {
            byte[] bytes = memoryStream.ToArray();
            long len = bytes.Length - lastPos;
            byte[] res = new byte[len];
            Array.Copy(bytes, lastPos, res, 0, len);
            lastPos = bytes.Length;
            return res;
        }
    }
}
