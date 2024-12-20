//f.olofsson 2018
//reading and parsing data from YDLIDAR X4 - see X4_Lidar_Development_Manual.pdf

//NOTE: need to set these two in Project Settings / Player:
// Scripting Runtime Version: Experimental (.NET 4.6 Equivalent)
// Api Compatibility Level: .NET 4.6

//keys:
// 'A' - toggle draw raw data (white)
// 'V' - toggle draw leap data (yellow)
// 'B' - toggle draw border (blue)
// 'C' - toggle draw calibration (green)
// 'Space' - take calibrating snapshot (make sure empty room and no people in sight)
// 'R' - start and stop recording
// 'P' - start and stop playback

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Ports;

public class RPLidar_S2 : MonoBehaviour
{

    //--settings
    public string portPath = "/dev/cu.SLAB_USBtoUART";  //EDIT to match your serial port e.g. COM3
    public string recordingPath = "Assets/Resources/";  //EDIT directory where to save recorded files
    public string playingPath = "Assets/Resources/rec07-10-18_04-30-26";  //EDIT to match filename for playback
                                                                          //public string playingPath = "Assets/Resources/rec07-10-18_04-36-30";
                                                                          //public string playingPath = "Assets/Resources/rec07-10-18_05-05-12";

    //--public
    [Range(0, 10)] public float drawScale = 1.0F;
    [Range(0, 10)] public float drawThresh = 0.25F;
    [Range(1, 200)] public int leapSamples = 4;    //number of samples before jump
    [Range(0, 5000)] public int leapThresh = 1000;  //in mm
    [Range(1, 200)] public int borderDetail = 20;  //number of adjacent samples to check
    [Range(0, 5000)] public int borderShrink = 300;  //in mm
    [Range(0, 1)] public float borderFilter = 0.95F;  //0= no smoothing, 1= full
    [Range(0, 1)] public float trackingFilter = 0.95F;  //0= no smoothing, 1= full
    public float trackingX;  //x position of tracked object
    public float trackingY;  //y position of tracked object
    public bool tracking;  //object found or not

    //--private
    private float[] scan;
    private float[] leap;
    private float[] border;
    private float[] calibration;
    private SerialPort port;
    private int waitState = 0;
    private byte state = 0;
    private int dlength, dmode, dtype;
    private int pcounter;
    private List<byte> minfo = new List<byte>();  //info message
    private List<byte> mstat = new List<byte>();  //status message
    private List<byte> pdata = new List<byte>();  //data package
    private BinaryWriter Writer = null;
    private BinaryReader reader;
    private bool drawRawData = true;
    private bool drawLeapData = false;
    private bool drawBorder = true;
    private bool drawCalibration = true;
    private bool recording = false;
    private bool playing = false;
    void Start()
    {
        scan = new float[720];
        leap = new float[720];
        border = new float[720];
        calibration = new float[720];
        port = new SerialPort(portPath, 1000000);
        try
        {
            port.Open();
        }
        catch (System.Exception)
        {
            string names = "";
            foreach (string name in SerialPort.GetPortNames())
            {
                names += name;
                names += ", ";
            }
            Debug.Log("warn: port could not open. available ports are: " + names);
            throw;
        }
        port.DtrEnable = true;
        port.ReadTimeout = 1000;  //need a high value here for Windows
        port.WriteTimeout = 1000;
    }
    void OnDisable()
    {
        if (recording)
        {
            Writer.Flush();
            Writer.Close();
        }
        if (playing)
        {
            reader.Close();
            reader.Dispose();
        }
        Debug.Log("closing port");
        port.Close();
    }

    //YDLidar X4
    //Cmd(0x60);  //start
    //Cmd(0x65);  //stop
    //Cmd(0x90);  //info
    //Cmd(0x91);  //status
    //Cmd(0x40);  //reboot

    //RPLidar S2
    //Cmd(0x20);  //start
    //Cmd(0x25);  //stop
    //Cmd(0x50);  //info
    //Cmd(0x52);  //status
    //Cmd(0x40);  //reboot

