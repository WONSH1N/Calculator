using Calculator.Model;
using Calculator.View;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {

            InitializeComponent();

            _viewModel = new Cal_ViewModel();
            DataContext = _viewModel;
        }

        private Cal_ViewModel _viewModel;


        // 화면 이동
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
        }
        private void OpenLoginWindow()
        {
            var loginView = new Login(); // View
            loginView.DataContext = new LoginControl(); // ViewModel 연결


            loginView.Owner = this;
            loginView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginView.ShowDialog(); // 모달로 띄우기
        }
        private void OpenAccountWindow()
        {
            var accountView = new Account(); // View
            accountView.DataContext = new AccountControl(); // ViewModel 연결

            PositionWindowRightOfMain(accountView);
            accountView.Owner = this;
            accountView.Show();
        }
        private void OpenCalNoteWindow()
        {
            var noteView = new CalNote();  // View
            noteView.DataContext = new CalNoteControl(); // ViewModel 연결

            PositionWindowRightOfMain(noteView);
            noteView.Owner = this;
            noteView.Show();
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var selected = (e.AddedItems[0] as ListBoxItem)?.Content?.ToString();
            MenuPopup.IsOpen = false;

            switch (selected)
            {
                case "로그인":
                    TryLogin();
                    break;
                case "로그아웃":
                    TryLogout();
                    break;
                case "계정관리":
                    OpenAccountWindow();
                    break;
                case "메모지":
                    OpenCalNoteWindow();
                    break;

            }

    (sender as ListBox).SelectedIndex = -1;
        }
        private void PositionWindowRightOfMain(Window newWindow)
        {
            newWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            var mainLeft = this.Left;
            var mainTop = this.Top;
            var mainWidth = this.ActualWidth;

            var screenWidth = SystemParameters.VirtualScreenWidth;

            // 오른쪽 공간 부족하면 왼쪽에 붙이기
            double desiredLeft = mainLeft + mainWidth;
            if (desiredLeft + newWindow.Width > screenWidth)
                desiredLeft = mainLeft - newWindow.Width;

            newWindow.Left = desiredLeft;
            newWindow.Top = mainTop;
        }

        private void CalDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private bool _isLoggedIn = false;

        private void CheckLoginStatus()
        {
            if (LoginCheck != null)
            {
                LoginCheck.Content = _isLoggedIn ? "로그아웃" : "로그인";
            }
        }
        private void TryLogin()
        {

            if (!_isLoggedIn)
            {
                OpenLoginWindow();
                _isLoggedIn = true; // 로그인 성공 가정
            }
            else
            {
                MessageBox.Show("이미 로그인되어 있습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            CheckLoginStatus();
        }
        private void TryLogout()
        {
            if (_isLoggedIn)
            {
                MessageBoxResult result = MessageBox.Show("로그아웃 하시겠습니까?", "로그아웃 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _isLoggedIn = false; // 로그아웃 처리
                    CheckLoginStatus();
                }
            }
            else
            {
                MessageBox.Show("로그인되어 있지 않습니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
