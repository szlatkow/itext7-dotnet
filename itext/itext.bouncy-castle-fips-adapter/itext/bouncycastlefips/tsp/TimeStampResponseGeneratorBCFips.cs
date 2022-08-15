/*
This file is part of the iText (R) project.
Copyright (c) 1998-2022 iText Group NV
Authors: iText Software.

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
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Tsp;
using iText.Commons.Bouncycastle.Tsp;
using iText.Commons.Utils;

namespace iText.Bouncycastlefips.Tsp {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Tsp.TimeStampResponseGenerator"/>.
    /// </summary>
    public class TimeStampResponseGeneratorBCFips : ITimeStampResponseGenerator {
        private readonly TimeStampResponseGenerator timeStampResponseGenerator;

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Tsp.TimeStampResponseGenerator"/>.
        /// </summary>
        /// <param name="timeStampResponseGenerator">
        /// 
        /// <see cref="Org.BouncyCastle.Tsp.TimeStampResponseGenerator"/>
        /// to be wrapped
        /// </param>
        public TimeStampResponseGeneratorBCFips(TimeStampResponseGenerator timeStampResponseGenerator) {
            this.timeStampResponseGenerator = timeStampResponseGenerator;
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Tsp.TimeStampResponseGenerator"/>.
        /// </summary>
        /// <param name="tokenGenerator">TimeStampTokenGenerator wrapper</param>
        /// <param name="algorithms">set of algorithm strings</param>
        public TimeStampResponseGeneratorBCFips(ITimeStampTokenGenerator tokenGenerator, ICollection<String> algorithms
            )
            : this(new TimeStampResponseGenerator(((TimeStampTokenGeneratorBCFips)tokenGenerator).GetTimeStampTokenGenerator
                (), algorithms)) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Tsp.TimeStampResponseGenerator"/>.
        /// </returns>
        public virtual TimeStampResponseGenerator GetTimeStampResponseGenerator() {
            return timeStampResponseGenerator;
        }

        /// <summary><inheritDoc/></summary>
        public virtual ITimeStampResponse Generate(ITimeStampRequest request, BigInteger bigInteger, DateTime date
            ) {
            try {
                return new TimeStampResponseBCFips(timeStampResponseGenerator.Generate(((TimeStampRequestBCFips)request).GetTimeStampRequest
                    (), bigInteger, date));
            }
            catch (TspException e) {
                throw new TSPExceptionBCFips(e);
            }
        }

        /// <summary>Indicates whether some other object is "equal to" this one.</summary>
        /// <remarks>Indicates whether some other object is "equal to" this one. Compares wrapped objects.</remarks>
        public override bool Equals(Object o) {
            if (this == o) {
                return true;
            }
            if (o == null || GetType() != o.GetType()) {
                return false;
            }
            iText.Bouncycastlefips.Tsp.TimeStampResponseGeneratorBCFips that = (iText.Bouncycastlefips.Tsp.TimeStampResponseGeneratorBCFips
                )o;
            return Object.Equals(timeStampResponseGenerator, that.timeStampResponseGenerator);
        }

        /// <summary>Returns a hash code value based on the wrapped object.</summary>
        public override int GetHashCode() {
            return JavaUtil.ArraysHashCode(timeStampResponseGenerator);
        }

        /// <summary>
        /// Delegates
        /// <c>toString</c>
        /// method call to the wrapped object.
        /// </summary>
        public override String ToString() {
            return timeStampResponseGenerator.ToString();
        }
    }
}