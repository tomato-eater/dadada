using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Amo : MonoBehaviour
{
    Matrix4x4 amo4x4 = Matrix4x4.identity;
    Enemy enemy;
    Vector3 fForwerd;

    public void SetRotAndEnemy(int i, Matrix4x4 mat4x4, Enemy target)
    {
        if (target != null)
            enemy = target;

        amo4x4 = mat4x4;


        Vector3 playerPos = mat4x4.GetColumn(3);
        fForwerd = playerPos + transform.forward * 30.0f;

        var nowRot = Quaternion.Euler(amo4x4.rotation.eulerAngles);

        var yRot = Quaternion.AngleAxis(90, Vector3.up);
        var xRot = Quaternion.AngleAxis(45 * i, Vector3.right);
        var nextRot = nowRot * yRot * xRot;

        Vector3 nowPos = amo4x4.GetColumn(3);

        Vector3 set = Vector3.zero; 
        Vector3 newPos = nowPos + (nextRot * set);

        amo4x4 = Matrix4x4.Rotate(nextRot);
        amo4x4.SetColumn(3, new Vector4(newPos.x, newPos.y, newPos.z, 1));

        SetMoveAndRot();
    }

    // Update is called once per frame
    void Update()
    {
        var target = (fForwerd - transform.position).normalized;
        // if (enemy != null) target = (enemy.transform.position - transform.position).normalized;
        var forwerd = transform.forward;
        var dirDot = Vector3.Dot(target, forwerd);
        var cross = Vector3.Cross(forwerd, target);
        var radian = Mathf.Min(Mathf.Acos(dirDot), 20f * Mathf.Deg2Rad);
        radian *= (cross.y / Mathf.Abs(cross.y));
        var rotMat = Matrix4x4.Rotate(Quaternion.Euler(0f, radian * Mathf.Rad2Deg, 0f));
        amo4x4 = amo4x4 * rotMat;

        Vector3 move = new Vector3(0, 0, 0.2f);
        var pos = amo4x4 * move;

        var newPos = amo4x4.GetColumn(3) + pos;
        amo4x4.SetColumn(3, newPos);

        SetMoveAndRot();
        /*
      
        
        var f = new Vector3(0, 0, 0.2f); 
        var move = amo4x4 * f;
        var pos = amo4x4.GetColumn(3) + move;
        amo4x4.SetColumn(3, pos);

        var dot = Vector3.Dot(target, forwerd);
      var liCos = Mathf.Cos(20f * Mathf.Deg2Rad);

      float x = 0;
      float y = 0;
      var cross = Vector3.Cross(forwerd, target);
      if(0.99f> dot)
      {
          var radian = Mathf.Min(Mathf.Acos(dot), 20f * Mathf.Deg2Rad);
          if (liCos <= dot)
              x = radian * (cross.x / Mathf.Abs(cross.x));

          y = radian * (cross.y / Mathf.Abs(cross.y));
          var rotMat = Matrix4x4.Rotate(Quaternion.Euler(x * Mathf.Rad2Deg, y * Mathf.Rad2Deg, 0f));
          amo4x4 = amo4x4 * rotMat;
      }

        var f = new Vector3(0, 0, 0.2f);
        var move = amo4x4 * f;

        var pos = amo4x4.GetColumn(3) + move;
        amo4x4.SetColumn(3, pos);*/


        /*
         * 
        float x = 0;
        float y = 0;
        var cross = Vector3.Cross(forwerd, target);
        Debug.Log(cross);
        if (cross.y <= cross.x)
        {
            var radian=Mathf.Min(Mathf.Acos( Vector3.Dot(target, forwerd)),);
        }
        else
        {

        }
         * 
        var forwerd = amo4x4 * new Vector3(0, 0, 1);
        var target = (fForwerd - transform.position).normalized;
        if (enemy != null)
            target = (enemy.transform.position - transform.position).normalized;
        var dot = Vector3.Dot(target, forwerd);
        var sross = Vector3.Cross(forwerd, target);
        var radian = Mathf.Min(Mathf.Acos(dot), (1f * Mathf.Deg2Rad));
        radian *= (sross.x / Mathf.Abs(sross.x));
        var x = Matrix4x4.Rotate(Quaternion.Euler(radian * Mathf.Rad2Deg, 0f, 0f));
        amo4x4 *= x;

        if (enemy != null)
        {
            var length = new Vector3(
                Mathf.Abs(enemy.transform.position.x - transform.position.x),
                Mathf.Abs(enemy.transform.position.y - transform.position.y),
                Mathf.Abs(enemy.transform.position.z - transform.position.z)
                );

            var check = (enemy.transform.localScale + transform.localScale) * 0.5f;
        }*/
    }

    /// <summary>
    /// 位置、回転の更新を行う
    /// </summary>
    void SetMoveAndRot()
    {
        transform.position = amo4x4.GetColumn(3);
        transform.rotation = amo4x4.rotation;
    }
}

