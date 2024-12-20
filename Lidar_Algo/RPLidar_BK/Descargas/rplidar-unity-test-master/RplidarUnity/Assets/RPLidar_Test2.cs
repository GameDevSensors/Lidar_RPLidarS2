//
//https://github.com/mikkleini/rplidar.net
//


using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using RPLidar;

public class RPLidar_Test2 : MonoBehaviour
{
    private Task lidarTask;
    private readonly Lidar lidar = new Lidar();
    private delegate void UpdateScanDelegate(Scan scan);
    private CancellationTokenSource cancellationSource;

    void Start()
    {
        cancellationSource = new CancellationTokenSource();

        // Decide port
        lidar.PortName = "COM4";

        // Set timeout high enough to allow slow speed scanning
        lidar.ReceiveTimeout = 3000;

        // Flipped ?
        lidar.IsFlipped = false;
    
        // Angle offset
        lidar.AngleOffset = 0.0f;

        // Try to open port
        if (lidar.Open())
        {
            // Start scan task
            lidarTask = Task.Run(() => Scan(ScanMode.Legacy, cancellationSource.Token));
        }
    }

    /// <summary>
    /// Scan task
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private void Scan(ScanMode mode, CancellationToken cancellationToken)
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
        //lidar.SetMotorSpeed(0);

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

    private void StopLidarTask()
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
            UpdateScan(scan);
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

    private void OnDisable()
    {
        StopLidarTask();
    }
}
