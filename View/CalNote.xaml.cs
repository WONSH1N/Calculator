using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DevExpress.Data;
using DevExpress.XtraRichEdit.Model;

namespace Calculator.View
{
    /// <summary>
    /// CalNote.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CalNote : Window
    {
        public CalNote(Cal_ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void DelNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is NoteItem item)
            {
               Notes.Remove(item);
            }
        }
    }

}
