using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.WinForms;

namespace AmapDemo
{
    public partial class Form1 : Form
    {
        private static string rootPath = Path.GetFullPath(@"..\..\RealTimeTrafficHtml");
        private readonly string AppKey = ConfigurationManager.AppSettings["AppKey"];

        public Form1()
        {
            InitializeComponent();
        }

        private bool realTimeTrafficisVisible = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            winformsMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldStreetsAndImageryOverlay worldOverlay = new WorldStreetsAndImageryOverlay();
            winformsMap1.Overlays.Add(worldOverlay);
            LayerOverlay layerOverlay = new LayerOverlay();
            var amapLayer = new AmapWalkingRoutePlanLayer(new Uri("http://restapi.amap.com/v3/direction/walking"), AppKey);
            layerOverlay.Layers.Add("AmapFeatureLayer", amapLayer);

            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(@"..\..\AppData\states.shp");
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);

            winformsMap1.Overlays.Add(layerOverlay);

            shapeFileLayer.Open();

            //104.076233,30.623196&destination=104.097133,30.636324
            winformsMap1.CurrentExtent = new RectangleShape(104.076233, 30.636324, 104.097133, 30.623196);
            winformsMap1.Dock = DockStyle.Fill;
            winformsMap1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            winformsMap1.FindFeatureLayer("AmapFeatureLayer").IsVisible = !winformsMap1.FindFeatureLayer("AmapFeatureLayer").IsVisible;
            winformsMap1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (realTimeTrafficisVisible = !realTimeTrafficisVisible)
            {
                RealTimeTraffic realTimeTraffic = new RealTimeTraffic();
                realTimeTraffic.BringToFront();
                realTimeTraffic.DocumentText = GetDocumentText();
                realTimeTraffic.Size = this.Size;
                realTimeTraffic.Dock = DockStyle.Fill;
                this.Controls.Add(realTimeTraffic);
                winformsMap1.Visible = false;
                button2.BringToFront();
            }
            else
            {
                winformsMap1.Dock = DockStyle.Fill;
                winformsMap1.Visible = true;
            }
        }

        private string GetDocumentText()
        {
            string realTimeTrafficPath = Path.Combine(rootPath, "RealTimeTraffic.html");
            if (!File.Exists(realTimeTrafficPath))
            {
                return "文件不存在";
            }
            return File.ReadAllText(realTimeTrafficPath);
        }
    }
}