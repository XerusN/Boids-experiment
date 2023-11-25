using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneSettings : ScriptableObject
{

    [Header("Boundaries")]
    public float xLimit = 20f;
    public float yLimit = 10f;

    [Header("Boids")]
    public int boidLimit = 10;



}
