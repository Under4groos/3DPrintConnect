using _3DPrintConnect.ComConnector;
using System.Windows;

namespace ViewMap;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
     
    COMConnector connector = new COMConnector();
    

    public MainWindow()
    {
        InitializeComponent();
        this.Loaded += MainWindow_Loaded;

        WinConsole.Initialize();
        connector.OnError += (result) =>
        {
            Console.WriteLine(result);
        };
        connector.OnDisposed += () =>
        {
            Console.WriteLine("IDisposable");
        };
        connector.OnCommandComplite += (command) =>
        {

            Console.WriteLine("\n-----------\n");

            Console.WriteLine(command.StringResult);



        };

        if (connector.Connect("COM21"))
        {
            connector.AddCommand($"G28");
            connector.Run();
        }
       

    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        border.PreviewMouseLeftButtonDown += Border_PreviewMouseLeftButtonDown;
        border.MouseMove += Border_MouseMove;
    }
    Point point;
    private void Border_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        point = e.GetPosition(border);
        this.Title = $"X{(int)point.X} Y{(int)point.Y}";
    }

    private void Border_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        pos.Margin = new Thickness((int)point.X, (int)point.Y, 0, 0);

        connector.AddCommand($"G1 X{(int)point.X} Y{(int)point.Y}");

        connector.AddCommand($"M114");
        connector.Run();
    }
}