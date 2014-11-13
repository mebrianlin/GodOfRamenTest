///* - Spatial full -
// * This example simply displays the spatial info if it is attached and displays the acceleration data 
// * for each axis as it is changing in pretty much raw form.  It also allows for modifying the sensitivity of 
// * each axis that is availabl on the attached spatial.

// * Please note that this example was designed to work with only one Phidget Spatial connected. For an 
// * example showing how to use two Phidgets of the same time concurrently, please see the Servo-multi example in the Servo Examples.

// * Copyright 2007 Phidgets Inc.  
// * This work is licensed under the Creative Commons Attribution 2.5 Canada License. 
// * To view a copy of this license, visit http://creativecommons.org/licenses/by/2.5/ca/
// */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using System.Threading;
//using System.IO;
//using System.Reflection;
//using Phidgets;  //needed for the spatial class and the phidgets exception class
//using Phidgets.Events; //needed for the phidget event handling

//namespace Spatial_full
//{
//    public partial class Form1 : Form
//    {
//        Assembly _assembly;
//        Stream _imageStream;

//        private Spatial spatial;
//        private ErrorEventBox errorBox;

//        double[] lastMsCount = { 0, 0, 0 };
//        bool[] lastMsCountGood = { false, false, false };
//        double[] gyroHeading = { 0, 0, 0 }; //degrees

//        List<double[]> compassBearingFilter = new List<double[]>();
//        int compassBearingFilterSize = 10;
//        double compassBearing = 0;

//        const double ambientMagneticField = 0.57142; //Calgary
//        const double ambientGravity = 1; //in G's

//        Bitmap spatialImage;

//        public Form1()
//        {
//            InitializeComponent();
//            errorBox = new ErrorEventBox();
//            this.Bounds = new Rectangle(this.Location, new Size(298, 386));
//        }

//        //initalize the phidget device and link the event handler code
//        private void Form1_Load(object sender, EventArgs e)
//        {
//            _assembly = Assembly.GetExecutingAssembly();
//            _imageStream = _assembly.GetManifestResourceStream("Spatial_full.1056_0_Top.png");
//            spatialImage = new Bitmap(_imageStream);
//            int newWidth = (int)((double)spatialImage.Width * ((double)compassView.Height / (double)spatialImage.Height) * 0.6);
//            int newHeight = (int)((double)compassView.Height * 0.6);
//            spatialImage = new Bitmap(spatialImage, newWidth, newWidth);

//            initAccelGraph();
//            initGyroGraph();
//            initMagFieldGraph();
//            initCompassBearingGraph();

//            Phidget.enableLogging(Phidget.LogLevel.PHIDGET_LOG_VERBOSE, null);

//            spatial = new Spatial();

//            spatial.Attach += new AttachEventHandler(spatial_Attach);
//            spatial.Detach += new DetachEventHandler(spatial_Detach);
//            spatial.Error += new Phidgets.Events.ErrorEventHandler(spatial_Error);

//            spatial.SpatialData += new SpatialDataEventHandler(spatial_SpatialData);

//            openCmdLine(spatial);
//        }

//        //When the application is being terminated, close the Phidget
//        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            spatial.Attach -= new AttachEventHandler(spatial_Attach);
//            spatial.Detach -= new DetachEventHandler(spatial_Detach);
//            spatial.SpatialData -= new SpatialDataEventHandler(spatial_SpatialData);

//            //run any events in the message queue - otherwise close will hang if there are any outstanding events
//            Application.DoEvents();

//            spatial.close();
//        }

//        //spatial attach event handler
//        void spatial_Attach(object sender, AttachEventArgs e)
//        {
//            Spatial attached = (Spatial)sender;

//            attachedTxt.Text = attached.Attached.ToString();
//            nameTxt.Text = attached.Name;
//            serialTxt.Text = attached.SerialNumber.ToString();
//            versionTxt.Text = attached.Version.ToString();

//            accelAxesTextBox.Text = attached.accelerometerAxes.Count.ToString();
//            gyroAxesTextBox.Text = attached.gyroAxes.Count.ToString();
//            compassAxesTextBox.Text = attached.compassAxes.Count.ToString();

//            switch (attached.ID)
//            {
//                case Phidget.PhidgetID.SPATIAL_ACCEL_3:
//                    this.Bounds = new Rectangle(this.Location, new Size(567, 500));
//                    break;
//                case Phidget.PhidgetID.SPATIAL_ACCEL_GYRO_COMPASS:
//                    this.Bounds = new Rectangle(this.Location, new Size(838, 614));
//                    break;
//            }

