using Array2DEditor;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class Buildable : MonoBehaviour
{
    //make an editor visual for the object and which spaces it occupies
    public Array2DBool BuildableSpaces { get { return buildableSpaces; } set { buildableSpaces = value; } }
    [SerializeField]
    Array2DBool buildableSpaces;

    private void OnDrawGizmos()
    {
        for(int i = 0; i < BuildableSpaces.GridSize.x; i++)
        {
            for (int k = 0; k < BuildableSpaces.GridSize.y; k++)
            {
                if (BuildableSpaces.GetCell(i, k))
                {
                    //draw square gizmo
                    //!! Warning !! to make it look good in the editor, remember that k is negative !!
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(transform.position + new Vector3(0.5f,0,-0.5f) + new Vector3(i,0,-k), new Vector3(1,0.01f,1));
                }
            }
        }
    }

}
