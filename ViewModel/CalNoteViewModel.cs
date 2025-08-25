using Calculator.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Calculator.ViewModel.CalNoteViewModel;

namespace Calculator.ViewModel
{
    public class CalNoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<NoteItem> Notes { get; } = new ObservableCollection<NoteItem>();

        public void AddNote(string exp, string result)
        {
            if (!string.IsNullOrWhiteSpace(exp) && !string.IsNullOrWhiteSpace(result))
            {
                // add 에서 Insert로 변경하여 최신 노트가 위에 오도록 함
                Notes.Insert(0, new NoteItem
                {
                    CalExp = exp,
                    Display = result
                });
            }
        }

        public void DelNote(NoteItem item)
        {
            if (item != null && Notes.Contains(item))
            {
                Notes.Remove(item);
                OnPropertyChanged(nameof(Notes));
            }
        }
        public class NoteItem
        {
            public string CalExp { get; set; } // 계산식
            public string Display { get; set; } // 표시할 결과
        }

       
        protected void OnPropertyChanged(string propertyName) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
