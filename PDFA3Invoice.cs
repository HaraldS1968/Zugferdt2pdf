﻿
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ECertificateAPI
{
    class PDFA3Invoice
    {

        public void CreatePDFA3Invoice(string pdfFilePath, string xmlFilePath, string xmlFileName,
             string outputPath, string documentType)
        {
            // =========== Create PDF/A-3U Document =================
            // Create PDF/A-3U writer instance from existing document
            PdfReader reader = new PdfReader(pdfFilePath);
            MemoryStream stream = new MemoryStream();
            Document pdfAdocument = new Document();
            PdfAWriter writer = this.CreatePDFAInstance(pdfAdocument, reader, stream);

            PdfArray array = new PdfArray();
            writer.ExtraCatalog.Put(new PdfName("AF"), array);



            //============= Create Exchange Invoice =================
            // 1 add xml to document
            PdfFileSpecification contentSpec = this.EmbeddedAttachment(xmlFilePath, xmlFileName,
                    "text/xml", new PdfName("Alternative"), writer, "Tax Invoice XML Data");
            array.Add(contentSpec.Reference);

            pdfAdocument.Close();
            reader.Close();

            File.WriteAllBytes(outputPath, stream.ToArray());
        }


        public PdfAWriter CreatePDFAInstance(Document targetDocument, PdfReader originalDocument, Stream os)
        {
            PdfAWriter writer = PdfAWriter.GetInstance(targetDocument, os, PdfAConformanceLevel.PDF_A_3U);
            writer.CreateXmpMetadata();

            if (!targetDocument.IsOpen())
                targetDocument.Open();

            PdfContentByte cb = writer.DirectContent; // Holds the PDF data	
            PdfImportedPage page;
            int pageCount = originalDocument.NumberOfPages;
            for (int i = 0; i < pageCount; i++)
            {
                targetDocument.NewPage();
                page = writer.GetImportedPage(originalDocument, i + 1);
                cb.AddTemplate(page, 0, 0);
            }
            return writer;
        }
        public PdfFileSpecification EmbeddedAttachment(string filePath, string fileName, string mimeType,
            PdfName afRelationship, PdfAWriter writer, string description)
        {
            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.MODDATE, new PdfDate(File.GetLastWriteTime(filePath)));
            PdfFileSpecification fileSpec = PdfFileSpecification.FileEmbedded(writer, filePath, fileName, null, mimeType,
                    parameters, 0);
            fileSpec.Put(new PdfName("AFRelationship"), afRelationship);
            writer.AddFileAttachment(description, fileSpec);
            return fileSpec;
        }
    }
}
