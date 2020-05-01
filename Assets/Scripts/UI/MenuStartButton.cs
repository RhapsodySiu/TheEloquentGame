using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartButton : MonoBehaviour
{

    public string enterScene;
    public void OnClicked()
    {
        SceneManager.LoadScene(enterScene);
    }
}
