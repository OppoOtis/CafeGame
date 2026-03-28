using UnityEngine;

public class PositionTesting : MonoBehaviour
{
    public Transform targetObject;

    private void Update()
    {
        transform.position = targetObject.position;
        transform.rotation = targetObject.rotation;
    }
}
