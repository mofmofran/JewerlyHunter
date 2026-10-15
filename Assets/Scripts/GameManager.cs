using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要
using UnityEngine.SceneManagement; // シーンマネージャーを使うのに必要
using TMPro;
using UnityEngine.InputSystem;                        // TextMeshProを使うのに必要

public class GameManager : MonoBehaviour
{
    public GameObject mainImage; // 画像を持つゲームオブジェクト
    public Sprite gameOverspr; //ゲームオーバー画像
    public Sprite gameClearspr; //ゲームクリア画像
    public GameObject panel;//パネル
    public GameObject restartButton;//リスタートボタン
    public GameObject nextButton;//ネクストボタン
    Image titleImage;//画像を表示するImageコンポーネント
    GameState gameState = GameState.InGame; //ゲームの状態                    
    public string nextSceneName; //次のシーン名を入れる変数
                                 //時間制限
    public GameObject timeBar;//時間表示イメージ
    public GameObject timeText;//時間テキスト
    TimeController timeCnt;//TimeControllerスクリプトを入れる変数

    //スコア追加
    public GameObject scoreText;//スコアテキスト
    public static int totalScore; //合計スコアを入れる変数
    public int stageScore; //ステージスコアを入れる変数


    public AudioClip meGameClear;//ゲームクリアSE
    public AudioClip meGameOver;//ゲームオーバーSE

    //ゲーム終了
    public void GameEnd()
    {
        if (gameState == GameState.GameClear)//ゲームクリアなら
        {
            Next();//次へ
        }
        else if (gameState == GameState.GameOver)//ゲームオーバーなら
        {
            Restart();//リスタートへ
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //テキストをみて完成させましょう
        Invoke("InactiveImage", 1.0f); // 1秒後に画像を非表示にする
        panel.SetActive(false); //パネルを非表示にする
        //時間制限追加
        timeCnt = GetComponent<TimeController>(); //TimeControllerスクリプトを取得する
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)//時間制限がない場合
            {
               timeBar.SetActive(false); //時間表示イメージを非表示にする
            }
        }
        //スコア追加
        UpdateScore(); //スコア表示を更新する
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            //テキストをみて完成させましょう
            //ゲームクリア
            gameState = GameState.GameClear;
            mainImage.SetActive(true); // 画像を表示する
            panel.SetActive(true); //パネルを表示する
                                   //リスタートボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            //restartButton.GetComponent<Button>().interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearspr; //ゲームクリア画像を表示する
            PlayerController.gameState = GameState.GameEnd; //ゲームの状態をゲームクリアにする

            //時間制限追加
            if (timeCnt != null)//時間制限がある場合
            {
                timeCnt.isTimeOver = true; //時間制限を止める
                //スコア追加
                //整数に代入することで少数を切り捨てる
                int time = (int)timeCnt.displayTime; //残り時間を整数にする
                totalScore += time * 10; //残り時間に10をかけてスコアにする
            }
            //スコア追加
            totalScore += stageScore; //ステージスコアを合計スコアに加える
            stageScore = 0; //ステージスコアを0にする
            UpdateScore(); //スコア表示を更新する
            AudioSource soundPlayer = GetComponent<AudioSource>();//AudioSourceを取得
            soundPlayer.Stop();//BGM停止
            soundPlayer.PlayOneShot(meGameClear);//ゲームクリア音を鳴らすメソッド
        }
        else if (PlayerController.gameState == GameState.GameOver)
        {
            //テキストをみて完成させましょう
            //ゲームオーバー
            gameState = GameState.GameOver;
            mainImage.SetActive(true); // 画像を表示する
            panel.SetActive(true); //パネルを表示する
                                   //ネクストボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverspr; //ゲームオーバー画像を表示する
            PlayerController.gameState = GameState.GameEnd; //ゲームの状態をゲームオーバーにする

            //時間制限追加
            if (timeCnt != null)//時間制限がある場合
            {
                timeCnt.isTimeOver = true; //時間制限を止める
            }

            AudioSource soundPlayer = GetComponent<AudioSource>();//AudioSourceを取得
            soundPlayer.Stop();//BGM停止
            soundPlayer.PlayOneShot(meGameOver);//ゲームオーバー音を鳴らすメソッド
        }


        else if (PlayerController.gameState == GameState.InGame)
        {
            //ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player"); //Playerタグのゲームオブジェクトを取得する
            //PlayerControllerスクリプトを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            //時間制限追加
            //タイム更新
            if (timeCnt != null)//時間制限がある場合
            {
                if (timeCnt.gameTime > 0.0f)//時間制限がある場合
                {
                    //整数に代入することで少数を切り捨てる
                    int time = (int)timeCnt.displayTime; //残り時間を整数にする
                    //タイム更新
                    timeText.GetComponent<TextMeshProUGUI>().text = time.ToString(); //残り時間を表示する
                    //タイムオーバー
                    if (time <= 0)
                    {
                        playerCnt.GameOver(); //ゲームの状態をゲームオーバーにする
                    }
                    //スコア追加
                    if(playerCnt.score != 0) 
                    { 
                        stageScore += playerCnt.score;
                        playerCnt.score = 0;
                        UpdateScore(); 
                    }
                }

            }

            
        }

    }
            // 画像を非表示にする
            void InactiveImage()
            {
                //テキストではハイライトされていませんがここも編集！テキストをみて完成させましょう
                mainImage.SetActive(false); // 画像を非表示にする
            }
    //スコア表示を更新する
    void UpdateScore()
    {
        int score = stageScore+totalScore; //スコアを計算する
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();//スコアを表示する
    }
    public void Restart()
    {
        //ゲームをリスタートする
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //現在のシーンを引数に入れ再読み込みする
    }
    public void Next()
    {
        //次のシーンに移動する
        SceneManager.LoadScene(nextSceneName); //次のシーンを読み込む
    }

}