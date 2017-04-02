
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Kinect.Utilities;
using System;
using System.Timers;
using System.Threading;
using System.Windows.Threading;





namespace Trace
{
    /// <summary>
    /// Interaction logic for GesturesPage.xaml
    /// </summary>
    public partial class GesturesPage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;

        public GesturesPage()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                
                _userReporter = new PlayersController();
                _userReporter.BodyEntered += UserReporter_BodyEntered;
                _userReporter.BodyLeft += UserReporter_BodyLeft;
                _userReporter.Start();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_userReporter != null)
            {
                _userReporter.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
               

                if (frame != null)
                {
                   // double fps = 1.0 / frame.ColorCameraSettings.FrameInterval.TotalSeconds;
                    if (viewer1.Visualization == Visualization.Color)
                    {
                        viewer1.Image = frame.ToBitmap();

                    }
                    /*
                    if (fps<29)
                    {
                        if (viewer1.Visualization == Visualization.Color)
                        {
                            viewer1.Image = frame.ToBitmap();
                        }
                    }
                */
                }
            }

            // Body
          //System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Az3o5\\Desktop\\Dynamic Features\\frame6.txt", true);
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body body = frame.Bodies().Closest();

                    if (body != null)
                    {
                        viewer1.DrawBody(body);

                        System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Az3o5\\Desktop\\Dynamic Features\\light.txt",true);
                        if (body != null && body.BodyJointTrack() == true && value == true)
                        {
                            /*
                            double height = body.Height();
                             tbHeight.Text = ((float)height).ToString();

                             file.Write(tbHeight.Text);
                             file.Write("\t");

                             double fullArm = (Length(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]) + Length(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft])) / 2;
                             tbLength1.Text = ((float)fullArm).ToString();
                             file.Write(tbLength1.Text);
                             file.Write("\t");

                             double thigh = (Length(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]) + Length(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft])) / 2 ;
                             tbLength2.Text = ((float)thigh).ToString();
                             file.Write(tbLength2.Text);
                             file.Write("\t");

                             double fullLeg = (Length(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]) + Length(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft])) / 2;
                             tbLength3.Text = ((float)fullLeg).ToString();
                             file.Write(tbLength3.Text);
                             file.Write("\t");


                             double torso = (Length(body.Joints[JointType.HipLeft], body.Joints[JointType.HipRight]));
                             tbLength4.Text = ((float)torso).ToString();
                             file.Write(tbLength4.Text);
                             file.Write("\t");

                             double upperArm = (Length(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]) + Length(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft])) / 2;
                             tbLength5.Text = ((float)upperArm).ToString();
                             file.Write(tbLength5.Text);
                             file.Write("\t");


                             double lowerArm = (Length(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]) + Length(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft])) / 2;
                             tbLength6.Text = ((float)lowerArm).ToString();
                             file.Write(tbLength6.Text);
                             file.Write("\n");
                            */ 
                           double stride = (KinectUtlities.Length(body.Joints[JointType.AnkleLeft], body.Joints[JointType.AnkleRight]));
                           tbLength7.Text = Math.Round((Decimal)stride, 6, MidpointRounding.AwayFromZero).ToString();
                            tbLength7.Text = ((float)stride).ToString();
                            
                            file.Write(tbLength7.Text);
                            file.Write("\n");
                            
                            
                            /*
                            double x1 = body.Joints[JointType.AnkleRight].Position.X;
                            file.Write(x1);
                            file.Write("\t");
                            tbLength1.Text = ((float)x1).ToString();

                            double y1 = body.Joints[JointType.AnkleRight].Position.Y;
                            file.Write(y1);
                            file.Write("\t");
                            tbLength2.Text = ((float)y1).ToString();

                            double z1 = body.Joints[JointType.AnkleRight].Position.Z;
                            file.Write(z1);
                            file.Write("\t\t");
                            tbLength3.Text = ((float)z1).ToString();

                            double x2 = body.Joints[JointType.AnkleLeft].Position.X;
                            file.Write(x2);
                            file.Write("\t");
                            tbLength4.Text = ((float)x2).ToString();

                            double y2 = body.Joints[JointType.AnkleLeft].Position.Y;
                            file.Write(y2);
                            file.Write("\t");
                            tbLength5.Text = ((float)y2).ToString();


                            double z2 = body.Joints[JointType.AnkleLeft].Position.Z;
                            file.Write(z2);
                            file.Write("\n");
                            tbLength6.Text = ((float)z2).ToString();
                            */




                        }
                        else
                        {
                            tbHeight.Text = "--";
                            tbLength1.Text = "--";
                            tbLength2.Text = "--";
                            tbLength3.Text = "--";
                            tbLength4.Text = "--";
                            tbLength5.Text = "--";
                            tbLength6.Text = "--";
                            
                            tbLength7.Text = "--";
                            
                        }
                        file.Close();

                    }
                }
            }
        }

        void UserReporter_BodyEntered(object sender, PlayersControllerEventArgs e)
        {
        }

        void UserReporter_BodyLeft(object sender, PlayersControllerEventArgs e)
        {
            viewer1.Clear();
            
            tbHeight.Text = "-";
            tbLength1.Text = "-";
            tbLength2.Text = "-";
            tbLength3.Text = "-";
            tbLength4.Text = "-";
            tbLength5.Text = "-";
            tbLength6.Text = "-";
            
            tbLength7.Text = "-";



        }
       

        


     
        
       
        bool value = false;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public void start()
        {
           
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            value = true; 
            Console.WriteLine(value);
            dispatcherTimer.Start();
            
        }

        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            value = false;
            
            dispatcherTimer.Stop();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            start();
        }
    }

}


