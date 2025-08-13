using UnityEngine;

public class Player : MonoBehaviour
{
    Matrix4x4 mat4x4 = Matrix4x4.identity;

    //プレハブ
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject amoPrefabu;

    //プレイヤーの視野
    [SerializeField] float maxAngle = 45.0f;

    //敵が湧く距離
    [SerializeField] float sprin_dis = 30.0f;
    //敵が湧く範囲
    [SerializeField] Vector3 spring_range = new Vector3(50, 10, 4);

    //敵とか
    [SerializeField] Enemy[] enemy = new Enemy[24];
    Enemy[] target = new Enemy[8];

    private void Start()
    {
        Application.targetFrameRate = 60;

        SetMoveAndRot();
        SummonEnemy();
    }

    void Update()
    {
        MoveAnoRot();

        if (Input.GetKey(KeyCode.Z))
            RockON();

        if (Input.GetKeyUp(KeyCode.Z))
        {
            for (int i = 0; i < 8; i++)
            {                
                GameObject amoObj = Instantiate(amoPrefabu, mat4x4.GetColumn(3), mat4x4.rotation);
                Amo amo = amoObj.GetComponent<Amo>();
                amo.SetRotAndEnemy(i, mat4x4, target[i]);
                if (target[i] != null)
                {
                    target[i]?.Target(false);
                    target[i] = null; //Targetから除去
                }
            }
        }
    }

    private void FixedUpdate()
    {
        SetMoveAndRot();
    }

    /// <summary>
    /// 位置、回転の初期化と更新
    /// </summary>
    void SetMoveAndRot()
    {
        transform.position = mat4x4.GetColumn(3);
        transform.rotation = mat4x4.rotation;
    }

    /// <summary>
    /// 敵の召喚
    /// </summary>
    void SummonEnemy()
    {
        Vector3 setSpringPos = transform.position + transform.forward * sprin_dis;
        Vector3[] setEnemyPos = new Vector3[24];
        for (int i = 0; i < 24; i++) 
        {
            // 敵の位置をランダムに設定
            setEnemyPos[i] = setSpringPos + new Vector3(
                Mathf.Round(Random.Range(-spring_range.x * 0.5f, spring_range.x * 0.5f)),
                Mathf.Round(Random.Range(-spring_range.y * 0.5f, spring_range.y * 0.5f)),
               Mathf.Round(Random.Range(-spring_range.z * 0.5f, spring_range.z * 0.5f))
            );

            for (int j = 0; j < i; j++)
                if (i != j && setEnemyPos[j] != Vector3.zero && Vector3.Distance(setEnemyPos[i], setEnemyPos[j]) < 1.5f) 
                {
                    setEnemyPos[i] = Vector3.zero;
                    i--;
                }
        }

        for (int i = 0; i < 24; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, setEnemyPos[i], Quaternion.identity);
            enemy[i] = enemyObj.GetComponent<Enemy>();
        }
    }

    /// <summary>
    /// 移動と旋回
    /// </summary>
    void MoveAnoRot()
    {
        //回転   
        float udRot = 0f;     //上下
        if (Input.GetKey(KeyCode.UpArrow))
            udRot = -0.5f;
        if (Input.GetKey(KeyCode.DownArrow))
            udRot = 0.5f;
        var xRot = Quaternion.AngleAxis(udRot, Vector3.right);

        float lrRot = 0f;     //左右
        if (Input.GetKey(KeyCode.LeftArrow))
            lrRot = -0.5f;
        if (Input.GetKey(KeyCode.RightArrow))
            lrRot = 0.5f;
        
        // 左右の回転を適用
        var yRot = Quaternion.AngleAxis(lrRot, Vector3.up);

        // 現在の回転を取得
        var nowRot = Quaternion.Euler(mat4x4.rotation.eulerAngles);

        // 新しい回転を計算
        var nextRot = yRot * nowRot * xRot;


        //移動
        var move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move.z = 0.1f;
        if (Input.GetKey(KeyCode.S))
            move.z = -0.1f;

        // 現在位置を取得
        Vector3 nowPos = mat4x4.GetColumn(3);

        // 向いている方向に移動
        Vector3 newPos = nowPos + (nextRot * move);

        // 回転・位置を反映
        mat4x4 = Matrix4x4.Rotate(nextRot);
        mat4x4.SetColumn(3, new Vector4(newPos.x, newPos.y, newPos.z, 1));
    }

    /// <summary>
    /// Zを押している間、標的とするEnemyを更新し続ける
    /// </summary>
    void RockON()
    {
        int count = 0;
        var playerView = mat4x4.rotation * Vector3.forward;
        for (int angle = 1; angle <= maxAngle; angle++)  //自身の中心から見ていく　
            {
            if (count == 8) break; // Targetの数が8個に達したら終了
            for (int i = 0; i < enemy.Length; i++)  // enemyの数だけループ
                {
                if (enemy[i] == null) continue;
                var PtoE = (enemy[i].transform.position - transform.position).normalized;
                var dot = Vector3.Dot(playerView, PtoE);
                if (Mathf.Cos(angle * Mathf.Deg2Rad) <= dot)  //視界に入った
                {
                    bool setTarget = true;
                    for (int rock = 0; rock < target.Length; rock++)  //Targetに既に登録されていないか確認
                        if (target[rock] == enemy[i])  //既に登録済みの場合は排除
                        {
                            setTarget = false;
                            break;
                        }
                    if (setTarget && enemy[i] && count != 8)  //無事に登録されていなかった者は、Targetに登録
                    {
                        target[count] = enemy[i];
                        target[count]?.Target(true); //敵の色を赤に変更
                        count++;
                    }
                }
                else//視界に入らなかった者達
                {
                    enemy[i]?.Target(false); //敵の色を白に戻す
                    for (int rock = 0; rock < target.Length; rock++) //Targetに登録された記録のある者達を確認
                        if (target[rock] == enemy[i])  //記録があった場合は除去
                            target[rock] = null;
                }
            }
        }
    }
}