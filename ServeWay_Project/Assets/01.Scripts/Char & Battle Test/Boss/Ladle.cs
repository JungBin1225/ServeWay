using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladle : MonoBehaviour
{
    private LineRenderer line;
    private int hitWall;
    private bool isReflect;

    public Vector3 start;
    public Vector3 target;
    public float length;
    public float damage;
    public Sprite sprite;
    public List<BoxCollider2D> colliders;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        hitWall = 0;
        isReflect = false;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        colliders[0].enabled = false;
        colliders[1].enabled = false;
        colliders[2].enabled = false;

        Ray2D ray = new Ray2D(start, target);
        line.SetPosition(0, start);
        int mask = 1 << LayerMask.NameToLayer("RayWall");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, length, mask);
        if (hit)
        {
            line.positionCount = 3;
            line.SetPosition(1, Vector3.Lerp(start, hit.point, 0.5f));
            line.SetPosition(2, hit.point);
            SetCollider(line.GetPosition(0), line.GetPosition(2), colliders[0]);

            var inDirection = (hit.point - (Vector2)start).normalized;
            var reflectionDir = Vector2.Reflect(inDirection, hit.normal);

            var hitDir = Vector3.Distance(line.GetPosition(0), line.GetPosition(2));
            RaycastHit2D hit2 = Physics2D.Raycast(hit.point + (reflectionDir * 0.001f), reflectionDir, length - hitDir, mask);

            if (hit2 && hitWall < 3)
            {
                line.positionCount = 7;
                line.SetPosition(3, Vector3.Lerp(hit.point, hit2.point, 0.5f));
                line.SetPosition(4, hit2.point);
                SetCollider(line.GetPosition(2), line.GetPosition(4), colliders[1]);

                var inDir = (hit2.point - hit.point).normalized;
                var reflect = Vector2.Reflect(inDir, hit2.normal);

                var hitDir2 = Vector3.Distance(line.GetPosition(2), line.GetPosition(4));

                Vector3 pos3 = line.GetPosition(4) + (new Vector3(reflect.x, reflect.y, 0) * (length - hitDir - hitDir2));
                line.SetPosition(5, Vector3.Lerp(hit2.point, pos3, 0.5f));
                line.SetPosition(6, pos3);
                SetCollider(line.GetPosition(4), line.GetPosition(6), colliders[2]);
            }
            else
            {
                if(hitWall < 3)
                {
                    line.positionCount = 5;
                    Vector3 pos2 = line.GetPosition(2) + (new Vector3(reflectionDir.x, reflectionDir.y, 0) * (length - hitDir));
                    line.SetPosition(3, Vector3.Lerp(hit.point, pos2, 0.5f));
                    line.SetPosition(4, pos2);
                    SetCollider(line.GetPosition(2), line.GetPosition(4), colliders[1]);
                }
            }

            if(Vector3.Distance(hit.point, start) < ray.direction.normalized.magnitude * 0.05f && !isReflect)
            {
                if(hitWall < 3)
                {
                    isReflect = true;
                    start = line.GetPosition(2);
                    target = new Vector3(reflectionDir.x, reflectionDir.y, 0);
                    hitWall += 1;
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                isReflect = false;
                start += new Vector3(ray.direction.x, ray.direction.y, 0) * 0.05f;
            }
        }
        else
        {
            line.positionCount = 3;
            Vector3 pos1 = line.GetPosition(0) + (new Vector3(ray.direction.x, ray.direction.y, 0) * length);
            line.SetPosition(1, Vector3.Lerp(start, pos1, 0.5f));
            line.SetPosition(2, pos1);
            SetCollider(line.GetPosition(0), line.GetPosition(2), colliders[0]);

            isReflect = false;
            start += new Vector3(ray.direction.x, ray.direction.y, 0) * 0.05f;
        }
    }

    private void SetCollider(Vector3 start, Vector3 end, BoxCollider2D collider)
    {
        collider.enabled = true;
        collider.size = new Vector2(Vector3.Distance(start, end), line.startWidth);
        Vector3 pos = (start + end) / 2;
        Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        collider.gameObject.transform.rotation = angleAxis;
        collider.gameObject.transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprite);
        }
    }

    /*private IEnumerator Fire()
    {
       
    }*/
}
