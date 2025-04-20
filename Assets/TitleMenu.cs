using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level_1"); // 게임 시작 시 로드할 씬 이름
    }

    public void OpenControls()
    {
        SceneManager.LoadScene("Controls"); // 조작법 씬 로드
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료
#endif
    }
}