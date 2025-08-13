using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Enemy : MonoBehaviour
{
    MeshRenderer mesh;

    Vector3 setPos;
    Vector3 speed;
    Vector3 dir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.white;

        setPos = transform.position;
        Vector3 newPos = new Vector3(
            setPos.x + Random.Range(-2.5f, 2.5f),
            setPos.y + Random.Range(-2.5f, 2.5f),
            setPos.z + Random.Range(-2.5f, 2.5f)
        );
        transform.position = newPos;

        speed = new Vector3(
            Mathf.Ceil(Random.Range(3f, 5f)),
            Mathf.Ceil(Random.Range(3f, 5f)),
            Mathf.Ceil(Random.Range(3f, 5f))
        );

        dir = new Vector3(
            Mathf.Sign(Mathf.Round(Random.Range(-1f, 0f))),
            Mathf.Sign(Mathf.Round(Random.Range(-1f, 0f))),
            Mathf.Sign(Mathf.Round(Random.Range(-1f, 0f)))
            );
    }

    // Update is called once per frame
    void Update()
    {
        var x = Quaternion.AngleAxis(dir.x * 360 / speed.x * Time.deltaTime, Vector3.up);
        var y = Quaternion.AngleAxis(dir.y*360 / speed.y*Time.deltaTime , Vector3.forward);
        var z = Quaternion.AngleAxis(dir.z * 360 / speed.z * Time.deltaTime, Vector3.right);

        var move = z * y * x;

        var vec = transform.position - setPos;
        vec = move * vec;
        transform.position = setPos + vec;
    }

    public void Target(bool rock)
    {
        mesh.material.color = rock ? Color.red : Color.white;
    }
}
