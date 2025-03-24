public interface ISceneLoader
{
    void PreloadNextScene();
    void PreloadGameOver();
    void PreloadTitleScreen();
    void SwitchToNextScene();
    void SwitchToGameOver();
    void SwitchToTitleScreen();
    void SwitchToCredits();
}