//            if (attached.accelerometerAxes.Count > 0)
//            {
//                accelBox.Visible = true;
//                accelGraphBox.Visible = true;
//            }
//            if (attached.compassAxes.Count > 0)
//            {
//                compassBox.Visible = true;
//                magFieldGraphBox.Visible = true;
//                compassGraphBox.Visible = true;
//            }
//            if (attached.gyroAxes.Count > 0)
//            {
//                gyroBox.Visible = true;
//                gyroGraphBox.Visible = true;
//            }

//            attached.DataRate = 32;

//            dataRateTrack.Minimum = attached.DataRateMax;
//            dataRateTrack.Maximum = 496;
//            dataRateTrack.Value = attached.DataRate;
//            dataRateTxt.Text = attached.DataRate.ToString();

//            lastMsCountGood[0] = false;
//            lastMsCountGood[1] = false;
//            lastMsCountGood[2] = false;
//            gyroHeading[0] = 0;
//            gyroHeading[1] = 0;
//            gyroHeading[2] = 0;
            
//            //enter board specific magnetic field corrections
//            //these are commented out because they will be different for every board - get them from the compass calibration program
//            //attached.setCompassCorrectionParameters(0.50756, -0.03738, -0.10788, -0.02335, 1.95880, 1.92179, 2.03008, 0.00722, 0.00979, 0.00708, -0.00225, 0.01012, -0.00237);
//        }

//        //spatial detach event handler
//        void spatial_Detach(object sender, DetachEventArgs e)
//        {
//            Spatial detached = (Spatial)sender;
//            attachedTxt.Text = detached.Attached.ToString();
//            nameTxt.Text = "";
//            serialTxt.Text = "";
//            versionTxt.Text = "";

//            accelBox.Visible = false;
//            accelGraphBox.Visible = false;
//            compassBox.Visible = false;
//            magFieldGraphBox.Visible = false;
//            gyroBox.Visible = false;
//            gyroGraphBox.Visible = false;
//            compassGraphBox.Visible = false;

//            this.Bounds = new Rectangle(this.Location, new Size(298, 386));
//        }

//        //spatial error event handler
//        void spatial_Error(object sender, Phidgets.Events.ErrorEventArgs e)
//        {
//            Phidget phid = (Phidget)sender;
//            DialogResult result;
//            switch (e.Type)
//            {
//                case PhidgetException.ErrorType.PHIDGET_ERREVENT_BADPASSWORD:
//                    phid.close();
//                    TextInputBox dialog = new TextInputBox("Error Event",
//                        "Authentication error: This server requires a password.", "Please enter the password, or cancel.");
//                    result = dialog.ShowDialog();
//                    if (result == DialogResult.OK)
//                        openCmdLine(phid, dialog.password);
//                    else
//                        Environment.Exit(0);
//                    break;
//                default:
//                    if (!errorBox.Visible)
//                        errorBox.Show();
//                    break;
//            }
//            errorBox.addMessage(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + e.Description);
//        }

//        void spatial_SpatialData(object sender, SpatialDataEventArgs e)
//        {
//            if (spatial.accelerometerAxes.Count > 0)
//            {
//                xTextBox.Text = e.spatialData[0].Acceleration[0].ToString("F3");
//                yTextBox.Text = e.spatialData[0].Acceleration[1].ToString("F3");
//                zTextBox.Text = e.spatialData[0].Acceleration[2].ToString("F3");
//                drawAccelGraph();
//            }

//            if(spatial.gyroAxes.Count > 0)
//            {
//                calculateGyroHeading(e.spatialData, 0); //x axis
//                calculateGyroHeading(e.spatialData, 1); //y axis
//                calculateGyroHeading(e.spatialData, 2); //z axis

//                headingXText.Text = gyroHeading[0].ToString("F3") + "?;
//                headingYText.Text = gyroHeading[1].ToString("F3") + "?;
//                headingZText.Text = gyroHeading[2].ToString("F3") + "?;

//                drawGyroGraph();
//            }

//            //Even when there is a compass chip, sometimes there won't be valid data in the event.
//            if (spatial.compassAxes.Count > 0 && e.spatialData[0].MagneticField.Length > 0)
//            {
//                compassXTxt.Text = spatial.compassAxes[0].MagneticField.ToString("F3");
//                compassYTxt.Text = spatial.compassAxes[1].MagneticField.ToString("F3");
//                compassZTxt.Text = spatial.compassAxes[2].MagneticField.ToString("F3");

