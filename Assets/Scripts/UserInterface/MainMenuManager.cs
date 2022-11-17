using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public Image _howToPanel;

    private bool _isShowHowToPanel = false;

    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleHowToPanel()
    {
        _isShowHowToPanel = !_isShowHowToPanel;
        _howToPanel.gameObject.SetActive(_isShowHowToPanel);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    private void Start()
    {
        _howToPanel.gameObject.SetActive(false);
    }
}
