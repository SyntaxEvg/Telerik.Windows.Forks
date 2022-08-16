using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.Model;

namespace testAllsyntax
{
    //class Program
    //{
    //    //public static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;
    //    //public static readonly string InputFileName = "te.docx.pdf";
    //    //public const string ResultFileName = "MergeResult.pdf";
    //    //public const int MergedDocumentPagesCount = 10000;

    //    //static void Main()
    //    //{
    //    //    if (File.Exists(ResultFileName))
    //    //    {
    //    //        File.Delete(ResultFileName);
    //    //    }

    //    //    using (PdfStreamWriter fileWriter = new PdfStreamWriter(File.OpenWrite(ResultFileName)))
    //    //    {
    //    //        fileWriter.Settings.DocumentInfo.Author = "Progress Software";
    //    //        fileWriter.Settings.DocumentInfo.Title = "Merged document";
    //    //        fileWriter.Settings.DocumentInfo.Description = "This big document is generated with PdfStreamWriter class in less than a second, with minimal memory footprint and optimized result file size.";

    //    //        using (PdfFileSource fileSource = new PdfFileSource(File.OpenRead(InputFileName)))
    //    //        {
    //    //            PdfPageSource pageToMerge = fileSource.Pages[0];

    //    //            for (int i = 0; i < MergedDocumentPagesCount; i++)
    //    //            {
    //    //                fileWriter.WritePage(pageToMerge);
    //    //            }
    //    //        }
    //    //    }

    //    //    Process.Start(ResultFileName);
    //    //}
    //}




    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "te.docx";
            string resultPath = "te.docx.pdf";
            ConverDocxToPdf(path, resultPath);
        }
        public static void ConverDocxToPdf(string path, string resultPath)
        {
            Telerik.Windows.Documents.Flow.FormatProviders.Docx.DocxFormatProvider provider = new Telerik.Windows.Documents.Flow.FormatProviders.Docx.DocxFormatProvider();

            using (PdfStreamWriter fileWriter = new PdfStreamWriter(File.OpenWrite(resultPath)))
            {
                fileWriter.Settings.DocumentInfo.Author = "Progress Software";
                fileWriter.Settings.DocumentInfo.Title = "Merged document";
                fileWriter.Settings.DocumentInfo.Description = "This big document is generated with PdfStreamWriter class in less than a second, with minimal memory footprint and optimized result file size.";
                RadFlowDocument document = provider.Import(File.OpenRead(path));
                //  fileWriter.



                //PdfPageSource pageToMerge = new PdfPageSource();

                //fileSource.Pages[0];

                //for (int i = 0; i < MergedDocumentPagesCount; i++)
                //{
                //    fileWriter.WritePage(pageToMerge);
                //}

            }

            var docxPRovider = new DocxFormatProvider();
            var pdfProvider = new PdfFormatProvider();











               using (Stream input = File.OpenRead(path))
            {
                RadFlowDocument document = provider.Import(input);
                var resultBytes = pdfProvider.Export(document);
                File.WriteAllBytes(resultPath, resultBytes);
            }

            //var docBytes = File.ReadAllBytes(path);
            //var document = docxPRovider.Import(docBytes);
            ////try
            ////{
            //    var resultBytes = pdfProvider.Export(document);
            //    File.WriteAllBytes(resultPath, resultBytes);
            ////}
            ////catch (Exception ex)
            ////{
            ////    Console.WriteLine(ex.Message);
            ////}
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
