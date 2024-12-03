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
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using iText.Commons;
using iText.Commons.Utils;
using iText.Kernel.Geom;
using iText.StyledXmlParser.Css.Util;
using iText.Svg;
using iText.Svg.Logs;
using iText.Svg.Renderers;
using iText.Svg.Renderers.Impl;

namespace iText.Svg.Utils {
    /// <summary>Utility class that facilitates parsing values from CSS.</summary>
    public sealed class SvgCssUtils {
        // TODO DEVSIX-2266
        private static readonly ILogger LOGGER = ITextLogManager.GetLogger(typeof(iText.Svg.Utils.SvgCssUtils));

        private SvgCssUtils() {
        }

        /// <summary>Splits a given String into a list of substrings.</summary>
        /// <remarks>
        /// Splits a given String into a list of substrings.
        /// The string is split up by commas and whitespace characters (\t, \n, \r, \f).
        /// </remarks>
        /// <param name="value">the string to be split</param>
        /// <returns>a list containing the split strings, an empty list if the value is null or empty</returns>
        public static IList<String> SplitValueList(String value) {
            IList<String> result = new List<String>();
            if (value != null && value.Length > 0) {
                value = value.Trim();
                String[] list = iText.Commons.Utils.StringUtil.Split(value, "[,|\\s]");
                foreach (String element in list) {
                    if (!String.IsNullOrEmpty(element)) {
                        result.Add(element);
                    }
                }
            }
            return result;
        }

