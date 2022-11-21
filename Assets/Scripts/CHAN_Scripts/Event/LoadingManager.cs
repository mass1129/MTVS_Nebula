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

    // ���� �̱����������� �������� 
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
    /// �� ��ȯ �� �ε��� ��������ִ� �Լ�
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
    /// �ش� �Լ��� �� �ε� �ڷ�ƾ�Լ�, ���⿡ �ε� ��, �ε� ������ ÷���ϸ� �ȴ�. 
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
            // ����� �� ��ȯ ���� �� ���� �ε� �� ���ۺ����ϵ���

        }
    }
    /// <summary>
    /// �ε尡 ������ ������ �ߵ��Ǵ� �Լ�
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
    /// �ε��� ���۵Ǵ� ���Ŀ� �ش� �Լ��� �ߵ��ȴ�. 
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
            // FadeIn �̸� ���� ��ξ������� alpha ���� ������Ŵ
            // FadeOut �̸� ���� ��������� alpha���� ���� ��Ŵ 
            sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIN ? 0 : 1, isFadeIN ? 1 : 0, curTime);
        }
        if (!isFadeIN)
        {
            gameObject.SetActive(false);
        }
        }

}
