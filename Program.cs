using System;
using System.IO;
using System.Linq;
using ECertificateAPI;


namespace Zugferdt2pdf
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() >= 2)
            {
                string anyarg = args.FirstOrDefault();
                string pdfFile = "";
                string xmlFilePath = "";

                if (anyarg.ToUpper().Contains("PDF"))
                {
                    pdfFile = anyarg;
                }
                else if (anyarg.ToUpper().Contains("XML"))
                {
                    xmlFilePath = anyarg;
                }

                if (args.Count() >= 2)
                {
                    anyarg = args[1];
                    if (anyarg.ToUpper().Contains("PDF"))
                    {
                        pdfFile = anyarg;
                    }
                    else if (anyarg.ToUpper().Contains("XML"))
                    {
                        xmlFilePath = anyarg;
                    }
                }



                PDFA3Invoice inv = new PDFA3Invoice();
                string xmlFile = "factur-x.xml";
                string outputFile = pdfFile.Substring(0, pdfFile.Length - 4) + "_X.pdf";


                inv.CreatePDFA3Invoice(pdfFile, xmlFilePath, xmlFile, outputFile, "");


                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Bitte 2 Parameter (Rechnung als pdf und X-Rechnung als XML) übergeben");
                Environment.Exit(-1);
            }
        }
    }
}
