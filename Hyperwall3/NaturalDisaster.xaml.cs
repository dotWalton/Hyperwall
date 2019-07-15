using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Rasters;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Hyperwall3
{
    /// <summary>
    /// UserControl class for creating Natural Disaster Before/After Scenes
    /// </summary>
    public partial class NaturalDisaster : UserControl
    {
        public NaturalDisaster()
        {
            InitializeComponent();
            Initialize();
            thumb.RenderTransform = new TranslateTransform() { X = 0, Y = 0 };
            NaturalDisasterAfter.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, 0, 0) };
            NaturalDisasterBefore.ViewpointChanged += ViewpointChanged;
            InfoText.Opacity = 0.25;
        }

        // This event handler was originally written by Thad Tilton with the Runtime SDK for .NET team at Esri and adapted to fit my system
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            try
            {
                var transform = (TranslateTransform)thumb.RenderTransform;
                transform.X = Math.Max(0, Math.Min(transform.X + e.HorizontalChange, this.ActualWidth - thumb.ActualWidth));
                NaturalDisasterAfter.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, transform.X, this.ActualHeight) };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // Syncs camera between Before and After Map
        private void ViewpointChanged(object sender, EventArgs e)
        {
            try
            {
                Camera beforeCamera = NaturalDisasterBefore.Camera;
                if (beforeCamera != null)
                    NaturalDisasterAfter.SetViewpointCamera(beforeCamera);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // Event handler corresponds to ComboBox on NaturalDisasterView.xaml
        private void OnBookmarkChooserSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected bookmark and apply the view point to the map
            Bookmark selectedBookmark = (Bookmark)e.AddedItems[0];
            Camera cam1 = NaturalDisasterBefore.Camera;
            NaturalDisasterBefore.SetViewpoint(selectedBookmark.Viewpoint);
        }

        // Initialize creates the Map objects, assigns rasters, and creates bookmarks
        private async void Initialize()
        {
            // add imagery basemap to both scenes
            Scene BeforeMap = new Scene(Basemap.CreateImageryWithLabels());
            Scene AfterMap = new Scene(Basemap.CreateImageryWithLabels());

            // wait for scenes to load
            await BeforeMap.LoadAsync();
            await AfterMap.LoadAsync();

            // Assigns Scene objects to View
            NaturalDisasterBefore.Scene = BeforeMap;
            NaturalDisasterAfter.Scene = AfterMap;
            
            // List containing paths to each "before" raster
            string[] beforeimages;
            String fpath = Path.Combine(Directory.GetCurrentDirectory(), "BeforeImages\\");

            beforeimages = Directory.GetFiles(fpath, "*", SearchOption.AllDirectories).Select(x => Path.GetFileName(x)).ToArray();

            // List containing paths to each "after" raster
            string[] afterimages;
            String fpath2 = Path.Combine(Directory.GetCurrentDirectory(), "AfterImages\\");

            afterimages = Directory.GetFiles(fpath2, "*", SearchOption.AllDirectories).Select(x => Path.GetFileName(x)).ToArray();

            // Iterate through "before" raster list and add each one to the BeforeMap
            foreach (var item in beforeimages)
            {
                // specify filepath to raster location
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "BeforeImages\\" + item);

                // Load the raster file
                Raster myRasterFile = new Raster(filepath);

                // Create the layer
                RasterLayer myRasterLayer = new RasterLayer(myRasterFile);

                // Add the layer and bookmark to the map
                BeforeMap.OperationalLayers.Add(myRasterLayer);

                // Wait for the layer to load
                await myRasterLayer.LoadAsync();

                //Creates an envelope for the current Raster
                var rasterGeometry = myRasterLayer.FullExtent;
                EnvelopeBuilder newEnvelope = new EnvelopeBuilder(rasterGeometry);
                newEnvelope.Expand(1.5);

                // Creates an envelope for comparison to bookmark location
                EnvelopeBuilder textEnvelope = new EnvelopeBuilder(rasterGeometry);
                textEnvelope.Expand(2.5);

                var xMax = newEnvelope.XMax;
                var yMax = newEnvelope.YMax;
                var xMin = newEnvelope.XMin;
                var yMin = newEnvelope.YMin;
                var spatialreference = newEnvelope.SpatialReference;

                // Converts newEnvelope to a geometry object that can be read as a Viewpoint
                Envelope rasterEnvelope = new Envelope(xMin, yMin, xMax, yMax, spatialreference);

                // Create Bookmark location and name for current raster
                // Raster needs spatial reference to load
                try
                {
                    if (rasterEnvelope.SpatialReference != null)
                    {
                        Viewpoint viewpoint = new Viewpoint(rasterEnvelope);
                        Bookmark bookmark = new Bookmark
                        {
                            Name = Path.GetFileNameWithoutExtension(item),
                            Viewpoint = viewpoint
                        };
                        NaturalDisasterBefore.Scene.Bookmarks.Add(bookmark);
                        BookmarkChooser.Items.Add(bookmark);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        
            // Iterate through "after" raster list and add each one to the AfterMap
            // Same as the one for the BeforeMap
            foreach (var item in afterimages)
            {
                // specify filepath to raster location
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "AfterImages\\" + item);

                // Load the raster file
                Raster myRasterFile = new Raster(filepath);

                // Create the layer
                RasterLayer myRasterLayer = new RasterLayer(myRasterFile);

                // Add the layer and bookmark to the map
                AfterMap.OperationalLayers.Add(myRasterLayer);

                // Wait for the layer to load
                await myRasterLayer.LoadAsync();

            }
        }
    }
}
