
using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace View3DMap;

public partial class MainWindow : Window
{


    public MainWindow()
    {
        InitializeComponent();
        CreatePlane();



    }
    private void Viewport_MouseMove(object sender, MouseEventArgs e)
    {
        

        Point mousePos = e.GetPosition(viewport);
        PointHitTestParameters hitParams = new PointHitTestParameters(mousePos);
        HitTestResult result = VisualTreeHelper.HitTest(viewport, mousePos);

        

        
        if(result.VisualHit is CubeVisual3D cube)
        {
            cube.Center = Point3D.Add(cube.Center, new Vector3D(5, 0, 0));
        }

       

    }

    private CubeVisual3D cube;
    private void CreatePlane()
    {


        var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
        meshBuilder.AddCube(HelixToolkit.Wpf.BoxFaces.All);

        var mesh = meshBuilder.ToMesh();

        cube = new CubeVisual3D
        {

            SideLength = 2,
            Fill = Brushes.Red,
            Center = new Point3D(10, 0, 0) // Начальная позиция куба
        };

        modelGroup.Children.Add(cube);




        Task.Run(async () =>
        {
            int a = 10;
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    fcube.Center = new Point3D(a += 1, 0, 0);
                });
                if(a > 30)
                {
                    a = 0;
                }


                await Task.Delay(100);
            }
           
        });






    }
}