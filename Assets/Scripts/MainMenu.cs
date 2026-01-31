using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animatorTitle;
    [SerializeField] private Animator animatorHelp;
    [SerializeField] private Animator animatorNewGame;
    [SerializeField] private Animator animatorSettings;
    [SerializeField] private Animator animatorLeaderboard;
    [SerializeField] private Animator animatorCredits;
    [SerializeField] private LevelLoader loader;

    [Header("Variables")]
    private float directionTitle = -1f;
    private float directionNewGame = -1f;
    private float directionSettings = -1f;
    private float directionLeaderboard = -1f;
    private float directionCredits = -1f;

    public void Help()
    {
        directionTitle *= -1f;
        animatorTitle.SetFloat("Speed", directionTitle);
        animatorHelp.SetFloat("Speed", directionTitle);

        float startTime = directionTitle < 0 ? 1f : 0f;

        animatorTitle.Play("TitleRight", 0, startTime);
        animatorHelp.Play("HelpRight", 0, startTime);
    }

    public void NewGame()
    {
        directionNewGame *= -1f;
        animatorTitle.SetFloat("Speed", directionNewGame);
        animatorNewGame.SetFloat("Speed", directionNewGame);

        float startTime = directionNewGame < 0 ? 1f : 0f;

        animatorTitle.Play("TitleRight", 0, startTime);
        animatorNewGame.Play("NewGameRight", 0, startTime);
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

    public void Credits()
    {
        directionCredits *= -1f;
        animatorTitle.SetFloat("Speed", directionCredits);
        animatorCredits.SetFloat("Speed", directionCredits);

        float startTime = directionCredits < 0 ? 1f : 0f;

        animatorTitle.Play("TitleBottom", 0, startTime);
        animatorCredits.Play("CreditsTop", 0, startTime);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
