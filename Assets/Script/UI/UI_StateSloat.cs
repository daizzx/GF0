
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StateSloat : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{



    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.OpenPanel("TipPanel");

       

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.ClosePanel("TipPanel");
    }

}
