using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamarinGoogleMapSample.Controls;
using XamarinGoogleMapSample.Droid;
using Xamarin.Forms.GoogleMaps;
using Android.Locations;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XamarinGoogleMapSample.Droid
{
    public class CustomMapRenderer : Xamarin.Forms.GoogleMaps.Android.MapRenderer
    {
        Android.Content.Context _context;
        public CustomMapRenderer(Android.Content.Context context) : base(context)
        {
            _context = context;
        }
        protected override void OnMapReady(GoogleMap nativeMap, Map map)
        {
            base.OnMapReady(nativeMap, map);
            nativeMap.SetMaxZoomPreference(17);
            nativeMap.SetMinZoomPreference(11);
            nativeMap.UiSettings.ZoomControlsEnabled = false;
            nativeMap.UiSettings.MyLocationButtonEnabled = true;
            nativeMap.UiSettings.MapToolbarEnabled = false;

            try
            {
                bool success = nativeMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(_context, Resource.Raw.map_style));

                if (!success)
                {
                    Android.Util.Log.Error("MAP ERROR", "스타일을 불러오지 못했습니다");
                }
            }
            catch (Android.Content.Res.Resources.NotFoundException e)
            {
                Android.Util.Log.Error("MAP ERROR", $"스타일 자체가 존재하지 않습니다");
            }
        }

        private double GetRadius()
        {

            var visibleRegion = NativeMap.Projection.VisibleRegion;

            var farRight = visibleRegion.FarRight;
            var farLeft = visibleRegion.FarLeft;
            var nearRight = visibleRegion.NearRight;
            var nearLeft = visibleRegion.NearLeft;

            float[] distanceWidth = new float[2];
            Location.DistanceBetween(
                    (farRight.Latitude + nearRight.Latitude) / 2,
                    (farRight.Longitude + nearRight.Longitude) / 2,
                    (farLeft.Latitude + nearLeft.Latitude) / 2,
                    (farLeft.Longitude + nearLeft.Longitude) / 2,
                    distanceWidth
                    );


            float[] distanceHeight = new float[2];
            Location.DistanceBetween(
                    (farRight.Latitude + nearRight.Latitude) / 2,
                    (farRight.Longitude + nearRight.Longitude) / 2,
                    (farLeft.Latitude + nearLeft.Latitude) / 2,
                    (farLeft.Longitude + nearLeft.Longitude) / 2,
                    distanceHeight
            );

            if (distanceWidth[0] > distanceHeight[0])
            {
                return distanceWidth[0] / 1000.0 / 2.0;
            }
            else
            {
                return distanceHeight[0] / 1000.0 / 2.0;
            }
        }
    }
}