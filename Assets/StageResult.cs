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
    private const string PLAYER_NAME = "PlayerName"; // PlayerPrefs 키
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, FILE);

    public static void SaveStage(int stage, int score)
    {
        // 기존 결과 불러오기
        StageResultList list = LoadInternal();

        // 플레이어 이름 가져오기
        string playerName = PlayerPrefs.GetString(PLAYER_NAME, "");

        // 새 기록 추가
        StageResult entry = new StageResult
        {
            playerName = playerName,
            stage = stage,
            score = score
        };
        list.results.Add(entry);

        // JSON으로 저장
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(filePath, json);
    }

    public static StageResultList LoadInternal()
    {
        if (!File.Exists(filePath))
            return new StageResultList();  // 파일 없으면 새 리스트

        string json = File.ReadAllText(filePath);
        StageResultList list = JsonUtility.FromJson<StageResultList>(json);
        return list ?? new StageResultList();
    }
}
