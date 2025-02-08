using Spire.Doc;
using System.Drawing.Printing;
using Utilities;

namespace PrintService
{
    public class SpirePrintService : IPrintService
    {
        public Task Print(string printer, string filePath)
        {
            Document doc = new Document(filePath);

            PrintDocument printDoc = doc.PrintDocument;

            printDoc.PrinterSettings.PrinterName = printer;

            printDoc.Print();

            return Task.CompletedTask;
        }
    }
}
