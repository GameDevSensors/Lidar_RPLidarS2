//
//https://github.com/mikkleini/rplidar.net
//

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using RPLidar;

public class CRpLidar
{
    public List<RPLidar.Measurement> scanPoints = new List<Measurement>();

    private Task lidarTask;
    private readonly Lidar lidar = new Lidar();
    private delegate void UpdateScanDelegate(Scan scan);
    private CancellationTokenSource cancellationSource;

    public string PortName
    {
        get => lidar.PortName;
        set => lidar.PortName = value;
    }

    public int BaudRate
    {
        get => lidar.Baudrate;
        set => lidar.Baudrate = value;
    }

    public int ReceiveTimeout
    {
        get => lidar.ReceiveTimeout;
        set => lidar.ReceiveTimeout = value;
    }

    public bool IsFlipped
    {
        get => lidar.IsFlipped;
        set => lidar.IsFlipped = value;
    }

    public float AngleOffset
    {
        get => lidar.AngleOffset;
        set => lidar.AngleOffset = value;
    }

    public ushort MotorSpeed
    {
        get => motorSpeed;
        set
        {
            motorSpeed = value;
        }
    }
    private ushort motorSpeed;

    public void SetMotorSpeed(ushort value)
    {
        motorSpeed = value;
        lidar.SetMotorSpeed(value);
    }

    public bool Initialize()
    {
        //cancellationSource = new CancellationTokenSource();

        // Try to open port
        if (lidar.Open())
        {
            // Try to start lidar
            return StartLidar(ScanMode.Legacy);
        }

        return false;
    }

    public void Stop()
    {
        // Close port
        lidar.Close();

        // Stop scanning
        lidar.StopScan();
        //lidar.ControlMotorDtr(true);

        // Report
        Debug.Log("Scanning stopped");
    }

    public void Reset()
    {
        lidar.Reset();
    }

    public List<RPLidar.Measurement> Scan()
    {
        // Try to get scan
        Scan scan = lidar.GetScan();
        if (scan == null)
        {
            // It was either cancellation or error
            return null;
        }

        return scan.Measurements;
    }

    public void RunScan()
    {
        // Start scan task
        lidarTask = Task.Run(() => Run(ScanMode.Legacy, cancellationSource.Token));
    }

    public void Run(ScanMode mode, CancellationToken cancellationToken)
    {
        // Main loop
        while (!cancellationToken.IsCancellationRequested)
        {
            // Run lidar
            if (!RunLidar(cancellationToken))
            {
                // Reset and try to start again
                lidar.Reset();
                continue;
            }
        }

        // Stop lidar
        StopLidar();
    }

    public bool UnityStart()
    {
        cancellationSource = new CancellationTokenSource();

        // Try to open port
        if (lidar.Open())
        {
            // Start scan task
            lidarTask = Task.Run(() => ScanLoop(ScanMode.Legacy, cancellationSource.Token));
            //Escribimos el puerto y la velocidad de transmision
            //Debug.Log("Puerto: " + lidar.PortName + " BaudRate: " + lidar.Baudrate);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Scan task
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public void ScanLoop(ScanMode mode, CancellationToken cancellationToken)
    {
        // Main loop
        while (!cancellationToken.IsCancellationRequested)
        {
            // Try to start lidar
            if (!StartLidar(mode))
            {
                // Reset and try to start again in a while to avoid high CPU load if something breaks
                lidar.Reset();
                Thread.Sleep(1000);
                continue;
            }

            // Run lidar
            if (!RunLidar(cancellationToken))
            {
                // Reset and try to start again
                lidar.Reset();
                continue;
            }
        }

        // Stop lidar
        StopLidar();
    }

    /// <summary>
    /// Start lidar (at least try)
    /// </summary>
    /// <param name="mode">Scan mode</param>
    /// <returns>true if succeeded, false if not</returns>
    private bool StartLidar(ScanMode mode)
    {
        // Get health
        HealthInfo health = lidar.GetHealth();
        if (health == null)
        {
            return false;
        }

        // Good health ?
        if (health.Status != HealthStatus.Good)
        {
            Debug.LogWarning($"Health {health.Status}, error code {health.ErrorCode}.");
            return false;
        }

        // Good health
        Debug.Log($"Health good.");

        // Get configuration
        Configuration config = lidar.GetConfiguration();
        if (config == null)
        {
            return false;
        }

        // Show configuration
        Debug.Log("Configuration:");
        foreach (KeyValuePair<ushort, ScanModeConfiguration> modeConfig in config.Modes)
        {
            Debug.Log($"0x{modeConfig.Key:X4} - {modeConfig.Value}"
                + (config.Typical == modeConfig.Key ? " (typical)" : string.Empty));
        }

        // Start motor
        //lidar.ControlMotorDtr(false);

        // Start scanning
        if (!lidar.StartScan(mode))
        {
            return false;
        }

        //0 a 1023
        lidar.SetMotorSpeed(motorSpeed);

        // Report
        Debug.Log("Scanning started.");

        return true;
    }

    /// <summary>
    /// Stop lidar
    /// </summary>
    private void StopLidar()
    {
        // Stop scanning
        lidar.StopScan();
        //lidar.ControlMotorDtr(true);

        // Report
        Debug.Log("Scanning stopped");
    }

    public void StopLidarTask()
    {
        // Cancel scanning
        if (cancellationSource != null)
        {
            cancellationSource.Cancel();
            cancellationSource.Dispose();
        }
        if (lidarTask != null)
        {
            lidarTask.GetAwaiter().GetResult();
        }

        // Close port
        lidar.Close();
    }

    /// <summary>
    /// Run lidar task
    /// </summary>
    /// <param name="cancellationToken"></param>
    private bool RunLidar(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Try to get scan
            Scan scan = lidar.GetScan(cancellationToken);
            if (scan == null)
            {
                // It was either cancellation or error
                return cancellationToken.IsCancellationRequested;
            }

            // Display it
            //UpdateScan(scan);
            scanPoints = new List<Measurement>(scan.Measurements);
        }

        // Normal exit
        return true;
    }

    /// <summary>
    /// Update scan
    /// </summary>
    /// <param name="scan">Scan object</param>
    private void UpdateScan(Scan scan)
    {
        // Show stats
        Debug.Log(scan.ScanRate.ToString("f2"));
        Debug.Log(scan.Measurements.Count.ToString());
        //Debug.Log(scanPoints.Count.ToString());

        /*foreach (Measurement measurement in scan.Measurements)
        {
            // Skip zero-distance (failed) measurements
            if (measurement.Distance <= float.Epsilon) continue;

            float X = (int)(measurement.Distance * (float)Mathf.Cos(Mathf.PI / 180.0f * measurement.Angle)) + transform.position.x;
            float Y = (int)(measurement.Distance * (float)Mathf.Sin(Mathf.PI / 180.0f * measurement.Angle)) + transform.position.y;

            if(measurement.Distance < 0.2f)
                Debug.Log("X: " + X + " Y: " + Y);

        }*/

        Debug.Log(scan.Measurements[0].Distance);
    }
}
