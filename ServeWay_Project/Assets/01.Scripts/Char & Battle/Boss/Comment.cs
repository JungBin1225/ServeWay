using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 start;
    private bool isWall;

    public GameObject box;
    public BoxCollider2D collider;
    public float damage;
    public Sprite sprite;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        start = transform.position;
        isWall = false;
        line.SetPosition(0, new Vector3(0, 0, 0));
        line.SetPosition(1, new Vector3(0, 0, 0));

        StartCoroutine(FireComment());
    }

    void Update()
    {
        
    }

    private IEnumerator FireComment()
    {
        collider.enabled = false;

        yield return new WaitForSeconds(0.5f);

        collider.enabled = true;
        float length = 0.1f;
        while (!isWall || Vector3.Distance(line.GetPosition(0), line.GetPosition(1)) > 0.5f)
        {
            Ray2D ray = new Ray2D(start, -transform.up);
            line.SetPosition(0, start);
            line.SetPosition(1, start);
            int mask = 1 << LayerMask.NameToLayer("RayWall");
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, length, mask);

            if(hit)
            {
                isWall = true;
                line.SetPosition(1, hit.point);
                start += new Vector3(ray.direction.x, ray.direction.y, 0) * Time.deltaTime * 15;
            }
            else
            {
                Vector3 pos = line.GetPosition(0) + (new Vector3(ray.direction.x, ray.direction.y, 0) * length);
                line.SetPosition(1, pos);

                length += Time.deltaTime * 15;
            }

            collider.size = new Vector2(Vector3.Distance(line.GetPosition(0), line.GetPosition(1)), line.startWidth);
            Vector3 boxPos = (line.GetPosition(0) + line.GetPosition(1)) / 2;
            Vector2 dir = new Vector2(boxPos.x - line.GetPosition(1).x, boxPos.y - line.GetPosition(1).y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            box.transform.rotation = angleAxis;
            box.transform.position = boxPos;

            yield return null;
        }

        line.enabled = false;

        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(sprite);
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
            FindObjectOfType<BloggerController>().PlayerCommentDamage();
        }
    }
}
