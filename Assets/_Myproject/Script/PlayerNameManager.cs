using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject mainMenuPanel;      // Panel utama Main Menu
    public GameObject nameInputPanel;     // Panel popup input nama
    public TMP_InputField nameInputField; // Input field untuk nama
    public Button saveNameButton;         // Tombol simpan

    private string defaultName = "Player";

    private void Start()
    {
        // Cek apakah nama sudah tersimpan di PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerName") && !string.IsNullOrEmpty(GetPlayerName()))
        {
            // Nama sudah ada -> tampilkan Main Menu, sembunyikan panel input
            ShowMainMenu();
        }
        else
        {
            // Nama belum ada -> tampilkan panel input, sembunyikan Main Menu
            ShowNameInput();
        }

        // Pasang listener tombol save
        if (saveNameButton != null)
        {
            saveNameButton.onClick.AddListener(SaveName);
        }
    }

    public void SaveName()
    {
        string newName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(newName))
        {
            newName = defaultName;
        }

        PlayerPrefs.SetString("PlayerName", newName);

        // Setelah menyimpan, tampilkan Main Menu dan sembunyikan panel input
        ShowMainMenu();

        Debug.Log("Player name saved: " + newName);
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (nameInputPanel != null) nameInputPanel.SetActive(false);
    }

    public void ShowNameInput()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (nameInputPanel != null) nameInputPanel.SetActive(true);
    }

    public string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName", defaultName);
    }
}