using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool GameOver;
    public GameObject GameOverPanel;
    public GameObject WinPanel; // Panel untuk kondisi menang
    public GameObject HTPButton;
    public GameObject HUD;
    public ScoreSystem ScoreSystem;
    public HealthManager HealthManager; // Referensi ke HealthManager

    // Variabel untuk StartGame
    public static bool IsGameStarted;
    public GameObject StartingText;

    public PlayerControl playerControl;
    public Animator PlayerAnimator;

    [SerializeField] private CutsceneManager cutsceneManager;
    private bool hasWon = false;

    void Start()
    {
        GameOver = false;
        Time.timeScale = 1; // untuk replay
        IsGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Perbaikan typo: 'If' jadi 'if', '!+=' jadi '!=', kapitalisasi ScoreSystem
        if (ScoreSystem != null && ScoreSystem.HasPlayerWon() && !hasWon)
        {
            hasWon = true; // Set flag agar tidak berulang
            StartCoroutine(WinSequence());
        }

        // Cek game over hanya jika belum menang
        if (GameOver && !hasWon)
        {
            // Perbaikan typo: 'Startcoroutine' jadi 'StartCoroutine'
            StartCoroutine(GameOverSequence());
        }

        // Detect Enter key to start the game
        // Perbaikan typo: 'Keycode' jadi 'KeyCode'
        if (Input.GetKeyDown(KeyCode.Return) && !IsGameStarted)
        {
            playerControl.enabled = true;
            // Perbaikan typo: 'IsGameStart' jadi 'IsGameStarted'
            IsGameStarted = true;
            Destroy(StartingText);
        }
    }

    IEnumerator GameOverSequence()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        if (LeaderboardManager.Instance != null)
        {
            int finalScore = Mathf.RoundToInt(ScoreSystem.Score);
            float finalDistance = playerControl.DistanceTraveled;
            float finalHealth = HealthManager.CurrentHealth;
            LeaderboardManager.Instance.AddEntry(playerName, finalScore, finalDistance, finalHealth);
            Debug.Log("Entry leaderboardd disimpan: Nama=" + playerName +
                      ", Score=" + finalScore +
                      ", Distance=" + finalDistance +
                      ", Health=" + finalHealth);
        }

        PlayerAnimator.SetTrigger("Die");
        // Perbaikan typo: 'retrn' jadi 'return'
        yield return new WaitForSecondsRealtime(4f);

        Time.timeScale = 0;
        HUD.SetActive(false);
        GameOverPanel.SetActive(true);
        if (ScoreSystem != null)
        {
            // Perbaikan typo: 'ScOreSystem' jadi 'ScoreSystem'
            ScoreSystem.DisplayGameOverScore();
        }
    }

    IEnumerator WinSequence()
    {
        // Perbaikan typo: 'cutceneManager' jadi 'cutsceneManager'
        if (cutsceneManager != null)
        {
            cutsceneManager.PlayCutscene();
        }

        // Perbaikan typo: 'yied' jadi 'yield'
        yield return null;
    }

    public void ShowWinPanel()
    {

        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        if (LeaderboardManager.Instance != null)
        {
           int finalScore = Mathf.RoundToInt(ScoreSystem.Score);
           float finalDistance = playerControl.DistanceTraveled;
           float finalHealth = HealthManager.CurrentHealth;
           LeaderboardManager.Instance.AddEntry(playerName, finalScore, finalDistance, finalHealth);
           Debug.Log("Entry leaderboardd disimpan: Nama=" + playerName +
                     ", Score=" + finalScore +
                     ", Distance=" + finalDistance +
                     ", Health=" + finalHealth);
        }

    
        HUD.SetActive(false);
        // Perbaikan typo: 'WinPANel' jadi 'WinPanel'
        WinPanel.SetActive(true);
        // Perbaikan typo: 'faLse' jadi 'false'
        GameOverPanel.SetActive(false); // Pastikan GameOverPanel tidak aktif

        // Perbaikan typo: 'ScoreSistem' jadi 'ScoreSystem'
        if (ScoreSystem != null)
        {
            ScoreSystem.DisplayGameOverScore();
        }

        Time.timeScale = 0; // Pause game setelah menampilkan panel
    }
}