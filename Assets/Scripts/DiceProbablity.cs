using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice Probability", menuName = "Dice/Probablity")]
public class DiceProbablity : ScriptableObject
{
    public AnimationCurve oneWeightCurve = new AnimationCurve();
    public AnimationCurve twoWeightCurve = new AnimationCurve();
    public AnimationCurve threeWeightCurve = new AnimationCurve();
    public AnimationCurve fourWeightCurve = new AnimationCurve();
    public AnimationCurve fiveWeightCurve = new AnimationCurve();
    public AnimationCurve sixWeightCurve = new AnimationCurve();

    private Dictionary<int, int> diceWeightPairs;

    private float lastCharacterMass = Mathf.Infinity;

    private const int MASSTOWEIGHTRATIO = 100;

    //public void Initialize(float characterMass)
    //{
    //    if (diceWeightPairs == null)
    //    {
    //        diceWeightPairs = new Dictionary<int, int>();

    //        diceWeightPairs.Add(1, GetWeightFromMass(1, characterMass));
    //        diceWeightPairs.Add(2, GetWeightFromMass(2, characterMass));
    //        diceWeightPairs.Add(3, GetWeightFromMass(3, characterMass));
    //        diceWeightPairs.Add(4, GetWeightFromMass(4, characterMass));
    //        diceWeightPairs.Add(5, GetWeightFromMass(5, characterMass));
    //        diceWeightPairs.Add(6, GetWeightFromMass(6, characterMass));
    //    }
    //}

    public int PickWeightedDiceRoll(float characterMass)
    {
        if (lastCharacterMass != characterMass)
        {
            //weights need to be recalculated
            CalculateWeights(characterMass);
        }

        //actully run weight calculation
        int randomWeight = Random.Range(0, GetSumOfWeights());
        for (int i = 1; i <= 6; ++i)
        {
            randomWeight -= diceWeightPairs[i];
            if (randomWeight < 0)
            {
                return i;
            }
        }

        Debug.LogError("No Weight Found for mass " + characterMass);
        return int.MinValue;
    }

    private void CalculateWeights(float characterMass)
    {
        if (diceWeightPairs == null)
        {
            diceWeightPairs = new Dictionary<int, int>();
        }

        for (int i = 1; i <= 6; i++)
        {
            if (!diceWeightPairs.ContainsKey(i))
            {
                diceWeightPairs.Add(i, GetWeightFromMass(i, characterMass));
            }
            else
            {
                diceWeightPairs[i] = GetWeightFromMass(i, characterMass);
            }
        }
        
    }

    private int GetSumOfWeights()
    {
        int sum = 0;
        for (int i = 1; i <= 6; i++)
        {
            sum += diceWeightPairs[i];
        }

        return sum;
    }

    private int GetWeightFromMass(int dieNum, float characterMass)
    {
        if (Mathf.Abs(characterMass) > 1.0f)
        {
            Debug.Log("Character Mass is too large/small");
            return int.MinValue;
        }

        switch (dieNum)
        {
            case 1:
                return Mathf.RoundToInt(oneWeightCurve.Evaluate(characterMass) * MASSTOWEIGHTRATIO);
            case 2:
                return Mathf.RoundToInt(twoWeightCurve.Evaluate(characterMass) * MASSTOWEIGHTRATIO);
            case 3:
                return Mathf.RoundToInt(threeWeightCurve.Evaluate(characterMass) * MASSTOWEIGHTRATIO);
            case 4:
                return Mathf.RoundToInt(fourWeightCurve.Evaluate(characterMass) * MASSTOWEIGHTRATIO);
            case 5:
                return Mathf.RoundToInt(fiveWeightCurve.Evaluate(characterMass) * MASSTOWEIGHTRATIO);
            case 6:
                return Mathf.RoundToInt(sixWeightCurve.Evaluate(characterMass) * MASSTOWEIGHTRATIO);
            default:
                Debug.LogError("Invaid Die number: " + dieNum);
                return int.MinValue;
        }
    }
}
