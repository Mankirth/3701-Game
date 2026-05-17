using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ToggleText : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private GameObject disabledText;

    async void Start()
    {
        await Awaitable.WaitForSecondsAsync(0.5f, CancellationToken.None);
        disabledText.SetActive(!toggle.isOn);
    }


    public void ToggleTextChange()
    {
        disabledText.SetActive(!toggle.isOn);
    }
}