//                try
//                {
//                    drawMagFieldGraph();
//                    calculateCompassBearing();
//                    drawCompassBearingGraph();
//                }
//                catch
//                {
//                }
//            }
//        }

//        private void dataRateTrack_Scroll(object sender, EventArgs e)
//        {
//            //can only pass values that are a multiple of 8
//            int val = dataRateTrack.Value;
//            if (val >= 8)
//            {
//                val = (int)(val / 8) * 8;
//            }
//            try
//            {
//                spatial.DataRate = val;
//                dataRateTxt.Text = spatial.DataRate.ToString();
//            }
//            catch { }
//        }

//        private void zeroGyroButton_Click(object sender, EventArgs e)
//        {
//            spatial.zeroGyro();
//            Thread.Sleep(100);
//            gyroHeading[0] = 0;
//            gyroHeading[1] = 0;
//            gyroHeading[2] = 0;
//        }

//        //This integrates gyro angular rate into heading over time
//        void calculateGyroHeading(SpatialEventData[] data, int index)
//        {
//            double gyro = 0;
//            for (int i = 0; i < data.Length; i++)
//            {
//                gyro = data[i].AngularRate[index];

//                if (lastMsCountGood[index])
//                {
//                    //calculate heading
//                    double timechange = data[i].Timestamp.TotalMilliseconds - lastMsCount[index]; // in ms
//                    double timeChangeSeconds = (double)timechange / 1000.0;

//                    gyroHeading[index] += timeChangeSeconds * gyro;
//                }

//                lastMsCount[index] = data[i].Timestamp.TotalMilliseconds;
//                lastMsCountGood[index] = true;
//            }
//        }

//        //This finds a magnetic north bearing, correcting for board tilt and roll as measured by the accelerometer
//        //This doesn't account for dynamic acceleration - ie accelerations other then gravity will throw off the calculation
//        double lastBearing=0;
//        void calculateCompassBearing()
//        {
//            double Xh = 0;
//            double Yh = 0;

//            //find the tilt of the board wrt gravity
//            Vector3 gravity = Vector3.Normalize(
//                new Vector3(
//                spatial.accelerometerAxes[0].Acceleration,
//                spatial.accelerometerAxes[2].Acceleration,
//                spatial.accelerometerAxes[1].Acceleration)
//            );

//            double pitchAngle = Math.Asin(gravity.X);
//            double rollAngle = Math.Asin(gravity.Z);

//            //The board is up-side down
//            if (gravity.Y < 0)
//            {
//                pitchAngle = -pitchAngle;
//                rollAngle = -rollAngle;
//            }

//            //Construct a rotation matrix for rotating vectors measured in the body frame, into the earth frame
//            //this is done by using the angles between the board and the gravity vector.
//            Matrix3x3 xRotMatrix = new Matrix3x3();
//            xRotMatrix.matrix[0, 0] = Math.Cos(pitchAngle); xRotMatrix.matrix[1, 0] = -Math.Sin(pitchAngle); xRotMatrix.matrix[2, 0] = 0;
//            xRotMatrix.matrix[0, 1] = Math.Sin(pitchAngle); xRotMatrix.matrix[1, 1] = Math.Cos(pitchAngle); xRotMatrix.matrix[2, 1] = 0;
//            xRotMatrix.matrix[0, 2] = 0; xRotMatrix.matrix[1, 2] = 0; xRotMatrix.matrix[2, 2] = 1;

//            Matrix3x3 zRotMatrix = new Matrix3x3();
//            zRotMatrix.matrix[0, 0] = 1; zRotMatrix.matrix[1, 0] = 0; zRotMatrix.matrix[2, 0] = 0;
//            zRotMatrix.matrix[0, 1] = 0; zRotMatrix.matrix[1, 1] = Math.Cos(rollAngle); zRotMatrix.matrix[2, 1] = -Math.Sin(rollAngle);
//            zRotMatrix.matrix[0, 2] = 0; zRotMatrix.matrix[1, 2] = Math.Sin(rollAngle); zRotMatrix.matrix[2, 2] = Math.Cos(rollAngle);

//            Matrix3x3 rotMatrix = Matrix3x3.Multiply(xRotMatrix, zRotMatrix);

