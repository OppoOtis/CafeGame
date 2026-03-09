using UnityEngine;

public class Cup : Grabbable, IWeighable
{
    public float Weight { get { return weight; } set { weight = value; } }
    public float weight;
}
