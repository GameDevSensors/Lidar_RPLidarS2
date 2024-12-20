using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCubes : MonoBehaviour
{
    public LayerMask cuboLayer;

    void Update()
    {
        List<Vector3> touchPoints = LidarManager.Instance.LidarTouchPoints;

        for (int i = 0; i < touchPoints.Count; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPoints[i]);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f, cuboLayer))
            //if (Physics.SphereCast(ray, 2f, out hit, 50f, cuboLayer))
            {
                GameObject cubo = hit.collider.gameObject;
                cubo.GetComponent<Animator>().Play("Brillo");
            }
        }
    }
}
