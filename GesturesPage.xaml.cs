using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Kinect.Utilities;
using System;




namespace LightBuzz.Vituvius.Samples.WPF
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
                    if (viewer1.Visualization == Visualization.Color)
                    {
                        viewer1.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body body = frame.Bodies().Closest();

                    if (body != null)
                    {
                        viewer1.DrawBody(body);


                        if (body != null && body.BodyJointTrack() == true)
                        {
                            double height = body.Kheight();
                            tbHeight.Text = ((float)height).ToString();

                            double length1 = (Length(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]) + Length(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft])) / 2;
                            tbLength1.Text = ((float)length1).ToString();

                            double length2 = (Length(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]) + Length(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft])) / 2 ;
                            tbLength2.Text = ((float)length2).ToString();

                        }
                        else
                        {
                            tbHeight.Text = "--";
                            tbLength1.Text = "--";
                            tbLength2.Text = "--";
                        }

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


        }
        public static double Length(Joint p1, Joint p2)
        {
            double x = Math.Pow(p1.Position.X - p2.Position.X, 2);
            double y = Math.Pow(p1.Position.Y - p2.Position.Y, 2);
            double z = Math.Pow(p1.Position.Z - p2.Position.Z, 2);

            return (Math.Sqrt(x + y + z));

        }

        public static double Length(params Joint[] joints)
        {
            double len = 0;
            for (int i = 0; i < joints.Length - 1; i++)
            {
                len += Length(joints[i], joints[i + 1]);

            }
            return len;

        }
    }

}


