using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loadlevel : MonoBehaviour
{
    public static Loadlevel instance;
    public GameObject loadingScreen;
    public Slider sliderCount;
    public TextMeshProUGUI progressText;

    void Awake()
    {
        instance = this;
    }

    public void LoadLevel(int sceneIndex)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadAsynchronously(sceneIndex));

    }
    void Start(){
        //progressText = GetComponent<TextMeshProUGUI>();
        progressText.SetText("Hello world");
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            sliderCount.value = progress;
           // progressText.text = progress * 100 + "%";
            progressText.SetText(progress*100+"%");
            yield return null;
        }
    }
}
