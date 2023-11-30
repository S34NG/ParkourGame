using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OptionPanel()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
