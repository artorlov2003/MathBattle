using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Application = Microsoft.Office.Interop.Word.Application;
using Table = Microsoft.Office.Interop.Word.Table;
using Window = System.Windows.Window;

namespace Матбои
{
    public partial class MainWindow : Window
    {
        private List<Gameround> gamerounds = new List<Gameround>();
        public class Gameround
        {
            private static int numof = 0;
            private static int sum1 = 0;
            private static int sum2 = 0;
            private static int sumz = 0;
            public string id { get; set; }
            public int first { get; set; }
            public int secound { get; set; }
            public int zhury { get; set; }

            public string vyzov { get; set; }

            public Gameround(int _first, int _secound)
            {
                first = _first;
                secound = _secound;
                zhury = 12 - first - secound;
                sum1 += first;
                sum2 += secound;
                sumz += zhury;
                numof++;
                id = numof.ToString();
            }

            public Gameround()
            {

            }

            public static Gameround getTotal()
            {
                Gameround getTotal = new Gameround();
                getTotal.id = "Итог";
                getTotal.first = sum1;
                getTotal.secound = sum2;
                getTotal.zhury = sumz;
                return getTotal;

            }
        }
        public static string first_players;
        public static string secound_players;
        public static string fn = "@@@";
        public static string sn = "@@@";
        public static bool command = false;
        private static bool firstlaunch = true;
        private static bool leader;
        public MainWindow()
        {
            if (firstlaunch)
            {
                FirstSettings firstSettings = new FirstSettings();
                if (fn == "@@@" && firstSettings.ShowDialog() != true)
                {
                    InitializeComponent();
                    Close();
                }


                InitializeComponent();
                fn = firstSettings.first_name;
                sn = firstSettings.secound_name;
                first_players = firstSettings.first_players;
                secound_players = firstSettings.secound_players;
                leader = firstSettings.leader;
                command = !leader;
                firstcommand.Header = fn;
                secoundcommand.Header = sn;
            }
            firstlaunch = false;
            InitializeComponent();
            ListView.Items.Add(Gameround.getTotal());

        }

        private void addround(object sender, RoutedEventArgs e)
        {
            AddRound addRound = new AddRound();
            addRound.first_name = fn;
            addRound.secound_name = sn;
            addRound.command = !command;
            addRound.Update();
            if (addRound.ShowDialog() == true)
            {
                Gameround gameround = new Gameround(addRound.first_point, addRound.secound_point);
                fn = addRound.first_name;
                firstcommand.Header = fn;
                sn = addRound.secound_name;
                secoundcommand.Header = sn;
                command = addRound.command;
                if (command)
                {
                    gameround.vyzov = fn;
                }
                else
                {
                    gameround.vyzov = sn;
                }
                ListView.Items.RemoveAt(ListView.Items.Count - 1);
                ListView.Items.Add(gameround);
                gamerounds.Add(gameround);
                ListView.Items.Add(Gameround.getTotal());

            }
        }

        private void Redraw(object sender, SizeChangedEventArgs e)
        {
            var wid = ListView.ActualWidth / 4.0d - 14;
            firstcommand.Width = wid;
            secoundcommand.Width = wid;
            //thirdcommand.Width = wid;
            Vyzover.Width = wid;
        }

        private void Confirmation(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтвержение", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }

            e.Cancel = true;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            gamerounds.Add(Gameround.getTotal());
            Application application = new Application();
            Document doc = application.Documents.Add();
            Range range = doc.Range();
            range.Text = "ПРОТОКОЛ МАТЕМАТИЧЕСКОГО БОЯ\n";
            range.Bold = 1;
            range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            range = doc.Range(range.End - 1);
            range.Bold = 0;
            range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            if (!(string.IsNullOrWhiteSpace(first_players) && string.IsNullOrWhiteSpace(secound_players)))
            {
                string[] fcommand = first_players.Split('\n');
                string[] scommand = secound_players.Split('\n');
                Table commands = range.Tables.Add(range, Math.Max(fcommand.Length, scommand.Length) + 1, 2);
                commands.Cell(1, 1).Range.Text = fn;
                commands.Cell(1, 1).Range.Bold = 1;
                commands.Cell(1, 2).Range.Text = sn;
                commands.Cell(1, 2).Range.Bold = 1;
                for (int i = 1; i <= Math.Max(fcommand.Length, scommand.Length); i++)
                {
                    if (i <= fcommand.Length && !string.IsNullOrWhiteSpace(fcommand[i - 1]))
                        commands.Cell(i + 1, 1).Range.Text = (i).ToString() + ". " + fcommand[i - 1].Replace("\r", "");
                    if (i <= scommand.Length && !string.IsNullOrWhiteSpace(scommand[i - 1]))
                        commands.Cell(i + 1, 2).Range.Text = (i).ToString() + ". " + scommand[i - 1].Replace("\r", "");
                }
            }
            range = doc.Range(range.End - 1);
            range.Text = "В конкурсе капитанов победил капитан команды ";
            if (leader)
                range.Text = "В конкурсе капитанов победил капитан команды " + fn;
            else
                range.Text = "В конкурсе капитанов победил капитан команды " + sn;
            range = doc.Range(range.End - 1);
            Table table = doc.Tables.Add(range, gamerounds.Count + 1, 5);
            table.Borders.Enable = 1;
            table.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth100pt;
            table.Borders.InsideLineWidth = WdLineWidth.wdLineWidth025pt;
            foreach (Row row in table.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    if (cell.RowIndex == 1)
                    {
                        if (cell.ColumnIndex == 1)
                        {
                            cell.Range.Text = "Раунд";
                            cell.Range.Bold = 1;
                        }
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Range.Text = fn;
                            cell.Range.Bold = 1;
                        }
                        if (cell.ColumnIndex == 3)
                        {
                            cell.Range.Text = "Вызов";
                            cell.Range.Bold = 1;
                        }
                        if (cell.ColumnIndex == 4)
                        {
                            cell.Range.Text = sn;
                            cell.Range.Bold = 1;
                        }
                        if (cell.ColumnIndex == 5)
                        {
                            cell.Range.Text = "Жюри";
                            cell.Range.Bold = 1;
                        }
                    }
                    else
                    {
                        Gameround gr = gamerounds[cell.RowIndex - 2];
                        if (cell.ColumnIndex == 1)
                        {
                            cell.Range.Text = gr.id;
                        }
                        if (cell.ColumnIndex == 2)
                        {
                            cell.Range.Text = gr.first.ToString();
                        }
                        if (cell.ColumnIndex == 3)
                        {
                            cell.Range.Text = gr.vyzov;
                        }
                        if (cell.ColumnIndex == 4)
                        {
                            cell.Range.Text = gr.secound.ToString();
                        }
                        if (cell.ColumnIndex == 5)
                        {
                            cell.Range.Text = gr.zhury.ToString();
                        }
                    }
                }
            }
            gamerounds.RemoveAt(gamerounds.Count - 1);
            range = doc.Range(range.End - 1);
            range.Text = "\n" + DateTime.Today.ToShortDateString();
            range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            doc.Save();
            doc.Close();
            application.Quit();
            SaveButton.IsEnabled = true;
        }
    }
}
