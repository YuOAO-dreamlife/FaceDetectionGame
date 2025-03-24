using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeSceneLoader : ISceneLoader
{
    private int _currentNextGameSceneIndex;
    AsyncOperation _preloadNextScene;
    AsyncOperation _preloadGameOver;
    AsyncOperation _preloadTitleScreen;

    public void PreloadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int lastGameSceneIndex = SceneManager.sceneCountInBuildSettings - 3;
        _currentNextGameSceneIndex = nextIndex > lastGameSceneIndex ? 1 : nextIndex;
        _preloadNextScene = SceneManager.LoadSceneAsync(_currentNextGameSceneIndex);
        _preloadNextScene.allowSceneActivation = false;
    }

    public void PreloadGameOver()
    {
        _preloadGameOver = SceneManager.LoadSceneAsync("GameOver");
        _preloadGameOver.allowSceneActivation = false;
    }

    public void PreloadTitleScreen()
    {
        _preloadTitleScreen = SceneManager.LoadSceneAsync("TitleScreen");
        _preloadTitleScreen.allowSceneActivation = false;
    }

    public void SwitchToNextScene()
    {
        _preloadNextScene.allowSceneActivation = true;
    }

    public void SwitchToGameOver()
    {
        _preloadGameOver.allowSceneActivation = true;
    }

    public void SwitchToTitleScreen()
    {
        _preloadTitleScreen.allowSceneActivation = true;
    }

    public void SwitchToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
