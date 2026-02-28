using Array2DEditor;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public int gridSize;

    //TODO replace the bool with something else, so we can identify which item is on which square, even if it occupies multiple squares
    bool[][] myGrid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myGrid = new bool[gridSize][];
        for(int i = 0; i < gridSize; i++)
        {
            myGrid[i] = new bool[gridSize];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int k = 0; k < gridSize; k++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(transform.position + new Vector3(0.5f, 0, -0.5f) + new Vector3(i, 0, -k), new Vector3(1, 0.01f, 1));
            }
        }
    }
}
