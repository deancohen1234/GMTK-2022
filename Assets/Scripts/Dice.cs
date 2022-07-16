using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DiceAxis
{
    public Vector3 axis;
    public int value;
}

public class Dice : MonoBehaviour
{
    public DiceAxis oneAxis; 
    public DiceAxis twoAxis; 
    public DiceAxis threeAxis; 
    public DiceAxis fourAxis; 
    public DiceAxis fiveAxis;
    public DiceAxis sixAxis;

    private const float DOTTHRESHOLD = 0.95f;

    //not happy with this :(
    public int GetFaceUpNumber()
    {
        float dot = 0;

        dot = GetAxisUpDot(oneAxis);
        if (dot >= DOTTHRESHOLD)
        {
            return oneAxis.value;
        }

        dot = GetAxisUpDot(twoAxis);
        if (dot >= DOTTHRESHOLD)
        {
            return twoAxis.value;
        }

        dot = GetAxisUpDot(threeAxis);
        if (dot >= DOTTHRESHOLD)
        {
            return threeAxis.value;
        }

        dot = GetAxisUpDot(fourAxis);
        if (dot >= DOTTHRESHOLD)
        {
            return fourAxis.value;
        }

        dot = GetAxisUpDot(fiveAxis);
        if (dot >= DOTTHRESHOLD)
        {
            return fiveAxis.value;
        }

        dot = GetAxisUpDot(sixAxis);
        if (dot >= DOTTHRESHOLD)
        {
            return sixAxis.value;
        }

        Debug.LogWarning("Dice could not find up!!");
        return 0;
    }

    public Vector3 GetWorldNumberAxis(int value)
    {
        switch (value)
        {
            case 1:
                return transform.TransformVector(oneAxis.axis);
            case 2:
                return transform.TransformVector(twoAxis.axis);
            case 3:
                return transform.TransformVector(threeAxis.axis);
            case 4:
                return transform.TransformVector(fourAxis.axis);
            case 5:
                return transform.TransformVector(fiveAxis.axis);
            case 6:
                return transform.TransformVector(sixAxis.axis);
            default:
                Debug.LogError("Invalid axis value: " + value);
                return Vector3.zero;
        }
    }

    private float GetAxisUpDot(DiceAxis diceAxis)
    {
        Vector3 axisVectorRight = new Vector3(diceAxis.axis.x * transform.right.x, diceAxis.axis.x * transform.right.y, diceAxis.axis.x * transform.right.z);
        Vector3 axisVectorUp = new Vector3(diceAxis.axis.y * transform.up.x, diceAxis.axis.y * transform.up.y, diceAxis.axis.y * transform.up.z);
        Vector3 axisVectorForward = new Vector3(diceAxis.axis.z * transform.forward.x, diceAxis.axis.z * transform.forward.y, diceAxis.axis.z * transform.forward.z);

        Vector3 axisVectorCombined = axisVectorRight + axisVectorUp + axisVectorForward;

        Debug.Log($"Dice Axis {diceAxis.value} with Vector: {axisVectorCombined} Transform right: {transform.right}");

        return Vector3.Dot(axisVectorCombined, Vector3.up);

    }
}
