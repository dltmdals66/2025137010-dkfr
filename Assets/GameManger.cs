using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;      // �÷��̾� �̸� �Է� �ʵ�
    public Button gameStartButton;         // ���� ���� ��ư

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� ������ ���
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    private void OnGameStartButtonClicked()
    {
        // �Էµ� �÷��̾� �̸� ��������
        string playerName = inputField.text;

        // �̸��� ��� ������ ��� �α� ��� �� ����
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("�÷��̾� �̸��� �Է��ϼ���.");
            return;
        }

        // PlayerPrefs�� �̸� ����
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log("�÷��̾� �̸� ���� ��: " + playerName);

        // ù ��° ���� ������ �ε�
        SceneManager.LoadScene("Level_1");
    }
}
