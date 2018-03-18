using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace AmapDemo
{
    public class AmapWalkingRoutePlanLayer : FeatureLayer
    {
        

        private bool enableEmbeddedStyle;
        private ClassBreakStyle classBreakStyle;
        public bool EnableEmbeddedStyle
        {
            get { return enableEmbeddedStyle; }
            set { enableEmbeddedStyle = value; }
        }

        protected override void DrawCore(GeoCanvas canvas, Collection<SimpleCandidate> labelsInAllLayers)
        {
            if (enableEmbeddedStyle)
            {
                var drawingFeatures = FeatureSource.GetFeaturesForDrawing(canvas.CurrentWorldExtent, canvas.Width, canvas.Height, ReturningColumnsType.AllColumns);
                classBreakStyle.Draw(drawingFeatures, canvas, new Collection<SimpleCandidate>(), labelsInAllLayers);
            }
            else
                base.DrawCore(canvas, labelsInAllLayers);
        }

        public AmapWalkingRoutePlanLayer(Uri serverUri, string key)
        {
            enableEmbeddedStyle = true;

            classBreakStyle = new ClassBreakStyle();
            classBreakStyle.ColumnName = "jf";
            classBreakStyle.ClassBreaks.Add(new ClassBreak(0, new LineStyle(new GeoPen(GeoColor.SimpleColors.Green, 2.0f))));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(4, new LineStyle(new GeoPen(GeoColor.SimpleColors.Yellow, 2.0f))));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(8, new LineStyle(new GeoPen(GeoColor.SimpleColors.Red, 2.0f))));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(10, new LineStyle(new GeoPen(GeoColor.SimpleColors.Black, 2.0f))));


            //http://restapi.amap.com/v3/direction/walking?key=您的key&origin=116.481028,39.989643&destination=116.434446,39.90816
            FeatureSource = new AmapWalkingRoutePlanSource(new Uri("http://restapi.amap.com/v3/direction/walking"), "429b85b27f3163adf6695c809068019f");
            
        }
       
    }
}
