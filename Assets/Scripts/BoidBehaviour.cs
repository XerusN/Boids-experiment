using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BoidBehaviour : MonoBehaviour
{

    public BoidSettings Settings;
    public SceneSettings Scene;

    public Vector3 direction;

    private float randomAngle;

    public GameObject goal;

    // Start is called before the first frame update
    void Start()
    {
        this.direction = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }



    private void Move()
    {

        this.direction = (this.direction + TowardGoal() + AvoidCollision() + RandomMove()).normalized;
        this.transform.position += this.direction * Settings.speed * Time.deltaTime;
        this.transform.up = this.direction;
    }

    private Vector3 RandomMove()
    {

        
        Vector3 change = this.transform.up;

        change = Quaternion.Euler(0, 0, this.randomAngle) * change;
        change *= Time.deltaTime * Settings.randomStrength;

        return change;
    }




    private Vector3 TowardGoal()
    {
        Vector3 change = Vector3.zero;
        
        //change = (goal.transform.position - this.transform.position).normalized * Settings.goalStrength * Time.deltaTime;
        return change;
    }

    private Vector3 AvoidCollision()
    {
        Vector3 change = Vector3.zero;
        Vector3 changeTemp = Vector3.zero;
        float angle;
        float dAngle = Settings.viewAngle / Settings.viewResolution;

        for (int i = 0; i < Settings.viewResolution / 2; i++)
        {
            angle = i * dAngle;
            changeTemp = AvoidSingleCollision(angle);
            if (i == 0)
            {
                change = changeTemp;
                
            }
            else
            {
                if (changeTemp.magnitude > change.magnitude)
                {
                    change = changeTemp;
                }
                changeTemp = AvoidSingleCollision(-angle);
            }
            if (changeTemp.magnitude > change.magnitude)
            {
                change = changeTemp;
            }
        }
        
        change *= Time.deltaTime * Settings.avoidStrength;


        return change;
    }


    private Vector3 AvoidSingleCollision(float angle)       //angle in degrees
    {
        Vector3 change = Vector3.zero;
        Vector3 dir;

        dir = Quaternion.Euler(0, 0, angle) * this.transform.up;
        Ray ray = new Ray(this.transform.position, dir);
        //Debug.Log(ray.direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Settings.viewRadius);
        if (hit.collider != null)
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance);
            dir = hit.transform.position - this.transform.position;
            change = dir / (dir.magnitude * dir.magnitude);
        } else
        {
            Debug.DrawRay(ray.origin, ray.direction);
        }

        return -change;
    }


    IEnumerator GenerateRandomAngle()
    {
        for (; ; )
        {
            randomAngle = Random.Range(-Settings.randomAngleRange, Settings.randomAngleRange);
            yield return new WaitForSeconds(10f);
        }
        

    }


}
