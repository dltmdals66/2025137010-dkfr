// 파일 이름이 ScoreDisplay.cs일 경우
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI stage1Text;
    public TextMeshProUGUI stage2Text;
    public TextMeshProUGUI stage3Text;
    public TextMeshProUGUI stage4Text;
    public TextMeshProUGUI stage5Text;

    void Start()
    {
        stage1Text.text = "STAGE 1 : " + HighScore.Load(1).ToString();
        stage2Text.text = "STAGE 2 : " + HighScore.Load(2).ToString();
        stage3Text.text = "STAGE 3 : " + HighScore.Load(3).ToString();
        stage4Text.text = "STAGE 4 : " + HighScore.Load(4).ToString();
        stage5Text.text = "STAGE 5 : " + HighScore.Load(5).ToString();
    }
}