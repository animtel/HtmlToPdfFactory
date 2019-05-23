using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdfFactory
{
    public class ConvertedPdf: IConvertedPdf
    {
        public byte[] PdfBytes { get; set; }

        public ConvertedPdf(byte[] pdfBytes)
        {
            PdfBytes = pdfBytes;
        }
    }
}
