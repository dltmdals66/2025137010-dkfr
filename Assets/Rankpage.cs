using System.Linq;
using UnityEngine;
using TMPro;

public class RankPage : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;    // Content가 붙을 부모 오브젝트
    [SerializeField] private GameObject rowPrefab;     // 하나의 랭크 행을 표시할 프리팹
    [SerializeField] private TMP_FontAsset headerFont; // (선택) 헤더용 폰트

    private StageResultList allData;

    private void Awake()
    {
        allData = StageResultSaver.LoadInternal();
        RefreshRankList();
    }

    private void RefreshRankList()
    {
        // (0) 기존에 생성된 자식들 모두 삭제
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        // (1) 1부터 5까지 스테이지 루프
        for (int stage = 1; stage <= 5; stage++)
        {
            // (2) 스테이지 헤더 생성
            var headerGO = new GameObject($"Stage{stage}_Header", typeof(RectTransform));
            headerGO.transform.SetParent(contentRoot, false);
            var headerText = headerGO.AddComponent<TextMeshProUGUI>();
            headerText.text = $"─── Stage {stage} ───";
            headerText.fontSize = 24;
            headerText.alignment = TextAlignmentOptions.Center;
            if (headerFont != null) headerText.font = headerFont;

            // (3) 해당 스테이지 데이터만 필터 & 내림차순 정렬
            var sortedData = allData.results
                                    .Where(r => r.stage == stage)
                                    .OrderByDescending(r => r.score)
                                    .ToList();

            // (4) 정렬된 데이터 만큼 rowPrefab으로 행 생성
            for (int i = 0; i < sortedData.Count; i++)
            {
                var row = Instantiate(rowPrefab, contentRoot);
                var rankText = row.GetComponentInChildren<TMP_Text>();
                rankText.text = $"{i + 1}. {sortedData[i].playerName} — {sortedData[i].score}";
            }

            // (5) 만약 해당 스테이지 기록이 없으면 “기록 없음” 표시
            if (sortedData.Count == 0)
            {
                var emptyGO = new GameObject($"Stage{stage}_Empty", typeof(RectTransform));
                emptyGO.transform.SetParent(contentRoot, false);
                var emptyText = emptyGO.AddComponent<TextMeshProUGUI>();
                emptyText.text = "  (기록 없음)";
                emptyText.fontSize = 18;
                emptyText.alignment = TextAlignmentOptions.Center;
                if (headerFont != null) emptyText.font = headerFont;
            }
        }
    }
}