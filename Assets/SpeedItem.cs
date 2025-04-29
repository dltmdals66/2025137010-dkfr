using UnityEngine;
using TMPro;

public class ScoreTest : MonoBehaviour
{
    public TextMeshProUGUI stage1;
    public TextMeshProUGUI stage2;

    void Start()
    {
        stage1.text = "STAGE 1 : " + HighScore.Load(1).ToString();
        stage2.text = "STAGE 2 : " + HighScore.Load(2).ToString();
    }
}