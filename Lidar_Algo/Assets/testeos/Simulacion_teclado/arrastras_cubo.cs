using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrastraCubo : MonoBehaviour
{
    public Camera camaraPrincipal;
    public float factorEscala = 1f; // Controla la velocidad de escalado
    public float escalaMinima = 0.1f; // Límite inferior para la escala
    public float escalaMaxima = 5f;  // Límite superior para la escala

    public TextMesh texto_posicion;

    private float posicionFijaZ; // Para mantener la posición fija en Z
    private Vector3 offset;      // Diferencia entre el mouse y la posición del objeto

    void Start()
    {
        // Guardamos la posición inicial en Z para usarla al mover el cubo
        posicionFijaZ = transform.position.z;
    }

    void Update()
    {
        // Escribimos la posición actual del objeto en el texto solo los ejes X y Y y con dos decimales
        texto_posicion.text = "[ " + transform.position.x.ToString("F2") + ", " + transform.position.y.ToString("F2") + " ], E:" + transform.localScale.x.ToString("F2");

        // si no se esta presionando el boton izquierdo del mouse
        if (!Input.GetMouseButton(0))
        {
            //Activamos el cursor del mouse
            Cursor.visible = true;
        }
    }

    // Método que se llama cuando se inicia el arrastre
    void OnMouseDown()
    {
        //Desactivamos el cursor del mouse
        Cursor.visible = false;
        // Calculamos el offset entre la posición del objeto y la posición del mouse
        Vector3 posicionMouse = Input.mousePosition;
        posicionMouse.z = camaraPrincipal.WorldToScreenPoint(transform.position).z; // Mantener la profundidad actual
        Vector3 posicionMouseEnMundo = camaraPrincipal.ScreenToWorldPoint(posicionMouse);
        offset = transform.position - posicionMouseEnMundo; // Diferencia inicial
    }

    // Método que se llama mientras se arrastra el objeto
    void OnMouseDrag()
    {
        
        // Obtenemos la posición actual del mouse en la pantalla
        Vector3 posicionMouse = Input.mousePosition;
        posicionMouse.z = camaraPrincipal.WorldToScreenPoint(transform.position).z; // Mantener la profundidad actual

        // Convertimos la posición del mouse a coordenadas del mundo
        Vector3 nuevaPosicion = camaraPrincipal.ScreenToWorldPoint(posicionMouse);

        // Aplicamos el offset para alinear correctamente el objeto con el mouse
        nuevaPosicion += offset;

        // Mantenemos la posición fija en Z
        nuevaPosicion.z = posicionFijaZ;

        // Actualizamos la posición del objeto
        transform.position = nuevaPosicion;
    }

    // Método que se llama cuando el mouse está sobre el objeto
    void OnMouseOver()
    {
        // Obtenemos el valor de la rueda del mouse
        float rueda = Input.GetAxis("Mouse ScrollWheel");
        
        // Calculamos la nueva escala
        Vector3 nuevaEscala = transform.localScale + new Vector3(rueda, rueda, rueda) * factorEscala;

        // Limitamos la escala para evitar que sea demasiado pequeña o grande
        nuevaEscala = new Vector3(
            Mathf.Clamp(nuevaEscala.x, escalaMinima, escalaMaxima),
            Mathf.Clamp(nuevaEscala.y, escalaMinima, escalaMaxima),
            Mathf.Clamp(nuevaEscala.z, escalaMinima, escalaMaxima)
        );

        // Aplicamos la nueva escala al objeto
        transform.localScale = nuevaEscala;
    }

}
