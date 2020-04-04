using System;
using System.Windows;
using System.Windows.Input;

namespace Матбои
{
    /// <summary>
    /// Логика взаимодействия для AddRound.xaml
    /// </summary>
    public partial class AddRound : Window
    {
        public String first_name;
        public String secound_name;
        public int first_point;
        public int secound_point;
        public bool command;
        public AddRound()
        {
            InitializeComponent();
        }

        public void Update()
        {
            fn.Content = first_name;
            sn.Content = secound_name;
            fp.Focus();
            if (command)
            {
                FButton.IsChecked = true;
            }
            else
            {
                SButton.IsChecked = true;
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                first_point = int.Parse(fp.Text);
                secound_point = int.Parse(sp.Text);
                DialogResult = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void Fp_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sp.Focus();
            }
        }

        private void Sp_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddButtonClick(sender, e);
            }
        }

        private void firstcheck(object sender, RoutedEventArgs e)
        {
            SButton.IsChecked = false;
            SButton.Content = "Защита";
            FButton.Content = "Вызов";
            command = true;
        }

        private void secoundchek(object sender, RoutedEventArgs e)
        {
            FButton.IsChecked = false;
            FButton.Content = "Защита";
            SButton.Content = "Вызов";
            command = false;

        }

        private void FUnchecked(object sender, RoutedEventArgs e)
        {
            if (FButton.Content.ToString() == "Защита")
                FButton.IsChecked = true;
        }

        private void SUnchecked(object sender, RoutedEventArgs e)
        {
            if (SButton.Content.ToString() == "Защита")
                SButton.IsChecked = true;
        }
    }
}
