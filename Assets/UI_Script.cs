using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayButton()
    {
        SceneManager.LoadScene("");
    }
    void ExitButton()
    {
        Application.Quit();
    }
}
