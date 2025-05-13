using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class StageResult
{
    public string playerName;
    public int stage;
    public int score;
}

[Serializable]
public class StageResultList
{
    public List<StageResult> results = new List<StageResult>();
}

public static class StageResultSaver
{
    private const string FILE = "stage_results.json";
    private const string PLAYER_NAME = "PlayerName"; // PlayerPrefs Ű
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, FILE);

    public static void SaveStage(int stage, int score)
    {
        // ���� ��� �ҷ�����
        StageResultList list = LoadInternal();

        // �÷��̾� �̸� ��������
        string playerName = PlayerPrefs.GetString(PLAYER_NAME, "");

        // �� ��� �߰�
        StageResult entry = new StageResult
        {
            playerName = playerName,
            stage = stage,
            score = score
        };
        list.results.Add(entry);

        // JSON���� ����
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(filePath, json);
    }

    public static StageResultList LoadInternal()
    {
        if (!File.Exists(filePath))
            return new StageResultList();  // ���� ������ �� ����Ʈ

        string json = File.ReadAllText(filePath);
        StageResultList list = JsonUtility.FromJson<StageResultList>(json);
        return list ?? new StageResultList();
    }
}
