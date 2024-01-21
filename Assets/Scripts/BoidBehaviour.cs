using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BoidBehaviour : MonoBehaviour
{

    public BoidSettings Settings;
    public SceneSettings Scene;

    public UnityEngine.Vector3 direction;

    private float randomAngle;

    public GameObject goal;

    // Start is called before the first frame update
    void Start()
    {
        this.direction = UnityEngine.Vector3.up;
        StartCoroutine(GenerateRandomAngle());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Debug.Log(randomAngle);
    }



    private void Move()
    {

        this.direction = (this.direction + TowardGoal() + AvoidCollision() + RandomMove() + AlignDirection()).normalized;
        this.transform.position += this.direction * Settings.speed * Time.deltaTime;
        this.transform.up = this.direction;
    }

    private UnityEngine.Vector3 RandomMove()
    {

        
        UnityEngine.Vector3 change = this.transform.up;

        change = UnityEngine.Quaternion.Euler(0, 0, this.randomAngle) * change;
        change *= Settings.randomStrength;

        return change;
    }




    private UnityEngine.Vector3 TowardGoal()
    {
        UnityEngine.Vector3 change = UnityEngine.Vector3.zero;
        
        change = (goal.transform.position - this.transform.position).normalized * Settings.goalStrength;
        return change;
    }

    // private Vector3 AvoidCollision()
    // {
    //     Vector3 change = Vector3.zero;
    //     Vector3 changeTemp = Vector3.zero;
    //     float angle;
    //     float dAngle = Settings.viewAngle / Settings.viewResolution;

    //     for (int i = 0; i < Settings.viewResolution / 2; i++)
    //     {
    //         angle = i * dAngle;
    //         changeTemp = AvoidSingleCollision(angle);
    //         if (i == 0)
    //         {
    //             change = changeTemp;
                
    //         }
    //         else
    //         {
    //             if (changeTemp.magnitude > change.magnitude)
    //             {
    //                 change = changeTemp;
    //             }
    //             changeTemp = AvoidSingleCollision(-angle);
    //         }
    //         if (changeTemp.magnitude > change.magnitude)
    //         {
    //             change = changeTemp;
    //         }
    //     }
        
    //     change *= Time.deltaTime * Settings.avoidStrength;


    //     return change;
    // }





    private UnityEngine.Vector3 AvoidCollision()
    {
        UnityEngine.Vector3 change = UnityEngine.Vector3.zero;
        float angle = 0;
        float dAngle = Settings.viewAngle / Settings.viewResolution;
        bool isPathClear = false;
        UnityEngine.Vector3 dir;

        while(!isPathClear && angle < Settings.viewAngle/2){
            for (int i = -1; i < 2; i += 2){
                dir = UnityEngine.Quaternion.Euler(0, 0, i * angle) * this.transform.up;
                Ray ray = new Ray(this.transform.position, dir);
                //Debug.Log(ray.direction);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, Settings.viewRadius);
                if (hit.collider != null)
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance);
                } else
                {
                    Debug.DrawRay(ray.origin, ray.direction);
                    change = dir;
                    isPathClear = true;
                }
            }
            
            angle += dAngle;
        }

        
        
        change *= Settings.avoidStrength;


        return change;
    }


    IEnumerator GenerateRandomAngle()
    {
        for (; ; )
        {
            randomAngle = Random.Range(-Settings.randomAngleRange, Settings.randomAngleRange);
            yield return new WaitForSeconds(2f);
        }
        

    }




    private UnityEngine.Vector3 AlignDirection(){

        UnityEngine.Vector3 change = UnityEngine.Vector3.zero;
        UnityEngine.Vector3 dir;
        float angle = 0;
        float dAngle = 360 / Settings.senseResolution;

        while(angle < 360f){
            dir = UnityEngine.Quaternion.Euler(0, 0, angle) * this.transform.up;
            Ray ray = new Ray(this.transform.position, dir);
            //Debug.Log(ray.direction);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Settings.viewRadius);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Boid"){
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance);
                    dir = hit.transform.up;
                    change += dir.normalized / (hit.distance * hit.distance);
                }
            } else
            {
                //Debug.DrawRay(ray.origin, ray.direction);
            }



            angle += dAngle;
        }
        change = change.normalized * Settings.alignStrength;

        return change;
    }


}