//            Vector3 data = new Vector3(spatial.compassAxes[0].MagneticField, spatial.compassAxes[2].MagneticField, -spatial.compassAxes[1].MagneticField);
//            Vector3 correctedData = Matrix3x3.Multiply(data, rotMatrix);

//            //These represent the x and y components of the magnetic field vector in the earth frame
//            Xh = -correctedData.Z;
//            Yh = -correctedData.X;

//            //we use the computed X-Y to find a magnetic North bearing in the earth frame
//            try
//            {
//                double bearing = 0;
//                double _360inRads = (360 * Math.PI / 180.0);
//                if (Xh < 0)
//                    bearing = Math.PI - Math.Atan(Yh / Xh);
//                else if (Xh > 0 && Yh < 0)
//                    bearing = -Math.Atan(Yh / Xh);
//                else if (Xh > 0 && Yh > 0)
//                    bearing = Math.PI * 2 - Math.Atan(Yh / Xh);
//                else if (Xh == 0 && Yh < 0)
//                    bearing = Math.PI / 2.0;
//                else if (Xh == 0 && Yh > 0)
//                    bearing = Math.PI * 1.5;
                
//                //The board is up-side down
//                if (gravity.Y < 0)
//                {
//                    bearing = Math.Abs(bearing - _360inRads);
//                }

//                //passing the 0 <-> 360 point, need to make sure the filter never contains both values near 0 and values near 360 at the same time.
//                if (Math.Abs(bearing - lastBearing) > 2) //2 radians == ~115 degrees
//                {
//                    if(bearing > lastBearing)
//                        foreach (double[] stuff in compassBearingFilter)
//                            stuff[0] += _360inRads;
//                    else
//                        foreach (double[] stuff in compassBearingFilter)
//                            stuff[0] -= _360inRads;
//                }

//                compassBearingFilter.Add(new double[] { bearing, pitchAngle, rollAngle });
//                if (compassBearingFilter.Count > compassBearingFilterSize)
//                    compassBearingFilter.RemoveAt(0);

//                bearing = pitchAngle = rollAngle = 0;
//                foreach (double[] stuff in compassBearingFilter)
//                {
//                    bearing += stuff[0];
//                    pitchAngle += stuff[1];
//                    rollAngle += stuff[2];
//                }
//                bearing /= compassBearingFilter.Count;
//                pitchAngle /= compassBearingFilter.Count;
//                rollAngle /= compassBearingFilter.Count;

//                compassBearing = bearing * (180.0 / Math.PI);
//                lastBearing = bearing;

//                bearingTxt.Text = (bearing * (180.0 / Math.PI)).ToString("F1") + "?;
//                xAngle.Text = (pitchAngle * (180.0 / Math.PI)).ToString("F1") + "?;
//                yAngle.Text = (rollAngle * (180.0 / Math.PI)).ToString("F1") + "?;
//            }
//            catch { }
//        }

//        #region Graphs

//        private BufferedGraphicsContext gyroGraphicsContext;
//        private BufferedGraphics gyroGraphicsBuffer;
//        private Bitmap gyroDrawingSurface;

//        private Rectangle[] gyrocircleRectangle = new Rectangle[3];
//        private Rectangle gyroboundsRectangle;
//        private Pen[] gyroxyAxisPen = new Pen[3];
//        private Pen gyrocirclePen;
//        private float[] gyrocircleDiameter = new float[3], gyrocircleRadius = new float[3];

//        private void initGyroGraph()
//        {

//            //initialize our graphical representation of the acceleration data
//            gyroDrawingSurface = new Bitmap(gyroView.ClientRectangle.Width, gyroView.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

//            gyroboundsRectangle = new Rectangle(0, 0, gyroDrawingSurface.Width, gyroDrawingSurface.Height);
//            gyroboundsRectangle.Inflate(-1, -1);

//            float xCenter = (float)(gyroboundsRectangle.Width / 2.0);
//            float yCenter = (float)(gyroboundsRectangle.Height / 2.0);

//            gyroxyAxisPen[0] = new Pen(Color.Red, 4);
//            gyroxyAxisPen[1] = new Pen(Color.Green, 4);
//            gyroxyAxisPen[2] = new Pen(Color.Blue, 4);

//            gyrocirclePen = new Pen(Color.Black, 1);

//            gyrocircleDiameter[0] = (float)(gyroDrawingSurface.Width / 1.5);
//            gyrocircleDiameter[1] = (float)(gyroDrawingSurface.Width / 1.6);
//            gyrocircleDiameter[2] = (float)(gyroDrawingSurface.Width / 1.714);

