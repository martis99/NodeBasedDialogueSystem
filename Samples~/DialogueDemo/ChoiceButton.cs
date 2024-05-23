using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoverColor;

    private Image buttonImage;
    private Color normalColor;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        normalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = normalColor;
    }
}
