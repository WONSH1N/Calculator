using Calculator.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public class CalNoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<NoteItem> Notes { get; } = new ObservableCollection<NoteItem>();

        public ICommand AddNoteCommand { get; }

        public CalNoteControl()
        {
            AddNoteCommand = new RelayCommand<string>(AddNote);
        }
      

        private void AddNote(string expr)
        {
            var value = "";/* 계산 로직 실행한 결과 */
            Notes.Add(new NoteItem { CalExp = expr, Display = value.ToString() });
        }
        public class NoteItem
        {
            public string CalExp { get; set; } // 계산식
            public string Display { get; set; } // 표시할 결과
        }
    }
    public class CalNoteControl
    {
        public ObservableCollection<NoteItem> Notes { get; set; } = new ObservableCollection<NoteItem>();

        public CalNoteControl()
        {
            // 초기 데이터 추가
            Notes.Add(new NoteItem { CalExp = "2 + 2", Display = "4" });
            Notes.Add(new NoteItem { CalExp = "3 * 3", Display = "9" });
            Notes.Add(new NoteItem { CalExp = "5 - 1", Display = "4" });
        }
    }
}