//            for (int i = 0; i < 3; i++)
//            {
//                gyrocircleRadius[i] = (float)(gyrocircleDiameter[i] / 2.0);
//                gyrocircleRectangle[i] = new Rectangle((int)Math.Round(xCenter - gyrocircleRadius[i]),
//                    (int)Math.Round(yCenter - gyrocircleRadius[i]), (int)Math.Round(gyrocircleDiameter[i]), (int)Math.Round(gyrocircleDiameter[i]));
//            }
//        }

//        private void drawGyroGraph()
//        {
//            //get our graphics buffer going so we can draw to our panel
//            gyroGraphicsContext = new BufferedGraphicsContext();
//            gyroGraphicsBuffer = gyroGraphicsContext.Allocate(Graphics.FromImage(gyroDrawingSurface), gyroboundsRectangle);
//            gyroGraphicsBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
//            gyroGraphicsBuffer.Graphics.Clear(SystemColors.Control);


//            float xCenter = (float)(gyroboundsRectangle.Width / 2.0);
//            float yCenter = (float)(gyroboundsRectangle.Height / 2.0);

//            //heading circles
//            for (int i = 0; i < spatial.gyroAxes.Count; i++)
//            {
//                gyroGraphicsBuffer.Graphics.DrawEllipse(gyrocirclePen, gyrocircleRectangle[i]);
//                gyroGraphicsBuffer.Graphics.DrawEllipse(
//                    gyroxyAxisPen[i],
//                    (float)(gyrocircleRadius[i] * Math.Cos(gyroHeading[i] * (Math.PI / 180.0)) + xCenter) - 2,
//                    (float)(-gyrocircleRadius[i] * Math.Sin(gyroHeading[i] * (Math.PI / 180.0)) + yCenter) - 2,
//                    4,
//                    4
//                );
//            }

//            gyroGraphicsBuffer.Render(gyroView.CreateGraphics());

//            gyroGraphicsBuffer.Dispose();

//            gyroGraphicsContext.Dispose();
//        }

//        private BufferedGraphicsContext accelGraphicsContext;
//        private BufferedGraphics accelGraphicsBuffer;
//        private Bitmap accelDrawingSurface;

//        private Rectangle accelcircleRectangle, accelboundsRectangle;
//        private Pen accelxyAxisPen, accelcirclePen;
//        private float accelcircleDiameter, accelcircleRadius;

//        private void initAccelGraph()
//        {

//            //initialize our graphical representation of the acceleration data
//            accelDrawingSurface = new Bitmap(panel1.ClientRectangle.Width, panel1.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

//            accelboundsRectangle = new Rectangle(0, 0, accelDrawingSurface.Width, accelDrawingSurface.Height);
//            accelboundsRectangle.Inflate(-1, -1);

//            float xCenter = (float)(accelboundsRectangle.Width / 2.0);
//            float yCenter = (float)(accelboundsRectangle.Height / 2.0);

//            accelxyAxisPen = new Pen(Color.Black, 2);
//            accelcirclePen = new Pen(Color.DarkBlue, 2);

//            accelcircleDiameter = (float)(accelDrawingSurface.Width / 1.6);
//            accelcircleRadius = accelcircleDiameter / 2;

//            accelcircleRectangle = new Rectangle((int)Math.Round(xCenter - accelcircleRadius), (int)Math.Round(yCenter - accelcircleRadius), (int)Math.Round(accelcircleDiameter), (int)Math.Round(accelcircleDiameter));
//        }

//        private void drawAccelGraph()
//        {
//            accelGraphicsContext = new BufferedGraphicsContext();
//            accelGraphicsBuffer = accelGraphicsContext.Allocate(Graphics.FromImage(accelDrawingSurface), accelboundsRectangle);
//            accelGraphicsBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
//            accelGraphicsBuffer.Graphics.Clear(SystemColors.Control);

//            float xCenter = (float)(accelboundsRectangle.Width / 2.0);
//            float yCenter = (float)(accelboundsRectangle.Height / 2.0);

//            float xOld = xCenter - (float)spatial.accelerometerAxes[0].Acceleration * accelcircleRadius * (float)(1 / ambientGravity);
//            float yOld = yCenter - (float)spatial.accelerometerAxes[1].Acceleration * accelcircleRadius * (float)(1 / ambientGravity);

