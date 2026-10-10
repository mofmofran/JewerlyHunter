using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          //X移動距離
    public float moveY = 2.0f;          //Y移動距離
    public float times = 3.0f;          //時間
    public float wait = 0.0f;           //停止時間
    public bool isMoveWhenOn = false;   //乗った時に動くフラグ
    public bool isCanMove = true;       //動くフラグ
    Vector3 startPos;                   //初期位置
    Vector3 endPos;                     //移動位置
    bool isReverse = false;             //反転フラグ
    float movep = 0;                    //移動補完値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;                                 //初期位置
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);  //移動位置
        if (isMoveWhenOn)//乗った時に動くフラグがオン
        {
            //乗った時に動くので最初は動かない
            isCanMove = false;
        }

        //テキストをみて完成させましょう

    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove)
        {
            //テキストをみて完成させましょう
            float distance = Vector2.Distance(startPos, endPos);//移動距離
            float ds = distance / times; //1秒の移動距離　移動距離をtimesに指定した時間で割る
            float df = ds * Time.deltaTime;//1フレームの移動距離　dfには移動距離/timesに前フレームとの差フレームを掛けた値が入る
            movep += df / distance;//移動補完値　移動進捗には距離分の1フレームの移動距離が加算されていく
            if (isReverse)//反転フラグが立ったら
            {
                transform.position = Vector2.Lerp(endPos, startPos, movep);//逆移動Vector2型のLerpメソッド引数は(スタート,ゴール,進捗)
            }
            else 
            {
                transform.position = Vector2.Lerp(startPos, endPos, movep);//正移動
            }
            if (movep >= 1.0f) //移動補完値が1以上あれば
            {
                movep = 0.0f;//移動補完値リセット
                isReverse = !isReverse;//移動を逆転
                isCanMove = false; //移動を停止
                if (isMoveWhenOn == false)
                {
                    //乗った時に動くフラグオフ
                    Invoke("Move", wait);//ウェイト分だけ遅延させてからムーブメソッドを実行
                }
            }
        }
    }

    //移動フラグを立てる
    public void Move()
    {
        //テキストをみて完成させましょう
        isCanMove = true; //移動フラグをオンにする
    }

    //移動フラグを下ろす
    public void Stop()
    {
        //テキストをみて完成させましょう
        isCanMove = false;//移動フラグをオフにする

    }

    //接触開始
    void OnCollisionEnter2D(Collision2D collision)//OncollisionEnter2Dはcollisionが乗った場合
    {
        //テキストをみて完成させましょう
        if (collision.gameObject.tag == "Player")//接触した物のタグがプレイヤーなら
        {
            //移動床の子にする
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
            {
                //乗った時に動くフラグON
                isCanMove = true; //移動フラグON
            }
        }
    }

    //接触終了
    void OnCollisionExit2D(Collision2D collision)
    {
        //テキストをみて完成させましょう
        if(collision.gameObject.tag == "Player")//接触していた物のタグがプレイヤーなら
        {
            //移動床の子から外す
            collision.transform.SetParent(null);
        }
    }

    //移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        //初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        //移動位置
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
