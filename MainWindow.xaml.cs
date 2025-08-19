using DevExpress.Xpf.Core;

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
    }
}
