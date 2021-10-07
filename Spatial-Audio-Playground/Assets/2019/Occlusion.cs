﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;


public class Occlusion : MonoBehaviour
{
    Vector3 SourcePosition;
    public Transform mCamera;

    [Header("Debug Info:")]
    public float DisToListener;
    public float CalculateUnder=40f;
    public float BlockedDistance;
   // public float HightDistance;
    public bool PassFloor;
    public bool PassWall;
    public float OcclusionPercentage {get;private set;}
    public float hit1;
    public float hit2;
    public bool Draw=false;
    public float value; 
    public bool mDebug;  
    
    // Use this for initialization
    void Start()
    {
        mCamera=Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        SourcePosition = transform.position;
        RaycastHit HitInfo1, HitInfo2;
        RaycastHit[] HitArrary;
        Vector3 RayDirection1 = mCamera.position - SourcePosition;
        Vector3 RayDirection2 = SourcePosition - mCamera.position;
        


        DisToListener = Vector3.Distance(SourcePosition, mCamera.position);

        //CalculateUnder = AkSoundEngine.GetMaxRadius(this.gameObject);
        

        if (DisToListener <= CalculateUnder)
        {

            Physics.Raycast(SourcePosition, RayDirection1, out HitInfo1, DisToListener, GameSetting.GameLayer.I_SoundCastAll);
            Physics.Raycast(mCamera.position, RayDirection2, out HitInfo2, DisToListener, GameSetting.GameLayer.I_SoundCastAll);


            HitArrary = Physics.RaycastAll(SourcePosition, RayDirection1, DisToListener);        

            hit1 = HitInfo1.distance;
            hit2 = HitInfo2.distance;
            
            if (HitInfo1.transform==null) hit1=DisToListener;
            if (HitInfo2.transform==null) hit2=DisToListener;

            if (mDebug) Debug.Log(HitInfo1.transform.name);
            if (mDebug) Debug.Log(HitInfo2.transform.name);


            PassFloor = false;
            PassWall = false;

            for (int i = 0; i < HitArrary.Length; i++)
            {
                if (HitArrary[i].transform.tag == "Floor")
                {
                    PassFloor = true;

                }

                if (HitArrary[i].transform.tag == "MeshWall")
                    PassWall = true;

                if (PassFloor && PassWall)
                    break;
            }

            if (hit1 + hit2 >= DisToListener) // if not blocked then only simple calculate
            {
                if (PassFloor)
                    OcclusionPercentage = 0.7f;
                else if (PassWall)
                    OcclusionPercentage = 0.25f;
                else
                    OcclusionPercentage = 0.0f;

                //AkSoundEngine.SetObjectObstructionAndOcclusion(gameObject, Camera.gameObject, 0.0f, OcclusionPercentage);
            } 
            else  //calculate of occlusion
            {                
                                
                // avoid camera and object being blocked by themself
                //if (hit2 < 0.4 && DisToListener - hit1 >= 0.4)                
                 //   hit2 = 0.4f;
                //if (hit1 < 0.1 && DisToListener - hit2 >= 0.1)
                  //  hit1 = 0.1f;
                
                
                BlockedDistance = DisToListener - hit1 - hit2;
                
                

                if (BlockedDistance <= 0.5)
                {
                    OcclusionPercentage = BlockedDistance / DisToListener;
                }

                else if (BlockedDistance <= 2)
                {
                    OcclusionPercentage = (BlockedDistance - 0.5f) / DisToListener;
                }
                else if (BlockedDistance <= 4)
                {
                    OcclusionPercentage = (1.25f * BlockedDistance - 1.0f) / DisToListener;
                }
                else
                {
                    OcclusionPercentage = BlockedDistance / DisToListener;
                }
             

                //extra occlusion added on floor and wall
                if (PassFloor)
                    OcclusionPercentage = OcclusionPercentage + 0.50f;

                if (PassWall && !PassFloor)
                    OcclusionPercentage = OcclusionPercentage + 0.1f;

                if (OcclusionPercentage > 1)
                    OcclusionPercentage = 1f;

                if (OcclusionPercentage < 0)
                    OcclusionPercentage = 0f;


                //AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, Camera.gameObject, 0.0f, OcclusionPercentage);
                
            }
            if (Draw){
                if (OcclusionPercentage >=0.7)
                    Debug.DrawRay(SourcePosition, RayDirection1, Color.red);
                else if (OcclusionPercentage >= 0.3)
                    Debug.DrawRay(SourcePosition, RayDirection1, Color.yellow);
                else
                    Debug.DrawRay(SourcePosition, RayDirection1, Color.green);
            }
            value=OcclusionPercentage;
        }


    }
}
