using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Mapping;

namespace Hyperwall3.MapClasses
{
    /// <summary>
    /// This class opens the EPA's Air Quality Now Web Map from ArcGIS Online
    /// </summary>
    public class AirQualityMap : INotifyPropertyChanged
    {
        // Property for REST endpoint for the "Ozone and PM (PM2.5 and PM10) - Current" Contours
        private Layer _AirNowLatest_Combined = new FeatureLayer(new Uri(
                "https://services.arcgis.com/cJ9YHowT8TU7DUyn/arcgis/rest/services/AirNowLatestContoursCombined/FeatureServer/0"));
        public Layer AirNowLatest
        {
            get { return _AirNowLatest_Combined;}
            set {_AirNowLatest_Combined = value;
                OnPropertyChanged();}
        }

        // "Ozone and PM (PM2.5 and PM10) - Today's Forecast" Sensor Locations
        private Layer _AirNowTodaysForecast = new FeatureLayer(new Uri(
                "https://services.arcgis.com/cJ9YHowT8TU7DUyn/arcgis/rest/services/Air_Now_Current_Monitors_Ozone_and_PM/FeatureServer/0"));
        public Layer AirNowTodaysForecast
        {
            get { return _AirNowTodaysForecast; }
            set { _AirNowTodaysForecast = value;
                OnPropertyChanged();}
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
