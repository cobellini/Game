using System;


[Serializable]
public class RandomIntGen
{
    public int minimumvVal;       //minimum value 
    public int maximumVal;       //max value

    public RandomIntGen(int min, int max) //constructor to set values.
    {
        minimumvVal = min;
        maximumVal = max;
    }

    public int Random //get random value
    {
        get { return UnityEngine.Random.Range(minimumvVal, maximumVal); }
    }
}