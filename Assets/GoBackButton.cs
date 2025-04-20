using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
    public void GoToTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
}