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
using iText.Commons.Utils;
using iText.Layout.Properties;

namespace iText.Layout.Renderer {
    internal class BottomToTopFlexItemMainDirector : FlexColumnItemMainDirector {
        internal BottomToTopFlexItemMainDirector() {
        }

        /// <summary><inheritDoc/></summary>
        public override void ApplyDirectionForLine<T>(IList<T> renderers) {
            JavaCollectionsUtil.Reverse(renderers);
        }

        public override void ApplyJustifyContent(IList<FlexUtil.FlexItemCalculationInfo> line, JustifyContent justifyContent
            , float freeSpace) {
            switch (justifyContent) {
                case JustifyContent.NORMAL:
                case JustifyContent.END:
                case JustifyContent.SELF_END:
                case JustifyContent.STRETCH:
                case JustifyContent.SELF_START:
                case JustifyContent.START:
                case JustifyContent.FLEX_START: {
                    line[line.Count - 1].yShift = freeSpace;
                    break;
                }

                case JustifyContent.CENTER: {
                    line[line.Count - 1].yShift = freeSpace / 2;
                    break;
                }

                case JustifyContent.FLEX_END:
                case JustifyContent.LEFT:
                case JustifyContent.RIGHT:
                default: {
                    break;
                }
            }
        }
        // We don't need to do anything in these cases
    }
}
