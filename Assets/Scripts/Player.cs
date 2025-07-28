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
        //��]   
        float udRot = 0f;     //�㉺
        if (Input.GetKey(KeyCode.UpArrow))
            udRot = -0.5f;
        if (Input.GetKey(KeyCode.DownArrow))
            udRot = 0.5f;
        var xRot = Quaternion.AngleAxis(udRot, Vector3.right);

        float lrRot = 0f;     //���E
        if (Input.GetKey(KeyCode.LeftArrow))
            lrRot = -0.5f;   
        if (Input.GetKey(KeyCode.RightArrow))
            lrRot = 0.5f;

        // ���݂̉�]���擾
        var nowRot = Quaternion.Euler(mat4x4.rotation.eulerAngles);

        // ���E�̉�]��K�p
        var yRot = Quaternion.AngleAxis(lrRot, Vector3.up);

        // �V������]���v�Z
        var nextRot = yRot * nowRot * xRot;


        //�ړ�
        var move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move.z = 0.1f;
        if (Input.GetKey(KeyCode.S))
            move.z = -0.1f;

        // ���݈ʒu���擾
        Vector3 nowPos = mat4x4.GetColumn(3);

        // �����Ă�������Ɉړ�
        Vector3 worldMove = nextRot * move;
        Vector3 newPos = nowPos + worldMove;

        // ��]�E�ʒu�𔽉f
        mat4x4 = Matrix4x4.Rotate(nextRot);
        mat4x4.SetColumn(3, new Vector4(newPos.x, newPos.y, newPos.z, 1));
    }

    private void FixedUpdate()
    {
        transform.position = mat4x4.GetColumn(3);
        transform.rotation = mat4x4.rotation;
    }
}