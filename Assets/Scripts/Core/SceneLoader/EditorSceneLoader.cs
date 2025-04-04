using UnityEngine.SceneManagement;

public class EditorSceneLoader : ISceneLoader
{
    public void PreloadNextScene() {}
    public void PreloadGameOver() {}
    public void PreloadTitleScreen() {}

    public void SwitchToNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        // int lastGameSceneIndex = SceneManager.sceneCountInBuildSettings - 3;
        // SceneManager.LoadScene(nextIndex > lastGameSceneIndex ? 1 : nextIndex);
        SceneManager.LoadScene(nextIndex);
    }

    public void SwitchToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    
    public void SwitchToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void SwitchToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
