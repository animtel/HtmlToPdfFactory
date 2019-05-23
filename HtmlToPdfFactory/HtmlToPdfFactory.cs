using HtmlAgilityPack;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdfFactory
{
    public class HtmlToPdfFactory : HtmlConverterFactory
    {
        public string Path { get; set; }
        public override IFormatter Formatter { get; set ; }

        public HtmlToPdfFactory(string path, IFormatter formatter)
        {
            Path = path;
            Formatter = formatter;
        }

        public override IConvertedPdf ConvertToPdf()
        {
            var html = Formatter.Format(Path);
            var lastPageHtml = GetHtmlWithTag(html, "last-page-body");
            var contentHtml = GetHtmlWithTag(html, "content-body");

            var pdfs = GetPdfsArrays(contentHtml, lastPageHtml);
            var mergedPdfs = MergePdfs(pdfs);

            return new ConvertedPdf(mergedPdfs);
        }

        private byte[][] GetPdfsArrays(params string[] htmls)
        {
            List<byte[]> result = new List<byte[]>();

            foreach (var html in htmls)
            {
                using (MemoryStream pdfContentStream = new MemoryStream())
                using (PdfWriter pdfContentWr = new PdfWriter(pdfContentStream))
                {
                    var contentDocum = HtmlConverter.ConvertToDocument(html, pdfContentWr);
                    contentDocum.SetMargins(0, 0, 0, 0);
                    contentDocum.Close();
                    result.Add(pdfContentStream.ToArray());
                }
            }

            return result.ToArray();
        }

        private byte[] MergePdfs(params byte[][] pdfs)
        {
            using (var content = new MemoryStream())
            using (var contentWriter = new PdfWriter(content))
            using (PdfDocument contentDoc = new PdfDocument(contentWriter))
            {
                foreach (var byteArr in pdfs)
                {
                    using (var contentOfByteArr = new MemoryStream(byteArr))
                    using (var contentOfByteArrReader = new PdfReader(contentOfByteArr))
                    using (PdfDocument contentOfByteArrDoc = new PdfDocument(contentOfByteArrReader))
                    {
                        var amountOfPages = contentOfByteArrDoc.GetNumberOfPages();
                        contentOfByteArrDoc.CopyPagesTo(1, amountOfPages, contentDoc);
                    }
                }
                contentDoc.Close();
                return content.ToArray();
            }
        }

        private string GetHtmlWithTag(string html, string tag)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var tagNodes = document.DocumentNode.Descendants("body")
                .FirstOrDefault()
                .ChildNodes
                .Where(x => x.Name != tag)
                .ToList();

            foreach (var node in tagNodes)
                node.Remove();

            var result = document.DocumentNode.OuterHtml;
            return result;
        }
    }
}
