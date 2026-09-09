using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    [SerializeField]
    [TextArea]
    private string tooltipMessage;

    [SerializeField]
    private GameObject tooltipMenu;
    [SerializeField]
    private TMP_Text tooltipText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.text = tooltipMessage;
        tooltipMenu.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipMenu.SetActive(false);
    }

}
