using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedAnimation : MonoBehaviour
{
    public Transform target;
    public AnimationClip clip;
    public Vector3 offset;//Simple offset to adjust stopping distance for animation

    [SerializeField] float amount;
    [SerializeField] float adjust = 0.25f;
    [SerializeField] float currentDistance;
     float startValue = 0.8f, interpolationFactor = 0.75f; // Adjust as needed for animation frame requirements
 

    void Update()
    {
        //We use a Mathf.Lerp to slow down the time of the maxDistanceDelta in the Vector3.MoveTowards as the speed
        //of the animation playing is too fast. For debugging and demo purposes, we keep it slower than required
        float time = Mathf.Lerp(0, 1, adjust); 

        transform.LookAt(target); //Simply to get correct orientation of the animation playing

        transform.position = Vector3.MoveTowards(transform.position, target.position - offset, time);  //Move the object

        currentDistance = Vector3.Distance(transform.position, target.position); //Calculate the distance from target for interpolation calculation

        amount = Mathf.Lerp(startValue,0, currentDistance - interpolationFactor);  //Calculate interpolation amount based on the distance

        clip.SampleAnimation(transform.gameObject, amount); //Lerp through frames in real time based on distance to target

    }
}
