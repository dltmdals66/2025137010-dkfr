using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level_1"); // ���� ���� �� �ε��� �� �̸�
    }

    public void OpenControls()
    {
        SceneManager.LoadScene("Controls"); // ���۹� �� �ε�
    }

    public void QuitGame()
    {
        Debug.Log("���� ����");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �����Ϳ��� ����
#endif
    }
}