using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherLaser : MonoBehaviour
{
    private LineRenderer line;
    private bool isMoving;

    public GameObject laserPrefab;
    public GameObject weaponObject;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        weaponObject.SetActive(false);
        isMoving = true;
    }

    void Update()
    {
        
    }

    private IEnumerator Fire(Color32 bulletColor, Vector3 player, float bulletDamage, List<Sprite> sprites)
    {
        SetSprite(player);

        line.enabled = true;
        line.gameObject.GetComponent<Animator>().SetBool("red", true);
        line.startColor = new Color(1, 0, 0);
        line.endColor = new Color(1, 0, 0);

        Vector3 target = player;
        Ray2D ray = new Ray2D(transform.position, target - transform.position);

        line.SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayWall") | 1 << LayerMask.NameToLayer("TileMap");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
        if (hit)
        {
            line.SetPosition(1, hit.point);
        }

        yield return new WaitForSeconds(0.75f);

        line.gameObject.GetComponent<Animator>().SetBool("red", false);
        line.startColor = bulletColor;
        line.endColor = bulletColor;

        GameObject laser = Instantiate(laserPrefab, this.transform);

        laser.GetComponent<EnemyLaser>().SetDamage(bulletDamage);
        laser.GetComponent<EnemyLaser>().SetCoolTime(0.2f);
        laser.GetComponent<EnemyLaser>().SetSprite(sprites);


        Vector3 start = line.GetPosition(0);
        Vector3 end = line.GetPosition(1);

        laser.transform.localScale = new Vector3(Vector3.Distance(start, end) / 3.5f, line.startWidth / 3.5f, 0);
        Vector3 pos = (start + end) / 2;
        Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        laser.transform.rotation = angleAxis;
        laser.transform.position = pos;

        float time = 0;
        while (time < 1.5f)
        {
            time += 0.15f;
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.2f);

        Destroy(laser);
        Destroy(this.gameObject);
    }

    private IEnumerator Pos(Vector3 pos)
    {
        isMoving = true;
        Vector3 dir = (pos - transform.position).normalized;
        Vector3 dis = (pos - transform.position).normalized;

        while(dir == dis)
        {
            transform.position += dir * Time.deltaTime * 20.0f;
            dis = (pos - transform.position).normalized;
            Debug.Log(dis);
            yield return null;
        }

        isMoving = false;
    }

    private void SetSprite(Vector3 player)
    {
        Vector2 direction = player - transform.position;
        weaponObject.SetActive(true);
        weaponObject.transform.parent.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        if (transform.position.y - player.y < 0)
        {
            weaponObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            weaponObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }

        if (player.x >= transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        weaponObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void FireStart(Color32 bulletColor, Vector3 player, float bulletDamage, List<Sprite> sprites)
    {
        StartCoroutine(Fire(bulletColor, player, bulletDamage, sprites));
    }

    public void SetPos(Vector3 pos)
    {
        StartCoroutine(Pos(pos));
    }

    public bool GetMoving()
    {
        return isMoving;
    }
}
