using UnityEngine;
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using UnityEngine.InputSystem;//InputSystemを使うのに必要

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名
    //public InputAction submitAction;//Input型の入力情報変数
    //private void OnEnable()
    //{
    //    submitAction.Enable();//InputActionを有効化
    //}
    //private void OnDisable()
    //{
    //    submitAction.Disable();//InputActionを無効化
    //}

    
    //ON+[アクション名]でメソッドを定義する
    void OnSubmit(InputValue value)
    {
        Load();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //Keyboard kb = Keyboard.current;//キーボードの有無
        //if (kb != null)//キーボードがあったら
        //{
        //    if(kb.enterKey.wasPressedThisFrame)//エンターキーがおされたら
        //    {
        //        Load();
        //    }
        //}

        //if (submitAction.WasPressedThisFrame())//このフレームに特定の何かが押されたら
        //{
        //    Load();
        //}
    }

    // シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
