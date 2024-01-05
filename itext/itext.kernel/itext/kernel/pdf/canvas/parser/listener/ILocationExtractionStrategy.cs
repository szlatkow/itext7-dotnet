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
using System.Collections.Generic;

namespace iText.Kernel.Pdf.Canvas.Parser.Listener {
    /// <summary>
    /// This is a special interface for
    /// <see cref="iText.Kernel.Pdf.Canvas.Parser.Filter.IEventFilter"/>
    /// that returns a collection of rectangles as result of its work.
    /// </summary>
    public interface ILocationExtractionStrategy : IEventListener {
        /// <summary>
        /// Returns the
        /// <see cref="iText.Kernel.Geom.Rectangle"/>
        /// s that have been processed so far.
        /// </summary>
        /// <returns>
        /// 
        /// <see cref="System.Collections.ICollection{E}"/>
        /// &lt;
        /// <see cref="IPdfTextLocation"/>
        /// &gt; instance with the current resultant IPdfTextLocations
        /// </returns>
        ICollection<IPdfTextLocation> GetResultantLocations();
    }
}