        /// <summary>Converts a float to a String.</summary>
        /// <param name="value">to be converted float value</param>
        /// <returns>the value in a String representation</returns>
        public static String ConvertFloatToString(float value) {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>Converts a double to a String.</summary>
        /// <param name="value">to be converted double value</param>
        /// <returns>the value in a String representation</returns>
        public static String ConvertDoubleToString(double value) {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>Parse length attribute and convert it to an absolute value.</summary>
        /// <param name="svgNodeRenderer">renderer for which length should be parsed</param>
        /// <param name="length">
        /// 
        /// <see cref="System.String"/>
        /// for parsing
        /// </param>
        /// <param name="percentBaseValue">the value on which percent length is based on</param>
        /// <param name="defaultValue">default value if length is not recognized</param>
        /// <param name="context">
        /// current
        /// <see cref="iText.Svg.Renderers.SvgDrawContext"/>
        /// </param>
        /// <returns>absolute value in points</returns>
        public static float ParseAbsoluteLength(AbstractSvgNodeRenderer svgNodeRenderer, String length, float percentBaseValue
            , float defaultValue, SvgDrawContext context) {
            float em = svgNodeRenderer.GetCurrentFontSize(context);
            float rem = context.GetCssContext().GetRootFontSize();
            return CssDimensionParsingUtils.ParseLength(length, percentBaseValue, defaultValue, em, rem);
        }

        /// <summary>Extract svg viewbox values.</summary>
        /// <param name="svgRenderer">
        /// the
        /// <see cref="iText.Svg.Renderers.ISvgNodeRenderer"/>
        /// instance that contains
        /// the renderer tree
        /// </param>
        /// <returns>float[4] or null, if no correct viewbox property is present.</returns>
        public static float[] ParseViewBox(ISvgNodeRenderer svgRenderer) {
            String vbString = svgRenderer.GetAttribute(SvgConstants.Attributes.VIEWBOX);
            // TODO: DEVSIX-3923 remove normalization (.toLowerCase)
            if (vbString == null) {
                vbString = svgRenderer.GetAttribute(SvgConstants.Attributes.VIEWBOX.ToLowerInvariant());
            }
            float[] values = null;
            if (vbString != null) {
                IList<String> valueStrings = iText.Svg.Utils.SvgCssUtils.SplitValueList(vbString);
                values = new float[valueStrings.Count];
                for (int i = 0; i < values.Length; i++) {
                    values[i] = CssDimensionParsingUtils.ParseAbsoluteLength(valueStrings[i]);
                }
            }
            if (values != null) {
                // the value for viewBox should be 4 numbers according to the viewBox documentation
                if (values.Length != SvgConstants.Values.VIEWBOX_VALUES_NUMBER) {
                    if (LOGGER.IsEnabled(LogLevel.Warning)) {
                        LOGGER.LogWarning(MessageFormatUtil.Format(SvgLogMessageConstant.VIEWBOX_VALUE_MUST_BE_FOUR_NUMBERS, vbString
                            ));
                    }
                    return null;
                }
                // in case when viewBox width or height is negative value is an error and
                // invalidates the ‘viewBox’ attribute (according to the viewBox documentation)
                if (values[2] < 0 || values[3] < 0) {
                    if (LOGGER.IsEnabled(LogLevel.Warning)) {
                        LOGGER.LogWarning(MessageFormatUtil.Format(SvgLogMessageConstant.VIEWBOX_WIDTH_AND_HEIGHT_CANNOT_BE_NEGATIVE
                            , vbString));
                    }
                    return null;
                }
            }
            return values;
        }

        /// <summary>
        /// Extract width and height of the passed SVGNodeRenderer,
        /// defaulting to respective viewbox values if either one is not present or
        /// to browser default if viewbox is missing as well.
        /// </summary>
        /// <param name="svgRenderer">
        /// the
        /// <see cref="iText.Svg.Renderers.ISvgNodeRenderer"/>
        /// instance that contains
        /// the renderer tree
        /// </param>
        /// <param name="em">em value in pt</param>
        /// <param name="rem">rem value in pt</param>
        /// <returns>Rectangle, where x,y = 0 and width and height are extracted ones by this method.</returns>
        public static Rectangle ExtractWidthAndHeight(ISvgNodeRenderer svgRenderer, float em, float rem) {
            float[] values = iText.Svg.Utils.SvgCssUtils.ParseViewBox(svgRenderer);
            float defaultWidth = values == null ? CssDimensionParsingUtils.ParseAbsoluteLength(SvgConstants.Values.DEFAULT_VIEWBOX_WIDTH
                ) : values[2];
            float defaultHeight = values == null ? CssDimensionParsingUtils.ParseAbsoluteLength(SvgConstants.Values.DEFAULT_VIEWBOX_HEIGHT
                ) : values[3];
            Rectangle result = new Rectangle(defaultWidth, defaultHeight);
            String width = svgRenderer.GetAttribute(SvgConstants.Attributes.WIDTH);
            if (CssTypesValidationUtils.IsRemValue(width)) {
                result.SetWidth(CssDimensionParsingUtils.ParseRelativeValue(width, rem));
            }
            else {
                if (CssTypesValidationUtils.IsEmValue(width)) {
                    result.SetWidth(CssDimensionParsingUtils.ParseRelativeValue(width, em));
                }
                else {
                    if (width != null) {
                        result.SetWidth(CssDimensionParsingUtils.ParseAbsoluteLength(width));
                    }
                    else {
                        if (values == null) {
                            LOGGER.LogWarning(SvgLogMessageConstant.MISSING_WIDTH);
                        }
                    }
                }
            }
            String height = svgRenderer.GetAttribute(SvgConstants.Attributes.HEIGHT);
            if (CssTypesValidationUtils.IsRemValue(height)) {
                result.SetHeight(CssDimensionParsingUtils.ParseRelativeValue(height, rem));
            }
            else {
                if (CssTypesValidationUtils.IsEmValue(height)) {
                    result.SetHeight(CssDimensionParsingUtils.ParseRelativeValue(height, em));
                }
                else {
                    if (height != null) {
                        result.SetHeight(CssDimensionParsingUtils.ParseAbsoluteLength(height));
                    }
                    else {
                        if (values == null) {
                            LOGGER.LogWarning(SvgLogMessageConstant.MISSING_HEIGHT);
                        }
                    }
                }
            }
            return result;
        }
    }
}