//            accelGraphicsBuffer.Graphics.DrawLine(accelxyAxisPen, xCenter, yCenter, xOld, yOld);
//            accelGraphicsBuffer.Graphics.DrawEllipse(accelcirclePen, new Rectangle((int)(xCenter - accelcircleRadius), (int)(yCenter - accelcircleRadius),
//                        (int)accelcircleDiameter, (int)accelcircleDiameter));

//            if (spatial.accelerometerAxes.Count == 3)
//            {
//                double zOut = spatial.accelerometerAxes[2].Acceleration * (float)(1 / ambientGravity);
//                if (zOut > 0)
//                {
//                    accelGraphicsBuffer.Graphics.DrawEllipse(new Pen(Color.Red, 2),
//                        new Rectangle((int)xCenter - (int)(accelcircleRadius * zOut), (int)yCenter - (int)(accelcircleRadius * zOut),
//                        (int)(accelcircleDiameter * zOut), (int)(accelcircleDiameter * zOut)));
//                }
//                else
//                {
//                    accelGraphicsBuffer.Graphics.DrawEllipse(new Pen(Color.Green, 2),
//                        new Rectangle((int)xCenter - (int)(accelcircleRadius * -zOut), (int)yCenter - (int)(accelcircleRadius * -zOut),
//                        (int)(accelcircleDiameter * -zOut), (int)(accelcircleDiameter * -zOut)));
//                }
//            }

//            accelGraphicsBuffer.Render(panel1.CreateGraphics());

//            accelGraphicsBuffer.Dispose();

//            accelGraphicsContext.Dispose();
//        }

//        private BufferedGraphicsContext magFieldGraphicsContext;
//        private BufferedGraphics magFieldGraphicsBuffer;
//        private Bitmap magFieldDrawingSurface;

//        private Rectangle magFieldCircleRectangle, magFieldBoundsRectangle;
//        private Pen magFieldXYAxisPen, magFieldCirclePen;
//        private float magFieldCircleDiameter, magFieldCircleRadius;

//        private void initMagFieldGraph()
//        {

//            //initialize our graphical representation of the compasseration data
//            magFieldDrawingSurface = new Bitmap(magFieldView.ClientRectangle.Width, magFieldView.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

//            magFieldBoundsRectangle = new Rectangle(0, 0, magFieldDrawingSurface.Width, magFieldDrawingSurface.Height);
//            magFieldBoundsRectangle.Inflate(-1, -1);

//            float xCenter = (float)(magFieldBoundsRectangle.Width / 2.0);
//            float yCenter = (float)(magFieldBoundsRectangle.Height / 2.0);

//            magFieldXYAxisPen = new Pen(Color.Black, 2);
//            magFieldCirclePen = new Pen(Color.DarkBlue, 2);

//            magFieldCircleDiameter = (float)(magFieldDrawingSurface.Width / 1.6);
//            magFieldCircleRadius = magFieldCircleDiameter / 2;

//            magFieldCircleRectangle = new Rectangle((int)Math.Round(xCenter - magFieldCircleRadius), (int)Math.Round(yCenter - magFieldCircleRadius), (int)Math.Round(magFieldCircleDiameter), (int)Math.Round(magFieldCircleDiameter));
//        }

//        private void drawMagFieldGraph()
//        {
//            magFieldGraphicsContext = new BufferedGraphicsContext();
//            magFieldGraphicsBuffer = magFieldGraphicsContext.Allocate(Graphics.FromImage(magFieldDrawingSurface), magFieldBoundsRectangle);
//            magFieldGraphicsBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
//            magFieldGraphicsBuffer.Graphics.Clear(SystemColors.Control);

//            float xCenter = (float)(magFieldBoundsRectangle.Width / 2.0);
//            float yCenter = (float)(magFieldBoundsRectangle.Height / 2.0);

//            float xOld = xCenter + (float)(spatial.compassAxes[0].MagneticField) * magFieldCircleRadius * (float)(1 / ambientMagneticField);
//            float yOld = yCenter + (float)(spatial.compassAxes[1].MagneticField) * magFieldCircleRadius * (float)(1 / ambientMagneticField);

//            magFieldGraphicsBuffer.Graphics.DrawLine(magFieldXYAxisPen, xCenter, yCenter, xOld, yOld);
//            magFieldGraphicsBuffer.Graphics.DrawEllipse(magFieldCirclePen, magFieldCircleRectangle);

