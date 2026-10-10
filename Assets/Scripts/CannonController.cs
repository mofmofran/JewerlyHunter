using UnityEngine;
using UnityEngine.UIElements;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;            //発生させるPrefabデータ
    public float delayTime = 3.0f;          //遅延時間
    public float fireSpeed = 4.0f;          //発射速度
    public float length = 8.0f;             //範囲

    GameObject player;                      //プレイヤー
    Transform gateTransform;                //発射口のTransform
    float passedTimes = 0;                  //経過時間
                                            
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //発射口オブジェクトのTransformを取得
        gateTransform = transform.Find("gate");
        //プレイヤーを取得
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //待機時間加算
        passedTimes += Time.deltaTime;
        //Playerとの距離チェック
        if (CheckLength(player.transform.position))//bool型の自作メソッド
        {
            //待機時間経過
            if (passedTimes > delayTime)
            {
                //テキストをみて完成させましょう
                passedTimes = 0; //時間を0にリセット
                //砲弾をプレハブから作る
                Vector2 pos = new Vector2(gateTransform.position.x,gateTransform.position.y);//砲弾の初期位置gateを取得する
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);//Instantiateで指定したオブジェクトを生成する(対象オブジェクト,場所Vector2でも3でも可,角度クォタリオン、アイデンティティはそのまま)
                //砲身が向いている方に発射する
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();//生成したobjのRB2Dを取得
                float angleZ = transform.localEulerAngles.z;//Z軸の回転をオイラー角で取得
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);//角度が分かってる時、長辺を1として底辺を割り出すCos。Mathf.Deg2Radでラジアン角になる。Deg2がオイラー角。
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);//角度が分かってる時、長辺を1として高さを割り出すSin。Mathf.Deg2Radでラジアン角になる。Deg2がオイラー角。
                //オイラー角がUNITYで使われていない。ラジアン角が使われる。
                Vector2 v = new Vector2(x, y) * fireSpeed;//弾速を掛けたものをvとする
                rbody.AddForce(v, ForceMode2D.Impulse);//vの方向と力でAddForceのImpulse！

            }
        }
    }
    //距離チェック
    bool CheckLength(Vector2 targetPos)//targetPosにはプレイヤーの位置
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);//自分からターゲットまでの距離を取得
        if (length >= d)//距離が設定されたlengthより小さくなったら
        {
            ret = true;//オン
        }
        return ret;//trueかfalseを返している
    }

    //範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);//ギズモに円形で距離を表示
    }
}
