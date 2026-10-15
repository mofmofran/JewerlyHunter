using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;//InputSystemを使うのに必要

public enum GameState           // ゲームの状態、enumで列挙型のクラスを作る
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
    public string stopAnime = "PlayerStop"; // 停止アニメーション
    public string moveAnime = "PlayerMove"; // 移動アニメーション
    public string jumpAnime = "PlayerJump"; // ジャンプアニメーション
    public string goalAnime = "PlayerGoal"; // ゴールアニメーション
    public string deadAnime = "PlayerOver"; // ゲームオーバーアニメーション
    string nowAnime = "";                  // 現在のアニメーション名
    string oldAnime = "";                  // 1フレーム前のアニメーション名

    // ゲームの状態（テキストは誤植なのでここは次の記述が正解）
    public static GameState gameState = GameState.InGame;

    //カメラ制御
    public float camLeft = 0.0f; // カメラの左端
    public float camRight = 0.0f; // カメラの右端
    public float camTop = 0.0f;    // カメラの上端
    public float camBottom = 0.0f; // カメラの下端

    //多重スクロール
    public GameObject subscreen;//サブスクリーンのオブジェクト

    //強制スクロール
    public bool isForceScrollx = false; // X軸の強制スクロールを有効にするかどうか
    public float forceScrollSpeedx = 0.5f; // X軸の強制スクロールの速度
    public bool isForceScrolly = false; // Y軸の強制スクロールを有効にするかどうか
    public float forceScrollSpeedy = 0.5f; // Y軸の強制スクロールの速度

    public int score = 0; // スコア

    InputAction moveAction;//Moveアクション
    InputAction jumpAction;//Jumpアクション

    //Submitアクションを受け取る
    void OnSubmit(InputValue value)//サブミットアクションを受け取る
    {
        if (gameState != GameState.InGame)//ゲーム中でなければ
        {
            GameManager gm = GameObject.FindFirstObjectByType<GameManager>();//gmにGameManagerを探して取得
            gm.GameEnd();//GameEndメソッドへ
        }
    }

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();   // Rigidbody2Dを取ってくる
        animator = this.GetComponent<Animator>();   // Animatorを取ってくる
        nowAnime = stopAnime; // 停止から開始する
        oldAnime = stopAnime; // 停止から開始する
        gameState = GameState.InGame; // ゲーム開始時はゲーム中

        PlayerInput input = GetComponent<PlayerInput>();//PlayerInputを取得
        moveAction = input.currentActionMap.FindAction("Move");//Moveアクションを取得
        jumpAction = input.currentActionMap.FindAction("Jump");//Jumpアクションを取得
        InputActionMap uiMap = input.actions.FindActionMap("UI");//UIマップを取得
        uiMap.Disable();//UIマップを無効化

    }

    void Update()
    {
        if (gameState != GameState.InGame) //ゲーム中でない場合
        {
            return; //アップデートを抜ける
        }
        //地上判定
        onGround = Physics2D.CircleCast(transform.position,//発射位置
            0.2f,//円の半径
            Vector2.down,//発射方向
            0.0f, //発射距離
            groundLayer);//検出するレイヤー

        //if (Input.GetButtonDown("Jump")) //キャラクターをジャンプさせる
        //{
        //    goJump = true; // ジャンプ開始フラグを立てる
        //}

        if (jumpAction.WasPressedThisFrame())//ジャンプキーの入力を確認
        {
            goJump = true;//ジャンプフラグを立てる
        }


        //axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする

        axisH = moveAction.ReadValue<Vector2>().x;//MoveキーのX軸を取得

        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        //アニメーション更新
        if (onGround) //地面にいる場合
        {

            if (axisH == 0.0f) //速度が0の場合
            {
                nowAnime = stopAnime; //停止アニメーション
            }
            else //速度が0でない場合
            {
                nowAnime = moveAnime; //移動アニメーション
            }
        }
        else //地面にいない場合
        {
            nowAnime = jumpAnime; //ジャンプアニメーション
        }
        if (nowAnime != oldAnime) 
        { 
            animator.Play(nowAnime);//アニメーションを再生する
            oldAnime = nowAnime;
        }




        //カメラ制御
        float x;
        float y;
        if (isForceScrollx) //X軸の強制スクロールが有効な場合
        {
            x = Camera.main.transform.position.x + (forceScrollSpeedx * Time.deltaTime); //カメラのx座標を強制スクロールさせる
        }
        else //X軸の強制スクロールが無効な場合
        {
            x = Mathf.Clamp(transform.position.x, camLeft, camRight); // カメラのx座標を制限する
        }

        if (isForceScrolly) //Y軸の強制スクロールが有効な場合
        {
            y = Camera.main.transform.position.y + (forceScrollSpeedy * Time.deltaTime); //カメラのy座標を強制スクロールさせる
        }
        else //Y軸の強制スクロールが無効な場合
        {
            y = Mathf.Clamp(transform.position.y, camBottom, camTop); // カメラのy座標を制限する
        }
        Vector3 camPos = new Vector3(x, y, -10); // カメラの位置のvector3を作る
        Camera.main.transform.position = camPos; // カメラの位置を更新する

        //サブスクリーンスクロール
        if(subscreen != null) //サブスクリーンが存在する場合
        {
            y= subscreen.transform.position.y;//サブスクリーンのy座標を取得する
            Vector3 subpos = new Vector3(x / 2.0f, y, subscreen.transform.position.z);//サブスクリーンの位置のvector3を作りX座標はカメラの位置の半分にする
            subscreen.transform.position = subpos;//サブスクリーンの位置を更新する
        }
    }

    void FixedUpdate()
    {
        if (gameState != GameState.InGame) //ゲーム中でない場合
        {
            return; //フィックスドアップデートを抜ける
        }
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
        else if (collision.gameObject.tag == "Dead" && gameState == GameState.InGame)//ゲーム中なら
        {
            GameOver();     // ゲームオーバー
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();// アイテムのスクリプトを取得
            score = item.itemdata.value; // スコア加算
            Destroy(collision.gameObject); // アイテムを消す
        }
    }
    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime); // ゴールアニメーション
        gameState = GameState.GameClear;// ゲームクリア
        GameStop(); // ゲーム停止
    }
    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime); // ゲームオーバーアニメーション

        gameState = GameState.GameOver;// ゲームオーバー
        GameStop(); // ゲーム停止
        // ゲームオーバーの処理
          GetComponent <CapsuleCollider2D>().enabled = false; // プレイヤーの当たり判定を無効化
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 少し上に跳ね上げる
    }

    // ゲーム停止
    void GameStop()//これが無いと、ゴールやゲームオーバー後にプレイヤーが動き続けてしまう
    {
        rbody.linearVelocity = new Vector2(0, 0); // 速度を0にして強制停止
        //Playerマップの無効化とUIマップの無効化
        PlayerInput input = GetComponent<PlayerInput>();//PlayerInputを取得
        input.currentActionMap.Disable();//Player無効化
        input.SwitchCurrentActionMap("UI");//アクションマップをUIに変更
        input.currentActionMap.Enable();//アクションマップになったUIを有効化
    }
}
