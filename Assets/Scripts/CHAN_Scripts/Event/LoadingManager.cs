using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadingManager : MonoBehaviour
{
    string loadSceneName;
    [SerializeField]
    CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField]
    GameObject lodingBar;

    // 먼저 싱글톤패턴으로 구현하자 
    public static LoadingManager instance;
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 씬 전환 시 로딩을 실행시켜주는 함수
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = sceneName;
        StartCoroutine(Load(sceneName));
    }
    /// <summary>
    /// 해당 함수는 씬 로딩 코루틴함수, 여기에 로딩 바, 로딩 아이콘 첨가하면 된다. 
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator Load(string sceneName)
    {
        yield return StartCoroutine(Fade(false));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        float curTime = 0;

        while (!op.isDone)
        {
            yield return null;
            // 여기는 씬 전환 끝날 때 까지 로딩 고리 빙글빙글하도록

        }
    }
    /// <summary>
    /// 로드가 끝나는 시점에 발동되는 함수
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }
    /// <summary>
    /// 로딩이 시작되는 직후에 해당 함수가 발동된다. 
    /// </summary>
    /// <param name="isFadeIN"></param>
    /// <returns></returns>
    IEnumerator Fade(bool isFadeIN)
    {
        float curTime = 0;
        while (curTime < 1)
        {
            yield return null;
            curTime += Time.unscaledDeltaTime*2f;
            // FadeIn 이면 점점 어두어지도록 alpha 값을 증가시킴
            // FadeOut 이면 점점 밝아지도록 alpha값을 감소 시킴 
            sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIN ? 0 : 1, isFadeIN ? 1 : 0, curTime);
        }
        if (!isFadeIN)
        {
            gameObject.SetActive(false);
        }
        }

}
