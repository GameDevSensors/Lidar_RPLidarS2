
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LidarManager : MonoBehaviour
{
    public LidarModel lidarModel;

    [Space]
    public float zoomSpeed = 1f;
    public float panningSpeed = 1f;
    public float epsilon = 0.1f;
    public float minDistanceThreshold = 0.1f;
    public ushort motorSpeed = 600;

    public bool showLidarSetupOnStart = false;

    [Space]
    public Material lineMaterial;
    public GameObject pointPrefab;
    public GameObject cursorPrefab;
    public GameObject cornerPrefab;
    public GameObject screenCursorPrefab;
    public GameObject canvasPrefab;
    public GameObject lidarModelParent;
    public GameObject X4;
    public GameObject S2;
    public Camera lidarCamera;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI speedText;
    public Slider rotatonSlider;
    public Slider speedSlider;
    public Button saveButton;
    public TextMeshProUGUI calibText;
    public TextMeshProUGUI widthText;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI buttonText;
    public GameObject portPanel;
    public TMP_InputField portInputField;
    public GameObject canvas;

    // Singleton
    private static LidarManager _instance;
    public static LidarManager Instance { get { return _instance; } }

    public enum LidarModel
    {
        YDLidar_X4 = 0,
        RPLidar_S2 = 1
    }

    [HideInInspector]
    public List<Vector3> LidarTouchPoints
    {
        get { return lidarTP; }
        set { }
    }

    [HideInInspector]
    public List<Vector3> LidarPoints
    {
        get { return lidarP; }
        set { }
    }

    private List<LidarTouchPoint> lidarTouchPoints = new List<LidarTouchPoint>();

    public struct LidarTouchPoint
    {
        public int id;
        public Vector3 position;

        public LidarTouchPoint(int id, Vector3 position)
        {
            this.id = id;
            this.position = position;
        }
    }

    private Dictionary<int, Vector3> idToPosition = new Dictionary<int, Vector3>();
    private int nextId = 1;
    private int cornerID = -1;

    private List<Transform> points = new List<Transform>();
    private List<Vector2> scanPoints = new List<Vector2>();
    private List<float> borderRanges = new List<float>();
    private List<Vector3> lidarTP = new List<Vector3>();
    private List<Vector3> lidarP = new List<Vector3>();
    private List<GameObject> cursors = new List<GameObject>();
    private List<GameObject> screenCursors = new List<GameObject>();
    private List<GameObject> corners = new List<GameObject>();

    private const string filePath = "C:/Users/saborea el susto/Downloads";
    private const string fileName = "lidarSettings.json";

    // YDLidar X4
    private CYdLidar ydLidarClass;
    private bool ret;
    private LaserScan scan;

    // RPLidar S2
    private CRpLidar rpLidarClass;

    private bool scanFlag = true;
    private bool borderFlag = false;
    private bool calibrating = false;
    private bool setup = false;
    private bool showCursors = false;
    private bool showScanedPoints = true;
    private bool runonce = false;
    private int lastTPCount = 0;
    private bool calibrated = false;
    private Vector3[] vertices = new Vector3[4];
    private Vector2[] normals = new Vector2[4];
    private Vector3 previousMousePosition;
    private Vector3 dragOffset;
    private LineRenderer areaLine;
    private string port = "COM8";

    private GameObject corner;
    private bool isDragging;

    private LidarSettings lidarSettings;
    private Coroutine calibrateCoroutine;

    private string[] keys = {
            "BLC",
            "BRC",
            "TRC",
            "TLC"
    };

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        if (File.Exists(filePath + fileName))
        {
            var sr = File.OpenText(filePath + fileName);
            var json = sr.ReadLine();
            lidarSettings = JsonUtility.FromJson<LidarSettings>(json);
        }
        else
        {
            Debug.Log("Could not Open the file: " + filePath + fileName + " for reading.");
            return;
        }
    }

    private void Start()
    {
        portPanel.SetActive(false);

        // Create childs
        GameObject goPoints = new GameObject("Points");
        goPoints.transform.parent = transform;
        goPoints.transform.rotation = Quaternion.Euler(180f, 0f, 0f);

        GameObject goCursors = new GameObject("Cursors");
        goCursors.transform.parent = transform;

        GameObject goCorners = new GameObject("Corners");
        goCorners.transform.parent = transform;

        GameObject goScreenCursors = new GameObject("ScreenCursors");
        goScreenCursors.transform.parent = transform;

        /*ret = lidarClass.initialize();
        if (ret)
        {
            ret = lidarClass.turnOn();
        }
        else
        {
            Debug.Log("error:" + lidarClass.DescribeError());
        }
        scan = new LaserScan();
        scan.scanFreq = 11f;

        // Start scan
        StartCoroutine(_Scan());*/


        // Setup area Line
        areaLine = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        areaLine.positionCount = 4;
        areaLine.material = lineMaterial;
        areaLine.startWidth = areaLine.endWidth = 0.03f;
        areaLine.loop = true;
        areaLine.enabled = false;

        // Get saved settings
        //if (PlayerPrefs.HasKey("LidarCamera_Slider"))
        if (lidarSettings != null)
        {
            //vertices[0] = new Vector3(PlayerPrefs.GetFloat("BLC_X"), PlayerPrefs.GetFloat("BLC_Y"), 0f);
            //vertices[1] = new Vector3(PlayerPrefs.GetFloat("BRC_X"), PlayerPrefs.GetFloat("BRC_Y"), 0f);
            //vertices[2] = new Vector3(PlayerPrefs.GetFloat("TRC_X"), PlayerPrefs.GetFloat("TRC_Y"), 0f);
            //vertices[3] = new Vector3(PlayerPrefs.GetFloat("TLC_X"), PlayerPrefs.GetFloat("TLC_Y"), 0f);

            vertices[0] = new Vector3(lidarSettings.BLC.x, lidarSettings.BLC.y, 0f);
            vertices[1] = new Vector3(lidarSettings.BRC.x, lidarSettings.BRC.y, 0f);
            vertices[2] = new Vector3(lidarSettings.TRC.x, lidarSettings.TRC.y, 0f);
            vertices[3] = new Vector3(lidarSettings.TLC.x, lidarSettings.TLC.y, 0f);

            //vertices = SortVertices(Vector3.forward, vertices.ToList());

            for (int i = 0; i < normals.Length; i++)
            {
                Vector2 dir = vertices[i < normals.Length - 1 ? i + 1 : 0] - vertices[i];
                Vector2 normal = new Vector2(-dir.y, dir.x);
                normal.Normalize();
                normals[i < normals.Length - 1 ? i + 1 : 0] = normal;

                //Debug.Log(i + " " + vertices[i]);
            }

            if (vertices[0].sqrMagnitude > 0f && vertices[1].sqrMagnitude > 0f && vertices[2].sqrMagnitude > 0f && vertices[3].sqrMagnitude > 0f)
            {
                calibrated = true;
            }

            if (lidarModel == LidarModel.RPLidar_S2)
            {
                motorSpeed = lidarSettings.MotorSpeed;
            }
        }

        transform.GetChild(0).gameObject.SetActive(false);

        widthText.text = Screen.width.ToString();
        heightText.text = Screen.height.ToString();

        X4.SetActive(lidarModel == LidarModel.YDLidar_X4);
        S2.SetActive(lidarModel == LidarModel.RPLidar_S2);
        speedSlider.gameObject.SetActive(lidarModel == LidarModel.RPLidar_S2);

        StartCoroutine(_WaitLidar());

        if (showLidarSetupOnStart)
        {
            Setup();
            canvas.SetActive(false);
            showScanedPoints = false;
            transform.Find("Points").gameObject.SetActive(showScanedPoints);
        }

        // cambiamos el size de la camara a 1
        lidarCamera.orthographicSize = 2f;
        // subimos la camara 3 unidad en el eje y
        lidarCamera.transform.position = new Vector3(0, 0.6f, 18);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C) && ret && calibrated)
        {
            showCursors = !showCursors;
            transform.Find("ScreenCursors").gameObject.SetActive(showCursors);
        }

        if (Input.GetKeyUp(KeyCode.P) && ret && calibrated)
        {
            showScanedPoints = !showScanedPoints;
            transform.Find("Points").gameObject.SetActive(showScanedPoints);
        }

        if (Input.GetKeyUp(KeyCode.L) && ret)
        {
            if(showLidarSetupOnStart)
            {
                canvas.SetActive(!canvas.activeSelf);
            }
            else
            {
                Setup();
            }
        }

        if (setup)
        {
#if UNITY_EDITOR
            // Debug borders
            if (showScanedPoints)
            {
                for (int i = 1; i < borderRanges.Count; i++)
                {
                    float rot = Mathf.Deg2Rad * transform.Find("Points").eulerAngles.z;
                    float x0 = borderRanges[i] * Mathf.Cos(scanPoints[i].x + rot);
                    float y0 = borderRanges[i] * Mathf.Sin(scanPoints[i].x + rot);

                    float x1 = borderRanges[i - 1] * Mathf.Cos(scanPoints[i - 1].x + rot);
                    float y1 = borderRanges[i - 1] * Mathf.Sin(scanPoints[i - 1].x + rot);

                    Debug.DrawLine(new Vector3(x0, y0, 0), new Vector3(x1, y1, 0), Color.blue);
                }

                // Debug laser rays
                for (int i = 0; i < points.Count; i++)
                {
                    Debug.DrawLine(lidarModelParent.transform.position, points[i].position);
                }
            }
#endif

            // Rotate Lidar
            //lidarModelParent.transform.GetChild(1).Rotate(lidarModelParent.transform.up, Time.deltaTime * 1000f);

            // Zoom (Mouse wheel)
            float zoomDelta = Input.mouseScrollDelta.y;
            if (zoomDelta != 0f)
            {
                lidarCamera.orthographicSize = Mathf.Clamp(lidarCamera.orthographicSize - zoomDelta * zoomSpeed, 0.15f, 20f);
                lidarModelParent.transform.localScale = Vector3.one * Mathf.Max(lidarCamera.orthographicSize / 10f, 0.1f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray mouseRay = lidarCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit, 100f, LayerMask.GetMask("Lidar")))
                {
                    if (hit.collider.name.Contains("Corner") && canvas.activeSelf)
                    {
                        corner = hit.transform.gameObject;
                        cornerID = corners.IndexOf(corner);
                        isDragging = true;

                        dragOffset = hit.point - hit.collider.transform.position;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                cornerID = -1;
            }

       

            // Pan (Right Click)
            if (Input.GetMouseButtonDown(1))
            {
                previousMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                Vector3 deltaMouse = Input.mousePosition - previousMousePosition;
                Vector3 panDirection = new Vector3(-deltaMouse.x, -deltaMouse.y, 0f) * panningSpeed * lidarCamera.orthographicSize / 10f;

                lidarCamera.transform.Translate(panDirection, Space.Self);
            }

            previousMousePosition = Input.mousePosition;
        }
    }

    private void LateUpdate()
    {
        if (isDragging)
        {
            Vector3 newPosition = lidarCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, (lidarCamera.transform.position.z > 0 ? 1f : -1f) * lidarCamera.transform.position.z));
            newPosition -= dragOffset;
            newPosition.z = 0f;
            corner.transform.position = newPosition;

            int indx = cornerID;
            /*if (corner.name.Contains("0")) indx = 0;
            else if (corner.name.Contains("1")) indx = 1;
            else if (corner.name.Contains("2")) indx = 2;
            else if (corner.name.Contains("3")) indx = 3;*/

            vertices[indx] = corner.transform.position;
            areaLine.SetPosition(indx, vertices[indx]);

            float distance = Vector3.Distance(vertices[indx], vertices[indx < 4 - 1 ? indx + 1 : 0]);
            corners[indx].GetComponentsInChildren<TextMeshProUGUI>(true)[1].GetComponentInChildren<TextMeshProUGUI>().text = distance.ToString("F2") + " m";
            corners[indx].GetComponentsInChildren<TextMeshProUGUI>(true)[1].transform.position = vertices[indx] + ((vertices[indx < 4 - 1 ? indx + 1 : 0] - vertices[indx]).normalized * distance / 2f);

            indx = indx == 0 ? 4 - 1 : indx - 1;
            distance = Vector3.Distance(vertices[indx], vertices[indx < 4 - 1 ? indx + 1 : 0]);
            corners[indx].GetComponentsInChildren<TextMeshProUGUI>(true)[1].GetComponentInChildren<TextMeshProUGUI>().text = distance.ToString("F2") + " m";
            corners[indx].GetComponentsInChildren<TextMeshProUGUI>(true)[1].transform.position = vertices[indx] + ((vertices[indx < 4 - 1 ? indx + 1 : 0] - vertices[indx]).normalized * distance / 2f);

            //vertices = SortVertices(Vector3.forward, vertices.ToList());

            for (int i = 0; i < normals.Length; i++)
            {
                Vector2 dir = vertices[i < normals.Length - 1 ? i + 1 : 0] - vertices[i];
                Vector2 normal = new Vector2(-dir.y, dir.x);
                normal.Normalize();
                normals[i < normals.Length - 1 ? i + 1 : 0] = normal;
            }
        }
    }

    private void Setup()
    {
        setup = !setup;

        areaLine.enabled = setup;

        transform.GetChild(0).gameObject.SetActive(setup);

        transform.Find("Cursors").gameObject.SetActive(setup);
        transform.Find("Points").gameObject.SetActive(setup);

        if (setup && !runonce && calibrated)
        {
            if (vertices[0].sqrMagnitude > 0f && vertices[1].sqrMagnitude > 0f && vertices[2].sqrMagnitude > 0f && vertices[3].sqrMagnitude > 0f)
            {
                calibrated = true;

                for (int i = 0; i < 4; i++)
                {
                    areaLine.SetPosition(i, vertices[i]);

                    if (i >= corners.Count)
                    {
                        corners.Add(Instantiate(cornerPrefab, transform.Find("Corners")));
                        corners[i].name = "Corner_" + i;
                        corners[i].GetComponentInChildren<TextMeshProUGUI>(true).text = keys[i];
                    }
                    corners[i].transform.position = vertices[i];
                }

                //Debug.Log(vertices[0] + " " + vertices[1] + " " + vertices[2] + " " + vertices[3] + " " + Screen.width + " " + Screen.height);
            }

            // Apply settings
            //if (PlayerPrefs.HasKey("LidarCamera_Slider"))
            if (lidarSettings != null)
            {
                //lidarCamera.transform.position = new Vector3(PlayerPrefs.GetFloat("LidarCamera_X_Pos"), PlayerPrefs.GetFloat("LidarCamera_Y_Pos"), PlayerPrefs.GetFloat("LidarCamera_Z_Pos"));
                //lidarCamera.transform.rotation = Quaternion.Euler(0f, PlayerPrefs.GetFloat("LidarCamera_Mirror_Rot"), 0f);
                //lidarCamera.orthographicSize = PlayerPrefs.GetFloat("LidarCamera_OrthoSize");
                //rotatonSlider.value = PlayerPrefs.GetFloat("LidarCamera_Slider");

                lidarCamera.transform.position = lidarSettings.Camera_Pos;
                lidarCamera.transform.rotation = Quaternion.Euler(0f, lidarSettings.Camera_Mirror_Rot, 0f);
                lidarCamera.orthographicSize = lidarSettings.Camera_OrthoSize;
                rotatonSlider.value = lidarSettings.Camera_Slider;

                lidarModelParent.transform.localScale = Vector3.one * Mathf.Max(lidarCamera.orthographicSize / 10f, 0.1f);
            }

            if (calibrated)
            {
                for (int i = 0; i < 4; i++)
                {
                    corners[i].GetComponentInChildren<TextMeshProUGUI>(true).transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);

                    GameObject lengthTxt = Instantiate(canvasPrefab, corners[i].transform);
                    lengthTxt.transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);

                    float distance = Vector3.Distance(vertices[i], vertices[i < 4 - 1 ? i + 1 : 0]);
                    lengthTxt.GetComponentInChildren<TextMeshProUGUI>().text = distance.ToString("F2") + " m";
                    lengthTxt.transform.position = vertices[i] + ((vertices[i < 4 - 1 ? i + 1 : 0] - vertices[i]).normalized * distance / 2f);
                }
            }

            runonce = true;
        }
    }

    // Funci�n para ordenar los puntos de las esquinas empezando de la esquina inferior izquierda y en sentido contrario al giro del reloj.
    // No funciona bien, hay que mejorar.
    public Vector3[] SortVertices(Vector3 normal, List<Vector3> verts)
    {
        Vector3 first = verts[0];

        List<Vector3> temp = verts.OrderBy(n => Vector3.Distance(n, first)).ToList();

        Vector3 refrenceVec = (temp[1] - first);

        List<Vector3> results = temp.Skip(1).OrderBy(n => Vector3.Angle(refrenceVec, n - first)).ToList();

        results.Insert(0, verts[0]);

        if ((Vector3.Cross(results[1] - results[0], results[2] - results[0]).normalized + normal.normalized).magnitude < 1.414f)
        {
            results.Reverse();
        }

        Vector3 tmp = results[0];
        results[0] = results[1];
        results[1] = results[2];
        results[2] = results[3];
        results[3] = tmp;

        return results.ToArray();
    }

    // Calibration
    private IEnumerator Calibrate()
    {
        if (lidarSettings != null)
        {
            for (int i = 0; i < 4; i++)
            {
                vertices[i] = Vector3.zero;
                normals[i] = Vector2.zero;
                areaLine.SetPosition(i, vertices[i]);
                corners[i].transform.position = vertices[i];
                corners[i].GetComponentsInChildren<TextMeshProUGUI>(true)[1].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        areaLine.loop = false;

        calibrating = true;

        float x = 0, y = 0;
        Vector3 worldPoint = Vector3.zero;
        Collider[] hitColliders;
        string instStart = "Se�ala la esquina ";
        string instEnd = " en la superficie interactiva y posteriormente confirma en la pantalla dando click en esa misma posici�n.";
        string[] instCorners = {
            "<b>inferior izquierda</b>",
            "<b>inferior derecha</b>",
            "<b>superior derecha</b>",
            "<b>superior izquierda</b>"
        };

        for (int i = 0; i < areaLine.positionCount; i++)
        {
            areaLine.SetPosition(i, new Vector3(0f, 0f, 0f));
            if (corners.Count > i) corners[i].transform.localScale = Vector3.zero;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 4; i++)
        {
            hitColliders = new Collider[] { };
            calibText.text = instStart + instCorners[i] + instEnd;

            bool click = false;
            //while (!(Input.GetMouseButtonUp(0) && hitColliders.Length > 0))
            while (!click)
            {
                worldPoint = lidarCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, (lidarCamera.transform.position.z > 0 ? 1f : -1f) * lidarCamera.transform.position.z));

                if (cursors.Count == 0)
                {
                    GameObject go = Instantiate(cursorPrefab, transform.Find("Cursors"));
                    cursors.Add(go);
                }
                else
                {
                    cursors[0].SetActive(true);
                }

                cursors[0].transform.position = worldPoint;

                //hitColliders = Physics.OverlapSphere(worldPoint, cursors[0].transform.localScale.x/2f, LayerMask.GetMask("Lidar"));

                if (!EventSystem.current.IsPointerOverGameObject())
                { 
                    click = Input.GetMouseButtonUp(0);
                }
                yield return null;
            }

            //x = hitColliders[0].transform.position.x;
            //y = hitColliders[0].transform.position.y;

            x = worldPoint.x;
            y = worldPoint.y;

            vertices[i] = new Vector3(x, y, 0f);

            for (int j = i; j < 4; j++)
            {
                areaLine.SetPosition(j, vertices[i]);
            }

            if (i >= corners.Count)
            {
                corners.Add(Instantiate(cornerPrefab, transform.Find("Corners")));
                corners[i].name = "Corner_" + i;
                corners[i].GetComponentInChildren<TextMeshProUGUI>(true).text = keys[i];
            }
            else
            {
                corners[i].transform.localScale = Vector3.one * cornerPrefab.transform.localScale.x;
            }
            corners[i].transform.position = vertices[i];

            if(corners[i].GetComponentsInChildren<TextMeshProUGUI>(true).Length < 2)
            {
                GameObject lengthTxt = Instantiate(canvasPrefab, corners[i].transform);
                lengthTxt.transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);
            }

            if (i > 0)
            {
                float distance = Vector3.Distance(vertices[i], vertices[i - 1]);
                corners[i - 1].GetComponentsInChildren<TextMeshProUGUI>(true)[1].text = distance.ToString("F2") + " m";
                corners[i - 1].GetComponentsInChildren<TextMeshProUGUI>(true)[1].transform.position = vertices[i- 1] + ((vertices[i] - vertices[i - 1]).normalized * distance / 2f);
            }
        }

        //vertices = SortVertices(Vector3.forward, vertices.ToList());

        for (int i = 0; i < normals.Length; i++)
        {
            Vector2 dir = vertices[i < normals.Length - 1 ? i + 1 : 0] - vertices[i];
            Vector2 normal = new Vector2(-dir.y, dir.x);
            normal.Normalize();
            normals[i < normals.Length - 1 ? i + 1 : 0] = normal;
        }

        float dist = Vector3.Distance(vertices[0], vertices[3]);
        corners[3].GetComponentsInChildren<TextMeshProUGUI>(true)[1].text = dist.ToString("F2") + " m";
        corners[3].GetComponentsInChildren<TextMeshProUGUI>(true)[1].transform.position = vertices[3] + ((vertices[0] - vertices[3]).normalized * dist / 2f);

        areaLine.loop = true;

        cursors[0].SetActive(false);

        calibText.text = "";

        calibrated = true;

        calibrating = false;

        buttonText.text = "Calibrate";

        yield return null;
    }

    public void OnChangePortInputFieldText(string text)
    {
        port = text;

        if (lidarModel == LidarModel.YDLidar_X4)
        {
            ydlidar.os_init();
            ydLidarClass = new CYdLidar();

            int optname = (int)LidarProperty.LidarPropSerialPort;
            ydLidarClass.setlidaropt(optname, port);

            optname = (int)LidarProperty.LidarPropSerialBaudrate;
            ydLidarClass.setlidaropt(optname, 128000);

            optname = (int)LidarProperty.LidarPropLidarType;
            int lidarType = (int)LidarTypeID.TYPE_TRIANGLE;
            ydLidarClass.setlidaropt(optname, lidarType);
        }
        else if (lidarModel == LidarModel.RPLidar_S2)
        {
            rpLidarClass.PortName = port;
        }
    }

    private IEnumerator _WaitLidar()
    {
        bool showPortPanel = false;

        while(SerialPort.GetPortNames().Length == 0)
        {
            yield return null;
        }

        port = SerialPort.GetPortNames()[0];
        portInputField.text = port;

        if (lidarModel == LidarModel.YDLidar_X4)
        {
            // YDLidar X4 initialization
            ydlidar.os_init();
            ydLidarClass = new CYdLidar();

            int optname = (int)LidarProperty.LidarPropSerialPort;
            ydLidarClass.setlidaropt(optname, port);

            optname = (int)LidarProperty.LidarPropSerialBaudrate;
            ydLidarClass.setlidaropt(optname, 128000);

            optname = (int)LidarProperty.LidarPropLidarType;

            int lidarType = (int)LidarTypeID.TYPE_TRIANGLE;
            ydLidarClass.setlidaropt(optname, lidarType);

            do
            {
                ret = ydLidarClass.initialize();
                if (ret)
                {
                    ret = ydLidarClass.turnOn();
                }
                else
                {
                    //Debug.Log("error:" + lidarClass.DescribeError());

                    if (!showPortPanel)
                    {
                        showPortPanel = true;
                        portPanel.SetActive(showPortPanel);
                    }
                }

                yield return null;
            }
            while (!ret);

            scan = new LaserScan();
            scan.scanFreq = 4f;
            scan.sampleRate = 4f;
        }
        else if(lidarModel == LidarModel.RPLidar_S2)
        {
            rpLidarClass = new CRpLidar();

            rpLidarClass.PortName = port;

            rpLidarClass.BaudRate = 1000000;

            rpLidarClass.MotorSpeed = motorSpeed;

            rpLidarClass.IsFlipped = false;

            speedSlider.SetValueWithoutNotify(motorSpeed);
            speedText.text = ((motorSpeed / 1023f) * 100f).ToString("F0") + "%";
            //Debug.Log(motorSpeed);
            //Debug.Log(((motorSpeed / 1023f) * 100f).ToString("F0") + "%");

            /*do
            {
                ret = rpLidarClass.Initialize();
                if(!ret)
                {
                    //Debug.Log("error:" + lidarClass.DescribeError());

                    if (!showPortPanel)
                    {
                        showPortPanel = true;
                        portPanel.SetActive(showPortPanel);
                    }

                    rpLidarClass.Reset();
                }

                yield return null;
            }
            while (!ret);*/

            do
            {
                ret = rpLidarClass.UnityStart();
                if (!ret)
                {
                    //Debug.Log("error:" + lidarClass.DescribeError());

                    if (!showPortPanel)
                    {
                        showPortPanel = true;
                        portPanel.SetActive(showPortPanel);
                    }

                    rpLidarClass.Reset();
                }

                yield return null;
            }
            while (!ret);

            //scanFlag = false;
        }

        portPanel.SetActive(false);

        // Start scan
        StartCoroutine(_Scan());
    }

    // Lidar Scan
    private IEnumerator _Scan()
    {
        float rot = Mathf.Deg2Rad * transform.Find("Points").eulerAngles.z;

        while (scanFlag)
        {
            if (ret && (lidarModel == LidarModel.YDLidar_X4 ? ydlidar.os_isOk() : true))
            {
                // Await lidar scan
                List<Vector2> touchPoints = new List<Vector2>();
                List<RPLidar.Measurement> rplidarpoints = new List<RPLidar.Measurement>();

                if (lidarModel == LidarModel.YDLidar_X4)
                {
                    //var t = Task.Run(async () => lidarClass.doProcessSimple(scan));
                    var t = Task.Run(async () => await Scan());
                    yield return new WaitUntil(() => t.IsCompleted);
                }
                else if (lidarModel == LidarModel.RPLidar_S2)
                {
                    rplidarpoints = new List<RPLidar.Measurement>(rpLidarClass.scanPoints);
                    yield return null;
                }

                lidarTP.Clear();

                Vector2 sp = Vector2.zero;
                int pointsCount = 0;

                if (lidarModel == LidarModel.YDLidar_X4)
                {
                    pointsCount = scan.points.Count;
                }
                else if (lidarModel == LidarModel.RPLidar_S2)
                {
                    pointsCount = rplidarpoints.Count;
                }

                if (points.Count > pointsCount)
                {
                    ///
                }

                
                // Iterate scan points
                for (int i = 0; i < pointsCount; i++)
                {
                    float x = 0f, y = 0f;

                    if (lidarModel == LidarModel.YDLidar_X4)
                    {
                        sp = new Vector2(scan.points[i].angle, scan.points[i].range);

                        x = sp.y * (float)Mathf.Cos(sp.x + rot);
                        y = sp.y * (float)Mathf.Sin(sp.x + rot);
                    }
                    else if(lidarModel == LidarModel.RPLidar_S2)
                    {
                        sp = new Vector2(rplidarpoints[i].Angle, rplidarpoints[i].Distance);

                        x = sp.y * (float)Mathf.Cos(Mathf.PI / 180.0f * sp.x);
                        y = sp.y * (float)Mathf.Sin(Mathf.PI / 180.0f * sp.x);
                    }

                    // range,angle -> x,y 
                    //float x = scan.points[i].range * Mathf.Cos(scan.points[i].angle + rot);
                    //float y = scan.points[i].range * Mathf.Sin(scan.points[i].angle + rot);

                    if (setup && showScanedPoints)
                    {
                        // Update points
                        if (scanPoints.Count > i)
                        {
                            //scanPoints[i] = new Vector2(scan.points[i].angle, scan.points[i].range);
                            scanPoints[i] = new Vector2(sp.x, sp.y);
                        }
                        else
                        {
                            //scanPoints.Add(new Vector2(scan.points[i].angle, scan.points[i].range));
                            scanPoints.Add(new Vector2(sp.x, sp.y));
                        }

                        // Update border
                        if (borderFlag && Time.timeSinceLevelLoad > 3f)
                        {
                            if (borderRanges.Count > i)
                            {
                                //if (borderRanges[i] < scan.points[i].range)
                                if (borderRanges[i] < sp.y)
                                {
                                    borderRanges[i] = sp.y;
                                }
                            }
                            else
                            {
                                borderRanges.Add(sp.y);
                            }
                        }

                        //UnityEngine.Debug.Log("angle: " + (scan.points[i].angle));
                        //Debug.Log("intensity: " + (scan.points[i].intensity));
                        //Debug.Log("angle: " + (scan.points[i].angle) + " range: " + (scan.points[i].range));


                        // Update points positions
                        if (i < points.Count)
                        {
                            if (Mathf.Abs(x - points[i].localPosition.x) > 0.05f || Mathf.Abs(y - points[i].localPosition.y) > 0.05f)
                            {
                                points[i].position = new Vector3(x, y, 0f);
                            }
                        }
                        else
                        {
                            GameObject go = Instantiate(pointPrefab, transform.Find("Points"));
                            go.transform.position = new Vector3(x, y, 0f);
                            points.Add(go.transform);
                        }

                        // Ignore points with a range less than 12 cm 
                        //if (scan.points[i].range > 0.12f)
                        if (sp.y > 0.12f)
                        {
                            if (!points[i].gameObject.activeSelf)
                            {
                                points[i].gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            points[i].gameObject.SetActive(false);
                        }
                    }

                    //if (scan.points[i].range > 0.12f)
                    if (sp.y > 0.12f)
                    {
                        // Check if any points is inside of interactive area
                        if (calibrated && !calibrating)
                        {
                            if (PointInArea(x, y))
                            {
                                touchPoints.Add(new Vector2(x, y));
                            }
                        }
                    }
                }

                for (int i = pointsCount; i < points.Count; i++)
                {
                    points[i].gameObject.SetActive(false);
                }

                // Iterate points inside of interactive area
                int tp = 0;
                if (touchPoints.Count > 0)
                {
                    List<List<Vector2>> clusters = DBSCAN(touchPoints, epsilon, 1);

                    // Group touch points
                    for (; tp < clusters.Count; tp++)
                    {
                        // Calculate centroid
                        Vector2 centroid = CalculateCentroid(clusters[tp]);
                        lidarP.Add(centroid);

                        // Transform centroid position to screen position
                        Vector2 inputPosition = centroid;

                        Vector2 outputPosition = RectangleMapPoint(inputPosition);
                        //Vector2 outputPosition = LidarPointToScreen(inputPosition);

                        lidarTP.Add(new Vector3(outputPosition.x, outputPosition.y, 0f));

                        if (setup)
                        {
                            // Update cursors
                            if (cursors.Count <= tp)
                            {
                                GameObject go = Instantiate(cursorPrefab, transform.Find("Cursors"));
                                go.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
                                cursors.Add(go);
                            }

                            cursors[tp].SetActive(true);
                            cursors[tp].transform.position = new Vector3(centroid.x, centroid.y, transform.position.z);
                        }

                        if (showCursors)
                        {
                            // Update screen cursors
                            if (screenCursors.Count <= tp)
                            {
                                GameObject go = Instantiate(screenCursorPrefab, transform.Find("ScreenCursors"));
                                go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                                screenCursors.Add(go);
                            }

                            screenCursors[tp].SetActive(true);
                            Camera camera = Camera.main;
                            if (setup)
                            {
                                camera = lidarCamera;
                                screenCursors[tp].transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            }
                            else
                            {
                                screenCursors[tp].transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                            }
                            screenCursors[tp].transform.position = camera.ScreenPointToRay(new Vector3(outputPosition.x, outputPosition.y, 0f)).GetPoint(camera.nearClipPlane);
                            screenCursors[tp].transform.rotation = Quaternion.Euler(0f, camera.transform.position.z > 0f ? 180f : 0f, 0f);
                        }
                    }
                }

                if (setup || showCursors)
                {
                    for (; tp < Mathf.Min(setup ? cursors.Count : screenCursors.Count, lastTPCount); tp++)
                    {
                        if (setup) cursors[tp].SetActive(false);
                        if (showCursors) screenCursors[tp].SetActive(false);
                    }
                }

                lastTPCount = lidarTP.Count;
               
                lidarTouchPoints.Clear();
                Dictionary<int, Vector3> newPositionToId = new Dictionary<int, Vector3>();
                foreach (Vector3 position in lidarTP)
                {
                    int id = FindClosestPositionId(position);
                    newPositionToId[id] = position;
                }

                idToPosition = newPositionToId;

                idToPosition.OrderByDescending(x => x.Key);

                //int tpc = 0;
                foreach (KeyValuePair<int, Vector3> entry in idToPosition)
                {
                    //tpc++;
                    lidarTouchPoints.Add(new LidarTouchPoint(entry.Key, entry.Value));

                    /*if (showCursors)
                    {
                        // Update screen cursors
                        if (screenCursors.Count <= entry.Key)
                        {
                            GameObject go = Instantiate(screenCursorPrefab, transform.Find("ScreenCursors"));
                            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                            screenCursors.Add(go);
                        }

                        Debug.Log(entry.Key);
                        screenCursors[entry.Key].SetActive(true);
                        Camera camera = Camera.main;
                        if (setup)
                        {
                            camera = lidarCamera;
                        }
                        screenCursors[entry.Key].transform.position = camera.ScreenPointToRay(new Vector3(entry.Value.x, entry.Value.y, 0f)).GetPoint(camera.nearClipPlane);
                    }*/
                }
            }
        }
    }

    // Is there any point inside the area?
    private bool PointInArea(float x, float y)
    {
        bool inside = false;
        int j = vertices.Length - 1;

        for (int i = 0; i < vertices.Length; j = i++)
        {
            if (((vertices[i].y <= y && y < vertices[j].y) || (vertices[j].y <= y && y < vertices[i].y)) &&
                (x < (vertices[j].x - vertices[i].x) * (y - vertices[i].y) / (vertices[j].y - vertices[i].y) + vertices[i].x))
            {
                inside = !inside;
            }
        }

        return inside;
    }

    // Rotation Slider callback
    public void OnRotateSliderChangeValue(float value)
    {
        float angle = Mathf.Lerp(0f, 360f, value);
        lidarCamera.transform.rotation = Quaternion.Euler(0f, lidarCamera.transform.eulerAngles.y, angle);
        angleText.text = angle.ToString("F0") + "�";

        if (corners.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                TextMeshProUGUI[] texts = corners[i].GetComponentsInChildren<TextMeshProUGUI>(true);

                for (int j = 0; j < texts.Length; j++)
                {
                    texts[j].transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);
                }
            }
        }
    }

    // Slider callback
    public void OnSpeedSliderChangeValue(float value)
    {
        motorSpeed = (ushort)value;
        rpLidarClass.SetMotorSpeed((ushort)value);
        speedText.text = ((value/1023f) * 100f).ToString("F0") + "%";
    }

    // Mirror button callback
    public void OnMirrorClick()
    {
        Vector3 mirrorPos = lidarCamera.transform.position;
        mirrorPos.z = -mirrorPos.z;
        lidarCamera.transform.position = mirrorPos;
        lidarCamera.transform.Rotate(Vector3.up, 180f);

        for (int i = 0; i < 4; i++)
        {
            TextMeshProUGUI[] texts = corners[i].GetComponentsInChildren<TextMeshProUGUI>(true);

            for (int j = 0; j < texts.Length; j++)
            {
                texts[j].transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);
            }
        }
    }

    // Start button callback
    public void OnStartClick()
    {
        if (calibrating && calibrateCoroutine != null)
        {
            StopCoroutine(calibrateCoroutine);
            calibrateCoroutine = null;
            buttonText.text = "Calibrate";
            OnResetToJSONClick();

            areaLine.loop = true;
            cursors[0].SetActive(false);
            calibText.text = "";
            calibrated = true;
            calibrating = false;

            for(int i = 0; i < corners.Count; i++)
            {
                corners[i].transform.localScale = Vector3.one * cornerPrefab.transform.localScale.x;
            }
        }
        else
        {
            calibrateCoroutine = StartCoroutine(Calibrate());
            buttonText.text = "Stop";
        }
    }

    // Save button callback
    public void OnSaveClick()
    {
        JSONSave();
    }

    private void PlayerPrefsSave()
    {
        if (calibrated /*&& saveButton.interactable*/)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                PlayerPrefs.SetFloat(keys[i] + "_X", vertices[i].x);
                PlayerPrefs.SetFloat(keys[i] + "_Y", vertices[i].y);
            }
        }
        //saveButton.interactable = false;

        PlayerPrefs.SetFloat("LidarCamera_X_Pos", lidarCamera.transform.position.x);
        PlayerPrefs.SetFloat("LidarCamera_Y_Pos", lidarCamera.transform.position.y);
        PlayerPrefs.SetFloat("LidarCamera_Z_Pos", lidarCamera.transform.position.z);
        PlayerPrefs.SetFloat("LidarCamera_Mirror_Rot", lidarCamera.transform.eulerAngles.y);
        PlayerPrefs.SetFloat("LidarCamera_OrthoSize", lidarCamera.orthographicSize);
        PlayerPrefs.SetFloat("LidarCamera_Slider", rotatonSlider.value);
    }

    private void JSONSave()
    {
        LidarSettings settings = new LidarSettings();
        settings.BLC = vertices[0];
        settings.BRC = vertices[1];
        settings.TRC = vertices[2];
        settings.TLC = vertices[3];
        settings.Camera_Pos = lidarCamera.transform.position;
        settings.Camera_Slider = rotatonSlider.value;
        settings.Camera_OrthoSize = lidarCamera.orthographicSize;
        settings.Camera_Mirror_Rot = lidarCamera.transform.eulerAngles.y;
        settings.MotorSpeed = motorSpeed;

        string json = JsonUtility.ToJson(settings);

        try
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            System.IO.File.WriteAllText(filePath + fileName, json);
        }
        catch (System.Exception ex)
        {
            string ErrorMessages = "File Write Error\n" + ex.Message;
            Debug.LogError(ErrorMessages);
        }

        // "LBC", //BRC
        // "RBC", //BLC
        // "RUC", //TRC
        // "LUC"  //TLC
    }

    // Delete button callback
    public void OnDeleteClick()
    {
        DeleteJSON();
    }

    private void DeletePlayerPrefs()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            vertices[i] = Vector3.zero;
            normals[i] = Vector2.zero;
            PlayerPrefs.DeleteKey(keys[i] + "_X");
            PlayerPrefs.DeleteKey(keys[i] + "_Y");
            corners[i].transform.position = vertices[i];
        }

        areaLine.SetPosition(0, vertices[0]);
        areaLine.SetPosition(1, vertices[1]);
        areaLine.SetPosition(2, vertices[2]);
        areaLine.SetPosition(3, vertices[3]);

        PlayerPrefs.DeleteKey("LidarCamera_Z_Pos");
        PlayerPrefs.DeleteKey("LidarCamera_X_Pos");
        PlayerPrefs.DeleteKey("LidarCamera_Y_Pos");
        PlayerPrefs.DeleteKey("LidarCamera_Mirror_Rot");
        PlayerPrefs.DeleteKey("LidarCamera_OrthoSize");
        PlayerPrefs.DeleteKey("LidarCamera_Slider");
    }

    private void DeleteJSON()
    {
        if (lidarSettings != null)
        {
            lidarCamera.transform.position = new Vector3(0f, 0f, 18f);
            lidarCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            rotatonSlider.value = 0f;
            lidarCamera.orthographicSize = 10f;
            lidarModelParent.transform.localScale = Vector3.one * Mathf.Max(lidarCamera.orthographicSize / 10f, 0.1f);

            motorSpeed = 600;
            rpLidarClass.SetMotorSpeed(motorSpeed);

            for (int i = 0; i < keys.Length; i++)
            {
                vertices[i] = Vector3.zero;
                normals[i] = Vector2.zero;
                corners[i].transform.position = vertices[i];
                areaLine.SetPosition(i, vertices[i]);
            }
        }

        /*areaLine.SetPosition(0, vertices[0]);
        areaLine.SetPosition(1, vertices[1]);
        areaLine.SetPosition(2, vertices[2]);
        areaLine.SetPosition(3, vertices[3]);*/

        try
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            System.IO.File.Delete(filePath + fileName);
        }
        catch (System.Exception ex)
        {
            string ErrorMessages = "File Write Error\n" + ex.Message;
            Debug.LogError(ErrorMessages);
        }
    }

    // Reset button callback
    public void OnResetClick()
    {   
        lidarCamera.transform.position = new Vector3(0f, 0f, 18f);
        lidarCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        rotatonSlider.value = 0f;
        lidarCamera.orthographicSize = 10.5f;
        lidarModelParent.transform.localScale = Vector3.one * Mathf.Max(lidarCamera.orthographicSize / 10f, 0.1f);
        motorSpeed = 600;
        rpLidarClass.SetMotorSpeed(motorSpeed);

        for (int i = 0; i < 4; i++)
        {
            vertices[i] = Vector3.zero;
            normals[i] = Vector2.zero;
            areaLine.SetPosition(i, vertices[i]);
            corners[i].transform.position = vertices[i];
        }
    }

    public void OnResetToJSONClick()
    {
        if (File.Exists(filePath + fileName))
        {
            var sr = File.OpenText(filePath + fileName);
            var json = sr.ReadLine();
            lidarSettings = JsonUtility.FromJson<LidarSettings>(json);
        }
        else
        {
            Debug.Log("Could not Open the file: " + filePath + fileName + " for reading.");
            return;
        }

        if (lidarSettings != null)
        {
            lidarCamera.transform.position = lidarSettings.Camera_Pos;
            lidarCamera.transform.rotation = Quaternion.Euler(0f, lidarSettings.Camera_Mirror_Rot, 0f);
            lidarCamera.orthographicSize = lidarSettings.Camera_OrthoSize;

            rotatonSlider.SetValueWithoutNotify(lidarSettings.Camera_Slider);
            float angle = Mathf.Lerp(0f, 360f, lidarSettings.Camera_Slider);
            lidarCamera.transform.rotation = Quaternion.Euler(0f, lidarCamera.transform.eulerAngles.y, angle);
            angleText.text = angle.ToString("F0") + "�";

            motorSpeed = lidarSettings.MotorSpeed;
            rpLidarClass.SetMotorSpeed(motorSpeed);
            speedSlider.SetValueWithoutNotify(motorSpeed);
            speedText.text = ((motorSpeed / 1023f) * 100f).ToString("F0") + "%";

            lidarModelParent.transform.localScale = Vector3.one * Mathf.Max(lidarCamera.orthographicSize / 10f, 0.1f);

            vertices[0] = new Vector3(lidarSettings.BLC.x, lidarSettings.BLC.y, 0f);
            vertices[1] = new Vector3(lidarSettings.BRC.x, lidarSettings.BRC.y, 0f);
            vertices[2] = new Vector3(lidarSettings.TRC.x, lidarSettings.TRC.y, 0f);
            vertices[3] = new Vector3(lidarSettings.TLC.x, lidarSettings.TLC.y, 0f);

            //vertices = SortVertices(Vector3.forward, vertices.ToList());

            for (int i = 0; i < 4; i++)
            {
                areaLine.SetPosition(i, vertices[i]);

                corners[i].transform.position = vertices[i];
                corners[i].GetComponentInChildren<TextMeshProUGUI>(true).transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);

                Vector2 dir = vertices[i < normals.Length - 1 ? i + 1 : 0] - vertices[i];
                Vector2 normal = new Vector2(-dir.y, dir.x);
                normal.Normalize();
                normals[i < normals.Length - 1 ? i + 1 : 0] = normal;   
            }

            for (int i = 0; i < 4; i++)
            {
                GameObject lengthTxt = corners[i].GetComponentsInChildren<TextMeshProUGUI>(true)[1].gameObject;
                lengthTxt.transform.rotation = Quaternion.LookRotation(lidarCamera.transform.forward, lidarCamera.transform.up);

                float distance = Vector3.Distance(vertices[i], vertices[i < 4 - 1 ? i + 1 : 0]);
                lengthTxt.GetComponentInChildren<TextMeshProUGUI>().text = distance.ToString("F2") + " m";
                lengthTxt.transform.position = vertices[i] + ((vertices[i < 4 - 1 ? i + 1 : 0] - vertices[i]).normalized * distance / 2f);
            }
        }
    }

    // Scan async method
    private async Task Scan()
    {
        await Task.Run(() =>
        {
            ydLidarClass.doProcessSimple(scan);
        });
    }

    // On disable script, turn off Lidar
    private void OnDisable()
    {
        if (lidarModel == LidarModel.YDLidar_X4)
        {
            if (ret)
            {
                ydLidarClass.turnOff();
                ydLidarClass.disconnecting();
                ydLidarClass.Dispose();
            }
        }
        else if(lidarModel == LidarModel.RPLidar_S2)
        {
            rpLidarClass.StopLidarTask();
        }

        scanFlag = false;
    }

