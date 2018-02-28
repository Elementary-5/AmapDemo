using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.WinForms;

namespace AmapDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            winformsMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldStreetsAndImageryOverlay worldOverlay = new WorldStreetsAndImageryOverlay();
            winformsMap1.Overlays.Add(worldOverlay);

            var test = new AmapWalkingRoutePlanLayer(new Uri("http://www.baidu.com"), "");

            LayerOverlay layerOverlay = new LayerOverlay();
            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(@"..\..\AppData\states.shp");
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);

            //layerOverlay.Layers.Add(test);

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            
            foreach(var fe in test.fea.AllFeatures)
            {
                inMemoryLayer.InternalFeatures.Add(fe);
            }
            //inMemoryLayer.InternalFeatures.Add("Polygon", new Feature(BaseShape.CreateShapeFromWellKnownData("POLYGON((10 60,40 70,30 85, 10 60))")));
            //inMemoryLayer.InternalFeatures.Add("Multipoint", new Feature(BaseShape.CreateShapeFromWellKnownData("MULTIPOINT(10 20, 30 20,40 20, 10 30, 30 30, 40 30)")));
            //inMemoryLayer.InternalFeatures.Add("Line", new Feature(BaseShape.CreateShapeFromWellKnownData("LineString(104.067105 30.642995, 104.067666 30.642660)")));
            //inMemoryLayer.InternalFeatures.Add("Rectangle", new Feature(new RectangleShape(65, 30, 95, 15)));
            //inMemoryLayer.InternalFeatures.Add("Line", new Feature(BaseShape.CreateShapeFromWellKnownData);
            //LineShape lineShape = new LineShape();
            //lineShape.Vertices.Add(new Vertex(104.0740526900, 30.6256401600));
            //lineShape.Vertices.Add(new Vertex(104.0946525800,30.6386345500));
            //Feature feature = new Feature(lineShape);
            //feature.ColumnValues.Add("a", Convert.ToString(104.067105));
            //inMemoryLayer.InternalFeatures.Add(feature);
            //inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.StandardColors.RoyalBlue);
            //inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Blue;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.OuterPen = new GeoPen(GeoColor.FromArgb(200, GeoColor.StandardColors.Red), 5);
            //inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.SymbolPen = new GeoPen(GeoColor.FromArgb(255, GeoColor.StandardColors.Green), 8);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            layerOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);

            winformsMap1.Overlays.Add(layerOverlay);

            shapeFileLayer.Open();


            winformsMap1.CurrentExtent = new RectangleShape(104.076202, 30.636441, 104.097145, 30.622833);
            winformsMap1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            winformsMap1.FindFeatureLayer("InMemoryFeatureLayer").IsVisible = !winformsMap1.FindFeatureLayer("InMemoryFeatureLayer").IsVisible;
            winformsMap1.Refresh();
        }
    }
}
