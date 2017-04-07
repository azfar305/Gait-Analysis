using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using Kinect.Utilities;
using System.Windows.Threading;

namespace Trace
{
    /// <summary>
    /// Interaction logic for GaitExtraction.xaml
    /// </summary>
    public partial class GaitExtraction : Page
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _userReporter;
        public GaitExtraction()
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
            //Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body body = frame.Bodies().Closest();

                    if (body != null)
                    {
                        viewer.DrawBody(body);

                        System.IO.StreamWriter file1 = new System.IO.StreamWriter("C:\\Users\\Az3o5\\Desktop\\Dynamic Features\\static.txt", true);
                        System.IO.StreamWriter file2 = new System.IO.StreamWriter("C:\\Users\\Az3o5\\Desktop\\Dynamic Features\\dynamic.txt", true);
                        Joint[] points = new Joint[20];

                        points[0] = body.Joints[JointType.Head];
                        points[1] = body.Joints[JointType.Neck];
                        points[2] = body.Joints[JointType.SpineShoulder];
                        points[3] = body.Joints[JointType.ShoulderLeft];
                        points[4] = body.Joints[JointType.ShoulderRight];
                        points[5] = body.Joints[JointType.ElbowLeft];
                        points[6] = body.Joints[JointType.ElbowRight];
                        points[7] = body.Joints[JointType.WristLeft];
                        points[8] = body.Joints[JointType.WristRight];
                        points[9] = body.Joints[JointType.HandLeft];
                        points[10] = body.Joints[JointType.HandRight];

                        points[11] = body.Joints[JointType.SpineMid];
                        var spineBase = body.Joints[JointType.SpineBase];
                        points[12] = body.Joints[JointType.HipLeft];
                        points[13] = body.Joints[JointType.HipRight];
                        points[14] = body.Joints[JointType.KneeLeft];
                        points[15] = body.Joints[JointType.KneeRight];
                        points[16] = body.Joints[JointType.AnkleLeft];
                        points[17] = body.Joints[JointType.AnkleRight];
                        points[18] = body.Joints[JointType.FootLeft];
                        points[19] = body.Joints[JointType.FootRight];
                        double incline, stride;
                        string flip1, flip2, flip3;
                        

                        if (val == true || value == true)
                        {


                            if (body != null && body.BodyJointTrack() == true && value == true)
                            {
                                
                                double height = body.Height();
                                flip3 = ((float)height).ToString();
                                file1.Write(flip3);
                                file1.Write(",");

                                double Kheight = body.Kheight();
                                flip3 = ((float)Kheight).ToString();
                                file1.Write(flip3);
                                file1.Write(",");
                                
                             
                                double fullArm = (KinectUtlities.Length(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]) + Length(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft])) / 2;
                                flip3 = ((float)fullArm).ToString();
                                file1.Write(flip3);
                                file1.Write(",");

                                double thigh = (KinectUtlities.Length(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]) + Length(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft])) / 2 ;
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


                            }
                            if(body != null && body.BodyJointTrack() == true && val == true)
                            {
                                stride = (KinectUtlities.Length(body.Joints[JointType.AnkleLeft], body.Joints[JointType.AnkleRight]));
                                flip2 = ((float)stride).ToString();
                                file2.Write(flip2);
                                file2.Write(",");


                                for (int i = 0; i < 20; i++)
                                {
                                    for (int j = 0; j < 20; j++)
                                    {
                                        if (j > i)
                                        {



                                            incline = spineBase.Angle(points[i], points[j]);
                                            flip1 = ((int)incline).ToString();
                                            file2.Write(flip1);

                                            file2.Write(",");
                                        }



                                    }
                                }

                                file2.Write("\n");

                            }
                        }
                        else
                        {
                            tbLength1.Text = "-";
                            tbLength2.Text = "-";
                            tbLength3.Text = "-";
                            tblAngle1.Text = "--";
                            tblAngle2.Text = "--";
                            tblAngle3.Text = "--";

                        }
                        file1.Close();

                        file2.Close();
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

          
        }

        bool value = false;
        bool val = false;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public void start1()
        {

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            value = true;
           
            dispatcherTimer.Start();

        }

        public void start2()
        {
            val = true;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            value = false;

            dispatcherTimer.Stop();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            start1();
        }
    }















}

