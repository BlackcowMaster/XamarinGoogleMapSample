using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinGoogleMapSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void btnSample1_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Views.Page_ContentViewSample());
        }

        private void btnSample2_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Views.Page_MVVMSample());
        }
        private void btnSample3_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Views.Page_CustomMapSample());
        }
    }
}
