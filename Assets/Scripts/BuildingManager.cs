using Array2DEditor;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public int gridSize;

    //This array only has strings that functions as IDs for all the objects in the scene
    string[][] myGrid;

    //I think the best way to handle this is to have each square point to an identifier of an object. The big question is how to create a unique identifier for each object
    //The simplest ID I can imagine is a time and date.
    //System.Guid.NewGuid().ToString() 
    Dictionary<string, Buildable> buildablesOnGrid;


    void Start()
    {
        myGrid = new string[gridSize][];
        for(int i = 0; i < gridSize; i++)
        {
            myGrid[i] = new string[gridSize];
        }

        buildablesOnGrid = new Dictionary<string, Buildable>();
    }

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
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position + new Vector3(0.5f, 0, -0.5f) + new Vector3(i, 0, -k), new Vector3(1, 0.01f, 1));
            }
        }
    }

    public bool AddObjectToGrid(Buildable newBuildable, Vector2Int gridPos)
    {
        //1. check if it fits
        //!! important !! checking happens from the corner
        for(int i = 0; i < newBuildable.BuildableSpaces.GridSize.x ; i++)
        {
            for (int k = 0; k < newBuildable.BuildableSpaces.GridSize.y; k++)
            {
                if (newBuildable.BuildableSpaces.GetCell(i, k))
                {
                    //myGrid[0].Length only works if the grid is a square
                    //Change this if we want a weird grid 
                    if (i >= myGrid.Length || k >= myGrid[0].Length || myGrid[gridPos.x + i][gridPos.y + k] != null)
                        return false;
                }
            }
        }

        //2. Generate ID for object
        string newID = System.Guid.NewGuid().ToString();

        //3. Add the object to the dictionary with the ID
        buildablesOnGrid.Add(newID, newBuildable);

        //4. Assign the ID to the correct spaces in the grid
        for (int i = 0; i < newBuildable.BuildableSpaces.GridSize.x; i++)
        {
            for (int k = 0; k < newBuildable.BuildableSpaces.GridSize.y; k++)
            {
                if (newBuildable.BuildableSpaces.GetCell(i, k))
                {
                    myGrid[gridPos.x + i][gridPos.y + k] = newID;
                }
            }
        }

        //5. set the object to the correct physical space on the grid
        //location = transform.position + gridpos;

        return true;
    }
}