//            if (spatial.compassAxes.Count == 3)
//            {
//                double zOut = (spatial.compassAxes[2].MagneticField) * (float)(1 / ambientMagneticField);
//                if (zOut > 0)
//                {
//                    magFieldGraphicsBuffer.Graphics.DrawEllipse(new Pen(Color.Red, 2),
//                        new Rectangle((int)xCenter - (int)(magFieldCircleRadius * zOut), (int)yCenter - (int)(magFieldCircleRadius * zOut),
//                        (int)(magFieldCircleDiameter * zOut), (int)(magFieldCircleDiameter * zOut)));
//                }
//                else
//                {
//                    magFieldGraphicsBuffer.Graphics.DrawEllipse(new Pen(Color.Green, 2),
//                        new Rectangle((int)xCenter - (int)(magFieldCircleRadius * -zOut), (int)yCenter - (int)(magFieldCircleRadius * -zOut),
//                        (int)(magFieldCircleDiameter * -zOut), (int)(magFieldCircleDiameter * -zOut)));
//                }
//            }

//            magFieldGraphicsBuffer.Render(magFieldView.CreateGraphics());

//            magFieldGraphicsBuffer.Dispose();

//            magFieldGraphicsContext.Dispose();
//        }

//        private BufferedGraphicsContext compassGraphicsContext;
//        private BufferedGraphics compassGraphicsBuffer;
//        private Bitmap compassDrawingSurface;

//        private RectangleF compasscircleRectangle;
//        private Rectangle compassboundsRectangle;
//        private Pen compassDotPen, compassCirclePen, compassTickPen, compassTickPenBig;
//        private float compassCircleDiameter, compassCircleRadius;

//        private void initCompassBearingGraph()
//        {

//            //initialize our graphical representation of the compasseration data
//            compassDrawingSurface = new Bitmap(compassView.ClientRectangle.Width, compassView.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

//            compassboundsRectangle = new Rectangle(0, 0, compassDrawingSurface.Width, compassDrawingSurface.Height);
//            compassboundsRectangle.Inflate(-1, -1);

//            float xCenter = (float)(compassboundsRectangle.Width / 2.0);
//            float yCenter = (float)(compassboundsRectangle.Height / 2.0);

//            compassDotPen = new Pen(Color.Red, 4);
//            compassCirclePen = new Pen(Color.Black, 2);
//            compassTickPen = new Pen(Color.DarkBlue, 1);
//            compassTickPenBig = new Pen(Color.DarkBlue, 2);

//            compassCircleDiameter = (float)(compassDrawingSurface.Width / 1.35);
//            compassCircleRadius = (float)(compassCircleDiameter / 2.0);

//            compasscircleRectangle = new RectangleF(xCenter - compassCircleRadius, yCenter - compassCircleRadius, compassCircleDiameter, compassCircleDiameter);
//        }

//        private void drawCompassBearingGraph()
//        {
//            compassGraphicsContext = new BufferedGraphicsContext();
//            compassGraphicsBuffer = compassGraphicsContext.Allocate(Graphics.FromImage(compassDrawingSurface), compassboundsRectangle);
//            compassGraphicsBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
//            compassGraphicsBuffer.Graphics.Clear(SystemColors.Control);

//            float xCenter = (float)(compassboundsRectangle.Width / 2.0);
//            float yCenter = (float)(compassboundsRectangle.Height / 2.0);

//            compassGraphicsBuffer.Graphics.DrawEllipse(compassCirclePen, compasscircleRectangle);

//            Point pt = new Point((int)(xCenter - spatialImage.Width / 2), (int)(yCenter - spatialImage.Height / 2));
//            compassGraphicsBuffer.Graphics.DrawImageUnscaled(rotateImage(spatialImage, (float)compassBearing), pt);

//            compassGraphicsBuffer.Graphics.DrawString("N", new Font("Arial", 16), new SolidBrush(Color.Black), xCenter-10, 0);
//            compassGraphicsBuffer.Graphics.DrawString("E", new Font("Arial", 16), new SolidBrush(Color.Black), xCenter*2-20, yCenter-12);
//            compassGraphicsBuffer.Graphics.DrawString("S", new Font("Arial", 16), new SolidBrush(Color.Black), xCenter-10, yCenter*2-20);
//            compassGraphicsBuffer.Graphics.DrawString("W", new Font("Arial", 16), new SolidBrush(Color.Black), 0, yCenter-12);