/*
var target = (enemy.transform.position - transform.position).normalized;
var dirDot = Vector3.Dot(transform.forward, target);
if (0.99f > dirDot)
{
    var Radian = Mathf.Acos(Mathf.Max(dirDot, Mathf.Cos(20 * Mathf.Deg2Rad)));
    var cross = Vector3.Cross(transform.forward, target);

    var xSet = Radian * (cross.x / Mathf.Abs(cross.x));
    x = xSet * Mathf.Rad2Deg;

    var ySet = Radian * (cross.y / Mathf.Abs(cross.y));
    y = ySet * Mathf.Rad2Deg;
}

/*
var target = (enemy.transform.position - transform.position).normalized;
var dirDot = Vector3.Dot(transform.forward, target);
float radio = Mathf.Acos(Mathf.Max(dirDot, Mathf.Cos(20 * Mathf.Deg2Rad)));
cross = Vector3.Cross(transform.forward, target);
cross.x *= (cross.x / Mathf.Abs(cross.x));
cross.y *= (cross.y / Mathf.Abs(cross.y));]

 
        var rot = Matrix4x4.identity;
        var forwerd = amo4x4 * new Vector3(0, 0, 1);
        var target = (fForwerd - transform.position).normalized;
        if (enemy != null)
            target = (enemy.transform.position - transform.position).normalized;
        Debug.Log(target);
        
        var cross = Vector3.Cross(forwerd, target);
        var radian = Mathf.Min(Mathf.Acos(Vector3.Dot(target, forwerd)), 20f * Mathf.Deg2Rad);

        var y = radian * (cross.y / Mathf.Acos(cross.y));
        var x = radian * (cross.x / Mathf.Acos(cross.x));

        var rotMat = Matrix4x4.Rotate(Quaternion.Euler(x * Mathf.Rad2Deg, y * Mathf.Rad2Deg, 0f));
        amo4x4 *= rotMat;

        var vec = new Vector3(0, 0, 0.2f);
        var move = amo4x4 * vec;

        var pos = amo4x4.GetColumn(3) + move;
        amo4x4.SetColumn(3, pos);


        var nowRot = Quaternion.Euler(amo4x4.rotation.eulerAngles);
        var nextRot = nowRot;
        var dot = Vector3.Dot(forwerd, target);
        if(0.99f> dot)
        {
            float x = 0f;
            float y = 0f;
            var radian = Mathf.Min(Mathf.Acos(dot), Mathf.Acos(Mathf.Cos(20 * Mathf.Deg2Rad)));
            var cross = Vector3.Cross(forwerd, target);
            var crossAbs = new Vector3(Mathf.Abs(cross.x), Mathf.Abs(cross.y), Mathf.Abs(cross.z));
            if(crossAbs.x < crossAbs.y)
                y = radian * (cross.y / Mathf.Abs(cross.y));
            else
                x = radian * (cross.x / Mathf.Abs(cross.x));
            var yRot = Quaternion.AngleAxis(y * Mathf.Rad2Deg, Vector3.up);
            var xRot = Quaternion.AngleAxis(x * Mathf.Rad2Deg, Vector3.right);
            nextRot = nowRot * yRot * xRot;
        }

        Vector3 nowPos = amo4x4.GetColumn(3);
        Vector3 move = new Vector3(0, 0, 0.2f);
        var newPos= nowPos + (nextRot * move);

        amo4x4 = Matrix4x4.Rotate(nextRot);
        amo4x4.SetColumn(3, new Vector4(newPos.x, newPos.y, newPos.z, 1));
 */

