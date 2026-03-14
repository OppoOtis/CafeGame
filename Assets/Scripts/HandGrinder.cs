using UnityEngine;

public class HandGrinder : Grabbable
{
    public float rotationSpeed;
    public override void GrabInteractionHold(Interactor _interactor, bool _left)
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed, Space.Self);
    }
}
