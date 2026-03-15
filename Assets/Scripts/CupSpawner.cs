using UnityEngine;

public class CupSpawner : Interactible
{
    public Transform spawnLocation;
    public GameObject cupPrefab;

    public override void Interact(Interactor _interactor, bool _left)
    {
        Instantiate(cupPrefab, spawnLocation.position, Quaternion.identity);
    }
}
