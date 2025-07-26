using UnityEngine;

public class Player : MonoBehaviour
{
    Matrix4x4 mat4x4 = Matrix4x4.identity;

    // Update is called once per frame
    void Update()
    {
        //à⁄ìÆ
        //ëOå„
        Vector3 vec = new Vector3 { };
        if (Input.GetKey(KeyCode.W))
            vec.z = 0.2f;
        if (Input.GetKey(KeyCode.S))
            vec.z = -0.2f;

        Matrix4x4 traMat4x4 = Matrix4x4.Translate(vec);

        //âÒì]
        //è„â∫
        float tu = 0f;
        if(Input.GetKey(KeyCode.UpArrow))
            tu = 0.2f;
        if (Input.GetKey(KeyCode.DownArrow))
            tu = -0.2f;
        //ç∂âE
        float lr = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
            lr = -0.2f;
        if (Input.GetKey(KeyCode.RightArrow))
            lr = 0.2f;

        var q = Quaternion.AngleAxis(0, transform.forward);

        //Matrix4x4 rotMat4x4 = Matrix4x4.Rotate(Quaternion.Euler(tu, lr, 0));

        //mat4x4 *= (traMat4x4 * rotMat4x4);
    }

    private void FixedUpdate()
    {
        transform.position = mat4x4.GetColumn(3);
        transform.rotation = mat4x4.rotation;
    }
}
