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
using iText.Layout.Font;
using iText.StyledXmlParser.Css.Media;
using iText.StyledXmlParser.Resolver.Resource;
using iText.Svg.Dummy.Factories;
using iText.Svg.Processors;
using iText.Svg.Renderers.Factories;

namespace iText.Svg.Dummy.Processors.Impl {
    public class DummySvgConverterProperties : ISvgConverterProperties {
        internal ISvgNodeRendererFactory rendererFactory;

        internal String baseUri;

        internal MediaDeviceDescription mediaDeviceDescription;

        internal IResourceRetriever resourceRetriever;

        public DummySvgConverterProperties() {
            rendererFactory = new DummySvgNodeFactory();
            mediaDeviceDescription = new MediaDeviceDescription("");
            baseUri = "";
            resourceRetriever = new DefaultResourceRetriever();
        }

        public virtual ISvgNodeRendererFactory GetRendererFactory() {
            return rendererFactory;
        }

        public virtual FontProvider GetFontProvider() {
            return null;
        }

        public virtual String GetCharset() {
            return null;
        }

        public virtual String GetBaseUri() {
            return baseUri;
        }

        public virtual iText.Svg.Dummy.Processors.Impl.DummySvgConverterProperties SetBaseUri(String baseUri) {
            return this;
        }

        public virtual MediaDeviceDescription GetMediaDeviceDescription() {
            return mediaDeviceDescription;
        }

        public virtual IResourceRetriever GetResourceRetriever() {
            return resourceRetriever;
        }

        public virtual iText.Svg.Dummy.Processors.Impl.DummySvgConverterProperties SetMediaDeviceDescription(MediaDeviceDescription
             mediaDeviceDescription) {
            return this;
        }

        public virtual iText.Svg.Dummy.Processors.Impl.DummySvgConverterProperties SetFontProvider(FontProvider fontProvider
            ) {
            return this;
        }
    }
}
