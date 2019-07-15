using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System.Windows;
using System.Windows.Controls;

namespace Hyperwall3
{
    /// <summary>
    /// This UserControl accesses the AirQualityMap class
    /// </summary>
    public partial class AirQuality : UserControl
    {
        public AirQuality()
        {
            InitializeComponent();
            Initialize();
        }
        
        // Creates Air Quality Map
        public void Initialize()
        {
            var mapClass = new MapClasses.AirQualityMap();
            var airQualityContour = mapClass.AirNowLatest;
            var airQualityCities = mapClass.AirNowTodaysForecast;
            Map airMap = new Map(Basemap.CreateDarkGrayCanvasVector());
            airMap.OperationalLayers.Add(airQualityContour);
            airMap.OperationalLayers.Add(airQualityCities);
            AirMap.Map = airMap;
            AirMap.Map.InitialViewpoint = new Viewpoint(new Envelope(-134.44, 12.8577894, -57.1276444, 57.91, new SpatialReference(4326)));
        }

        // Toggles Contours on/off
        private void AQLatest_Click(object sender, RoutedEventArgs e)
        {
            if (AirMap.Map.OperationalLayers[0].IsVisible == false)
            {
                AirMap.Map.OperationalLayers[0].IsVisible = true;
            }
            else
            {
                AirMap.Map.OperationalLayers[0].IsVisible = false;
            }
        }

        // Toggles Sensor Points on/off
        private void AQToday_Click(object sender, RoutedEventArgs e)
        {
            if (AirMap.Map.OperationalLayers[1].IsVisible == false)
            {
                AirMap.Map.OperationalLayers[1].IsVisible = true;
            }
            else
            {
                AirMap.Map.OperationalLayers[1].IsVisible = false;
            }
        }
    }
}
