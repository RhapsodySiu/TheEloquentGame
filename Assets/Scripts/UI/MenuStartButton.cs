using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartButton : MonoBehaviour
{

    public string enterScene;
    public AudioSource onClickedSound;
    private Animation animation;

    private void Start()
    {
        animation = GetComponentInParent<Animation>();
        if (animation == null)
        {
            Debug.LogError("Cannot find animation");
        }
    }
    public void OnClicked()
    {
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        Debug.Log("Play animation");
        onClickedSound.Play();
        animation.Play();
        while (animation.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(enterScene);
    }
}
