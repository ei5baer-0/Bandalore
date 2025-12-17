using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator animatorTitle;
    public Animator animatorNewGame;
    public Animator animatorContinue;
    public Animator animatorSettings;
    public Animator animatorLeaderboard;

    [SerializeField] private LevelLoader loader;

    private float directionTitle = -1f;
    private float directionNewGame = -1f;
    private float directionSettings = -1f;
    private float directionLeaderboard = -1f;

    public void NewGame()
    {
        directionTitle *= -1f;
        animatorTitle.SetFloat("Speed", directionTitle);
        animatorNewGame.SetFloat("Speed", directionTitle);

        float startTime = directionTitle < 0 ? 1f : 0f;

        animatorTitle.Play("TitleRight", 0, startTime);
        animatorNewGame.Play("NewGameRight", 0, startTime);
    }

    public void Continue()
    {
        directionNewGame *= -1f;
        animatorNewGame.SetFloat("Speed", directionNewGame);
        animatorContinue.SetFloat("Speed", directionNewGame);

        float startTime = directionNewGame < 0 ? 1f : 0f;

        animatorNewGame.Play("NewGameLeft", 0, startTime);
        animatorContinue.Play("ContinueRight", 0, startTime);
    }

    public void ToScene(string sceneName)
    {
        loader.LevelTransition(sceneName);
    }

    public void Settings()
    {
        directionSettings *= -1f;
        animatorTitle.SetFloat("Speed", directionSettings);
        animatorSettings.SetFloat("Speed", directionSettings);

        float startTime = directionSettings < 0 ? 1f : 0f;

        animatorTitle.Play("TitleLeft", 0, startTime);
        animatorSettings.Play("SettingsLeft", 0, startTime);
    }

    public void Leaderboard()
    {
        directionLeaderboard *= -1f;
        animatorTitle.SetFloat("Speed", directionLeaderboard);
        animatorLeaderboard.SetFloat("Speed", directionLeaderboard);

        float startTime = directionLeaderboard < 0 ? 1f : 0f;

        animatorTitle.Play("TitleLeft", 0, startTime);
        animatorLeaderboard.Play("LeaderboardLeft", 0, startTime);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Stop");
    }
}
