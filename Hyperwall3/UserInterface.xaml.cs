using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;

namespace Hyperwall3
{
    /// <summary>
    /// The UserInterface acts as the controller for the entire application, it handles all object creation
    /// and event handling between the exhibit's touchscreen and video wall
    /// </summary>
    public partial class UserInterface : Window
    { 
        // Object creation for UserInterface Window (MapName1) And the VideoWall Window (MapName2)
        NaturalDisaster NaturalDisaster1 = new NaturalDisaster();
        NaturalDisaster NaturalDisaster2 = new NaturalDisaster();
        AirQuality AirQuality1 = new AirQuality();
        AirQuality AirQuality2 = new AirQuality();
        HeatVulnerability HeatVulnerability1 = new HeatVulnerability();
        HeatVulnerability HeatVulnerability2 = new HeatVulnerability();
        VideoWall videoWall = new VideoWall();

        public UserInterface()
        {
            InitializeComponent();
            // Fires the ViewPointChanged events whenever UserInterface ViewPoint Changes
            if (AirQuality1.AirMap != null)
            {
                AirQuality1.AirMap.ViewpointChanged += ViewpointChanged;
            }
            if (HeatVulnerability1.HeatVuln != null)
            {
                HeatVulnerability1.HeatVuln.ViewpointChanged += ViewpointChanged1;
            }
            if (NaturalDisaster1.NaturalDisasterBefore.Camera != null)
            {
                NaturalDisaster1.NaturalDisasterBefore.ViewpointChanged += CameraChanged;
                NaturalDisaster1.thumb.DragDelta += ThumbChanged;
            }

            // Syncs Click Events between Windows
            AirQuality1.AQLatest.Click += videoWallChangeLayerAQLatestVis;
            AirQuality1.AQToday.Click += videoWallChangeLayerAQTodayVis;
            HeatVulnerability1.AvgTempButton.Click += AvgTempButton_Click;
            HeatVulnerability1.HeatVulnButton.Click += HeatVulnButton_Click;
            HeatVulnerability1.ImperviousSurfacesButton.Click += ImperviousSurfacesButton_Click;
            HeatVulnerability1.PovertyButton.Click += PovertyButton_Click;
            HeatVulnerability1.TreeCanopyButton.Click += TreeCanopyButton_Click;
            // Method for creating video wall display on secondary monitor
            // ADD OR REMOVE "//" TO FRONT OF "VideoWallShow()" BELOW TO TURN VIDEOWALL ON/OFF
            //VideoWallShow();
        }

