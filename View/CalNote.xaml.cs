using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DevExpress.Data;
using DevExpress.XtraRichEdit.Model;
using Calculator.ViewModel;

namespace Calculator.View
{
    /// <summary>
    /// CalNote.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalNote : Window
    {
        private CalNoteViewModel _viewModel;

        // ✅ 외부에서 ViewModel을 주입받는 생성자
        public CalNote(CalNoteViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public CalNoteViewModel ViewModel => _viewModel;

        private void DelNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CalNoteViewModel.NoteItem item)
            {
                _viewModel.DelNote(item);
            }
            else
            {
                MessageBox.Show("삭제할 노트를 선택하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

}
