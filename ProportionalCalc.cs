using UnityEngine;

public class ProportionalCalc : MonoBehaviour
{
    [Range(-0.5f, 0.5f)] // Unity attribute to create a slider in the Inspector to adjust this value within the specified range.
    public float scaleMe; // Input value to be scaled.

    public float scaledValue; // Resulting scaled value.

    void Update()
    {
        // Call the scale function to scale the input value and assign it to the 'scaled' variable.
        scaledValue = scale(-0.5f, 0.5f, 0, 100, scaleMe);
    }

    // Method to scale a value from one range to another.
    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        // Calculate the range of the old and new values.
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);

        // Calculate the scaled value using the proportional scaling formula.
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        // Return the scaled value.
        return NewValue;
    }
}

