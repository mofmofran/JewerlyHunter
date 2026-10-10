using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 移動速度
    public bool isToRight = false;      // true=右向き　false=左向き
    public float revTime = 0;           // 反転時間
    public LayerMask groundLayer;       // 地面レイヤー
    bool onGround = false;              // 地面フラグ
    float time = 0;
    public Transform groundCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);// 向きの変更
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
                                                               // 足元に地面があるか調べる
        bool isGround = Physics2D.Raycast(groundCheck.position,
            Vector2.down,
            0.1f,
            groundLayer);
        if (!isGround)
        {
            isToRight = !isToRight;//フラグを反転させる
            time = 0;//タイマーを初期化
            if (isToRight)
            {
                transform.localScale = new Vector2(-1, 1);//向きの初期化
            }
            else
            {
                transform.localScale = new Vector2(1, 1);//向きの初期化
            }
        }
        if (revTime > 0)
        {
            //テキストをみて完成させましょう
            time += Time.deltaTime;//デルタタイムを加算
            if (time >= revTime)
            {
                isToRight = !isToRight;//フラグを反転させる
                time = 0;//タイマーを初期化
                if (isToRight)
                {
                    transform.localScale = new Vector2(-1, 1);//向きの初期化
                }
                else
                {
                    transform.localScale = new Vector2(1, 1);//向きの初期化
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (onGround)//地面にいるとき
        {
            //テキストをみて完成させましょう
            //速度を更新する
            //RB2Dを取ってくる
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (isToRight)
            {
                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocity.y);//正方向へ移動させる
            }
            else
            {
                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocity.y);//逆方向に移動させる
            }
        }
    }

    // なにかに接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
            //テキストをみて完成させましょう
            isToRight = !isToRight;//フラグ反転
            time = 0;//タイマー初期化
            if (isToRight)
            {
                transform.localScale = new Vector2(-1, 1);//向きの変更
            }
            else
            {
                transform.localScale = new Vector2(1, 1);//向きの変更
            }
    }
}

