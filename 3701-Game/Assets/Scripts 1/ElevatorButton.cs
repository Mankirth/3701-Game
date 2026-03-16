using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightObject;
    
    public void OnHover()
    {
        highlightObject.SetActive(true);
    }

    public void ExitHover()
    {
        highlightObject.SetActive(false);
    }
}
