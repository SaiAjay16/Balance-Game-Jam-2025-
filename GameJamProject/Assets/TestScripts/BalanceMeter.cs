using UnityEngine;
using UnityEngine.UI;

public class BalanceMeter : MonoBehaviour
{
    public Slider balanceSlider;
    public Image fillImage;

    public Color goodColor = Color.green;
    public Color neutralColor = Color.yellow;
    public Color badColor = Color.red;

    public float changeAmount = 10f; // How much you "intend" to change per press
    public float smoothSpeed = 5f; // How fast it actually moves

    private float targetValue; // Desired value we want to move toward

    private void Start()
    {
        targetValue = balanceSlider.value; // Start at the current slider value
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetValue += changeAmount;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            targetValue -= changeAmount;
        }

        // Clamp targetValue between min and max
        targetValue = Mathf.Clamp(targetValue, balanceSlider.minValue, balanceSlider.maxValue);

        // Smoothly move balanceSlider.value toward targetValue
        balanceSlider.value = Mathf.Lerp(balanceSlider.value, targetValue, Time.deltaTime * smoothSpeed);

        UpdateFillColor();
    }

    private void UpdateFillColor()
    {
        float t = balanceSlider.value / balanceSlider.maxValue;

        // Red to Yellow (Bad to Neutral)
        if (t < 0.5f)
        {
            fillImage.color = Color.Lerp(badColor, neutralColor, t * 2f);
        }
        // Yellow to Green (Neutral to Good)
        else
        {
            fillImage.color = Color.Lerp(neutralColor, goodColor, (t - 0.5f) * 2f);
        }
    }

}