#if UNITY_EDITOR
    // Draw interactive area
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(vertices[0], 0.01f);
        Gizmos.DrawSphere(vertices[1], 0.01f);
        Gizmos.DrawSphere(vertices[2], 0.01f);
        Gizmos.DrawSphere(vertices[3], 0.01f);

        Gizmos.DrawLine(vertices[0], vertices[1]);
        Gizmos.DrawLine(vertices[1], vertices[2]);
        Gizmos.DrawLine(vertices[2], vertices[3]);
        Gizmos.DrawLine(vertices[3], vertices[0]);

        //Gizmos.color = Color.blue;
        //if(cursors.Count > 0) Gizmos.DrawSphere(new Vector3(cursors[0].transform.position.x, cursors[0].transform.position.y, 0f), 0.01f);
    }
#endif

    // Gets the centroid
    private Vector2 CalculateCentroid(List<Vector2> points)
    {
        Vector2 centroid = Vector2.zero;

        foreach (Vector2 point in points)
        {
            centroid += point;
        }

        centroid /= points.Count;

        return centroid;
    }

    // Transforms the point inside the interactive area to screen coordinates
    public Vector2 RectangleMapPoint(Vector2 inputPosition)
    {
        float minX, maxX, minY, maxY;

        if(lidarCamera.transform.position.z > 0)
        {
            minX = Mathf.Min(vertices[0].x, vertices[3].x);
            maxX = Mathf.Max(vertices[1].x, vertices[2].x);
            minY = Mathf.Min(vertices[0].y, vertices[1].y);
            maxY = Mathf.Max(vertices[2].y, vertices[3].y);
        }
        else
        {
            minX = Mathf.Max(vertices[0].x, vertices[3].x);
            maxX = Mathf.Min(vertices[1].x, vertices[2].x);
            minY = Mathf.Max(vertices[0].y, vertices[1].y);
            maxY = Mathf.Min(vertices[2].y, vertices[3].y);
        }

        /*if(Mathf.DeltaAngle(rotatonSlider.value, 180f) > 25f )
        {
            minY = Mathf.Min(vertices[0].y, vertices[1].y);
            maxY = Mathf.Max(vertices[2].y, vertices[3].y);
        }
        else 
        {
            minY = Mathf.Max(vertices[0].y, vertices[1].y);
            maxY = Mathf.Min(vertices[2].y, vertices[3].y);
        }*/

        float u = Mathf.InverseLerp(minX, maxX, inputPosition.x);
        float v = Mathf.InverseLerp(minY, maxY, inputPosition.y);

        /*if(Mathf.DeltaAngle(lidarSettings != null ? lidarSettings.Camera_Slider : 0f, 180f) > 22.5f)
        {
            u = 1f - u;
        }

        if(lidarSettings != null ? lidarSettings.Camera_Pos.z < 0f : false)
        {
            v = 1f - v;
        }*/

        Vector2 outputPosition = new Vector2(Mathf.Lerp(0f, Screen.width, u), Mathf.Lerp(0f, Screen.height, v));

        return outputPosition;
    }

    public Vector2 InverseRectangleMapPoint(Vector2 inputPosition)
    {
        float minX, maxX, minY, maxY;

        if (lidarCamera.transform.position.z > 0)
        {
            minX = Mathf.Min(vertices[0].x, vertices[3].x);
            maxX = Mathf.Max(vertices[1].x, vertices[2].x);
            minY = Mathf.Min(vertices[0].y, vertices[1].y);
            maxY = Mathf.Max(vertices[2].y, vertices[3].y);
        }
        else
        {
            minX = Mathf.Max(vertices[0].x, vertices[3].x);
            maxX = Mathf.Min(vertices[1].x, vertices[2].x);
            minY = Mathf.Max(vertices[0].y, vertices[1].y);
            maxY = Mathf.Min(vertices[2].y, vertices[3].y);
        }

        Vector2 outputPosition = new Vector2(Mathf.Lerp(minX, maxX, inputPosition.x / Screen.width), Mathf.Lerp(minY, maxY, inputPosition.y));

        return outputPosition;
    }

    // Transforms the point inside the interactive area to screen coordinates
    private Vector2 LidarPointToScreen(Vector2 inputPosition)
    {
        float u, v;
        Vector3 point = new Vector3(inputPosition.x, inputPosition.y, 0f);

        u = (((point - vertices[0]) * normals[0]) / ((point - vertices[0]) * normals[0] + ((point - vertices[2]) * normals[2]))).x;
        v = (((point - vertices[0]) * normals[1]) / ((point - vertices[0]) * normals[1] + ((point - vertices[3]) * normals[3]))).y;

        //Debug.Log("Input: " + inputPosition + " Output: u " + u + " v " + v + " Screen: " + Screen.width + " " + Screen.height);

        if(Mathf.DeltaAngle(lidarSettings.Camera_Slider, 180f) > 22.5f)
        {
            u = 1f - u;
        }

        if(lidarSettings.Camera_Pos.z < 0f)
        {
            v = 1f - v;
        }

        return new Vector2(u * Screen.width, v * Screen.height);
    }

    // DBSCAN - Data clustering algorithm ------------------------------------
    private List<List<Vector2>> DBSCAN(List<Vector2> points, float epsilon, int minPoints)
    {
        List<List<Vector2>> clusters = new List<List<Vector2>>();
        List<bool> visited = new List<bool>(new bool[points.Count]);

        for (int i = 0; i < points.Count; i++)
        {
            if (!visited[i])
            {
                visited[i] = true;
                List<Vector2> neighbors = GetNeighbors(points, points[i], epsilon);

                if (neighbors.Count < minPoints)
                {
                    continue;
                }

                List<Vector2> cluster = new List<Vector2>();
                cluster.Add(points[i]);

                ExpandCluster(points, visited, neighbors, cluster, epsilon, minPoints);

                clusters.Add(cluster);
            }
        }

        return clusters;
    }

    private void ExpandCluster(List<Vector2> points, List<bool> visited, List<Vector2> neighbors, List<Vector2> cluster, float epsilon, int minPoints)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            int index = points.IndexOf(neighbors[i]);

            if (!visited[index])
            {
                visited[index] = true;
                List<Vector2> newNeighbors = GetNeighbors(points, neighbors[i], epsilon);

                if (newNeighbors.Count >= minPoints)
                {
                    neighbors.AddRange(newNeighbors);
                }
            }

            if (!cluster.Contains(neighbors[i]))
            {
                cluster.Add(neighbors[i]);
            }
        }
    }

    private List<Vector2> GetNeighbors(List<Vector2> points, Vector2 point, float epsilon)
    {
        List<Vector2> neighbors = new List<Vector2>();

        foreach (Vector2 p in points)
        {
            if (Vector2.Distance(point, p) <= epsilon)
            {
                neighbors.Add(p);
            }
        }

        return neighbors;
    }
    // DBSCAN - Data clustering algorithm ------------------------------------

    private int FindClosestPositionId(Vector3 currentPosition)
    {
        int closestId = -1;
        float closestDistance = float.MaxValue;

        foreach (KeyValuePair<int, Vector3> entry in idToPosition)
        {
            Vector3 storedPosition = entry.Value;
            float distance = Vector3.Distance(currentPosition, storedPosition);
            if (distance < minDistanceThreshold && distance < closestDistance)
            {
                closestId = entry.Key;
                closestDistance = distance;
            }
        }

        if (closestId == -1)
        {
            closestId = nextId;
            nextId++;
        }

        return closestId;
    }
    
    public LidarTouchPoint GetLidarTouchPoint(int id)
    {
        return lidarTouchPoints[id];
    }
}

// Settings class -> JSON
[System.Serializable]
public class LidarSettings
{
    public Vector2 BLC;
    public Vector2 BRC;
    public Vector2 TRC;
    public Vector2 TLC;
    public Vector3 Camera_Pos;
    public float Camera_Slider;
    public float Camera_OrthoSize;
    public float Camera_Mirror_Rot;
    public ushort MotorSpeed;
}