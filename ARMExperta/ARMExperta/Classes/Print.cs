using System;
using System.Linq;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows;

namespace ARMExperta.Classes
{
    class Print
    {
        static Print print;
        PdfPTable table;
        PdfPCell cell;
        BaseFont baseFont; 
        iTextSharp.text.Font font;

        Print() { }

        public static Print GetPrint
        {
            get
            {
                if (print == null) print = new Print();
                MessageBox.Show("Объект печати выдан");
                return print;
            }
        }
  
        public void PrintDocument(TreeViewModal tree)
        {
            MessageBox.Show("Старт процедуры печати");
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            MessageBox.Show("Документ создан программно");
            PdfWriter.GetInstance(doc, new FileStream("pdfTables.pdf", FileMode.Create));
            MessageBox.Show("Документ создан физически");
            doc.Open();
            baseFont = BaseFont.CreateFont("C:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            foreach (TreeViewModal tvm in tree.Children)
            {
                table = new PdfPTable(2);
                cell = new PdfPCell(new Phrase(tvm.Naim, font));
                cell.Colspan = 2;
                cell.Border = 0;
                table.AddCell(cell);
                foreach (TreeViewModal child in tvm.Children) RecursOutput(child, "");
                if (tvm.ExpertOpinion > 50) cell = new PdfPCell(new Phrase("Критерий '" + tvm.Naim + "' удовлетворяет требованиям нормативных документов", font));
                else cell = new PdfPCell(new Phrase("Критерий '" + tvm.Naim + "' не удовлетворяет требованиям нормативных документов", font));
                cell.Colspan = 2;
                cell.Border = 0;
                table.SpacingBefore = 10f;
                table.SpacingAfter = 12.5f;
                table.AddCell(cell);
                doc.Add(table);
            }
            doc.Close();
            System.Diagnostics.Process.Start("pdfTables.pdf");
        }

        void RecursOutput(TreeViewModal tvm,string space)
        {
            table.AddCell(new Phrase(space+tvm.Naim, font));
            table.AddCell(new Phrase(tvm.ExpertOpinion.ToString(), font));
            foreach (TreeViewModal child in tvm.Children) RecursOutput(child, space + "    ");
        }
    }
}
