using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class NumberFormatter : MonoBehaviour
{
    // Input string representing a number
    public string inputNumber;

    // Input integer representing a big number
    public int inputBigNumber;

    // BigInteger representation of the input big number
    BigInteger bigInteger;

    // Update is called once per frame
    private void Update()
    {
        // Convert the input big number to BigInteger
        bigInteger = (BigInteger)inputBigNumber;

        // Format the BigInteger into the desired format
        inputNumber = FormatNumber(bigInteger);
    }

    // Formats a BigInteger into a human-readable string representation
    public static string FormatNumber(BigInteger bigInteger)
    {
        // If the number is less than one million, format it with commas
        if (bigInteger < 1000000)
        {
            return bigInteger.ToString("###,##0");
        }

        // Get the amount of zeros of the number
        int zeros = (int)BigInteger.Log10(bigInteger);

        // Get the first two digits and add the corresponding letters to it (e.g., 1.7ae)
        return (float)BigInteger.Divide(dividend: bigInteger,divisor:BigInteger.Pow(value: 10,exponent: zeros -1)) /10 + GetLettersBasedOnZeros(zeros);
    }
   
    // Returns a string representing letters based on the number of zeros
    private static string GetLettersBasedOnZeros(int zeros)
    {
        zeros -= 6; // Subtract 6 because we're starting at 1 million with 'a'
        char firstLetter = GetCharBasedOnInt(Mathf.FloorToInt(zeros / 52f));
        char secondLetter = GetCharBasedOnInt(Mathf.FloorToInt(zeros % 52f));
        return "" + firstLetter + secondLetter;
    }

    // Returns a character based on an integer value
    private static char GetCharBasedOnInt(int intValue)
    {
        // ASCII codes for A-Z (65-90) and a-z (97-122)
        if (intValue < 26)
        {
            return (char)(97 + intValue); // Convert to lowercase letter
        }
        if (intValue < 52)
        {
            return (char)(65 + intValue - 26); // Convert to uppercase letter
        }

        // If intValue is out of range, return a dash
        return '-';
    }
}