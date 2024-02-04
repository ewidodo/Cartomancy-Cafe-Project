using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MathUtils
{
    // factor == 0: back to b value / no smoothing
    // smaller factors are faster lerps
    // factor == 1: back to a value / infinite smoothing
    // larger factors are slower lerps
    public static float Approach(float a, float b, float factor, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Pow(factor, dt));
    }

    public static float PythagoreanDistance(float a, float b)
    {
        return Mathf.Sqrt( a*a + b*b );
    }

    public static float LinearMap(float value, float inputLow, float inputHigh, float outputLow, float outputHigh, bool clamp = false ) 
    {
        float output = outputLow + (outputHigh - outputLow) * (value - inputLow) / (inputHigh - inputLow);

        if( clamp )
        {
            float actualOutputLow = Mathf.Min( outputLow, outputHigh );
            float actualOutputHigh = Mathf.Max( outputLow, outputHigh );

            output = Mathf.Clamp( output, actualOutputLow, actualOutputHigh );
        }

        return output;
    }

    public static Vector3 LinearMapVector(float value, float inputLow, float inputHigh, Vector3 outputLow, Vector3 outputHigh, bool clamp = false ) 
    {
        float x = LinearMap( value, inputLow, inputHigh, outputLow.x, outputHigh.x, clamp );
        float y = LinearMap( value, inputLow, inputHigh, outputLow.y, outputHigh.y, clamp );
        float z = LinearMap( value, inputLow, inputHigh, outputLow.z, outputHigh.z, clamp );

        return new Vector3( x, y, z );
    }

    public static float CircularMap(float value, float inputLow, float inputHigh, float source, float destination, float minValue, float maxValue, bool clamp = false)
    {
        float inputDifference = destination - source;
        float range = maxValue - minValue;
        float output = 0f;

        
        if (Mathf.Abs(inputDifference) > 0.5f * range)
        {
            // Input difference is greater than half of the range
            output = inputDifference < 0 ?
                LinearMap(value, inputLow, inputHigh, source, destination + range, clamp) : // destination < source
                LinearMap(inputLow + inputHigh - value, inputLow, inputHigh, destination - range, source, clamp) ; // source < destination

            output = output < 0 ?
                output + range :
                output % range ;
        }
        else
        {
            // Input difference is less than half of the range, do linear map normally
            output = LinearMap(value, inputLow, inputHigh, source, destination, clamp);
        }

        return output;
    }

    public static Color LinearMapColor(float value, float inputLow, float inputHigh, Color a, Color b, bool clamp = false)
    {
        float a_h, a_s, a_v,
              b_h, b_s, b_v,
              out_h, out_s, out_v;
        Color.RGBToHSV(a, out a_h, out a_s, out a_v);
        Color.RGBToHSV(b, out b_h, out b_s, out b_v);

        out_h = CircularMap(value, inputLow, inputHigh, a_h, b_h, 0f, 1f, clamp);
        out_s = LinearMap(value, inputLow, inputHigh, a_s, b_s, clamp);
        out_v = LinearMap(value, inputLow, inputHigh, a_v, b_v);

        return Color.HSVToRGB(out_h, out_s, out_v);
    }

    public static Vector3 UnitVector( float value = 1.0f )
    {
        return new Vector3( value, value, value );
    }

    public static Vector3 RandomPointInCircle(Transform trans, float radius, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector3 position = trans.right * Mathf.Sin( rad ) + trans.forward * Mathf.Cos( rad );
        return trans.position + position * radius;
    }
}
