using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Calculator.View
{
    /// <summary>
    /// Account.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Account : Window
    {
        public ObservableCollection<AccountItem> Accounts { get; set; }
        public Account()
        {
            InitializeComponent();
            Accounts = new ObservableCollection<AccountItem>();
            AccountDataGrid.ItemsSource = Accounts;

        }
        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            // 예시 데이터 조회
            Accounts.Clear();
            Accounts.Add(new AccountItem
            {
                Account = 1,
                Id = "PTNC",
                Password = "PTNC",
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Authority = "admin"
            });
            Accounts.Add(new AccountItem
            {
                Account = 2,
                Id = "Staff",
                Password = "1w@ntg0h0me",
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Authority = "user"
            });
            Accounts.Add(new AccountItem
            {
                Account = 3,
                Id = "Newcomer",
                Password = "p2r0o2t5n0804c",
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Authority = "guest"
            });
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountDataGrid.SelectedItem is AccountItem selectedAccount)
            {
                Accounts.Remove(selectedAccount);
            }
            else
            {
                MessageBox.Show("삭제할 계정을 선택하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    public class AccountItem
    {
        public int Account { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public string Date { get; set; }
        public string Authority { get; set; }
    }
}
