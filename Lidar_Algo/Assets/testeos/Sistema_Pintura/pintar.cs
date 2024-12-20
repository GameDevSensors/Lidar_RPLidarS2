using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pintar : MonoBehaviour
{
    [SerializeField] private GameObject pincel;

    // Posición donde va a aparecer el pincel.
    private Vector3 posicionPincel;

    [SerializeField] private float tiempo_dibujo = 0;
    private float tiempo = 0;

    void Update()
    {
        // Guardamos la posición del objeto en cada frame.
        posicionPincel = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto tiene la etiqueta "dibujar".
        if (other.CompareTag("dibujar"))
        {
            // Crea un nuevo objeto pincel en la posición actualizada.
            //GameObject Pincel_S = Instantiate(pincel, posicionPincel, Quaternion.identity);

            // Hacemos hijo del objeto al pincel.
            //Pincel_S.transform.SetParent(other.transform);

            // Rotamos el pincel para que apunte en la dirección correcta.
            //Pincel_S.transform.rotation = Quaternion.Euler(0, 180, 0);

            // Destruimos el objeto pincel después de 10 segundo.
            //Destroy(Pincel_S, 10);

            // tiene que estar 2 segundos en el mismo lugar para que pinte
            tiempo += Time.deltaTime;

            if (tiempo >= tiempo_dibujo)
            {
                // Crea un nuevo objeto pincel en la posición actualizada.
                GameObject Pincel_S = Instantiate(pincel, posicionPincel, Quaternion.identity);

                // Hacemos hijo del objeto al pincel.
                Pincel_S.transform.SetParent(other.transform);

                // Rotamos el pincel para que apunte en la dirección correcta.
                Pincel_S.transform.rotation = Quaternion.Euler(0, 180, 0);

                // Destruimos el objeto pincel después de 10 segundo.
                Destroy(Pincel_S, 10);

                //tiempo = 0;
            }

        }
    }
}
