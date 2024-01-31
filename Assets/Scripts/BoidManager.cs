using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{

    public GameObject boidPrefab;
    public GameObject goalPrefab;
    public SceneSettings Scene;

    private float margin = 1.0f;


    // Start is called before the first frame update
    void Start()
    {

        Instantiate(goalPrefab);

        Vector3 spawnPos;
        float rotation;
        GameObject boid;

        for (int i = 0; i < Scene.boidLimit; i++)
        {
            spawnPos = new Vector3(Random.Range(-Scene.xLimit + margin, Scene.xLimit - margin), Random.Range(-Scene.yLimit - margin,Scene.yLimit + margin), 0);
            rotation = Random.Range(0, 360);

            boid = Instantiate(boidPrefab, spawnPos, Quaternion.Euler(0, 0, rotation));
            boid.transform.SetParent(this.transform);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
