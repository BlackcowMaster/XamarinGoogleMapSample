# XamarinGoogleMapSample

해당 프로젝트는 https://github.com/INHack20/Xamarin.Forms.GoogleMaps 를 기반으로 제작 된 샘플 프로젝트입니다
실제 모든 기능을 구현하지 않았지만 누군가에겐 도움 되셨으면 좋겠습니다.
MAUI 샘플에서 뵙겠습니다.

궁금하신 점은 한국 자마린 & 마우이 단톡방을 이용해주십시오
https://open.kakao.com/o/g8lzoOm

[개발 참고사항]
- Circle은 보여지는 부분보다 크게 비율을 잡아서 그리는게 필요합니다. 원을 그린다고 해도 실제 핀들의 중심보다 작게 그립니다
- 초심자가 보고 이해할 수 있도록 MVVM 버전과 그렇지 않은 버전을 분리했습니다
- 현재 위치에서 재검색은 서버와 통신하신 후에 가져오고 그리고 중심점과 원, 핀 등을 그리시면 되겠습니다
- CustomMap이란 걸 별도로 구현하여 Android에 적용하였습니다

[Android]

1. Manifest에 권한 설정이 되어 있으며 "APIKEY"를 구글 개발자 콘솔에서 맵 API를 발급 하신 후 교체하시면 됩니다
   <meta-data android:name="com.google.android.geo.API_KEY" android:value="APIKEY" />

2. MainActivity.cs 파일에서 다음과 같이 선언이 필요합니다.
   Xamarin.FormsGoogleMaps.Init(this, saveInstanceState);
   
3. 안드로이드 프로젝트 Nuget에서 Plugin.CurrentActivity 를 다운 받고 다음과 같이 선언합니다
   Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
   
4. CachingNativeBitmapDescriptorFactory를 추가로 구현되어 있지만 FFImageLoading을 사용하는 경우엔 크게 필요하진 않습니다

[iOS]
1. App.Delegate.cs에 다음과 같이 선언해주십시오
-> Xamarin.FormsGoogleMaps.Init("API KEY"); // Google Maps API for iOS 생성

2. Info.plist에서 다음과 같이 
-> 개인정보 - 위치 상요 설명
-> 개인정보 - 위치 사용시 사용 설명
-> 개인정보 - 위치 항상 사용 및 위치 사용 시 설명
-> 개인정보 - Location Always 사용 설명
