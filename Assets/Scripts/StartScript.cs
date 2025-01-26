using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void PlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
