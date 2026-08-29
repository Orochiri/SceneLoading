using System.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int startingScene = 1;

    public void StartGame()
    {
        SceneLoader.Instance.StartGame(startingScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
