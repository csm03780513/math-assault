using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class MainMenuController : MonoBehaviour
{
    public static MainMenuController mainMenuController;
    Loadlevel loadlevel;

    AudioSource audioSource;
    public AudioClip mainMenuAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Screen.fullScreen = true;
        mainMenuController = this;
        loadlevel = Loadlevel.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainMenuAudio, 0.6f);
        }
        
    }

    public void OpenGamePlayScene()
    {
        loadlevel.LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
