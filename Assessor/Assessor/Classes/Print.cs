using System;
using System.Linq;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.IO;

namespace Assessor.Classes
{
    class Print
    {
        static Print print;
        Application application;
        Document document;
        Selection selection;
        Table table;
        string fileName;
        int i;

        Print() { }

        public static Print Init()
        {
            if (print == null) print = new Print();
            return print;
        }

        void ExpandTree(TreeViewExpertModal tree, Table table,string space)
        {
            table.Cell(table.Rows.Count, 1).Range.Text = space + tree.Naim;
            table.Cell(table.Rows.Count, 2).Range.Text = tree.ExpertOpinion.ToString();
            table.Rows.Add();
            foreach (TreeViewExpertModal child in tree.Children) ExpandTree(child, table,space+"    ");
        }
    
        public void PrintDocument(TreeViewExpertModal tree)
        {
            application = new Application();
            fileName = Path.GetTempFileName();
            document = application.Documents.Open(fileName);
            i = 1;
            selection = application.Selection;
            foreach (TreeViewExpertModal child in tree.Children)
            {
                document.Tables.Add(selection.Range, 1, 2);
                table = document.Tables[i];
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                ExpandTree(child, table, "");
                table.Rows.Last.Delete();
                i++;
                selection.EndKey(6);
                if (child.ExpertOpinion > 50) selection.TypeText("Критерий '"+child.Naim +"' удовлетворяет требованиям нормативных документов");
                else selection.TypeText("Критерий '"+child.Naim + "' не удовлетворяет требованиям нормативных документов");
                selection.TypeText("\r  ");
            }         
            foreach (Paragraph paragraph in document.Paragraphs)
                if (paragraph.Range.Text.Trim() == string.Empty)
                {
                    paragraph.Range.Select();
                    application.Selection.Delete();
                }
            application.Visible = true;
        }        
    }
}
