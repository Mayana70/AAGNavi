using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace AAGNavi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class Page2 : ContentPage
    {
        public const double EARTHRADIUSM = 6378137; //Erdradius
        public Page2()
        {
            InitializeComponent();

        }

        public async void btn_clicked(object sender, System.EventArgs e)
        {
            Location Posit1 = new Location();
            Location Posit2 = new Location();
            Location Posit3 = new Location();
            Posit1.Latitude = 49.039544;   // Stolzenberger
            Posit1.Longitude = 9.094320;

            Posit2.Latitude = 49.03748;   // Levin
            Posit2.Longitude = 9.089379;

            Posit3.Latitude = 49.035169;   // Josia
            Posit3.Longitude = 9.09200;

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Posit1.Latitude = 49.039544;   // Stolzenberger
                    Posit1.Longitude = 9.094320;

                    Posit2.Latitude = 49.03748;   // Levin
                    Posit2.Longitude = 9.089379;

                    Posit3.Latitude = 49.035169;   // Josia
                    Posit3.Longitude = 9.09200;

                    lblheader.Text = "Deine letzten Koordinaten, die ich habe, sind:";
                    lblLat.Text = "Breite = "+ location.Latitude.ToString("###.0000") + " Nord";
                    lblLong.Text = "Länge = "+ location.Longitude.ToString("###.0000") + " Ost";
                    lblAlt.Text = "Höhe = " + Convert.ToInt16(location.Altitude).ToString() + " ü. NN";
                    double Abstand1 = new double();
                    double Abstand2 = new double();
                    double lat1 = new double();
                    double lat2 = new double();
                    Abstand1 = 1000*Location.CalculateDistance(Posit1, location, DistanceUnits.Kilometers);
                    Abstand2 = 1000*Location.CalculateDistance(Posit2, location, DistanceUnits.Kilometers);
                    // nun händische Berechnung des Kurswinkels
                    double loclat = location.Latitude / 180.0 * Math.PI; // Breitengrad der Position des Handies

                    lat1 = Posit1.Latitude / 180.0 * Math.PI;    // Breitengrad der Zielposition1
                    lat2 = Posit2.Latitude / 180.0 * Math.PI;    // Breitengrad der Zielposition2
                    double winkel1 = new double(); winkel1 = 0;
                    double alpha1 = new double(); alpha1 = 0;
                    double Nenner1 = new double(); Nenner1 = 0.0001;
                    Nenner1 = (Math.Cos(loclat) * Math.Sin(Abstand1 / EARTHRADIUSM));
                    if (Nenner1 == 0) { Nenner1 = 0.00001; }

                    alpha1 = (Math.Acos(
                        (Math.Sin(lat1) - Math.Sin(loclat) * Math.Cos(Abstand1/ EARTHRADIUSM)) /
                        (Math.Cos(loclat) * Math.Sin(Abstand1/ EARTHRADIUSM)))) * 180.0 / Math.PI;
                    if ((double.IsInfinity(alpha1)) || (double.IsNaN(alpha1)))
                    {
                        alpha1 = 180;
                    }

                    //Console.WriteLine("winkel zu Posit 1 =" + alpha1);
                    if (location.Longitude > Posit1.Longitude) { winkel1 = 360 - alpha1; } else { winkel1 = alpha1; }
                    
                    double winkel2 = new double(); winkel2 = 0;
                    double alpha2 = new double(); alpha2 = 0;
                    double Nenner2 = new double(); Nenner2 = 0.0001;
                    Nenner2 = (Math.Cos(loclat) * Math.Sin(Abstand2 / EARTHRADIUSM));
                    if (Math.Abs(Nenner2) > 0.0001) { } else { Nenner2 = 0.0001; }
                    alpha2 = (Math.Acos(
                         (Math.Sin(lat2) - Math.Sin(loclat) * Math.Cos(Abstand2 / EARTHRADIUSM)) /
                         Nenner2)) * 180.0 / Math.PI;
                    if ((double.IsInfinity(alpha2)) || (double.IsNaN(alpha2)))
                    {
                        alpha2 = 180;
                    }
                    if (location.Longitude > Posit2.Longitude) { winkel2 = 360 - alpha2; } else { winkel2 = alpha2; }
                    Int16 Richtung1 = new Int16(); Richtung1 = 20;  // Himmelsrichtung von 0 ab im Uhrzeigersinn
                    Richtung1 = Convert.ToInt16(winkel1 / 45);
                    Int16 Richtung2 = new Int16(); Richtung2 = 20;  // Himmelsrichtung von 0 ab im Uhrzeigersinn
                    Richtung2 = Convert.ToInt16(winkel2 / 45);
                    String Richtungstext1 = "empty";
                    String Richtungstext2 = "empty";
                    switch (Richtung1)
                    {
                        case 0: Richtungstext1 = "(Das wäre etwa nach Norden)"; break;
                        case 1: Richtungstext1 = "(Das wäre etwa nach Nordosten)"; break;
                        case 2: Richtungstext1 = "(Das wäre etwa nach Osten)"; break;
                        case 3: Richtungstext1 = "(Das wäre etwa nach Südosten)"; break;
                        case 4: Richtungstext1 = "(Das wäre etwa nach Süden)"; break;
                        case 5: Richtungstext1 = "(Das wäre etwa nach Südwesten)"; break;
                        case 6: Richtungstext1 = "(Das wäre etwa nach Westen)"; break;
                        case 7: Richtungstext1 = "(Das wäre etwa nach Nordwesten)"; break;
                        default: Richtungstext1 = "(immer dahin wo die Nase hinzeigt)"; break;
                    }
                    switch (Richtung2)
                    {
                        case 0: Richtungstext2 = "(Das wäre etwa nach Norden)"; break;
                        case 1: Richtungstext2 = "(Das wäre etwa nach Nordosten)"; break;
                        case 2: Richtungstext2 = "(Das wäre etwa nach Osten)"; break;
                        case 3: Richtungstext2 = "(Das wäre etwa nach Südosten)"; break;
                        case 4: Richtungstext2 = "(Das wäre etwa nach Süden)"; break;
                        case 5: Richtungstext2 = "(Das wäre etwa nach Südwesten)"; break;
                        case 6: Richtungstext2 = "(Das wäre etwa nach Westen)"; break;
                        case 7: Richtungstext2 = "(Das wäre etwa nach Nordwesten)"; break;
                        default: Richtungstext2 = "(immer dahin wo die Nase hinzeigt)"; break;
                    }

                    lblAnw0.Text = "...aber vielleicht hilft das:";
                    lblAnw1.Text = "zu Stolzenberger: " + Abstand1.ToString("#####") + " m  Winkel " + winkel1.ToString("###") + "° ";   //("+ Richtung +")";
                    lblAnw1b.Text = Richtungstext1;

                    lblAnw2.Text = "nach Hause: " + Abstand2.ToString("#####") + " m  Winkel " + winkel2.ToString("###") + "° ";
                    lblAnw4.Text = "leider aktualisiert sich das noch nicht selbst.";
                    lblAnw5.Text = "(Kommt noch. Einmal google maps machen hilft aber.)";
                    lblAnw2b.Text = Richtungstext2;

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception  
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception  
            }
            catch (Exception ex)
            {
                // Unable to get location  
            }
        }





    }
}