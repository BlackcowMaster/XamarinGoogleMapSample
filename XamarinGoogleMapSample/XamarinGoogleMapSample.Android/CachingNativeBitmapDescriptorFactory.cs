using System;
using System.Collections.Concurrent;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using Xamarin.Forms.GoogleMaps;
using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;

namespace XamarinGoogleMapSample.Droid
{
    public sealed class CachingNativeBitmapDescriptorFactory : IBitmapDescriptorFactory
    {
        private readonly ConcurrentDictionary<string, AndroidBitmapDescriptor> _cache = new ConcurrentDictionary<string, AndroidBitmapDescriptor>();

        public AndroidBitmapDescriptor ToNative(BitmapDescriptor desc)
        {
            var factory = DefaultBitmapDescriptorFactory.Instance;
            if (!string.IsNullOrEmpty(desc.Id))
            {
                var cache = _cache.GetOrAdd(desc.Id, _ => factory.ToNative(desc));
                return cache;
            }
            return factory.ToNative(desc);
        }
    }
}