using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthImage;
    public TextMeshProUGUI healthText;
    public float healthChangeDuration = 0.6f;
    public Ease healthChangeEase = Ease.OutBounce;

    private int startingHealth;

    //last saved health in this health bar
    private int currentHealthVal;

    public void Initialize(int _startingHealth)
    {
        startingHealth = _startingHealth;
        currentHealthVal = startingHealth;

        if (healthText)
        {
            healthText.text = currentHealthVal.ToString();
        }
    }

    public void UpdateHealthValue(int value)
    {
        //value is different, we need to update
        if (value != currentHealthVal)
        {
            currentHealthVal = value;

            //update bar
            float percentage = (float)currentHealthVal / (float)startingHealth;
            healthImage.DOFillAmount(percentage, healthChangeDuration).SetEase(healthChangeEase);

            //update text
            healthText.text = currentHealthVal.ToString();
        }
    }
}
