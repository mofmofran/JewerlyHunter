using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock;
    public Sprite imageOff;
    public Sprite imageOn;
    public bool on = false; // スイッチの状態(true:押されている false:押されていない)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (on)//onならば
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;//オンの画像を取ってくる
        }
        else//違うならば
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;//オフの画像をとってくる
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")//接触した物がプレイヤーなら
        {
            if (on)//オンの場合
            {
                on = false;//オフにする
                GetComponent<SpriteRenderer>().sprite = imageOff;//オフの画像にする
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();//targetMoveBlockのMovingBlockを取得する
                movBlock.Stop();//そのブロックのStopメソッドを使う
            }
            else//違う場合
            {
                on = true;//オンにする
                GetComponent<SpriteRenderer>().sprite = imageOn;//オンの画像にする
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();//targetMoveBlockのMovingBlockを取得する
                movBlock.Move();//そのブロックのMoveメソッドを使う
            }
        }
    }
}
