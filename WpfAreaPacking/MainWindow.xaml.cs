using System.Windows;


namespace WpfAreaPacking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //new AreaPacker(MyCanvas, this);
            new AreaPackerNew(MyCanvas, this);
        }

        private void PackingClick(object sender, RoutedEventArgs e)
        {
            //new AreaPacker(MyCanvas, this);
            new AreaPackerNew(MyCanvas, this);
        }
    }
}