using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    public GameObject cubo_generador; // Prefab del cubo a generar
    private List<GameObject> cubos = new List<GameObject>(); // Lista para almacenar los cubos
    private HashSet<char> letrasAsignadas = new HashSet<char>(); // Conjunto para rastrear las letras ya asignadas

    void Start()
    {
        // Si la lista de cubos no está vacía
        if (PlayerPrefs.HasKey("CubosCount"))
        {
            int cubosCount = PlayerPrefs.GetInt("CubosCount"); // Obtener la cantidad de cubos
            for (int i = 0; i < cubosCount; i++)
            {
                // Crear un cubo en la posición guardada
                GameObject nuevoCubo = Instantiate(cubo_generador, new Vector3(
                    PlayerPrefs.GetFloat("Cubo" + i + "X"),
                    PlayerPrefs.GetFloat("Cubo" + i + "Y"),
                    PlayerPrefs.GetFloat("Cubo" + i + "Z")
                ), Quaternion.identity);

                cubos.Add(nuevoCubo); // Agregar el cubo a la lista

                // Asignar la letra guardada al cubo
                SimulateKeyPressUnity script = nuevoCubo.GetComponent<SimulateKeyPressUnity>();
                if (script != null)
                {
                    string letraGuardada = PlayerPrefs.GetString("Cubo" + i + "Letra");
                    script.Tecla_activacion = letraGuardada; // Asignar la letra al script
                    letrasAsignadas.Add(letraGuardada[0]); // Marcar la letra como asignada
                }

                // Asignamos la escala guardada al cubo
                nuevoCubo.transform.localScale = new Vector3(
                    PlayerPrefs.GetFloat("Cubo" + i + "ScaleX"),
                    PlayerPrefs.GetFloat("Cubo" + i + "ScaleY"),
                    PlayerPrefs.GetFloat("Cubo" + i + "ScaleZ")
                );
            }
        }

        //Quitamos la pantalla completa
        Screen.fullScreen = false;
    }

    // Update es llamado una vez por frame
    void Update()
    {
        // Crear un cubo con la tecla 'Q' y borrarlo con 'B'
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InstanciarCubo();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            DestruirCubo();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GuardarCubos();
        }
    }

    // Método para instanciar un cubo en el origen
    public void InstanciarCubo()
    {
        GameObject nuevoCubo = Instantiate(cubo_generador, new Vector3(0, 0, 0), Quaternion.identity);
        cubos.Add(nuevoCubo); // Agregar el cubo a la lista
        AsignarLetra(nuevoCubo); // Asignar una letra al cubo
    }

    // Método para destruir el último cubo instanciado
    public void DestruirCubo()
    {
        if (cubos.Count > 0) // Asegurarse de que haya cubos para destruir
        {
            GameObject ultimoCubo = cubos[cubos.Count - 1]; // Obtener el último cubo
            cubos.RemoveAt(cubos.Count - 1); // Eliminarlo de la lista
            Destroy(ultimoCubo); // Destruir el objeto
        }
    }

    // Método para asignar una letra única a un cubo
    public void AsignarLetra(GameObject cubo)
    {
        char letra;
        do
        {
            letra = (char)Random.Range(65, 91); // Generar una letra aleatoria (ASCII A-Z)
        } while (letrasAsignadas.Contains(letra)); // Repetir si la letra ya fue asignada

        letrasAsignadas.Add(letra); // Registrar la letra como asignada

        // Asegurarse de que el cubo tiene el script y asignar la letra
        SimulateKeyPressUnity script = cubo.GetComponent<SimulateKeyPressUnity>();
        if (script != null)
        {
            script.Tecla_activacion = letra.ToString(); // Convertir el char a string
        }
        else
        {
            Debug.LogWarning("El cubo no tiene el script 'SimulateKeyPressUnity'.");
        }
    }

    // Método para guardar toda la lista de los cubos para usarla aunque reinicie el juego
    public void GuardarCubos()
    {
        PlayerPrefs.SetInt("CubosCount", cubos.Count); // Guardar la cantidad de cubos
        for (int i = 0; i < cubos.Count; i++)
        {
            PlayerPrefs.SetFloat("Cubo" + i + "X", cubos[i].transform.position.x); // Guardar la posición X
            PlayerPrefs.SetFloat("Cubo" + i + "Y", cubos[i].transform.position.y); // Guardar la posición Y
            PlayerPrefs.SetFloat("Cubo" + i + "Z", cubos[i].transform.position.z); // Guardar la posición Z

            // Guardar la letra asignada al cubo
            SimulateKeyPressUnity script = cubos[i].GetComponent<SimulateKeyPressUnity>();
            if (script != null)
            {
                PlayerPrefs.SetString("Cubo" + i + "Letra", script.Tecla_activacion); // Guardar la letra
            }

            // Guardamos la escala del cubo
            PlayerPrefs.SetFloat("Cubo" + i + "ScaleX", cubos[i].transform.localScale.x);
            PlayerPrefs.SetFloat("Cubo" + i + "ScaleY", cubos[i].transform.localScale.y);
            PlayerPrefs.SetFloat("Cubo" + i + "ScaleZ", cubos[i].transform.localScale.z);
        }

        PlayerPrefs.Save(); // Guardar los cambios en PlayerPrefs
    }

    
    //Metodo donde si se oprime el boton central del mouse se puede mover la camara segun arrastres el mouse
    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * 5;
        float rotY = Input.GetAxis("Mouse Y") * 5;
        Camera.main.transform.Rotate(Vector3.up, -rotX, Space.World);
        Camera.main.transform.Rotate(Vector3.right, rotY, Space.World);
    }
}
