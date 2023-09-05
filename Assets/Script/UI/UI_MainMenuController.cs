using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("OrderScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
