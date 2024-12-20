using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SimulateKeyPressUnity : MonoBehaviour
{
    bool isPressed = false;

    //public string windowName = "Resolume Arena - Example (1280 x 720)";
    public string Tecla_activacion = "w";

    // bit de la tecla de activación
    byte virtualKeyCode_f; // Tecla 'w'

    // Importa funciones de la API de Windows
    [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("USER32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    // Constantes para la simulación de teclas
    private const uint KEYEVENTF_KEYDOWN = 0x0000;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    // Método para enviar una tecla
    public void SimulateKey(byte virtualKeyCode)
    {
        keybd_event(virtualKeyCode, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
        keybd_event(virtualKeyCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    // Método para simular interacción con la calculadora
    public void InteractWithCalculator()
    {
        // Encuentra la ventana de la calculadora
        IntPtr calculatorHandle = FindWindow(null, "Resolume Arena - Example (1280 x 720)");

        if (calculatorHandle == IntPtr.Zero)
        {
            Debug.LogError("La Calculadora no está ejecutándose.");
            return;
        }

        // Trae la calculadora al frente
        SetForegroundWindow(calculatorHandle);

        // Simula la pulsación de teclas (Ejemplo: 1 * 11 =)
        //SimulateKey(0x31); // Tecla '1'
        //SimulateKey(0x31); // Tecla '1'
        //SimulateKey(0x31); // Tecla '1'
        //SimulateKey(0x6A); // Tecla '*'
        //SimulateKey(0x31); // Tecla '1'
        //SimulateKey(0x31); // Tecla '1'
        //SimulateKey(0x0D); // Tecla 'Enter'

        // Simulamos la letra 'w'
        SimulateKey(virtualKeyCode_f); // Tecla 'w'
    }

    private void Start()
    {
        // Obtenemos los bytes de la tecla de activación
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Tecla_activacion);
        byte virtualKeyCode = bytes[0];

        virtualKeyCode_f = virtualKeyCode;

        // Imprimimos el valor de la tecla de activación
        Debug.Log("Tecla de activación: " + virtualKeyCode);
    }

    // Ejemplo de uso al presionar una tecla
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Cambia Space por cualquier tecla deseada
        {
            InteractWithCalculator();
        }
    }

    void Sin_presionar()
    {
        isPressed = false;
    }

    // Con el isTrigger activado, el método se ejecutará una sola vez
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("boton") && !isPressed)
        {
            InteractWithCalculator();
            isPressed = true;

            // Despues de 5 segundos, se podrá volver a presionar el botón
            Invoke("Sin_presionar", 5f);
        }
    }
}
