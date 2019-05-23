using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdfFactory
{
    public abstract class HtmlConverterFactory
    {
        public abstract IFormatter Formatter { get; set; }
        public abstract IConvertedPdf ConvertToPdf();
    }
}
