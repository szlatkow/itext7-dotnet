/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using iText.Kernel.Colors.Gradients;
using iText.Kernel.Pdf.Xobject;

namespace iText.Layout.Properties {
    public class BackgroundImage {
        private static readonly BlendMode DEFAULT_BLEND_MODE = BlendMode.NORMAL;

        protected internal PdfXObject image;

        /// <summary>Whether the background repeats in the x dimension.</summary>
        [System.ObsoleteAttribute(@"Replace this field with BackgroundRepeat instance.")]
        protected internal bool repeatX;

        /// <summary>Whether the background repeats in the y dimension.</summary>
        [System.ObsoleteAttribute(@"Replace this field with BackgroundRepeat instance.")]
        protected internal bool repeatY;

        protected internal AbstractLinearGradientBuilder linearGradientBuilder;

        private BlendMode blendMode = DEFAULT_BLEND_MODE;

        private BackgroundPosition position;

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeat">
        /// background-repeat property.
        /// <see cref="BackgroundRepeat"/>
        /// instance.
        /// </param>
        /// <param name="position">
        /// background-position property.
        /// <see cref="BackgroundPosition"/>
        /// instance.
        /// </param>
        /// <param name="linearGradientBuilder">
        /// background-image property.
        /// <see cref="iText.Kernel.Colors.Gradients.AbstractLinearGradientBuilder"/>
        /// instance.
        /// </param>
        /// <param name="blendMode">
        /// the image's blend mode.
        /// <see cref="BlendMode"/>
        /// instance.
        /// </param>
        protected internal BackgroundImage(PdfXObject image, BackgroundRepeat repeat, BackgroundPosition position, 
            AbstractLinearGradientBuilder linearGradientBuilder, BlendMode blendMode) {
            this.image = image;
            this.repeatX = repeat.IsRepeatX();
            this.repeatY = repeat.IsRepeatY();
            this.position = position;
            this.linearGradientBuilder = linearGradientBuilder;
            if (blendMode != null) {
                this.blendMode = blendMode;
            }
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfImageXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeat">
        /// background repeat property.
        /// <see cref="BackgroundRepeat"/>
        /// instance.
        /// </param>
        /// <param name="blendMode">
        /// the image's blend mode.
        /// <see cref="BlendMode"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfImageXObject image, BackgroundRepeat repeat, BlendMode blendMode)
            : this(image, repeat, new BackgroundPosition(), null, blendMode) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfFormXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeat">
        /// background repeat property.
        /// <see cref="BackgroundRepeat"/>
        /// instance.
        /// </param>
        /// <param name="blendMode">
        /// the image's blend mode.
        /// <see cref="BlendMode"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfFormXObject image, BackgroundRepeat repeat, BlendMode blendMode)
            : this(image, repeat, new BackgroundPosition(), null, blendMode) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfImageXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeat">
        /// background-repeat property.
        /// <see cref="BackgroundRepeat"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfImageXObject image, BackgroundRepeat repeat)
            : this(image, repeat, new BackgroundPosition(), null, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfFormXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeat">
        /// background-repeat property.
        /// <see cref="BackgroundRepeat"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfFormXObject image, BackgroundRepeat repeat)
            : this(image, repeat, new BackgroundPosition(), null, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfImageXObject"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfImageXObject image)
            : this(image, new BackgroundRepeat(), new BackgroundPosition(), null, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfFormXObject"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfFormXObject image)
            : this(image, new BackgroundRepeat(), new BackgroundPosition(), null, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfImageXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeatX">defines whether background is repeated in x dimension.</param>
        /// <param name="repeatY">defines whether background is repeated in y dimension.</param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfImageXObject image, bool repeatX, bool repeatY)
            : this(image, new BackgroundRepeat(repeatX, repeatY), new BackgroundPosition(), null, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance.
        /// </summary>
        /// <param name="image">
        /// background-image property.
        /// <see cref="iText.Kernel.Pdf.Xobject.PdfFormXObject"/>
        /// instance.
        /// </param>
        /// <param name="repeatX">defines whether background is repeated in x dimension.</param>
        /// <param name="repeatY">defines whether background is repeated in y dimension.</param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(PdfFormXObject image, bool repeatX, bool repeatY)
            : this(image, new BackgroundRepeat(repeatX, repeatY), new BackgroundPosition(), null, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance with linear gradient.
        /// </summary>
        /// <param name="linearGradientBuilder">
        /// the linear gradient builder representing the background image.
        /// <see cref="iText.Kernel.Colors.Gradients.AbstractLinearGradientBuilder"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(AbstractLinearGradientBuilder linearGradientBuilder)
            : this(linearGradientBuilder, DEFAULT_BLEND_MODE) {
        }

        /// <summary>
        /// Creates a new
        /// <see cref="BackgroundImage"/>
        /// instance with linear gradient and custom blending mode.
        /// </summary>
        /// <param name="linearGradientBuilder">
        /// the linear gradient builder representing the background image.
        /// <see cref="iText.Kernel.Colors.Gradients.AbstractLinearGradientBuilder"/>
        /// instance.
        /// </param>
        /// <param name="blendMode">
        /// the image's blend mode.
        /// <see cref="BlendMode"/>
        /// instance.
        /// </param>
        [System.ObsoleteAttribute(@"Remove this constructor in 7.2.")]
        public BackgroundImage(AbstractLinearGradientBuilder linearGradientBuilder, BlendMode blendMode)
            : this(null, new BackgroundRepeat(false, false), new BackgroundPosition(), linearGradientBuilder, blendMode
                ) {
        }

        public virtual PdfImageXObject GetImage() {
            return image is PdfImageXObject ? (PdfImageXObject)image : null;
        }

        public virtual PdfFormXObject GetForm() {
            return image is PdfFormXObject ? (PdfFormXObject)image : null;
        }

        /// <summary>Gets background-position.</summary>
        /// <returns>
        /// 
        /// <see cref="BackgroundPosition"/>
        /// </returns>
        public virtual BackgroundPosition GetBackgroundPosition() {
            return position;
        }

        public virtual AbstractLinearGradientBuilder GetLinearGradientBuilder() {
            return this.linearGradientBuilder;
        }

        public virtual bool IsBackgroundSpecified() {
            return image is PdfFormXObject || image is PdfImageXObject || linearGradientBuilder != null;
        }

        public virtual bool IsRepeatX() {
            return repeatX;
        }

        public virtual bool IsRepeatY() {
            return repeatY;
        }

        public virtual float GetWidth() {
            return (float)image.GetWidth();
        }

        public virtual float GetHeight() {
            return (float)image.GetHeight();
        }

        /// <summary>Get the image's blend mode.</summary>
        /// <returns>
        /// the
        /// <see cref="BlendMode"/>
        /// representation of the image's blend mode
        /// </returns>
        public virtual BlendMode GetBlendMode() {
            return blendMode;
        }

        /// <summary>
        /// <see cref="BackgroundImage"/>
        /// builder class.
        /// </summary>
        public class Builder {
            private PdfXObject image;

            private AbstractLinearGradientBuilder linearGradientBuilder;

            private BackgroundPosition position = new BackgroundPosition();

            private BackgroundRepeat repeat = new BackgroundRepeat();

            private BlendMode blendMode = DEFAULT_BLEND_MODE;

            public Builder() {
            }

            /// <summary>Sets image.</summary>
            /// <remarks>
            /// Sets image.
            /// <para />
            /// Makes linearGradientBuilder null as far as we can't have them both.
            /// </remarks>
            /// <param name="image">
            /// 
            /// <see cref="iText.Kernel.Pdf.Xobject.PdfXObject"/>
            /// to be set.
            /// </param>
            /// <returns>
            /// this
            /// <see cref="Builder"/>.
            /// </returns>
            public virtual BackgroundImage.Builder SetImage(PdfXObject image) {
                this.image = image;
                this.linearGradientBuilder = null;
                return this;
            }

            /// <summary>Sets linearGradientBuilder.</summary>
            /// <remarks>
            /// Sets linearGradientBuilder.
            /// <para />
            /// Makes image null as far as we can't have them both. It also makes background-repeat: no-repeat.
            /// </remarks>
            /// <param name="linearGradientBuilder">
            /// 
            /// <see cref="iText.Kernel.Colors.Gradients.AbstractLinearGradientBuilder"/>
            /// to be set.
            /// </param>
            /// <returns>
            /// this
            /// <see cref="Builder"/>.
            /// </returns>
            public virtual BackgroundImage.Builder SetLinearGradientBuilder(AbstractLinearGradientBuilder linearGradientBuilder
                ) {
                this.linearGradientBuilder = linearGradientBuilder;
                this.repeat = new BackgroundRepeat(false, false);
                this.image = null;
                return this;
            }

            /// <summary>Sets background-repeat.</summary>
            /// <param name="repeat">
            /// 
            /// <see cref="BackgroundRepeat"/>
            /// to be set.
            /// </param>
            /// <returns>
            /// this
            /// <see cref="Builder"/>.
            /// </returns>
            public virtual BackgroundImage.Builder SetBackgroundRepeat(BackgroundRepeat repeat) {
                this.repeat = repeat;
                return this;
            }

            /// <summary>Sets background-position.</summary>
            /// <param name="position">
            /// 
            /// <see cref="BackgroundPosition"/>
            /// to be set.
            /// </param>
            /// <returns>
            /// this
            /// <see cref="Builder"/>.
            /// </returns>
            public virtual BackgroundImage.Builder SetBackgroundPosition(BackgroundPosition position) {
                this.position = position;
                return this;
            }

            /// <summary>Set the image's blend mode.</summary>
            /// <param name="blendMode">
            /// 
            /// <see cref="BlendMode"/>
            /// to be set.
            /// </param>
            /// <returns>
            /// this
            /// <see cref="Builder"/>.
            /// </returns>
            public virtual BackgroundImage.Builder SetBackgroundBlendMode(BlendMode blendMode) {
                if (blendMode != null) {
                    this.blendMode = blendMode;
                }
                return this;
            }

            /// <summary>
            /// Builds new
            /// <see cref="BackgroundImage"/>
            /// using set fields.
            /// </summary>
            /// <returns>
            /// new
            /// <see cref="BackgroundImage"/>.
            /// </returns>
            public virtual BackgroundImage Build() {
                return new BackgroundImage(image, repeat, position, linearGradientBuilder, blendMode);
            }
        }
    }
}
