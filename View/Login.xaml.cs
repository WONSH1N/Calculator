using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.View
{
    /// <summary>
    /// Loggin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            CheckPasswordHintVisibility();
        }
        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.Text == "Username")
            {
                comboBox.Text = "";
                comboBox.Foreground = Brushes.Black;
            }
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (string.IsNullOrWhiteSpace(comboBox.Text))
            {
                comboBox.Text = "Username";
                comboBox.Foreground = Brushes.Gray;
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordInput.Password))
            {
                PasswordHint.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordHint.Visibility = Visibility.Collapsed;
            }
        }
        private void CheckPasswordHintVisibility()
        {
            PasswordHint.Visibility = string.IsNullOrEmpty(PasswordInput.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        // 로그인 버튼 클릭 이벤트 핸들러
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new ViewModel.LoginViewModel();

            string userId = IdComboBox.Text.Trim();
            string password = PasswordInput.Password;

            if (viewModel.Login(userId, password))
            {
                // 로그인 성공
                DialogResult = true;
                this.Close(); // 로그인 창 닫기
            }
            else
            {
                // 로그인 실패
                MessageBox.Show("로그인 실패! 아이디와 비밀번호를 확인하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
