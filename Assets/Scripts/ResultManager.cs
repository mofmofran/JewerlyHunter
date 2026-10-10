using UnityEngine;
using UnityEngine.SceneManagement; //シーンの切り替えに必要
using TMPro;//TextMeshProの使用に必要

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;//スコア表示用テキストの参照
    public string sceneName;//シーン名
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = GameManager.totalScore.ToString();//ゲームマネージャーのトータルスコアをスコアテキストのテキストに代入
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //シーンを読み込む
    public void Load() 
    {
        SceneManager.LoadScene(sceneName);//シーン名のシーンを読み込む
    }
}
