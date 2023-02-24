using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace XamarinGoogleMapSample.ViewModels
{
    public class Page_MVVMSampleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Properties
        private string _Title = "MVVM 지도 샘플";
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                OnPropertyChanged();
            }
        }

        private Xamarin.Forms.GoogleMaps.Map _CurrentMap;
        public Xamarin.Forms.GoogleMaps.Map CurrentMap
        {
            get => _CurrentMap;
            set
            {
                _CurrentMap = value;
                OnPropertyChanged();
            }
        }

        private Xamarin.Forms.GoogleMaps.Position _CurrentPosition;
        public Xamarin.Forms.GoogleMaps.Position CurrentPosition
        {
            get => _CurrentPosition;
            set
            {
                _CurrentPosition = value;
                OnPropertyChanged();
            }
        }

        private Xamarin.Forms.GoogleMaps.Distance _CurrentDistance;
        public Xamarin.Forms.GoogleMaps.Distance CurrentDistance
        {
            get => _CurrentDistance;
            set
            {
                _CurrentDistance = value;
                OnPropertyChanged();
            }
        }

        private int _CurrentZoomLevel;
        public int CurrentZoomLevel
        {
            get => _CurrentZoomLevel;
            set
            {
                if (_CurrentZoomLevel != value)
                {
                    _CurrentZoomLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public Page_MVVMSampleViewModel()
        {
            double sampleLat = 37.54815;
            double smapleLot = 126.9207986;
            CurrentPosition = new Xamarin.Forms.GoogleMaps.Position(sampleLat, smapleLot);

            var Span = Xamarin.Forms.GoogleMaps.MapSpan.FromCenterAndRadius(CurrentPosition, Xamarin.Forms.GoogleMaps.Distance.FromKilometers(10));
            CurrentDistance = Span.Radius;
            CurrentMap = new Xamarin.Forms.GoogleMaps.Map();
            CurrentMap.SelectedPinChanged += CurrentMap_SelectedPinChanged;
            CurrentMap.MoveToRegion(Span, false);
            CurrentMap.CameraIdled += CurrentMap_CameraIdled;
        }

        private void CurrentMap_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            var position = e.Position.Target;
            CurrentPosition = position;
            CurrentZoomLevel = (int)e.Position.Zoom;
            CurrentDistance = CurrentMap.VisibleRegion.Radius;
#if DEBUG
            System.Diagnostics.Debug.WriteLine("현재 줌 레벨 : " + CurrentZoomLevel);
#endif
        }

        private void CurrentMap_SelectedPinChanged(object sender, SelectedPinChangedEventArgs e)
        {
            if (e.SelectedPin != null)
            {

            }
        }
    }
}
