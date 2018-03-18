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
            LayerOverlay layerOverlay = new LayerOverlay();

            var amapLayer = new AmapWalkingRoutePlanLayer(new Uri("https://traffic.cit.api.here.com/traffic/6.2/flow.json"),"");
            layerOverlay.Layers.Add("AmapFeatureLayer", amapLayer);

            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(@"..\..\AppData\states.shp");
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);

            winformsMap1.Overlays.Add(layerOverlay);

            shapeFileLayer.Open();


            winformsMap1.CurrentExtent = new RectangleShape(104.076202, 30.636441, 104.097145, 30.622833);
            winformsMap1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            winformsMap1.FindFeatureLayer("AmapFeatureLayer").IsVisible = !winformsMap1.FindFeatureLayer("AmapFeatureLayer").IsVisible;
            winformsMap1.Refresh();
        }
    }
}
