using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using MeshGeometry3D = System.Windows.Media.Media3D.MeshGeometry3D;

namespace View3DMap.CustomMesh
{
    
    public partial class TestModel :  MeshElement3D
    {
        private const double grid = 0.008;
        private const double margin = 0.0001;
        private const double wallThickness = 0.001;
        private const double plateThickness = 0.0032;
        private const double brickThickness = 0.0096;
        private const double knobHeight = 0.0018;
        private const double knobDiameter = 0.0048;
        private const double outerDiameter = 0.00651;
        private const double axleDiameter = 0.00475;
        private const double holeDiameter = 0.00485;


        public int Divisions
        {
            get; set;
        } = 12;

        public int Height
        {
            get; set;
        } = 3;
        public int Rows
        {
            get; set;
        } = 2;

        public int Columns
        {
            get; set;
        } = 6;







        protected void OnChanged()
        {
            OnGeometryChanged();
        }

        // http://www.robertcailliau.eu/Lego/Dimensions/zMeasurements-en.xhtml
        public static double GridUnit
        {
            get { return grid; }
        }
        public static double HeightUnit
        {
            get { return plateThickness; }
        }

        protected override MeshGeometry3D Tessellate()
        {
            double width = Columns * grid - margin * 2;
            double length = Rows * grid - margin * 2;
            double height = Height * plateThickness;
            var builder = new MeshBuilder(true, true);

            for (int i = 0; i < Columns; i++)
                for (int j = 0; j < Rows; j++)
                {
                    var o = new Point3D((i + 0.5) * grid, (j + 0.5) * grid, height);
                    builder.AddCone(o, new Vector3D(0, 0, 1) , (float)knobDiameter / 2, (float)knobDiameter / 2, (float)knobHeight, false, true,
                                    Divisions);
                    builder.AddPipe(new Point3D(o.X, o.Y, o.Z - wallThickness) , new Point3D(o.X, o.Y, wallThickness),
                                    (float)knobDiameter, (float)outerDiameter, Divisions);
                }

            builder.AddBox(new Point3D(Columns * 0.5 * grid, Rows * 0.5 * grid, height - wallThickness / 2) , (float)width, (float)length,
                          (float)wallThickness,
                          BoxFaces.All);
            builder.AddBox(new Point3D(margin + wallThickness / 2, Rows * 0.5 * grid, height / 2 - wallThickness / 2) ,
                           (float)wallThickness, (float)length, (float)(height - wallThickness),
                           BoxFaces.All ^ BoxFaces.Top);
            builder.AddBox(
                new Point3D(Columns * grid - margin - wallThickness / 2, Rows * 0.5 * grid, height / 2 - wallThickness / 2) ,
                (float)wallThickness, (float)length, (float)(height - wallThickness),
                BoxFaces.All ^ BoxFaces.Top);
            builder.AddBox(new Point3D(Columns * 0.5 * grid, margin + wallThickness / 2, height / 2 - wallThickness / 2) ,
                           (float)width, (float)wallThickness, (float)(height - wallThickness),
                           BoxFaces.All ^ BoxFaces.Top);
            builder.AddBox(
                new Point3D(Columns * 0.5 * grid, Rows * grid - margin - wallThickness / 2, height / 2 - wallThickness / 2) ,
                (float)width, (float)wallThickness, (float)(height - wallThickness),
                BoxFaces.All ^ BoxFaces.Top);

            return builder.ToMesh();
        }
    }
}
