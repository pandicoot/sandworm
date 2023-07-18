
using UnityEngine;

public readonly struct Angle
{
    public float Value { get; }  // Radians. 0 is +x axis. Counterclockwise convention.

    public Angle(float val)
    {
        Value = (val + 2 * Mathf.PI) % (2 * Mathf.PI);
    }

    //public static float ToNormalisedValue(float val)
    //{
    //    float res = val - ((int)(val / (2 * Mathf.PI))) * 2 * Mathf.PI;
    //    if (res < 0)
    //    {
    //        res += 2 * Mathf.PI;
    //    }
    //    return res;
    //}

    //public Angle(float val)
    //{
    //    // Value goes from 0 to 2pi
    //    // Wolfram on Unity Forums
    //    //Value = ToNormalisedValue(val);

    //    //val %= 2 * Mathf.PI;
    //    //if (Mathf.Abs(val) <= Mathf.PI)
    //    //{
    //    //    Value = val;
    //    //}
    //    //else
    //    //{
    //    //    Value = val - Mathf.Sign(val) * 2 * Mathf.PI;
    //    //}

    //    //if (val > 0)
    //    //{
    //    //    val %= 2 * Mathf.PI;
    //    //    if (val < Mathf.PI)
    //    //    {
    //    //        Value = val;
    //    //    }
    //    //    else
    //    //    {
    //    //        Value = -2 * Mathf.PI + val;
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    val %= 2 * Mathf.PI;
    //    //    if (val > -Mathf.PI)
    //    //    {
    //    //        Value = val;
    //    //    }
    //    //    else
    //    //    {
    //    //        Value = 2 * Mathf.PI + val;
    //    //    }
    //    //}
    //}

    //public static bool operator ==(Angle a, Angle b)
    //{
    //    return a.Value == b.Value;
    //}
    //public static bool operator !=(Angle a, Angle b)
    //{
    //    return a.Value != b.Value;
    //}

    public static bool operator >(Angle a, Angle b)
    {
        var delta = ((2 * Mathf.PI - b.Value) + a.Value) % (2 * Mathf.PI);
        return (delta > 0 && delta < Mathf.PI);
    }
    public static bool operator <(Angle a, Angle b)
    {
        var delta = ((2 * Mathf.PI - a.Value) + b.Value) % (2 * Mathf.PI);
        return (delta > 0 && delta < Mathf.PI);
    }
    public static Angle operator +(Angle a, Angle b)
    {
        return new Angle(a.Value + b.Value);
    }
    public static Angle operator -(Angle a, Angle b)
    {
        return new Angle(a.Value - b.Value);
    }

}
