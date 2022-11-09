using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Color startColor, endColor;
    private Color colorChanger, baseColorChanger;
    private bool hasCalculated=false;

    public bool HasCalculated { get => hasCalculated; set => hasCalculated = value; }

    // Start is called before the first frame update
    private void Start()
    {
        mat.color = startColor;

        baseColorChanger = endColor - startColor;
        colorChanger = baseColorChanger;
    }

    public void SetupColorChanger(float stamina)
    {
        colorChanger = baseColorChanger / stamina;
    }

    public void ChangeToEndColor(float staminaValue)
    {
        mat.color = mat.color + colorChanger * staminaValue;
    }

    public void ChangeToStartColor(float staminaValue)
    {
        mat.color = mat.color - colorChanger * staminaValue;
    }
}