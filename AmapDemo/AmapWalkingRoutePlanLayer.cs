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
        private Uri serverUri;
        private string key;

        private bool enableEmbeddedStyle;
        public AmapWalkingRoutePlanSource fea;
        public bool EnableEmbeddedStyle
        {
            get => enableEmbeddedStyle;
            set => enableEmbeddedStyle = value;
        }

        public AmapWalkingRoutePlanLayer(Uri serverUri, string key)
        {
            enableEmbeddedStyle = true;
            this.serverUri = serverUri;
            this.key = key;
            //http://restapi.amap.com/v3/direction/walking?key=您的key&origin=116.481028,39.989643&destination=116.434446,39.90816
            fea = new AmapWalkingRoutePlanSource(new Uri("http://restapi.amap.com/v3/direction/walking"), "429b85b27f3163adf6695c809068019f");
            
        }
       
    }
}
