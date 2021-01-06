using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfAnimatedGif;
using System.Timers;
using System.Windows.Threading;

namespace Weather_App
{
    public partial class MainWindow : Window
    {
        const string APP_ID = "857b10b83e76a76f33a0e45d56e85b65"; // ( openweather API )
        const string ID = "1735227"; // Kuantan openweatherid
        const string islamprayerzone = "PHG02";
        const string timezoneformat = "12-hour";

        private void CityName_data(){
            try{
                string query = String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&appid={1}&units=metric", ID, APP_ID);
                JObject response = JObject.Parse(new System.Net.WebClient().DownloadString(query));
                if (response.SelectToken("cod").ToString().Equals("200")){
                    displayWeatherImage(Convert.ToInt32(response.SelectToken("weather[0].id").ToString()), response.SelectToken("weather[0].icon").ToString());
                    lblCityAndCountry.Content = response.SelectToken("name").ToString() + ", Malaysia"; // + response.SelectToken("sys.country").ToString();
                    lblWeather.Content = response.SelectToken("main.temp").ToString() + " °C, " + response.SelectToken("weather[0].main").ToString();
                    lblWeatherDescription.Content = response.SelectToken("weather[0].description").ToString();
                }else if (response.SelectToken("cod").ToString().Equals("429")){
                        MessageBox.Show("The account is temporary blocked due to exceeding the requests limitition.\nPlease try agian later.");
                }
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void islamPrayer_data(){
            try
            {
                string query = String.Format("http://api.azanpro.com/times/today.json?zone={0}&format={1}", islamprayerzone, timezoneformat);
                JObject response = JObject.Parse(new System.Net.WebClient().DownloadString(query));
                lblPrayer.Content = "Waktu Solat";
                lblPrayerDescription.Content =  "Imsak : " + response.SelectToken("prayer_times.imsak") + Environment.NewLine +
                                                "Subuh : " + response.SelectToken("prayer_times.subuh") + Environment.NewLine +
                                                "Syuruk : " + response.SelectToken("prayer_times.syuruk") + Environment.NewLine +
                                                "Zohor : " + response.SelectToken("prayer_times.zohor") + Environment.NewLine +
                                                "Asar : " + response.SelectToken("prayer_times.asar") + Environment.NewLine +
                                                "Maghrib : " + response.SelectToken("prayer_times.maghrib") + Environment.NewLine +
                                                "Isyak : " + response.SelectToken("prayer_times.isyak") + Environment.NewLine;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void displayWeatherImage(int weatherId, string daynight)
        {
            var image = new BitmapImage();
            image.BeginInit();

            if (weatherId >= 200 && weatherId <= 232){
                if(daynight == "11d"){
                    image.UriSource = new Uri("../../../icons/11d.gif", UriKind.Relative);
                }else if(daynight == "11n"){
                    image.UriSource = new Uri("../../../icons/11n.gif", UriKind.Relative);
                }
            }else if (weatherId >= 300 && weatherId <= 321){
                if (daynight == "09d"){
                    image.UriSource = new Uri("../../../icons/09d.gif", UriKind.Relative);
                }else if (daynight == "09n"){
                    image.UriSource = new Uri("../../../icons/09n.gif", UriKind.Relative);
                }
            }
            else if (weatherId >= 500 && weatherId <= 531){
                if (daynight == "10d"){
                    image.UriSource = new Uri("../../../icons/10d.gif", UriKind.Relative);
                }else if (daynight == "10n"){
                    image.UriSource = new Uri("../../../icons/10n.gif", UriKind.Relative);
                }
            }
            else if (weatherId >= 701 && weatherId <= 781){
                if (daynight == "50d"){
                    image.UriSource = new Uri("../../../icons/50d.gif", UriKind.Relative);
                }else if (daynight == "50n"){
                    image.UriSource = new Uri("../../../icons/50n.gif", UriKind.Relative);
                }
            }
            else if (weatherId == 800){
                if (daynight == "01d"){
                    image.UriSource = new Uri("../../../icons/01d.gif", UriKind.Relative);
                } else if (daynight == "01n"){
                    image.UriSource = new Uri("../../../icons/01n.gif", UriKind.Relative);
                }
            }
            else if (weatherId >= 801 && weatherId <= 804){
                if (daynight == "02d" || daynight == "03d" || daynight == "04d"){
                    image.UriSource = new Uri("../../../icons/0234d.gif", UriKind.Relative);
                }else if (daynight == "02n" || daynight == "03n" || daynight == "04n"){
                    image.UriSource = new Uri("../../../icons/0234n.gif", UriKind.Relative);
                }
            }

            image.EndInit();
            ImageBehavior.SetAnimatedSource(icon, image);
            ImageBehavior.SetRepeatBehavior(icon, RepeatBehavior.Forever);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e){
            CityName_data();
        }


        public MainWindow()
        {
            InitializeComponent();
            CityName_data();
            islamPrayer_data();

            //  DispatcherTimer updates weather api every 2 minutes
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
        }
    }
}