    void Cmd(byte command)
    {
        Byte[] bytes = new Byte[2];
        bytes[0] = 0xA5;
        bytes[1] = command;
        port.Write(bytes, 0, 2);
    }
    void Update()
    {
        if (port.IsOpen)
        {
            if (waitState == 0 && Time.frameCount > 10)
            {  //after startup, wait for 10 frames
                Cmd(0x50);  //send info command
                waitState = 1;
            }
            else if (waitState == 1 && Time.frameCount > 20)
            {  //wait for 20 frames
                Cmd(0x52);  //send status command
                waitState = 2;
            }
            else if (waitState == 2 && Time.frameCount > 30)
            {  //wait for 30 frames
                Cmd(0x20);  //send start command
                waitState = 3;
            }
            try
            {
                serialEvent();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        //--here call different draw and debug methods
        if (drawRawData)
        {  //white+grey
            dataDrawRaw();
        }
        if (drawLeapData)
        {  //yellow
            dataDrawLeap();
        }
        if (drawBorder)
        {  //blue
            dataDrawBorder();
        }
        if (drawCalibration)
        {  //green
            dataDrawCalibration();
        }

        //--main methods
        if (recording)
        {
            dataRecord();
        }
        else if (playing)
        {
            dataPlay();
        }
        dataLeap();
        dataBorder();
        dataTracking();
        trackingDraw();

        //--keys
        if (Input.GetKeyDown(KeyCode.A))
        {
            drawRawData = !drawRawData;
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            drawLeapData = !drawLeapData;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            drawBorder = !drawBorder;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            drawCalibration = !drawCalibration;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < border.Length; i++)
            {  //copy over current border once
                calibration[i] = border[i];
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (playing)
            {
                Debug.Log("warn: first stop playing (P)");
            }
            else
            {
                if (!recording)
                {
                    string fileName = recordingPath + "rec" + System.DateTime.Now.ToString("MM-dd-yy_hh-mm-ss");
                    Debug.Log("recording to: " + fileName);
                    Writer = new BinaryWriter(File.OpenWrite(fileName));
                }
                else
                {
                    Writer.Flush();
                    Writer.Close();
                    Debug.Log("stopped recording");
                }
                recording = !recording;
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (recording)
            {
                Debug.Log("warn: first stop recording (R)");
            }
            else
            {
                playing = !playing;
                if (playing)
                {
                    if (File.Exists(playingPath))
                    {
                        reader = new BinaryReader(File.Open(playingPath, FileMode.Open));
                        Debug.Log("playback started");
                    }
                    else
                    {
                        Debug.Log("error: file not found - edit path");
                    }
                }
                else
                {
                    Debug.Log("playback stopped");
                    reader.Close();
                    reader.Dispose();
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    /// record and playback from file
    ///
    void dataRecord()
    {  //save to disk
        for (int i = 0; i < scan.Length; i++)
        {
            Writer.Write(scan[i]);
        }
    }
    void dataPlay()
    {  //read from disk
        for (int i = 0; i < scan.Length; i++)
        {
            scan[i] = reader.ReadSingle();
        }
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            playing = false;
            reader.Close();
            reader.Dispose();
            Debug.Log("end of file");
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    /// draw graphics
    ///
    void dataDrawRaw()
    {  //draw the raw data
        Vector3 lastV = new Vector3(0, 0, 0);
        for (int i = 0; i < scan.Length; i++)
        {
            if (scan[i] > 0)
            {
                Vector3 v = scaleVector(toVector(i, scan[i]));
                if (lastV.magnitude > 0)
                {
                    if (Vector3.Distance(v, lastV) > drawThresh)
                    {
                        Debug.DrawLine(v, lastV, Color.grey);
                    }
                    else
                    {
                        Debug.DrawLine(v, lastV, Color.white);
                    }
                }
                lastV = v;
            }
        }
    }
    void dataDrawLeap()
    {
        Vector3 lastV = new Vector3(0, 0, 0);
        for (int i = 0; i < leap.Length; i++)
        {
            Vector3 v = scaleVector(toVector(i, leap[i]));
            if (i > 0)
            {
                Debug.DrawLine(v, lastV, Color.yellow);
            }
            lastV = v;
        }
    }
    void dataDrawBorder()
    {  //draw the border
        Vector3 lastV = new Vector3(0, 0, 0);
        for (int i = 0; i < border.Length; i++)
        {
            Vector3 v = scaleVector(toVector(i, border[i]));
            if (i > 0)
            {
                Debug.DrawLine(v, lastV, Color.blue);
            }
            lastV = v;
        }
    }
    void dataDrawCalibration()
    {  //draw the calibrated border
        Vector3 lastV = new Vector3(0, 0, 0);
        for (int i = 0; i < calibration.Length; i++)
        {
            Vector3 v = scaleVector(toVector(i, calibration[i]));
            if (i > 0)
            {
                Debug.DrawLine(v, lastV, Color.green);
            }
            lastV = v;
        }
    }
    void trackingDraw()
    {  //draw a cross if object found
        Vector3 v = scaleVector(new Vector3(trackingX, trackingY, 0));
        Debug.DrawLine(v + new Vector3(-0.1F, -0.1F, 0), v + new Vector3(0.1F, 0.1F, 0), Color.red);
        Debug.DrawLine(v + new Vector3(-0.1F, 0.1F, 0), v + new Vector3(0.1F, -0.1F, 0), Color.red);
    }
    Vector3 toVector(int index, float dist)
    {
        float a = (index / 720.0F) * Mathf.PI * 2.0F;
        float x = Mathf.Sin(a) * dist;
        float y = Mathf.Cos(a) * dist;
        return new Vector3(x, y, 0);
    }
    Vector3 scaleVector(Vector3 v)
    {
        return v * 0.001F * drawScale;
    }

    //////////////////////////////////////////////////////////////////////////////////////
    /// calculate border and analyse
    ///
    void dataLeap()
    {  //look before you leap
        int startIndex = 0;
        float prev = scan[0];
        while ((prev == 0.0F) && (startIndex < scan.Length))
        {  //find first non-zero distance
            prev = scan[startIndex];
            startIndex++;
        }
        float dist = prev;
        int counter = 0;
        for (int i = 0; i < scan.Length; i++)
        {
            int index = (startIndex + i) % scan.Length;
            if (scan[index] > 0.0F)
            {
                if (Mathf.Abs(scan[index] - dist) > leapThresh)
                {
                    counter++;
                    if (counter > leapSamples)
                    {
                        counter = 0;
                        dist = scan[index];
                    }
                }
                else
                {
                    counter = 0;
                    dist = scan[index];
                }
            }
            leap[index] = dist;
        }
    }
    void dataBorder()
    {  //calculate border
        int start = 0 - Mathf.FloorToInt((borderDetail + 1) / 2.0F);
        int end = Mathf.FloorToInt((borderDetail + 1) / 2.0F - 0.1F);
        for (int i = 0; i < leap.Length; i++)
        {
            float min = 99999.9F;
            int hits = 0;  //number of valid minimum points found per group
            for (int j = start; j < end; j++)
            {  //detail group
                int index = i + j;
                if (index < 0)
                {
                    index = leap.Length + index;
                }
                float distance = leap[index % leap.Length];
                if (distance > 0 && distance < min)
                {
                    min = distance;
                    hits++;
                }
            }
            if (hits > 0)
            {
                float newBorder = Mathf.Max(min - borderShrink, 1);
                border[i] = borderFilter * border[i] + ((1 - borderFilter) * newBorder);  //smoothing
            }
        }
    }
    void dataTracking()
    {  //object tracking
        float minDist = 99999.9F;
        int minIndex = -1;
        for (int i = 0; i < calibration.Length; i++)
        {
            if (leap[i] > 0 && leap[i] < calibration[i] && leap[i] < minDist)
            {
                minDist = leap[i];
                minIndex = i;
            }
        }
        tracking = minIndex > -1;
        if (tracking)
        {
            Vector3 v = toVector(minIndex, minDist);
            trackingX = trackingFilter * trackingX + ((1 - trackingFilter) * v.x);  //smoothing
            trackingY = trackingFilter * trackingY + ((1 - trackingFilter) * v.y);  //smoothing
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    /// read data from serial port
    ///
    void serialEvent()
    {
        //Debug.Log(port.ReadLine());
        while (port.BytesToRead > 0)
        {
            byte b = (byte)port.ReadByte();

            //Debug.Log (b);
            //Debug.Log (state);
            switch (state)
            {
                case 0:
                    if (b == 0xA5)
                    {
                        state = 1;
                    }
                    break;
                case 1:
                    if (b == 0x5A)
                    {
                        state = 2;
                    }
                    else
                    {
                        state = 0;
                    }
                    break;
                case 2:
                    dlength = b;
                    state = 3;
                    break;
                case 3:
                    dlength += (b << 8);
                    state = 4;
                    break;
                case 4:
                    dlength += (b << 16);
                    state = 5;
                    break;
                case 5:
                    dlength += ((b & 63) << 24);
                    //Debug.Log("dlength: " + ((int)dlength).ToString("X"));
                    dmode = b >> 6;  //0=single, 1=continuous, 2+3=undefined
                    state = 6;
                    //Debug.Log(((int)b).ToString("X"));
                    //Debug.Log("dmode: " + ((int)dmode).ToString("X"));
                    break;
                case 6:
                    dtype = b;
                    Debug.Log(((int)b).ToString("X"));
                    if (dtype == 0x81 && dmode == 1)
                    {  //scanning
                        state = 100; //100
                        Debug.Log("Bytes to read: " + port.BytesToRead);
                    }
                    else if (dtype == 0x04 && dmode == 0)
                    {  //device information
                        state = 10;
                    }
                    else if (dtype == 0x06 && dmode == 0)
                    {  //health status
                        state = 11;
                    }
                    else
                    {
                        Debug.Log("fail: parser - type code not found");
                        state = 0;
                    }
                    break;
                case 10:
                    minfo.Add(b);
                    dlength--;
                    if (dlength == 0)
                    {
                        string modelnum = "" + minfo[0];
                        string firmware = "" + minfo[2] + "." + minfo[1];
                        string hardware = "" + minfo[3];
                        string serialnum = "";
                        for (int i = 0; i < 16; i++)
                        {
                            serialnum += minfo[i + 4];
                        }
                        Debug.Log("info: Model number " + modelnum + ", Firmware version " + firmware + ", Hardware version " + hardware + ", Serial number " + serialnum);
                        minfo.Clear();
                        state = 0;
                    }
                    break;
                case 11:
                    mstat.Add(b);
                    dlength--;
                    if (dlength == 0)
                    {
                        string status = "";
                        if (mstat[0] == 0)
                        {
                            status = "device is running normally";
                        }
                        else if (mstat[0] == 1)
                        {
                            status = "device is running";
                        }
                        else if (mstat[0] == 2)
                        {
                            status = "device is running incorrectly";
                        }
                        string error = "" + mstat[1] + mstat[2];
                        Debug.Log("status: " + status);
                        if (error != "00")
                        {
                            Debug.Log("error: status error code " + error);
                        }
                        mstat.Clear();
                        state = 0;
                    }
                    break;
                case 99:
                    state = 100;
                    break;
                case 100:
                    //Debug.Log ("scanning: started");
                    Debug.Log(((int)b).ToString("X"));

                    var bits = new BitArray(new byte[] { b });
                    Debug.Log(bits[0].ToString() + bits[1].ToString());
                    Debug.Log("Quality: " + ((int)(b>>2)).ToString("X"));

                    //if (b == 0xAA)
                    if (bits[0] == true && bits[1] == false)
                    {  //packet header (PH)
                        pdata.Clear();
                        pdata.Add(b);
                        state = 101;
                        Debug.Log("New frame");
                    }
                    else if (b == 0xA5)
                    {
                        state = 1;
                    }
                    else
                    {
                        state = 0;
                    }
                    break;
                case 101:
                    if (b == 0x55)
                    {
                        pdata.Add(b);
                        state = 102;
                    }
                    else
                    {
                        state = 0;
                    }
                    break;
                case 102:
                    pdata.Add(b);  //packet type (CT). 0=point cloud, 1=zero packet
                    state = 103;
                    break;
                case 103:
                    pdata.Add(b);  //sample quantity (LSN)
                    pcounter = b * 2;
                    state = 104;
                    break;
                case 104:
                    pdata.Add(b);  //starting angle (FSA)
                    state = 105;
                    break;
                case 105:
                    pdata.Add(b);
                    state = 106;
                    break;
                case 106:
                    pdata.Add(b);  //end angle (LSA)
                    state = 107;
                    break;
                case 107:
                    pdata.Add(b);
                    state = 108;
                    break;
                case 108:
                    pdata.Add(b);  //check code (CS)
                    state = 109;
                    break;
                case 109:
                    pdata.Add(b);
                    state = 110;
                    break;
                case 110:
                    pdata.Add(b);  //sampling data (Si)
                    pcounter--;
                    if (pcounter == 0)
                    {
                        //if (checksum())
                        //{
                            parsePacket();
                        //}
                        //else
                        //{
                        //    Debug.Log("error: packet checksum");
                        //}
                        state = 102; //100
                    }
                    break;

                case 200:
                    //Debug.Log(((int)b).ToString("X"));
                    state = 200;
                    break;
            }
        }
    }
    bool checksum()
    {
        int a = pdata[0] + (pdata[1] << 8);
        a ^= pdata[4] + (pdata[5] << 8);
        for (int i = 0; i < pdata[3]; i++)
        {
            a ^= pdata[i * 2 + 10] + (pdata[i * 2 + 11] << 8);
        }
        a ^= pdata[2] + (pdata[3] << 8);
        a ^= pdata[6] + (pdata[7] << 8);
        int c = pdata[8] + (pdata[9] << 8);  //check code
        return a == c;
    }
    void parsePacket()
    {
        int ct = pdata[2];  //package type
        if (ct == 0)
        {
            //Debug.Log ("_______point cloud packet");
            int lsn = pdata[3];  //sample quantity
            int fsa = pdata[4] + (pdata[5] << 8);  //starting angle
            int lsa = pdata[6] + (pdata[7] << 8);  //end angle
            float dist1 = distance(1);    //in mm
            float distLsn = distance(lsn);  //in mm
            float angleFsa = ((fsa >> 1) / 64.0F) + angCorrect(dist1);  //in deg
            float angleLsa = ((lsa >> 1) / 64.0F) + angCorrect(distLsn);  //in deg
            storeInScanArray(angleFsa, dist1);
            storeInScanArray(angleLsa, distLsn);
            float diffAngle;
            if (angleFsa > angleLsa)
            {
                diffAngle = angleLsa + 360.0F - angleFsa;
            }
            else
            {
                diffAngle = angleLsa - angleFsa;
            }
            for (int i = 2; i < (lsn - 1); i++)
            {
                float distMid = distance(i);  //in mm
                float angleMid = (diffAngle / (lsn - 1)) * (i - 1) + angleFsa + angCorrect(distMid);  //in deg
                if (angleMid < 0.0F)
                {
                    angleMid = 360.0f - angleMid;
                }
                storeInScanArray(angleMid % 360.0F, distMid);
            }
        }
        else if (ct == 1)
        {
            //Debug.Log ("_______zero packet");
            //TODO what to do here? just ignore?
        }
    }
    void storeInScanArray(float angle, float distance)
    {
        int index = Mathf.RoundToInt(angle * 2.0F) % 720;
        scan[index] = distance;
    }
    float distance(int index)
    {
        int pindex = (index - 1) * 2 + 10;  //byte index in data package
        return (pdata[pindex] + (pdata[pindex + 1] << 8)) / 4.0F;
    }
    float angCorrect(float distance)
    {
        if (distance > 0.0F)
        {
            return Mathf.Atan(21.8F * ((155.3F - distance) / (155.3F * distance))) / Mathf.PI * 180.0F;
        }
        return 0.0F;
    }
}