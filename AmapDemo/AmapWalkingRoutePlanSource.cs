using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AmapDemo
{
    public class AmapWalkingRoutePlanSource : FeatureSource
    {
        private Uri serverUri;
        private string key;
        private Collection<Feature> features;

        private readonly double PI = 3.14159265358979324;

        public Collection<Feature> AllFeatures
        {
            get => features;
        }

        public AmapWalkingRoutePlanSource(Uri serverUri, string key)
        {
            this.serverUri = serverUri;
            this.key = key;
            GetFeature();
        }

        protected override Collection<Feature> GetAllFeaturesCore(IEnumerable<string> returningColumnNames)
        {
            RectangleShape boundingBox = new RectangleShape(-180, 90, 180, -90);
            return GetFeaturesInsideBoundingBox(boundingBox, returningColumnNames);
        }

        protected override Collection<Feature> GetFeaturesForDrawingCore(RectangleShape boundingBox, double screenWidth, double screenHeight, IEnumerable<string> returningColumnNames)
        {
            return GetFeaturesInsideBoundingBox(boundingBox, returningColumnNames);
        }
        private void GetFeature()
        {
            features = new Collection<Feature>();

            if (serverUri != null && !string.IsNullOrEmpty(key))
            {
                //http://restapi.amap.com/v3/direction/walking?key=您的key&origin=116.481028,39.989643&destination=116.434446,39.90816
                var requestUrl = serverUri.AbsoluteUri + "?key=" + key + "&origin=104.076233,30.623196&destination=104.097133,30.636324";
                var request = WebRequest.Create(new Uri(requestUrl));
                var reponse = request.GetResponse();

                using (var stream = reponse.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();
                    var jsonArray = (JObject)JsonConvert.DeserializeObject(content);
                    var route = jsonArray["route"];
                    var paths = route["paths"];
                    foreach (var path in paths)
                    {
                        var steps = path["steps"];
                        foreach (var step in steps)
                        {
                            var polyline = step["polyline"];
                            var lineShapes = polyline.ToObject<string>().Trim().Split(';');
                            var lineShape = new LineShape();
                            foreach (var line in lineShapes)
                            {
                                var longLat = line.Split(',');
                                //纬经度
                                var dic = delta(double.Parse(longLat[1]), double.Parse(longLat[0]));

                                //var longValue = double.Parse(longLat[0]);
                                //var latValue = double.Parse(longLat[1]);
                                lineShape.Vertices.Add(new Vertex(dic["lon"], dic["lat"]));
                            }
                            var feature = new Feature(lineShape);
                            features.Add(feature);
                        }
                    }
                }
            }

        }
        private new Collection<Feature> GetFeaturesInsideBoundingBox(RectangleShape boundingBox, IEnumerable<string> returningColumnNames)
        {
             features = new Collection<Feature>();

            if(serverUri != null && !string.IsNullOrEmpty(key))
            {
                //http://restapi.amap.com/v3/direction/walking?key=您的key&origin=116.481028,39.989643&destination=116.434446,39.90816
                var requestUrl = serverUri.AbsoluteUri + "?key=" + key + "&origin=104.076245,30.622981&destination=104.097273,30.636459";
                var request = WebRequest.Create(new Uri(requestUrl));
                var reponse = request.GetResponse();

                using (var stream = reponse.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();
                    var jsonArray = (JObject)JsonConvert.DeserializeObject(content);
                    var route = jsonArray["route"];
                    var paths = route["paths"];
                    foreach (var path in paths)
                    {
                        var steps = path["steps"];
                        foreach (var step in steps)
                        {
                            var polyline = step["polyline"];
                            var lineShapes = polyline.ToObject<string>().Trim().Split(';');
                            var lineShape = new LineShape();
                            foreach (var line in lineShapes)
                            {
                                var longLat = line.Split(',');
                                //纬经度
                                var dic = delta(double.Parse(longLat[1]), double.Parse(longLat[0]));

                                //var longValue = double.Parse(longLat[0]);
                                //var latValue = double.Parse(longLat[1]);
                                lineShape.Vertices.Add(new Vertex(dic["lon"], dic["lat"]));
                            }
                            var feature = new Feature(lineShape);
                            features.Add(feature);
                        }
                    }
                }
            }

            return features;
        }
        /**
     * @author 作者:
     * 方法描述:方法可以将高德地图SDK获取到的GPS经纬度转换为真实的经纬度，可以用于解决安卓系统使用高德SDK获取经纬度的转换问题。
     * @param 需要转换的经纬度
     * @return 转换为真实GPS坐标后的经纬度
     * @throws <异常类型> {@inheritDoc} 异常描述
     */
        private Dictionary<String, Double> delta(double lat, double lon)
        {
            double a = 6378245.0;//克拉索夫斯基椭球参数长半轴a
            double ee = 0.00669342162296594323;//克拉索夫斯基椭球参数第一偏心率平方
            double dLat = this.transformLat(lon - 105.0, lat - 35.0);
            double dLon = this.transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * this.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * this.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * this.PI);

            Dictionary<String, Double> dic = new Dictionary<String, Double>();
            dic.Add("lat", lat - dLat);
            dic.Add("lon", lon - dLon);

            return dic;
        }
        //转换经度
        private double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * this.PI) + 20.0 * Math.Sin(2.0 * x * this.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * this.PI) + 40.0 * Math.Sin(x / 3.0 * this.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * this.PI) + 300.0 * Math.Sin(x / 30.0 * this.PI)) * 2.0 / 3.0;
            return ret;
        }
        //转换纬度
        private double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * this.PI) + 20.0 * Math.Sin(2.0 * x * this.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * this.PI) + 40.0 * Math.Sin(y / 3.0 * this.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * this.PI) + 320 * Math.Sin(y * this.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }
    }
}
