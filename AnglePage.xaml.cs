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

      
        JointType _end = JointType.ShoulderLeft;
        JointType _center = JointType.ElbowLeft;
        JointType _start = JointType.WristLeft;
        
        JointType _start2 = JointType.ShoulderRight;
        JointType _center2 = JointType.ElbowRight;
        JointType _end2 = JointType.WristRight;
        /*
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
          //  System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Az3o5\\Desktop\\Dynamic Features\\angles10.txt", true);

            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // var bodies = frame.Bodies();
                    Body body = frame.Bodies().Closest();
                    // _userReporter.Update(bodies);

                   // Body body = bodies.Closest();

                    if (body != null)
                    {
                        System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Az3o5\\Desktop\\Dynamic Features\\angles69.txt", true);
                        viewer.DrawBody(body);
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

                         points[11]= body.Joints[JointType.SpineMid];
                         var spineBase = body.Joints[JointType.SpineBase];
                         points[12] = body.Joints[JointType.HipLeft];
                         points[13] = body.Joints[JointType.HipRight];
                         points[14] = body.Joints[JointType.KneeLeft];
                         points[15] = body.Joints[JointType.KneeRight];
                         points[16] = body.Joints[ JointType.AnkleLeft];
                         points[17] = body.Joints[JointType.AnkleRight];
                         points[18] = body.Joints[JointType.FootLeft];
                         points[19] = body.Joints[ JointType.FootRight];


                        //  JointType _center = spineBase;

                        double incline, incline2, incline3;
                        int k = 0;
                        string flip, flip2, flip3, txt1, txt2, txt3;
                        if (body != null && body.BodyJointTrack() == true && value == true)
                        {

                            //  incline = points[5].Angle(points[3], points[7]);
                            //incline2 = points[6].Angle(points[8], points[4]);

                            //incline3 = spineBase.Angle(points[2], points[11]);
                            //  file1.Write(flip);
                            //file1.Write("\n");

                            // flip = ((int)incline).ToString();
                            //flip2 = ((int)incline2).ToString();
                            //flip3 = ((int)incline3).ToString();

                            double stride = (KinectUtlities.Length(body.Joints[JointType.AnkleLeft], body.Joints[JointType.AnkleRight]));
                            flip2 = ((float)stride).ToString();
                            file.Write(flip2);
                            file.Write(",");



                            for (int i=0;i<20;i++)
                            {
                                for(int j =0;j<20;j++)
                                {
                                    if(j>i)
                                    {



                                        incline = spineBase.Angle(points[i], points[j]);
                                        flip = ((int)incline).ToString();
                                        file.Write(flip);
                                        
                                        file.Write(",");
                                     }
                                        
                                        
                                        
                                    }
                                }

                            //file.Write("\n\n\n\n\n\n");










                            // angle.Update(body.Joints[_start], body.Joints[_center], body.Joints[_end], 50);

                            // angle2.Update(body.Joints[_start], body.Joints[_center], body.Joints[_end], 50);
                            //angle3.Update(body.Joints[_start2], body.Joints[_center2], body.Joints[_end2], 50);
                            // angle4.Update(body.Joints[_start4], body.Joints[_center], body.Joints[_end4], 50);
                            /*
                            txt1 = ((int)angle2.Angle).ToString();
                            tblAngle.Text = txt1;
                            file.Write(txt1);
                            file.Write(",");
                            tblAngle2.Text = flip;
                            file.Write(flip);
                            file.Write(",");
                            
                            txt2= ((int)angle3.Angle).ToString();
                             tblAngle3.Text=txt2;
                            file.Write(txt2);
                            file.Write(",");
                            tblAngle4.Text = flip2;
                            file.Write(flip2);
                            file.Write(",");
                            file.Write(flip3);
                            file.Write("\n");
                            */
                            //tblAngle4.Text = ((int)angle4.Angle).ToString();
                            file.Write("\n");
                        }
                        else
                        {
                            tblAngle.Text = "--";
                            tblAngle2.Text = "--";
                            tblAngle3.Text = "--";
                            tblAngle4.Text = "--";


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
            viewer.Clear();
            angle.Clear();

            tblAngle.Text = "-";
            tblAngle2.Text = "-";
            tblAngle3.Text = "-";
            tblAngle4.Text = "-";
        }
        bool value = false;
        int count = 0;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            begin();
        }

        public void begin()
        {
            if (count % 2 == 0)
                value = true;
            else
                value = false;
        }
    }
}
