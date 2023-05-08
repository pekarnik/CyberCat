using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("DemoScene");
    }
}
