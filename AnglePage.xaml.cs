using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;
using Kinect.Utilities;

namespace LightBuzz.Vituvius.Samples.WPF
{
    /// <summary>
    /// Interaction logic for AnglePage.xaml
    /// </summary>
    public partial class AnglePage : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;

      /* 
        JointType _start = JointType.ShoulderLeft;
        JointType _center = JointType.ElbowLeft;
        JointType _end = JointType.WristLeft;
        JointType _start2 = JointType.ShoulderRight;
        JointType _center2 = JointType.ElbowRight;
        JointType _end2 = JointType.WristRight;
        JointType _start3 = JointType.SpineShoulder;
        JointType _center3 = JointType.ShoulderRight;
        JointType _end3 = JointType.ElbowRight;
        JointType _start4 = JointType.SpineShoulder;
        JointType _center4 = JointType.ShoulderLeft;
        JointType _end4 = JointType.ElbowLeft;

    */
        public AnglePage()
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
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();

                    _userReporter.Update(bodies);

                    Body body = bodies.Closest();

                    if (body != null)
                    {
                        viewer.DrawBody(body);
                        JointType[] points = new JointType[20];

                         points[0] =  JointType.Head;
                         points[1]  = JointType.Neck;
                        points[2] = JointType.SpineShoulder;
                         points[3] = JointType.ShoulderLeft;
                         points[4] = JointType.ShoulderRight;
                        points[5] = JointType.ElbowLeft;
                        points[6] = JointType.ElbowRight;
                        points[7] = JointType.WristLeft;
                        points[8] = JointType.WristRight;
                        points[9] = JointType.HandLeft;
                        points[10] = JointType.HandRight;

                        points[11]= JointType.SpineMid;
                        var spineBase = JointType.SpineBase;
                        points[12] = JointType.HipLeft;
                        points[13] = JointType.HipRight;
                        points[14] = JointType.KneeLeft;
                        points[15] = JointType.KneeRight;
                        points[16] = JointType.AnkleLeft;
                        points[17] = JointType.AnkleRight;
                        points[18] = JointType.FootLeft;
                        points[19] = JointType.FootRight;

                        JointType _center = spineBase;


                        if (body != null && body.BodyJointTrack() == true)
                        {
                            /*
                            angle.Update(body.Joints[points[0]], body.Joints[_center], body.Joints[_end], 50);
                            angle2.Update(body.Joints[_start2], body.Joints[_center], body.Joints[_end2], 50);
                            angle3.Update(body.Joints[_start3], body.Joints[_center], body.Joints[_end3], 50);
                            angle4.Update(body.Joints[_start4], body.Joints[_center], body.Joints[_end4], 50);

                            tblAngle.Text = ((int)angle.Angle).ToString();
                            tblAngle2.Text = ((int)angle2.Angle).ToString();
                            tblAngle3.Text = ((int)angle3.Angle).ToString();
                            tblAngle4.Text = ((int)angle4.Angle).ToString();
                            */
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
            viewer.Clear();
            angle.Clear();

            tblAngle.Text = "-";
            tblAngle2.Text = "-";
            tblAngle3.Text = "-";
            tblAngle4.Text = "-";
        }
    }
}
