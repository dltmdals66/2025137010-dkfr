
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemSO data;    // Inspector �巡��

    public int GetPoint()
    {
        return data.point;           // ItemSO�� point �� ��ȯ
    }
}
