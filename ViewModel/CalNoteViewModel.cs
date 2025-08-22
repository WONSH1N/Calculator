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
                Notes.Add(new NoteItem
                {
                    CalExp = exp,
                    Display = result
                });
            }
        }

        public void DelNote(NoteItem item)
        {
            if (Notes.Contains(item))
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

        public void Evaluate(string expression)
        {

        }
        protected void OnPropertyChanged(string propertyName) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
