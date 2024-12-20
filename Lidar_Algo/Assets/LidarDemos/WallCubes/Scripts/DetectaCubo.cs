using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectaCubo : MonoBehaviour
{
    public LayerMask cuboLayer;
    

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cuboLayer))
        {
            Debug.Log("tocaCubo");
            GameObject cubo = hit.collider.gameObject;
            cubo.GetComponent<Animator>().Play("Brillo");
        }
    }
}