//            //Ticks around the compass circle
//            for (int i = 0; i < 360; i+=10)
//            {
//                Pen p = compassTickPen;
//                int tickSize = 4;
//                if (i == 0 || i == 90 || i == 180 || i == 270)
//                {
//                    p = compassTickPenBig;
//                    tickSize = 6;
//                }
//                compassGraphicsBuffer.Graphics.DrawLine(p,
//                (float)((compassCircleRadius + tickSize) * Math.Cos(i * (Math.PI / 180.0)) + xCenter),
//                (float)(-(compassCircleRadius + tickSize) * Math.Sin(i * (Math.PI / 180.0)) + yCenter),
//                (float)((compassCircleRadius - tickSize) * Math.Cos(i * (Math.PI / 180.0)) + xCenter),
//                (float)(-(compassCircleRadius - tickSize) * Math.Sin(i * (Math.PI / 180.0)) + yCenter));
//            }

//            //Marker on the compass circle
//            compassGraphicsBuffer.Graphics.DrawEllipse(
//                compassDotPen,
//                (float)(compassCircleRadius * Math.Cos((-compassBearing+90) * (Math.PI / 180.0)) + xCenter) - 2,
//                (float)(-compassCircleRadius * Math.Sin((-compassBearing+90) * (Math.PI / 180.0)) + yCenter) - 2,
//                4,
//                4
//            );

//            compassGraphicsBuffer.Render(compassView.CreateGraphics());

//            compassGraphicsBuffer.Dispose();

//            compassGraphicsContext.Dispose();
//        }

//        private Bitmap rotateImage(Bitmap b, float angle)
//        {
//            //create a new empty bitmap to hold rotated image
//            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
//            //make a graphics object from the empty bitmap
//            Graphics g = Graphics.FromImage(returnBitmap);
//            //move rotation point to center of image
//            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
//            //rotate
//            g.RotateTransform(angle);
//            //move image back
//            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
//            //draw passed in image onto graphics object
//            g.DrawImage(b, new Point(0, 0));
//            return returnBitmap;
//        }
//        #endregion

//        //Parses command line arguments and calls the appropriate open
//        #region Command line open functions
//        private void openCmdLine(Phidget p)
//        {
//            openCmdLine(p, null);
//        }
//        private void openCmdLine(Phidget p, String pass)
//        {
//            int serial = -1;
//            int port = 5001;
//            String host = null;
//            bool remote = false, remoteIP = false;
//            string[] args = Environment.GetCommandLineArgs();
//            String appName = args[0];

//            try
//            { //Parse the flags
//                for (int i = 1; i < args.Length; i++)
//                {
//                    if (args[i].StartsWith("-"))
//                        switch (args[i].Remove(0, 1).ToLower())
//                        {
//                            case "n":
//                                serial = int.Parse(args[++i]);
//                                break;
//                            case "r":
//                                remote = true;
//                                break;
//                            case "s":
//                                remote = true;
//                                host = args[++i];
//                                break;
//                            case "p":
//                                pass = args[++i];
//                                break;
//                            case "i":
//                                remoteIP = true;
//                                host = args[++i];
//                                if (host.Contains(":"))
//                                {
//                                    port = int.Parse(host.Split(':')[1]);
//                                    host = host.Split(':')[0];
//                                }
//                                break;
//                            default:
//                                goto usage;
//                        }
//                    else
//                        goto usage;
//                }

//                if (remoteIP)
//                    p.open(serial, host, port, pass);
//                else if (remote)
//                    p.open(serial, host, pass);
//                else
//                    p.open(serial);
//                return; //success
//            }
//            catch { }
//        usage:
//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine("Invalid Command line arguments." + Environment.NewLine);
//            sb.AppendLine("Usage: " + appName + " [Flags...]");
//            sb.AppendLine("Flags:\t-n   serialNumber\tSerial Number, omit for any serial");
//            sb.AppendLine("\t-r\t\tOpen remotely");
//            sb.AppendLine("\t-s   serverID\tServer ID, omit for any server");
//            sb.AppendLine("\t-i   ipAddress:port\tIp Address and Port. Port is optional, defaults to 5001");
//            sb.AppendLine("\t-p   password\tPassword, omit for no password" + Environment.NewLine);
//            sb.AppendLine("Examples: ");
//            sb.AppendLine(appName + " -n 50098");
//            sb.AppendLine(appName + " -r");
//            sb.AppendLine(appName + " -s myphidgetserver");
//            sb.AppendLine(appName + " -n 45670 -i 127.0.0.1:5001 -p paswrd");
//            MessageBox.Show(sb.ToString(), "Argument Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

//            Application.Exit();
//        }
//        #endregion
//    }
//}