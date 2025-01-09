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

namespace iText.IO.Font {
    public class FontIdentification {
        // name ID 5
        private String ttfVersion;

        // name ID 3
        private String ttfUniqueId;

        // /UniqueID
        private int? type1Xuid;

        // OS/2.panose
        private String panose;

        public virtual String GetTtfVersion() {
            return ttfVersion;
        }

        public virtual String GetTtfUniqueId() {
            return ttfUniqueId;
        }

        public virtual int? GetType1Xuid() {
            return type1Xuid;
        }

        public virtual String GetPanose() {
            return panose;
        }

        protected internal virtual void SetTtfVersion(String ttfVersion) {
            this.ttfVersion = ttfVersion;
        }

        protected internal virtual void SetTtfUniqueId(String ttfUniqueId) {
            this.ttfUniqueId = ttfUniqueId;
        }

        protected internal virtual void SetType1Xuid(int? type1Xuid) {
            this.type1Xuid = type1Xuid;
        }

        protected internal virtual void SetPanose(byte[] panose) {
            this.panose = iText.Commons.Utils.JavaUtil.GetStringForBytes(panose);
        }

        protected internal virtual void SetPanose(String panose) {
            this.panose = panose;
        }
    }
}
