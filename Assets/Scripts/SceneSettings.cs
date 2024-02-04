using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneSettings : ScriptableObject
{

    [Header("Boundaries")]
    public float xLimit = 10f;
    public float yLimit = 5f;
    public float zLimit = 0f;

    [Header("Boids")]
    public int boidLimit = 20;



}
