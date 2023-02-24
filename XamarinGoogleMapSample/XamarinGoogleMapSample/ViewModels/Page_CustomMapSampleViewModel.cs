using FFImageLoading;
using FFImageLoading.Transformations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using XamarinGoogleMapSample.Controls;
using XamarinGoogleMapSample.Interfaces;
using static System.Net.WebRequestMethods;

namespace XamarinGoogleMapSample.ViewModels
{
    public class Page_CustomMapSampleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #region Command
        public Xamarin.Forms.Command ZoomInCommand { get; set; }
        public Xamarin.Forms.Command ZoomOutCommand { get; set; }
        public Xamarin.Forms.Command MyLocationCommand { get; set; }
        public Xamarin.Forms.Command CurrentLocationFindSpaceCommand { get; set; }

        public Xamarin.Forms.Command Sample1Command { get; set; }
        public Xamarin.Forms.Command Sample2Command { get; set; }
        public Xamarin.Forms.Command Sample3Command { get; set; }
        #endregion

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

        private CustomMap _CurrentMap;
        public CustomMap CurrentMap
        {
            get => _CurrentMap;
            set
            {
                _CurrentMap = value;
                OnPropertyChanged();
            }
        }

        private int _RuntimeSize;
        public int RuntimeSize
        {
            get => _RuntimeSize;
            set
            {
                _RuntimeSize = value;
                OnPropertyChanged();
            }
        }

        

        private bool _IsLoading;
        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                _IsLoading = value;
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

