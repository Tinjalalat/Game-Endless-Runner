using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : MonoBehaviour
{
    public GameObject[] JalanPrefabs; // Array berisi prefab segmen jalan
    public Transform PlayerTransform;  // Referensi transform pemain

    public float SpawnZ = 0f;
    public float TileLength = 30f;
    public int NumberOfTiles = 6;
    public int SafeZone = 35;

    private List<GameObject> _activeTiles = new List<GameObject>();
    private Queue<int> _lastSpawnedIndexes = new Queue<int>();

    void Start()
    {
        for (int i = 0; i < NumberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0); // Spawn segmen jalan pertama secara lurus/aman
            else
                SpawnTile(RandomPrefabIndex());
        }
    }

    void Update()
    {
        // Ketika pemain mencapai threshold tertentu, lakukan siklus spawn-delete
        if (PlayerTransform.position.z - SafeZone > SpawnZ - (NumberOfTiles * TileLength))
        {
            SpawnTile(RandomPrefabIndex());
            DeleteTile();
        }
    }

    public void SpawnTile(int prefabIndex)
    {
        GameObject go = Instantiate(JalanPrefabs[prefabIndex], transform.forward * SpawnZ, Quaternion.identity); // Perbaikan sintaks matematika rotasi & posisi
        _activeTiles.Add(go);
        SpawnZ += TileLength;
    }

    private void DeleteTile()
    {
        Destroy(_activeTiles[0]);
        _activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (JalanPrefabs.Length <= 1) return 0;

        int randomIndex = Random.Range(0, JalanPrefabs.Length);

        // Mencegah pengulangan segmen jalan yang sama dalam 3 spawn terakhir
        while (_lastSpawnedIndexes.Contains(randomIndex))
        {
            randomIndex = Random.Range(0, JalanPrefabs.Length);
        }

        _lastSpawnedIndexes.Enqueue(randomIndex);
        if (_lastSpawnedIndexes.Count > 3)
        {
            _lastSpawnedIndexes.Dequeue();
        }

        return randomIndex;
    }
}