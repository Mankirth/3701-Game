using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchGradeMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TMP_Text perfectCount, goodCount, missCount, dodgeCount;

    [SerializeField]
    private GameManager gameManager;


    public void ShowGrades()
    {
        perfectCount.text = gameManager.perfectTotalCount.ToString();
        goodCount.text = gameManager.goodTotalCount.ToString();
        missCount.text = gameManager.missTotalCount.ToString();
        dodgeCount.text = gameManager.dodgeTotalCount.ToString();
    }
}
