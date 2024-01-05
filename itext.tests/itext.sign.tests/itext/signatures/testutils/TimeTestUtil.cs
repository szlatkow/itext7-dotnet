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

namespace iText.Signatures.Testutils {
    public class TimeTestUtil {
        private const int MILLIS_IN_DAY = 86_400_000;

        // This method is used to trim the hours of the day, so that two dates could be compared
        // with a day accuracy. We need such a method since in .NET the signing DateTime extracted
        // from the signature depends on the current time zone set on the machine.
        // TODO DEVSIX-5812 Remove the method
        public static long GetFullDaysMillis(double millis) {
            return (long)millis / MILLIS_IN_DAY;
        }

        /// <summary>A date time value to be used in test instead of current date time to get consistent results</summary>
        public static DateTime TEST_DATE_TIME = new DateTime(2000, 2, 14, 14, 14, 2, DateTimeKind.Utc);
    }
}
