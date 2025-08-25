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
        private readonly CalNoteViewModel _viewModel; // 클래스 필드 선언

        public CalNote()
        {
            InitializeComponent();
            _viewModel = new CalNoteViewModel(); // ViewModel 인스턴스 생성, xaml에서 실행할 때도 안전
            DataContext = _viewModel;
        }

        // 외부에서 ViewModel을 주입받는 생성자
        public CalNote(CalNoteViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel; // 주입받은 ViewModel 사용 ( MainWindow.xaml.cs에서 전달된 ViewModel 보관 )
            DataContext = _viewModel;
        }

        private void DelNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CalNoteViewModel.NoteItem note)
            {
                _viewModel.DelNote(note);
            }
            else
            {
                MessageBox.Show("삭제할 노트를 선택하세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

}
