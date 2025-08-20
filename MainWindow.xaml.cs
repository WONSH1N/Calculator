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
            var noteView = new CalNote(); // View
            noteView.DataContext = new CalNoteControl(); // ViewModel 연결

            PositionWindowRightOfMain(noteView);
            noteView.Owner = this;
            noteView.Show();
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
        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var selected = (e.AddedItems[0] as ListBoxItem)?.Content?.ToString();
            MenuPopup.IsOpen = false;

            switch (selected)
            {
                case "계정관리":
                    OpenAccountWindow();
                    break;
                case "메모지":
                    OpenCalNoteWindow();
                    break;
            }

    (sender as ListBox).SelectedIndex = -1;
        }
    }
}
