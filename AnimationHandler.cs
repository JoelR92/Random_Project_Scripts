using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    
    bool mouseClick;
    public Animator anim;
    public bool animStart;
    public float animationInFrames;
    [Space]
    [Header("Transition States")]
    public int transitionCount = 0;
    public int previousState;
     int idleState;
    [Header("Debugging, can be removed")]
    public string AnimOne, AnimTwo;
    public bool animOne, animTwo;
    public bool pressed;

    [Header("Counting Click for keeping track of transition")]
    public float countingInterval = 1f; // Time interval for counting button presses
    public float resetInterval = 5f; // Time interval for resetting the counter
    private int buttonPressCount = 0; // Counter for button presses
    private float countingTimer = 0f; // Timer for counting interval
    private float resetTimer = 0f; // Timer for reset interval
    public int clickCount;
    public int maxCount;
    private void Start()
    {
        if(anim == null){ Debug.LogError("No Animator Detected"); }else{anim = GetComponent<Animator>();}
        previousState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
      //  idleState = 1432961145;
        idleState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash; //Store the idle animation state
    }
    private void Update()
    {
        mouseClick = Input.GetMouseButtonDown(0) && Input.GetMouseButton(0); //Check for input every frame to avoid missinput

        anim.SetBool("InputDetected", mouseClick);//Set the input detection in animator

        //Set the animation parameter "Timer" from the current Animator State to increase .1f from start and normalize the animation length from 0 to 1 regardless of animation
        //length to find beginning and ending points for easy transitions and scaleable animation workflow
        anim.SetFloat("Timer", Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f), .1f, Time.deltaTime);

        anim.SetInteger("Y", clickCount);
        anim.SetBool("AnimFin", animStart);
   
        animationInFrames = anim.GetFloat("Timer") * 1000;

        // Check if the current state is different from the previous state
        int currentState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        int currentTransition = anim.GetAnimatorTransitionInfo(0).fullPathHash;
        if (currentState != previousState && currentTransition != previousState) { transitionCount++; previousState = currentState; }
        //Check if current state is the idle state and reset variables
        if (currentState == idleState) { transitionCount = 0; }


        //Debugging //Can be removed
        animOne = anim.GetCurrentAnimatorStateInfo(0).IsName(AnimOne);
        animTwo = anim.GetCurrentAnimatorStateInfo(0).IsName(AnimTwo);

        //Simple conditional blocks for resetting transitions of a two animation Animator controller
        if (mouseClick)
        {
            if(buttonPressCount < maxCount)
            {
                buttonPressCount++;
            }
            else { buttonPressCount = 2; }
           
            countingTimer = countingInterval;
            resetTimer = resetInterval;
        }
       
        // Decrement the counting timer
        countingTimer -= Time.deltaTime;
       
        if (countingTimer <= 0f )
        {
            buttonPressCount = 0;
        }
        // Decrement the reset timer
        resetTimer -= Time.deltaTime;
        if (resetTimer <= 0f)
        {
            buttonPressCount = 0;
            resetTimer = resetInterval;
        }
         clickCount = GetButtonPressCount(); 
       
    }
    // Use this method to retrieve the button press count
    public int GetButtonPressCount()
    {
        return buttonPressCount;
    }

    //Method for possible use in the animations themselves to manually set animation events to trigger start and end frames of animations
    public void AnimStart(int start)
    {
        if(start == 1) { animStart = true; }
        if(start == 0) { animStart = false; }
        
    }
}