using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum buildIndexes
{
    MainMenu,
}

/// <summary>
/// Scene transition manager
/// </summary>
public class SceneController : SingletonDontDestroy<SceneController>
{
    /*
     Script needs a reference to an image on a Canvas.
     Image will scale according to the using screen. Image will fade in and out between the scenes when called.
     Set off the image via the inspector.
    */
    
    [SerializeField] private Image _blackImageFader;

    private void Start()
    {
        _blackImageFader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
        _blackImageFader.gameObject.SetActive(false);
    }

    public static void LoadScene(int _buildIndex = (int)buildIndexes.MainMenu,
                                    float _faderDuration = 1f,
                                        float _transitionWaitTime = 1f)
    {
        Debug.Assert(Instance, "LoadScene method been called but Instance is null");
        if (Instance)
            Instance.StartCoroutine(Instance.FadeScene(_buildIndex, _faderDuration, _transitionWaitTime));
    }

    private IEnumerator FadeScene(int _buildIndex, float _faderDuration, float _transitionWaitTime)
    {
        _blackImageFader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / _faderDuration)
        {
            _blackImageFader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        _blackImageFader.color = new Color(0, 0, 0, 1);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_buildIndex);
        while (!asyncOperation.isDone)
            yield return null;

        yield return new WaitForSeconds(_transitionWaitTime);

        for (float t = 0; t < 1; t += Time.deltaTime / _faderDuration)
        {
            _blackImageFader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        _blackImageFader.gameObject.SetActive(false);
    }
}