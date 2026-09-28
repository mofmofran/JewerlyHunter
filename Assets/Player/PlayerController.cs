using UnityEngine;

public enum GameState           // ゲームの状態
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数
    float axisH = 0.0f;             // 入力
    public float speed = 3.0f;      // 移動速度   
    public float jump = 9.0f;       // ジャンプ力
    public LayerMask groundLayer;  // 着地出来るレイヤー
    bool goJump = false;                   // ジャンプ開始フラグ
    bool onGround = false;                  // 地面フラグ
    //アニメーション対応
    Animator animator;                // アニメーター

    // ゲームの状態（テキストは誤植なのでここは次の記述が正解）
    public static GameState gameState = GameState.InGame;

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();   // Rigidbody2Dを取ってくる
       
    }

    void Update()
    {
        //地上判定
        onGround = Physics2D.CircleCast(transform.position,//発射位置
            0.2f,//円の半径
            Vector2.down,//発射方向
            0.0f, //発射距離
            groundLayer);//検出するレイヤー

        if (Input.GetButtonDown("Jump")) //キャラクターをジャンプさせる
        {
            goJump = true; // ジャンプ開始フラグを立てる
        }


        axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする


        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

           
    }

    void FixedUpdate()
    {
        
        if (onGround || axisH != 0.0f) //地面にいるor速度が0でない場合
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }

        if (onGround && goJump) //地面の上でジャンプキーが押された
        {
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0.0f, jump); //ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); //瞬発的な力を加える
            goJump = false; //ジャンプ開始フラグを下ろす
        }


    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
    }
    // ゴール
    public void Goal()
    {
        
    }
    // ゲームオーバー
    public void GameOver()
    {
        
        gameState = GameState.GameOver;
    }

    // ゲーム停止
    void GameStop()
    {

    }
}
