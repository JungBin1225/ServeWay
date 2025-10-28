using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladle : MonoBehaviour
{
    private int hitWall;
    private bool fireFinish;
    private MissionManager mission;

    public Vector3 start;
    public GameObject target;
    public float length;
    public float damage;
    public Sprite sprite;
    public GameObject head;
    public List<BoxCollider2D> colliders;
    public List<LineRenderer> lines;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        fireFinish = false;
        hitWall = 0;
        length = 0;

        StartCoroutine(FireLadle());
    }

    void Update()
    {
        /*if (Time.timeScale == 0) return;

        colliders[0].enabled = false;
        colliders[1].enabled = false;
        colliders[2].enabled = false;

        lines[0].enabled = false;
        lines[1].enabled = false;
        lines[2].enabled = false;

        Ray2D ray = new Ray2D(start, target.transform.position - start);
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

            if (hit2 && hitWall < 2)
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
                if(hitWall < 2)
                {
                    line.positionCount = 5;
                    Vector3 pos2 = line.GetPosition(2) + (new Vector3(reflectionDir.x, reflectionDir.y, 0) * (length - hitDir));
                    line.SetPosition(3, Vector3.Lerp(hit.point, pos2, 0.5f));
                    line.SetPosition(4, pos2);
                    SetCollider(line.GetPosition(2), line.GetPosition(4), colliders[1]);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }

            if(Vector3.Distance(hit.point, start) < ray.direction.normalized.magnitude * 0.05f && !isReflect)
            {
                if(hitWall < 2)
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
                //start += new Vector3(ray.direction.x, ray.direction.y, 0) * Time.deltaTime * 10;
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
            //start += new Vector3(ray.direction.x, ray.direction.y, 0) * Time.deltaTime * 10;
        }

        if(hitWall >= 2)
        {
            Destroy(this.gameObject);
        }
        length += Time.deltaTime * 10;*/
    }

    private IEnumerator FireLadle()
    {
        colliders[0].enabled = false;
        colliders[1].enabled = false;
        colliders[2].enabled = false;

        lines[0].enabled = false;
        lines[1].enabled = false;
        lines[2].enabled = false;

        bool ishit = false;
        int mask = 1 << LayerMask.NameToLayer("RayWall");

        for (int i = 0; i < 3; i++)
        {
            ishit = false;
            Ray2D ray = new Ray2D(start + (target.transform.position - start).normalized, target.transform.position - start);
            length = 0;

            lines[i].enabled = true;
            lines[i].positionCount = 2;
            lines[i].SetPosition(0, start);
            while (!ishit)
            {
                length += Time.deltaTime * 15;
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, length, mask);

                if (hit)
                {
                    ishit = true;
                    lines[i].SetPosition(1, hit.point);
                    SetCollider(lines[i].GetPosition(0), lines[i].GetPosition(1), colliders[i]);
                    start = hit.point;
                }
                else
                {
                    Vector3 pos = lines[i].GetPosition(0) + (new Vector3(ray.direction.x, ray.direction.y, 0) * length);
                    lines[i].SetPosition(1, pos);
                    SetCollider(lines[i].GetPosition(0), lines[i].GetPosition(1), colliders[i]);
                }
                yield return null;
            }

            hitWall++;
        }

        fireFinish = true;
        yield return new WaitForSeconds(0.5f);

        for(int i = 2; i >= 0; i--)
        {
            Vector3 dir = (lines[i].GetPosition(1) - lines[i].GetPosition(0)).normalized;
            bool isEnd = (lines[i].GetPosition(1).x - lines[i].GetPosition(0).x) > 0;

            while (isEnd == (lines[i].GetPosition(1).x - lines[i].GetPosition(0).x) > 0)
            {
                lines[i].SetPosition(1, lines[i].GetPosition(1) - dir * Time.deltaTime * 25);
                SetCollider(lines[i].GetPosition(0), lines[i].GetPosition(1), colliders[i]);

                //head.transform.position = lines[i].GetPosition(1);
                yield return null;
            }
            lines[i].enabled = false;
            colliders[i].enabled = false;
        }

        Destroy(this.gameObject);
    }

    private void SetCollider(Vector3 start, Vector3 end, BoxCollider2D collider)
    {
        collider.enabled = true;
        collider.size = new Vector2(Vector3.Distance(start, end), lines[0].startWidth * 0.25f);
        Vector3 pos = (start + end) / 2;
        Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        collider.gameObject.transform.rotation = angleAxis;
        collider.gameObject.transform.position = pos;

        Quaternion angleAxis2 = Quaternion.Euler(angleAxis.eulerAngles + new Vector3(0, 0, 180));
        head.transform.position = end;
        head.transform.rotation = angleAxis2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(sprite);
            collision.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
        }

        if(collision.tag == "Boss" && hitWall > 0 && !fireFinish)
        {
            mission.OccurreEvent(10, 1);
        }
    }

    /*private IEnumerator Fire()
    {
       
    }*/
}
