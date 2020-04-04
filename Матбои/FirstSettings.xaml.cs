using System;
using System.ComponentModel;
using System.Windows;

namespace Матбои
{
    /// <summary>
    /// Логика взаимодействия для FirstSettings.xaml
    /// </summary>
    public partial class FirstSettings : Window
    {
        public String first_name;
        public String secound_name;
        public string first_players;
        public string secound_players;
        public bool leader = true;
        public FirstSettings()
        {
            InitializeComponent();
        }
        private void firstcheck(object sender, RoutedEventArgs e)
        {
            SButton.Content = "Поражение";
            FirstButton.Content = "Победа";
            leader = true;
            SButton.IsChecked = false;
        }

        private void secoundchek(object sender, RoutedEventArgs e)
        {
            FirstButton.Content = "Поражение";
            SButton.Content = "Победа";
            leader = false;

            FirstButton.IsChecked = false;
        }

        private void FUnchecked(object sender, RoutedEventArgs e)
        {
            if (FirstButton.Content.ToString() == "Победа")
                FirstButton.IsChecked = true;
        }

        private void SUnchecked(object sender, RoutedEventArgs e)
        {
            if (SButton.Content.ToString() == "Победа")
                SButton.IsChecked = true;
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fn.Text) || string.IsNullOrWhiteSpace(sn.Text))
                {
                    Exception exception = new Exception("Введите названия команд");
                    throw exception;
                }
                first_name = fn.Text;
                secound_name = sn.Text;
                first_players = fp.Text;
                secound_players = sp.Text;
                DialogResult = true;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (DialogResult != true)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти?", "Подтвержение", MessageBoxButton.YesNo,
                       MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }

                e.Cancel = true;
            }
        }
    }
}
