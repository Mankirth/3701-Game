using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoiceObject : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public GameObject dialogueObject;

    public void SetText(string text1, string text2)
    {
        button1.GetComponent<TMP_Text>().text = text1;
        button2.GetComponent<TMP_Text>().text = text2;
    }

    public void SelectOption1()
    {

    }

    public void SelectOption2()
    {

        {


        }
    }
}
