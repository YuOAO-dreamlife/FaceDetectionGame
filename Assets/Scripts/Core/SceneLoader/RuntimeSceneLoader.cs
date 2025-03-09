using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeSceneLoader : ISceneLoader
{
    AsyncOperation operation;

    public void PreloadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int lastGameSceneIndex = SceneManager.sceneCountInBuildSettings - 2;
        operation = SceneManager.LoadSceneAsync(nextIndex > lastGameSceneIndex ? 1 : nextIndex);
        operation.allowSceneActivation = false;
    }

    public void PreloadGameOver()
    {
        operation = SceneManager.LoadSceneAsync("GameOver");
    }

    public void PreloadTitleScreen()
    {
        operation = SceneManager.LoadSceneAsync("TitleScreen");
    }

    public void SwitchToNextScene()
    {
        operation.allowSceneActivation = true;
    }

    public void SwitchToGameOver()
    {
        operation.allowSceneActivation = true;
    }

    public void SwitchToTitleScreen()
    {
        operation.allowSceneActivation = true;
    }
}
