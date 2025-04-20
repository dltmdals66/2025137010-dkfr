using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObject : MonoBehaviour
{
    public int nextSceneIndexOffset = 1;      // 다음 씬으로 이동할 오프셋 (기본은 +1)
    public GameObject gameClearImage;         // 게임 클리어 시 띄울 UI 이미지

    public void moveTonextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + nextSceneIndexOffset;

        // 다음 씬이 존재하면 이동
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // 다음 씬이 없으면 게임 클리어
            Debug.Log("다음 씬이 없습니다. 게임 클리어!");

            if (gameClearImage != null)
            {
                gameClearImage.SetActive(true); // 클리어 이미지 보이기
                Time.timeScale = 0f;            // 게임 정지 (선택사항)
            }
        }
    }
}