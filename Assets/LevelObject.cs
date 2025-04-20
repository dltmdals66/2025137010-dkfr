using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObject : MonoBehaviour
{
    public int nextSceneIndexOffset = 1;      // ���� ������ �̵��� ������ (�⺻�� +1)
    public GameObject gameClearImage;         // ���� Ŭ���� �� ��� UI �̹���

    public void moveTonextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + nextSceneIndexOffset;

        // ���� ���� �����ϸ� �̵�
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // ���� ���� ������ ���� Ŭ����
            Debug.Log("���� ���� �����ϴ�. ���� Ŭ����!");

            if (gameClearImage != null)
            {
                gameClearImage.SetActive(true); // Ŭ���� �̹��� ���̱�
                Time.timeScale = 0f;            // ���� ���� (���û���)
            }
        }
    }
}