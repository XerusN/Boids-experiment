using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BoidBehaviour : MonoBehaviour
{
    public int id = 0;


    public BoidSettings Settings;
    public SceneSettings Scene;
    
    

    public GameObject goal;

    public Vector3 target;
    public Vector3 direction;
    public float speed;

    private BoidManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("Boid Manager").GetComponent<BoidManager>();
        if (Manager == null)
        {
            Debug.LogError("Main Manager not found!");
        }

        this.transform.position = new Vector3(Random.Range(-Scene.xLimit + 0.5f, Scene.xLimit - 0.5f), Random.Range(-Scene.yLimit + 0.5f, Scene.yLimit - 0.5f), 0);
        target = new Vector3(Random.Range(-Scene.xLimit + 0.5f, Scene.xLimit - 0.5f), Random.Range(-Scene.yLimit + 0.5f, Scene.yLimit - 0.5f), 0);
        direction = (this.target - this.transform.position).normalized;
        speed = Settings.defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        direction = this.transform.up;
        UpdateDirection();
        Move();
    }




    private void UpdateDirection()
    {
        direction = Interact();
        //Limit to avoid sudden changes in direction
        direction = Vector3.RotateTowards(this.transform.up, direction, Settings.maxRotation * Mathf.PI / 180, 0f);
        AvoidCollision();

        direction = new Vector3(direction.x, direction.y, 0);
    }



    private void Move()
    {
        //speed = (180 - Mathf.Abs(Vector3.Angle(this.transform.up, direction)) % 180)*Settings.defaultSpeed;
        speed = Settings.defaultSpeed;
        this.transform.up = direction.normalized;
        this.transform.position += transform.up * speed * Time.deltaTime;
    }

    private Vector3 Interact()
    {
        Vector3 change = Vector3.zero;

        GameObject[] closeBoids = new GameObject[Settings.nbInfluent];
        closeBoids = InteractableList();

        change += SteerAway(closeBoids);
        change += Align(closeBoids);
        change += TowardMiddle(closeBoids);


        return change;
    }



    private GameObject[] InteractableList()
    {
        GameObject currentBoid = null;
        GameObject[] boidArray = new GameObject[Settings.nbInfluent];
        float[] distanceArray = new float[Settings.nbInfluent];

        for (int i = 0;  i < Settings.nbInfluent; i++)
        {
            distanceArray[i] = Mathf.Infinity;
        }

        for (int i = 0; i < Manager.boidList.Count; i++)
        {
            if (i == this.id) { break; }

            currentBoid = Manager.boidList[i];

            for (int j = 0; j < boidArray.Length; j++)
            {
                if (boidArray[j] == null)
                {
                    boidArray[j] = currentBoid;
                    distanceArray[j] = DistanceToThis(currentBoid);
                    break;
                }

                if (DistanceToThis(currentBoid) < distanceArray[j])
                {
                    InsertBoth(boidArray, distanceArray, currentBoid, j);
                    break;
                }
            }
        }

        return boidArray;
    }


    private Vector3 SteerAway(GameObject[] influentBoids)
    {
        Vector3 change = Vector3.zero;

        for (int i = 0;i < influentBoids.Length; i++)
        {
            if (influentBoids[i] == null) { break; }
            change += (this.transform.position - influentBoids[i].transform.position).normalized * Mathf.Pow(DistanceToThis(influentBoids[i]), -1f);
        }
        
        return change*Settings.avoidStrength;
    }

    private Vector3 Align(GameObject[] influentBoids)
    {
        Vector3 change = Vector3.zero;

        for (int i = 0; i < influentBoids.Length; i++)
        {
            if (influentBoids[i] == null) { break; }
            change += influentBoids[i].transform.up;
        }

        return change * Settings.alignStrength;
    }

    private Vector3 TowardMiddle(GameObject[] influentBoids)
    {
        Vector3 change = Vector3.zero;

        for (int i = 0; i < influentBoids.Length; i++)
        {
            if (influentBoids[i] == null) { break; }
            change += (influentBoids[i].transform.position - this.transform.position).normalized * Mathf.Pow(DistanceToThis(influentBoids[i]), 1f);
        }
        change = change.normalized;

        return change * Settings.middleStrength;
    }


    private void AvoidCollision()   //overwrite direction to find a clear path as close as possible to 
    {
        Vector3 change = Vector3.zero;
        Vector3 changeTemp = UnityEngine.Vector3.zero;
        float angle;
        float dAngle = Settings.viewAngle / Settings.viewResolution;
        Vector3 dir = new Vector3();
        bool isPathClear = false;

        for (int i = 0; i < Settings.viewResolution / 2; i++)
        {
            angle = i * dAngle;
            
            for (int j = 0; j < 2; j++)
            {
                dir = Quaternion.Euler(0, 0, i * angle * (float)Mathf.Pow(-1, j)) * direction;
                Ray ray = new Ray(this.transform.position, dir);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Settings.viewRadius);
                if (hit.collider != null)
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance);
                    if (hit.collider.gameObject.CompareTag("Obstacle") == true)
                    {
                        continue;
                    }
                    else
                    {
                        isPathClear = true;
                        break;
                    }
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction);
                    isPathClear = true;
                    break;
                }

            }

            if (isPathClear == true)
            {
                break;
            }



        }

        direction = dir;
    }


    private float DistanceToThis(GameObject gameObject)
    {
        float distance = 0f;
        distance = (gameObject.transform.position - this.transform.position).magnitude;
        return distance;
    }

    private void InsertBoth(GameObject[] boidArray, float[] distanceArray, GameObject currentBoid, int i)
    {
        GameObject tempObject1 = currentBoid;
        GameObject tempObject2 = null;
        float tempDist1 = DistanceToThis(currentBoid);
        float tempDist2 = 0f;

        for (int j = i; j < boidArray.Length; j++)
        {
            tempObject2 = boidArray[j];
            tempDist2 = distanceArray[j];
            boidArray[j] = tempObject1;
            distanceArray[j] = tempDist1;
            tempObject1 = tempObject2;
            tempDist1 = tempDist2;
        }
    }

}
