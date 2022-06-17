using System;
using System.Windows;
using WpfApp1.ViewModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

namespace WpfApp1.View
{
    public partial class MenuWindow : System.Windows.Window
    {
        private bool _canChange = false;
        public MenuWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelMenuWindow();
            
            
        }

        private void ChangeProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            nameTBox.IsReadOnly = _canChange;
            surnameTBox.IsReadOnly = _canChange;
            lastnameTBox.IsReadOnly = _canChange;
            loginTBox.IsReadOnly = _canChange;
            phoneTBox.IsReadOnly = _canChange;
            if (_canChange)
            {
                _canChange = false;
                ChangeProfileBtn.Content = "Изменить";
                return;
            }
            ChangeProfileBtn.Content = "Готово";
            _canChange = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != String.Empty)
            {
                EXEL(fbd.SelectedPath);
                WORD(fbd.SelectedPath);
                PDF(fbd.SelectedPath);
                System.Windows.MessageBox.Show("Запись отчетов прошла успешно!", "Информация", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Information);
            }
        }

        private void EXEL(string path)
        {
            int row = 2;
            Excel.Application exApp = new Excel.Application();
            exApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)exApp.ActiveSheet;

            workSheet.Cells[1, 1] = "Номер";
            workSheet.Cells[1, 2] = "Название";
            workSheet.Cells[1, 3] = "Дата публикации";
            workSheet.Cells[1, 4] = "Взявший пользователь";
            workSheet.Cells[1, 5] = "Статус";

            for (int i = 0; i < MyTasksForReport.Items.Count; i++)
            {
                Model.Task taskForAdd = MyTasksForReport.Items.GetItemAt(i) as Model.Task;

                workSheet.Cells[row, "A"] = taskForAdd.Id;
                workSheet.Cells[row, "B"] = taskForAdd.Title;
                workSheet.Cells[row, "C"] = taskForAdd.DatePub;
                if (taskForAdd.TakeUser == null)
                {
                    workSheet.Cells[row, "D"] = "Empty";
                }
                else
                {
                    workSheet.Cells[row, "D"] = taskForAdd.TakeUserNavigation.Login.ToString();
                }
                workSheet.Cells[row, "E"] = taskForAdd.Status.Title.ToString();

                ++row;
            }
            workSheet.Columns.AutoFit();
            try
            {
                workSheet.SaveAs(path + "\\TaskEXEL_User_" + Connection.userAuth.Login.ToString() + ".xlsx");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения EXEL!", "Ошибка!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }
            exApp.Quit();
        }

        private void WORD(string path)
        {
            object start = 0;
            object end = 0;
            Word.Document wordDoc = new Word.Document();
            Word.Range tableLocation = wordDoc.Range(ref start, ref end);
            wordDoc.Tables.Add(tableLocation, MyTasksForReport.Items.Count+1, 5);
            var table = wordDoc.Tables[1];
            table.AllowAutoFit = true;
            table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;


            Word.Cell cell = table.Cell(1, 1);
            int rowWord = 2;

            table.Cell(1, 1).Range.Text = "Номер";
            table.Cell(1, 2).Range.Text = "Название";
            table.Cell(1, 3).Range.Text = "Дата публикации";
            table.Cell(1, 4).Range.Text = "Взявший пользователь";
            table.Cell(1, 5).Range.Text = "Статус";

            for (int i = 0; i < MyTasksForReport.Items.Count; i++)
            {
                Model.Task taskForAdd = MyTasksForReport.Items.GetItemAt(i) as Model.Task;

                table.Cell(rowWord, 1).Range.Text = taskForAdd.Id.ToString();
                table.Cell(rowWord, 2).Range.Text = taskForAdd.Title;
                table.Cell(rowWord, 3).Range.Text = taskForAdd.Description;
                table.Cell(rowWord, 4).Range.Text = taskForAdd.DatePub.ToString();
                if (taskForAdd.TakeUser==null)
                {
                    table.Cell(rowWord, 5).Range.Text = "Empty";
                }
                else
                {
                    table.Cell(rowWord, 5).Range.Text = taskForAdd.TakeUserNavigation.Login.ToString();
                }

                rowWord++;
            }

            try
            {
                wordDoc.SaveAs(path + "\\TaskWORD_User_" + Connection.userAuth.Login.ToString() + ".docx");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения WORD!", "Ошибка!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }
        }

        private void PDF(string path)
        {
            Aspose.Pdf.Document document = new Aspose.Pdf.Document();
            Aspose.Pdf.Page page = document.Pages.Add();
            Aspose.Pdf.Table table = new Aspose.Pdf.Table();
            table.Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Black));
            table.DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Black));

            Aspose.Pdf.Row head = table.Rows.Add();
            head.Cells.Add("Номер");
            head.Cells.Add("Название");
            head.Cells.Add("Дата публикации");
            head.Cells.Add("Взявший пользователь");
            head.Cells.Add("Статус");

            for (int i = 0; i < MyTasksForReport.Items.Count; i++)
            {
                Model.Task taskForAdd = MyTasksForReport.Items.GetItemAt(i) as Model.Task;
                Aspose.Pdf.Row row = table.Rows.Add();
                row.Cells.Add(taskForAdd.Id.ToString());
                row.Cells.Add(taskForAdd.Title);
                row.Cells.Add(taskForAdd.DatePub.ToString());
                if (taskForAdd.TakeUser == null)
                {
                    row.Cells.Add("Empty");
                }
                else
                {
                    row.Cells.Add(taskForAdd.TakeUserNavigation.Login);
                }
                row.Cells.Add(taskForAdd.Status.Title);
            }

            table.DefaultColumnWidth = "61";
            page.Paragraphs.Add(table);
            try
            {
                document.Save(path + "\\TaskPDF_User_" + Connection.userAuth.Login.ToString() + ".pdf");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения, PDF!", "Ошибка!", MessageBoxButton.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }
        }
    }
}
