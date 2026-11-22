using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform entryContainer;    // Content dari ScrollRect
    public GameObject entryPrefab;      // Prefab dengan 1 Image + 5 TextMeshPro
    public Sprite[] rankIcons;          // 5 icon (index 0 = rank1, index1 = rank2, ...)

    private void Start()
    {
        RefreshLeaderboardDisplay();
    }

    public void RefreshLeaderboardDisplay()
    {
        // Hapus semua entry lama
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }

        List<LeaderboardEntry> entries = LeaderboardManager.Instance.GetEntries();

        if (entries.Count == 0)
        {
            GameObject emptyObj = Instantiate(entryPrefab, entryContainer);
            TMP_Text[] texts = emptyObj.GetComponentsInChildren<TMP_Text>();
            if (texts.Length > 0)
            {
                texts[0].text = "Belum ada data";
            }
            return;
        }

        for (int i = 0; i < entries.Count; i++)
        {
            LeaderboardEntry entry = entries[i];
            GameObject newEntry = Instantiate(entryPrefab, entryContainer);
            TMP_Text[] texts = newEntry.GetComponentsInChildren<TMP_Text>();

            // Isi teks (asumsi urutan: Nama, Score, Distance, Health, Date)
            if (texts.Length >= 5)
            {
                texts[0].text = entry.playerName;
                texts[1].text = entry.score.ToString();
                texts[2].text = entry.distance.ToString("F1") + " m";
                texts[3].text = entry.health.ToString("F1");
                texts[4].text = entry.date;
            }
            else
            {
                Debug.LogWarning("Leaderboard entry prefab needs at least 5 TextMeshPro children.");
            }

            // Ganti icon berdasarkan peringkat (i+1)
            SetRankIcon(newEntry, i + 1);
        }
    }

    private void SetRankIcon(GameObject entry, int rank)
    {
        // Cari komponen Image pada prefab (hanya satu)
        Image img = entry.GetComponentInChildren<Image>();
        if (img == null)
        {
            Debug.LogWarning("Entry prefab does not have an Image component.");
            return;
        }

        // Pastikan rankIcon tersedia (rank mulai dari 1)
        if (rankIcons != null && rank >= 1 && rank <= rankIcons.Length)
        {
            img.sprite = rankIcons[rank - 1];
            img.gameObject.SetActive(true);
        }
        else
        {
            // Jika tidak ada icon untuk rank tersebut, sembunyikan image
            img.gameObject.SetActive(false);
        }
    }
}