        private List<Models.CustomMapInfo> _CurrentPinList;
        public List<Models.CustomMapInfo> CurrentPinList
        {
            get => _CurrentPinList;
            set
            {
                _CurrentPinList = value;
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

        string jsonSample = "[  " +
                   "{    \"Name\": \"Gyeongbokgung Palace\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5788,    \"Longitude\": 126.9778  }, " +
                   "{    \"Name\": \"Namsan Tower\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5512,    \"Longitude\": 126.9882  },  " +
                   "{    \"Name\": \"Bukchon Hanok Village\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5823,    \"Longitude\": 126.9850  },  " +
                   "{    \"Name\": \"Dongdaemun Market\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5702,    \"Longitude\": 127.0106  },  " +
                   "{    \"Name\": \"Lotte World Tower\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5126,    \"Longitude\": 127.1025  },  " +
                   "{    \"Name\": \"Myeong-dong Shopping Street\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5635,    \"Longitude\": 126.9829  },  " +
                   "{    \"Name\": \"Gangnam Station\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.4982,    \"Longitude\": 127.0276  },  " +
                   "{    \"Name\": \"Seoul Forest\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5479,    \"Longitude\": 127.0443  },  " +
                   "{    \"Name\": \"Cheonggyecheon Stream\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5686,    \"Longitude\": 126.9999  }, " +
                   " {   \"Name\": \"Gwangjang Market\",    \"ImgPath\": \"https://i.pravatar.cc/300\",    \"Latitude\": 37.5709,    \"Longitude\": 126.9996  }]";

        public Page_CustomMapSampleViewModel()
        {
            double sampleLat = 37.54815;
            double smapleLot = 126.9207986;
            CurrentPosition = new Xamarin.Forms.GoogleMaps.Position(sampleLat, smapleLot);

            var Span = Xamarin.Forms.GoogleMaps.MapSpan.FromCenterAndRadius(CurrentPosition, Xamarin.Forms.GoogleMaps.Distance.FromKilometers(10));
            CurrentDistance = Span.Radius;
            CurrentMap = new CustomMap();
            CurrentMap.SelectedPinChanged += CurrentMap_SelectedPinChanged;
            CurrentMap.MoveToRegion(Span, false);
            CurrentMap.CameraIdled += CurrentMap_CameraIdled;

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                RuntimeSize = 70;
            }
            else
            {
                RuntimeSize = 140;
            }

            CurrentLocationFindSpaceCommand = new Command(async (obj) =>
            {
                IsLoading = true;
                GetPositionMarker();
                IsLoading = false;
            });

            MyLocationCommand = new Xamarin.Forms.Command(async (object obj) =>
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    var result = await App.Current.MainPage.DisplayAlert("안내", "현재 위치 권한을 허용하지 않았습니다. 다시 요청할까요", "확인", "취소");
                    if (result)
                    {
                        DependencyService.Get<IAppSettingsHelper>().OpenLocationSetting();
                    }
                }
                else
                {
                    IsLoading = true;
                    var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(10),
                    });
                    CurrentPosition = new Position(location.Latitude, location.Longitude);
                    var newSpan = Xamarin.Forms.GoogleMaps.MapSpan.FromCenterAndRadius(CurrentPosition, CurrentDistance);
                    IsLoading = false;
                    CurrentMap.MoveToRegion(newSpan, false);
                    GetPositionMarker();
                }

            });

            ZoomInCommand = new Xamarin.Forms.Command(async (obj) =>
            {
                await CurrentMap.MoveCamera(CameraUpdateFactory.NewPositionZoom(CurrentMap.CameraPosition.Target, CurrentMap.CameraPosition.Zoom + 1));
            });

            ZoomOutCommand = new Xamarin.Forms.Command(async (obj) =>
            {
                await CurrentMap.MoveCamera(CameraUpdateFactory.NewPositionZoom(CurrentMap.CameraPosition.Target, CurrentMap.CameraPosition.Zoom + -1));
            });

            Sample1Command = new Xamarin.Forms.Command(async (obj) =>
            {
                CurrentMap.Circles.Clear();
                CurrentMap.Pins.Clear();

                IsLoading = true;
                CurrentPosition = new Xamarin.Forms.GoogleMaps.Position(sampleLat, smapleLot);
                var dummyItems = JsonConvert.DeserializeObject<List<Models.CustomMapInfo>>(jsonSample);

                foreach (var item in dummyItems)
                {
                    var newPin = new Xamarin.Forms.GoogleMaps.Pin
                    {
                        Type = Xamarin.Forms.GoogleMaps.PinType.Generic,
                        Position = new Xamarin.Forms.GoogleMaps.Position(item.Latitude, item.Longitude),
                        Label = item.Name,
                        Tag = item,
                        Anchor = new Xamarin.Forms.Point(0.5f, 0.5f)
                    };
                    CurrentMap.Pins.Add(newPin);
                }                
                CurrentPinList = dummyItems;
                IsLoading = false;
                CurrentMap.MoveToRegion(FromPositions(), false);

                if (CurrentPinList.Count <= 0)
                {
                    await App.Current.MainPage.DisplayAlert("안내", "현재 위치에 등록 된 공간이 없습니다","확인"); 
                }
            });
            Sample2Command = new Xamarin.Forms.Command(async (obj) =>
            {
                DrawCircle();
            });
            Sample3Command = new Xamarin.Forms.Command(async (obj) =>
            {
                CurrentMap.Circles.Clear();
                CurrentMap.Pins.Clear();

                IsLoading = true;
                CurrentPosition = new Xamarin.Forms.GoogleMaps.Position(sampleLat, smapleLot);
                var dummyItems = JsonConvert.DeserializeObject<List<Models.CustomMapInfo>>(jsonSample);
                var circleTransformation = new CircleTransformation { BorderHexColor = "#555555", BorderSize = 10 };
                int count = 0;
                foreach (var item in dummyItems)
                {
                    ++count;
                    var circleImageVariable = await FFImageLoading.ImageService.Instance
                        .LoadUrl(item.ImgPath+$"?image={count}")
                        .DownSample(RuntimeSize, RuntimeSize)
                        .Transform(circleTransformation)
                        .AsPNGStreamAsync();

                    var newPin = new Xamarin.Forms.GoogleMaps.Pin
                    {
                        Icon = Xamarin.Forms.GoogleMaps.BitmapDescriptorFactory.FromStream(circleImageVariable),
                        Type = Xamarin.Forms.GoogleMaps.PinType.Generic,
                        Position = new Xamarin.Forms.GoogleMaps.Position(item.Latitude, item.Longitude),
                        Label = item.Name,
                        Tag = item,
                        Anchor = new Xamarin.Forms.Point(0.5f, 0.5f)
                    };
                    CurrentMap.Pins.Add(newPin);
                }
                CurrentPinList = dummyItems;
                IsLoading = false;
                CurrentMap.MoveToRegion(FromPositions(), false);

                if (CurrentPinList.Count <= 0)
                {
                    await App.Current.MainPage.DisplayAlert("안내", "현재 위치에 등록 된 공간이 없습니다", "확인");
                }
                else
                {

                    DrawCircle();
                }
            });
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

        private void DrawCircle()
        {
            CurrentMap.Circles.Clear();

            var cicleInstance = new Circle();
            cicleInstance.StrokeWidth = 1f;
            cicleInstance.StrokeColor = Color.FromRgba(50, 0, 0, 20);
            cicleInstance.FillColor = Color.FromRgba(10, 0, 0, 20);

            if (CurrentMap.VisibleRegion == null)
            {
                var spanData = Xamarin.Forms.GoogleMaps.MapSpan.FromCenterAndRadius(CurrentPosition, CurrentDistance);
                cicleInstance.Center = spanData.Center;
                cicleInstance.Radius = new Distance(spanData.Radius.Meters * 1.2);
            }
            else
            {
                cicleInstance.Center = CurrentMap.VisibleRegion.Center;
                cicleInstance.Radius = new Distance(CurrentDistance.Meters * 1.2);
            }
            CurrentMap.Circles.Add(cicleInstance);
        }

        private MapSpan FromPositions()
        {

            double minLat = double.MaxValue;
            double minLon = double.MaxValue;
            double maxLat = double.MinValue;
            double maxLon = double.MinValue;

            foreach (var p in CurrentPinList)
            {
                minLat = Math.Min(minLat, p.Latitude);
                minLon = Math.Min(minLon, p.Longitude);
                maxLat = Math.Max(maxLat, p.Latitude);
                maxLon = Math.Max(maxLon, p.Longitude);
            }

            return new MapSpan(
                new Position((minLat + maxLat) / 2d, (minLon + maxLon) / 2d),
                maxLat - minLat,
                maxLon - minLon);
        }

        private async void GetPositionMarker()
        {
            bool testSuccess = true; //여기에서 서버랑 통신을 해야 됨
            CurrentMap.Circles.Clear();
            CurrentMap.Pins.Clear();
            //CurrentPinList = new List<MapPositionInfo>();
        }
    }
}
