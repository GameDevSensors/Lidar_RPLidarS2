using UnityEngine;

public class MatrixPrefabCreator : MonoBehaviour
{
    public GameObject prefab; // El prefab que quieres instanciar
    public int rows = 5; // Número de filas en la matriz
    public int columns = 5; // Número de columnas en la matriz
    public float separation = 1.0f; // Separación entre los prefabs en unidades

    private void Start()
    {
        CreateMatrix();
    }

    private void CreateMatrix()
    {
        Vector3 initialOffset = new Vector3((columns - 1) * separation * 0.5f, (rows - 1) * separation * 0.5f, 0f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * separation, row * separation, 0f) - initialOffset;
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }
}
