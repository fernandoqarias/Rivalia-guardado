using UnityEngine;
using UnityEngine.SceneManagement;

public class UIVictoryActions : MonoBehaviour
{
    public void ResetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game (solo funciona buildeado)");
    }
}