        // These events sync the VideoWall with the UserInterface Window
        private void TreeCanopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (HeatVulnerability1.HeatVuln.Map.OperationalLayers[2].IsVisible == false)
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[2].IsVisible = false;
            }
            else
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[0].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[1].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[2].IsVisible = true;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[3].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[4].IsVisible = false;
            }
        }
        private void PovertyButton_Click(object sender, RoutedEventArgs e)
        {
            if (HeatVulnerability1.HeatVuln.Map.OperationalLayers[4].IsVisible == false)
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[4].IsVisible = false;
            }
            else
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[0].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[1].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[4].IsVisible = true;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[3].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[2].IsVisible = false;
            }
        }
        private void ImperviousSurfacesButton_Click(object sender, RoutedEventArgs e)
        {
            if (HeatVulnerability1.HeatVuln.Map.OperationalLayers[1].IsVisible == false)
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[1].IsVisible = false;
            }
            else
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[0].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[2].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[1].IsVisible = true;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[3].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[4].IsVisible = false;
            }
        }
        private void HeatVulnButton_Click(object sender, RoutedEventArgs e)
        {
            if (HeatVulnerability1.HeatVuln.Map.OperationalLayers[3].IsVisible == false)
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[3].IsVisible = false;
            }
            else
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[0].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[1].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[3].IsVisible = true;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[2].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[4].IsVisible = false;
            }
        }
        private void AvgTempButton_Click(object sender, RoutedEventArgs e)
        {
            if (HeatVulnerability1.HeatVuln.Map.OperationalLayers[0].IsVisible == false)
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[0].IsVisible = false;
            }
            else
            {
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[2].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[1].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[0].IsVisible = true;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[3].IsVisible = false;
                HeatVulnerability2.HeatVuln.Map.OperationalLayers[4].IsVisible = false;
            }
        }
        private void videoWallChangeLayerAQLatestVis(object sender, RoutedEventArgs e)
        {
            if (AirQuality1.AirMap.Map.OperationalLayers[0].IsVisible == false)
            {
                AirQuality2.AirMap.Map.OperationalLayers[0].IsVisible = false;
            }
            else
            {
                AirQuality2.AirMap.Map.OperationalLayers[0].IsVisible = true;
            }
        }
        private void videoWallChangeLayerAQTodayVis(object sender, RoutedEventArgs e)
        {
            if (AirQuality1.AirMap.Map.OperationalLayers[1].IsVisible == false)
            {
                AirQuality2.AirMap.Map.OperationalLayers[1].IsVisible = false;
            }
            else
            {
                AirQuality2.AirMap.Map.OperationalLayers[1].IsVisible = true;
            }
        }

        // Creates the Video Wall display Window and attaches it to the computers secondary screen
        public void VideoWallShow()
        {
            try
            {
                var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
                var secondaryScreen = Screen.AllScreens.First(screen => screen != primaryScreen);
                videoWall.Left = secondaryScreen.Bounds.Left;
                videoWall.Top = secondaryScreen.Bounds.Top;
                videoWall.Width = secondaryScreen.Bounds.Width;
                videoWall.Height = secondaryScreen.Bounds.Height;
                videoWall.WindowState = WindowState.Normal;
                videoWall.Loaded += (_s, _e) => videoWall.WindowState = WindowState.Maximized;
                videoWall.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
                
        // Opens the Air Quality Map and sends it to the video wall mapview
        private void AirQualityMap_Click(object sender, RoutedEventArgs e)
        {
            ActiveMap.Content = AirQuality1;
            videoWall.VideoWallDisplay.Content = AirQuality2;
            AirQuality2.AQLatest.Visibility = Visibility.Collapsed;
            AirQuality2.AQToday.Visibility = Visibility.Collapsed;
        }

        // Opens the Heat Vulnerability mmpk and gives it a basemap
        private void HeatVulnerabilityMap_Click(object sender, RoutedEventArgs e)
        {
            ActiveMap.Content = HeatVulnerability1;
            videoWall.VideoWallDisplay.Content = HeatVulnerability2;
            HeatVulnerability2.HeatVulnButton.Visibility = Visibility.Collapsed;
            HeatVulnerability2.ImperviousSurfacesButton.Visibility = Visibility.Collapsed;
            HeatVulnerability2.AvgTempButton.Visibility = Visibility.Collapsed;
            HeatVulnerability2.PovertyButton.Visibility = Visibility.Collapsed;
            HeatVulnerability2.TreeCanopyButton.Visibility = Visibility.Collapsed;

        }

        // Opens the Natural Disaster Scenes
        private void NaturalDisasterMap_Click(object sender, RoutedEventArgs e)
        {
            ActiveMap.Content = NaturalDisaster1;
            videoWall.VideoWallDisplay.Content = NaturalDisaster2;
            NaturalDisaster2.BookmarkChooser.Visibility = Visibility.Collapsed;
        }

        // syncs the UI's view and the Video Wall's viewpoints as needed
        private void ViewpointChanged(object sender, EventArgs e)
        {
            try
            {
                Viewpoint UIMapViewpoint = AirQuality1.AirMap.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
                var UIMapViewGeometry = UIMapViewpoint.TargetGeometry.Extent;
                EnvelopeBuilder newEnvelope = new EnvelopeBuilder(UIMapViewGeometry);
                // The video walls map extent is 1.4x the UI's extent
                newEnvelope.Expand(1.4);
                AirQuality2.AirMap.SetViewpoint(new Viewpoint(newEnvelope.Extent));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ViewpointChanged1(object sender, EventArgs e)
        {
            try
            {
                Viewpoint UIMapViewpoint = HeatVulnerability1.HeatVuln.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
                var UIMapViewGeometry = UIMapViewpoint.TargetGeometry.Extent;
                EnvelopeBuilder newEnvelope = new EnvelopeBuilder(UIMapViewGeometry);
                // The video walls map extent is 1.4x the UI's extent
                newEnvelope.Expand(1.4);
                HeatVulnerability2.HeatVuln.SetViewpoint(new Viewpoint(newEnvelope.Extent));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // syncs the UI's camera and the video wall's camera as needed
        private void CameraChanged(object sender, EventArgs e)
        {
            try
            {
                Camera cam1 = NaturalDisaster1.NaturalDisasterBefore.Camera;
                cam1.Elevate(100000);
                if (cam1 != null)
                {
                    NaturalDisaster2.NaturalDisasterBefore.SetViewpointCamera(cam1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // syncs the thumb drag from NaturalDisaster1 and 2
        private void ThumbChanged(object sender, DragDeltaEventArgs e)
        {
            var transform = (TranslateTransform)NaturalDisaster2.thumb.RenderTransform;
            transform.X = Math.Max(0, Math.Min(transform.X + e.HorizontalChange, 
                NaturalDisaster2.ActualWidth - (NaturalDisaster2.thumb.ActualWidth - 8)));
            NaturalDisaster2.NaturalDisasterAfter.Clip = new RectangleGeometry()
            { Rect = new Rect(0, 0, transform.X, NaturalDisaster2.ActualHeight) };
        }
    }
}