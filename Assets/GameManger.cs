using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;      // 플레이어 이름 입력 필드
    public Button gameStartButton;         // 게임 시작 버튼

    private void Start()
    {
        // 버튼 클릭 이벤트에 리스너 등록
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    private void OnGameStartButtonClicked()
    {
        // 입력된 플레이어 이름 가져오기
        string playerName = inputField.text;

        // 이름이 비어 있으면 경고 로그 출력 후 종료
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("플레이어 이름을 입력하세요.");
            return;
        }

        // PlayerPrefs에 이름 저장
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log("플레이어 이름 저장 됨: " + playerName);

        // 첫 번째 레벨 씬으로 로드
        SceneManager.LoadScene("Level_1");
    }
}
