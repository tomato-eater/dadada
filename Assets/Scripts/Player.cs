using UnityEngine;

public class Player : MonoBehaviour
{
    Matrix4x4 mat4x4 = Matrix4x4.identity;

    //�v���n�u
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject amoPrefabu;

    //�v���C���[�̎���
    [SerializeField] float maxAngle = 45.0f;

    //�G���N������
    [SerializeField] float sprin_dis = 30.0f;
    //�G���N���͈�
    [SerializeField] Vector3 spring_range = new Vector3(50, 10, 4);

    //�G�Ƃ�
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
                    target[i] = null; //Target���珜��
                }
            }
        }
    }

    private void FixedUpdate()
    {
        SetMoveAndRot();
    }

    /// <summary>
    /// �ʒu�A��]�̏������ƍX�V
    /// </summary>
    void SetMoveAndRot()
    {
        transform.position = mat4x4.GetColumn(3);
        transform.rotation = mat4x4.rotation;
    }

    /// <summary>
    /// �G�̏���
    /// </summary>
    void SummonEnemy()
    {
        Vector3 setSpringPos = transform.position + transform.forward * sprin_dis;
        Vector3[] setEnemyPos = new Vector3[24];
        for (int i = 0; i < 24; i++) 
        {
            // �G�̈ʒu�������_���ɐݒ�
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
    /// �ړ��Ɛ���
    /// </summary>
    void MoveAnoRot()
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
        
        // ���E�̉�]��K�p
        var yRot = Quaternion.AngleAxis(lrRot, Vector3.up);

        // ���݂̉�]���擾
        var nowRot = Quaternion.Euler(mat4x4.rotation.eulerAngles);

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
        Vector3 newPos = nowPos + (nextRot * move);

        // ��]�E�ʒu�𔽉f
        mat4x4 = Matrix4x4.Rotate(nextRot);
        mat4x4.SetColumn(3, new Vector4(newPos.x, newPos.y, newPos.z, 1));
    }

    /// <summary>
    /// Z�������Ă���ԁA�W�I�Ƃ���Enemy���X�V��������
    /// </summary>
    void RockON()
    {
        int count = 0;
        var playerView = mat4x4.rotation * Vector3.forward;
        for (int angle = 1; angle <= maxAngle; angle++)  //���g�̒��S���猩�Ă����@
            {
            if (count == 8) break; // Target�̐���8�ɒB������I��
            for (int i = 0; i < enemy.Length; i++)  // enemy�̐��������[�v
                {
                if (enemy[i] == null) continue;
                var PtoE = (enemy[i].transform.position - transform.position).normalized;
                var dot = Vector3.Dot(playerView, PtoE);
                if (Mathf.Cos(angle * Mathf.Deg2Rad) <= dot)  //���E�ɓ�����
                {
                    bool setTarget = true;
                    for (int rock = 0; rock < target.Length; rock++)  //Target�Ɋ��ɓo�^����Ă��Ȃ����m�F
                        if (target[rock] == enemy[i])  //���ɓo�^�ς݂̏ꍇ�͔r��
                        {
                            setTarget = false;
                            break;
                        }
                    if (setTarget && enemy[i] && count != 8)  //�����ɓo�^����Ă��Ȃ������҂́ATarget�ɓo�^
                    {
                        target[count] = enemy[i];
                        target[count]?.Target(true); //�G�̐F��ԂɕύX
                        count++;
                    }
                }
                else//���E�ɓ���Ȃ������ҒB
                {
                    enemy[i]?.Target(false); //�G�̐F�𔒂ɖ߂�
                    for (int rock = 0; rock < target.Length; rock++) //Target�ɓo�^���ꂽ�L�^�̂���ҒB���m�F
                        if (target[rock] == enemy[i])  //�L�^���������ꍇ�͏���
                            target[rock] = null;
                }
            }
        }
    }
}