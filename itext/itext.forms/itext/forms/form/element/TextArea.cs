/*
This file is part of the iText (R) project.
Copyright (c) 1998-2023 Apryse Group NV
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
using iText.Forms.Form;
using iText.Forms.Form.Renderer;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace iText.Forms.Form.Element {
    /// <summary>
    /// Extension of the
    /// <see cref="FormField{T}"/>
    /// class representing a button so that
    /// a
    /// <see cref="iText.Forms.Form.Renderer.TextAreaRenderer"/>
    /// is used instead of the default renderer for fields.
    /// </summary>
    public class TextArea : FormField<iText.Forms.Form.Element.TextArea>, IPlaceholderable {
        /// <summary>The placeholder paragraph.</summary>
        private Paragraph placeholder;

        /// <summary>
        /// Creates a new
        /// <see cref="TextArea"/>
        /// instance.
        /// </summary>
        /// <param name="id">the id</param>
        public TextArea(String id)
            : base(id) {
        }

        /* (non-Javadoc)
        * @see FormField#getDefaultProperty(int)
        */
        public override T1 GetDefaultProperty<T1>(int property) {
            switch (property) {
                case FormProperty.FORM_FIELD_ROWS: {
                    return (T1)(Object)2;
                }

                case FormProperty.FORM_FIELD_COLS: {
                    return (T1)(Object)20;
                }

                default: {
                    return base.GetDefaultProperty<T1>(property);
                }
            }
        }

        /// <summary><inheritDoc/></summary>
        public virtual Paragraph GetPlaceholder() {
            return placeholder;
        }

        /// <summary><inheritDoc/></summary>
        public virtual void SetPlaceholder(Paragraph placeholder) {
            this.placeholder = placeholder;
        }

        /* (non-Javadoc)
        * @see com.itextpdf.layout.element.AbstractElement#makeNewRenderer()
        */
        protected override IRenderer MakeNewRenderer() {
            return new TextAreaRenderer(this);
        }
    }
}
