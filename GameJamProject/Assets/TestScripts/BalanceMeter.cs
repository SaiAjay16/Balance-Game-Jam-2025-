using UnityEngine;
using UnityEngine.UI;

public class BalanceMeter : MonoBehaviour
{
    [Header("References")]
    public RectTransform marker;
    public RectTransform fillArea;

    public RectTransform leftFillRect;
    public RectTransform rightFillRect;

    public Image leftFillImage;
    public Image rightFillImage;

    [Header("Colors")]
    public Color goodColor = new Color(0f, 0.8f, 0f);             // Bright Green
    public Color semiGoodColor = new Color(0.5f, 1f, 0.5f);       // Light Green
    public Color neutralColor = new Color(1f, 1f, 0.5f);          // Light Yellow
    public Color semiBadColor = new Color(1f, 0.5f, 0f);          // Orange
    public Color badColor = new Color(0.803f, 0.11f, 0.094f);     // #CD1C18 custom red

    [Header("Settings")]
    public float markerMoveSpeed = 5f;
    public float colorLerpSpeed = 5f;
    private int currentStep = 0;
    private readonly int maxStep = 2; // Maximum left/right steps

    private Vector2 targetMarkerPos;

    private void Start()
    {
        SetMarkerPosition(true); // Snap instantly at start
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentStep < maxStep)
            {
                currentStep++;
                SetMarkerPosition();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentStep > -maxStep)
            {
                currentStep--;
                SetMarkerPosition();
            }
        }

        marker.anchoredPosition = Vector2.Lerp(marker.anchoredPosition, targetMarkerPos, Time.deltaTime * markerMoveSpeed);

        UpdateFillColors();
    }

    private void SetMarkerPosition(bool instant = false)
    {
        float width = fillArea.rect.width;
        float stepWidth = width / (maxStep * 2);

        targetMarkerPos = new Vector2(stepWidth * currentStep, 0f);

        if (instant)
        {
            marker.anchoredPosition = targetMarkerPos;
        }

        SetFillSizes();
    }

    private void SetFillSizes()
    {
        float fullWidth = fillArea.rect.width;
        float halfWidth = fullWidth / 2f;
        float stepWidth = fullWidth / (maxStep * 2);

        float markerPositionX = halfWidth + (currentStep * stepWidth);

        // LeftFill stretches from left to marker
        leftFillRect.sizeDelta = new Vector2(markerPositionX, leftFillRect.sizeDelta.y);

        // RightFill stretches from marker to right
        rightFillRect.sizeDelta = new Vector2(fullWidth - markerPositionX, rightFillRect.sizeDelta.y);
    }

    private void UpdateFillColors()
    {
        Color leftTargetColor = neutralColor;
        Color rightTargetColor = neutralColor;

        if (currentStep == 2)
        {
            leftTargetColor = goodColor;
            rightTargetColor = goodColor;
        }
        else if (currentStep == 1)
        {
            leftTargetColor = neutralColor;
            rightTargetColor = semiGoodColor;
        }
        else if (currentStep == 0)
        {
            leftTargetColor = neutralColor;
            rightTargetColor = neutralColor;
        }
        else if (currentStep == -1)
        {
            leftTargetColor = semiBadColor;
            rightTargetColor = neutralColor;
        }
        else if (currentStep == -2)
        {
            leftTargetColor = badColor;
            rightTargetColor = badColor;
        }

        leftFillImage.color = Color.Lerp(leftFillImage.color, leftTargetColor, Time.deltaTime * colorLerpSpeed);
        rightFillImage.color = Color.Lerp(rightFillImage.color, rightTargetColor, Time.deltaTime * colorLerpSpeed);
    }
}
