using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; // 削除する時間指定

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime); // 削除設定　第二引数に入れられた変数秒後に作動する
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject); // 何かに接触したら消す
    }
}
