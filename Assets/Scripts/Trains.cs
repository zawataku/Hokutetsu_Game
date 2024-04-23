using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 列挙型 TRAINS_TYPE の定義
public enum TRAINS_TYPE
{
    _BUS = 1,
    _2200 = 2,
    _3750 = 3,
    _6000 = 4,
    _ED20 = 5,
    _7100 = 6,
    _7700 = 7,
    _8800 = 8,
    _03 = 9,
}

// 列車を表すクラス Trains の定義
public class Trains : MonoBehaviour
{
    public TRAINS_TYPE TrainsType;  // 列車のタイプ
    private static int Trains_Serial = 0;  // 列車のシリアル番号
    private int My_Serial;  // 列車の個々のシリアル番号
    public bool isDestroyed = false;  // 列車が破壊されたかどうかを示すフラグ

    [SerializeField] private Trains nextTrainsPrefab;  // 次の列車のプレハブ
    [SerializeField] private int score;

    public static UnityEvent<int> OnScoreAdded = new UnityEvent<int>();

    // 列車が生成されたときの初期化
    private void Awake()
    {
        My_Serial = Trains_Serial;  // シリアル番号の割り当て
        Trains_Serial++;  // 次のシリアル番号の準備
    }

    // 列車同士が衝突したときの処理
    private void OnCollisionEnter2D(Collision2D other)
    {
        // 列車がすでに破壊されている場合は処理をしない
        if (isDestroyed)
        {
            return;
        }

        // 衝突したオブジェクトが Trains コンポーネントを持っている場合
        if (other.gameObject.TryGetComponent(out Trains otherTrains))
        {
            // 衝突した列車が同じタイプの場合
            if (otherTrains.TrainsType == TrainsType)
            {
                // 次の列車プレハブが設定されている場合
                if (nextTrainsPrefab != null)
                {
                    OnScoreAdded.Invoke(score);

                    // 自分と衝突した列車を破壊
                    isDestroyed = true;
                    otherTrains.isDestroyed = true;
                    Destroy(gameObject);
                    Destroy(other.gameObject);

                    // 生成する列車の位置と回転の計算
                    Vector3 center = (transform.position + other.transform.position) / 2;
                    Quaternion rotation = Quaternion.Lerp(transform.rotation, other.transform.rotation, 0.5f);

                    // 次の列車の生成
                    Trains next = Instantiate(nextTrainsPrefab, center, rotation);
                    next.score = score; // スコアを設定

                    // 生成した列車に速度を与える
                    Rigidbody2D nextRb = next.GetComponent<Rigidbody2D>();
                    Vector3 velocity = (GetComponent<Rigidbody2D>().velocity + other.gameObject.GetComponent<Rigidbody2D>().velocity) / 2;
                    nextRb.velocity = velocity;

                    // 生成した列車に角速度を与える
                    float angularVelocity = (GetComponent<Rigidbody2D>().angularVelocity + other.gameObject.GetComponent<Rigidbody2D>().angularVelocity) / 2;
                    nextRb.angularVelocity = angularVelocity;
                }
                // 次の列車プレハブが null の場合は何もしない
            }
        }
        // 衝突した列車が異なるタイプの場合
        else
        {
            // 何もせずに終了
            return;
        }

    }
}