using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要

public class GameManager : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //テキストをみて完成させましょう

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            //テキストをみて完成させましょう


        }
        else if (PlayerController.gameState == GameState.GameOver)
        {
            //テキストをみて完成させましょう


        }
        else if (PlayerController.gameState == GameState.InGame)
        {
            
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        //テキストではハイライトされていませんがここも編集！テキストをみて完成させましょう

    }

}
