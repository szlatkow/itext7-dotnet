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
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Colorspace;

namespace iText.Kernel.Pdf.Function {
    public class PdfType4Function : AbstractPdfFunction<PdfStream> {
        public PdfType4Function(PdfStream dict)
            : base(dict) {
        }

        public PdfType4Function(double[] domain, double[] range, byte[] code)
            : base(new PdfStream(code), PdfFunctionFactory.FUNCTION_TYPE_4, domain, range) {
        }

        public PdfType4Function(float[] domain, float[] range, byte[] code)
            : this(ConvertFloatArrayToDoubleArray(domain), ConvertFloatArrayToDoubleArray(range), code) {
        }

        public override bool CheckCompatibilityWithColorSpace(PdfColorSpace alternateSpace) {
            return GetInputSize() == 1 && GetOutputSize() == alternateSpace.GetNumberOfComponents();
        }

        public override double[] Calculate(double[] input) {
            throw new NotSupportedException(KernelExceptionMessageConstant.TYPE4_EXECUTION_NOT_SUPPORTED);
        }
    }
}
