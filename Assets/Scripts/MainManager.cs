using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class MainManager : MonoBehaviour
{
    public SceneSettings Scene;
    public GameObject boundaryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject bound;

        bound = Instantiate(boundaryPrefab, new Vector3(Scene.xLimit + 0.5f, 0, 0), Quaternion.Euler(0, 0, 0));
        bound.transform.localScale = new Vector3(1.0f, Scene.yLimit * 2.1f, 1.0f);
        bound.transform.SetParent(this.transform);

        bound = Instantiate(boundaryPrefab, new Vector3(-(Scene.xLimit + 0.5f), 0, 0), Quaternion.Euler(0, 0, 0));
        bound.transform.localScale = new Vector3(1.0f, Scene.yLimit * 2.1f, 1.0f);
        bound.transform.SetParent(this.transform);

        bound = Instantiate(boundaryPrefab, new Vector3(0, Scene.yLimit + 0.5f, 0), Quaternion.Euler(0, 0, 0));
        bound.transform.localScale = new Vector3(Scene.xLimit * 2.1f, 1.0f , 1.0f);
        bound.transform.SetParent(this.transform);

        bound = Instantiate(boundaryPrefab, new Vector3(0, -(Scene.yLimit + 0.5f), 0), Quaternion.Euler(0, 0, 0));
        bound.transform.localScale = new Vector3(Scene.xLimit * 2.1f, 1.0f, 1.0f);
        bound.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
