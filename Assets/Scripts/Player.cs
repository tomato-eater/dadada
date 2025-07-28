using UnityEngine;

public class Player : MonoBehaviour
{
    Matrix4x4 mat4x4 = Matrix4x4.identity;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
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

        // 現在の回転を取得
        var nowRot = Quaternion.Euler(mat4x4.rotation.eulerAngles);

        // 左右の回転を適用
        var yRot = Quaternion.AngleAxis(lrRot, Vector3.up);

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
        Vector3 worldMove = nextRot * move;
        Vector3 newPos = nowPos + worldMove;

        // 回転・位置を反映
        mat4x4 = Matrix4x4.Rotate(nextRot);
        mat4x4.SetColumn(3, new Vector4(newPos.x, newPos.y, newPos.z, 1));
    }

    private void FixedUpdate()
    {
        transform.position = mat4x4.GetColumn(3);
        transform.rotation = mat4x4.rotation;
    }
}