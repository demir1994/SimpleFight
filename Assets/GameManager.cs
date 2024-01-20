using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// Win panel
    /// </summary>
    public GameObject winPanel;

    /// <summary>
    /// Static reference
    /// </summary>
    public static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
    }

    /// <summary>
    /// OnButton click load scene
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void LoadScene_OnButton(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// If player wins
    /// </summary>
    public void WinGame()
    {
        // reset animator component to prevent background bugs
        Player.instance.animator.Rebind();

        // disable player control
        Player.instance.enabled = false;

        // disable bot control
        Bot.instance.enabled = false;

        // set up win panel
        winPanel.SetActive(true);
    }

    /// <summary>
    /// If player lose
    /// </summary>
    public void LoseGame()
    {
        // do the lose game condition
    }
}
