using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject
{
    [Header("Movement")]
    public float defaultSpeed = 1.0f;
    //public float rotationSpeed = Mathf.PI / 2;
    public float goalStrength = 1.0f;
    public float avoidStrength = 1.0f;
    public float alignStrength = 1.0f;
    public float middleStrength = 1.0f;
    public float randomStrength = 1.0f;
    public float randomAngleRange = 60f;
    public float maxRotation = 60f;     //Degrees



    [Header("View")]
    public float viewRadius = 1.0f;
    public float viewAngle = 180f;
    public int viewResolution = 12;
    public int senseResolution = 40;
    public int nbInfluent = 6;


    
    
}
