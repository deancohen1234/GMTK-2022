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
    public Transform diceMeshTransform;

    [Space(10)]
    public DiceAxis oneAxis; 
    public DiceAxis twoAxis; 
    public DiceAxis threeAxis; 
    public DiceAxis fourAxis; 
    public DiceAxis fiveAxis;
    public DiceAxis sixAxis;

    private const float DOTTHRESHOLD = 0.85f;

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

    public DiceAxis GetDiceAxis(int value)
    {
        switch(value)
        {
            case 1:
                return oneAxis;
            case 2:
                return twoAxis;
            case 3:
                return threeAxis;
            case 4:
                return fourAxis;
            case 5:
                return fiveAxis;
            case 6:
                return sixAxis;
            default:
                return default;
        }
    }

    public Vector3 GetDiceAxisWorldVector(int value)
    {
        DiceAxis diceAxis = GetDiceAxis(value);

        return CalculateWorldAxisVector(diceAxis);
    }

    private Vector3 CalculateWorldAxisVector(DiceAxis diceAxis)
    {
        Vector3 diceLocalVector = diceAxis.axis;

        Vector3 axisVectorRight = new Vector3(diceLocalVector.x * transform.right.x,
            diceLocalVector.x * transform.right.y,
            diceLocalVector.x * transform.right.z);
        Vector3 axisVectorUp = new Vector3(diceLocalVector.y * transform.up.x,
            diceLocalVector.y * transform.up.y,
            diceLocalVector.y * transform.up.z);
        Vector3 axisVectorForward = new Vector3(diceLocalVector.z * transform.forward.x,
            diceLocalVector.z * transform.forward.y,
            diceLocalVector.z * transform.forward.z);

        return axisVectorRight + axisVectorUp + axisVectorForward;
    }

    private float GetAxisUpDot(DiceAxis diceAxis)
    {
        Vector3 worldAxisVector = CalculateWorldAxisVector(diceAxis);

        return Vector3.Dot(worldAxisVector, Vector3.up);

    }


}
