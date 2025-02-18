using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public enum EnemyHairColor //적 헤어 색
{
    black,
    blond,
    red,
    skyblue
}

public enum EnemyHairType //적 헤어 종류
{
    hair1,
    hair2,
    hair3,
    hair4
}

public enum EnemyState //적 애니메이션 상태
{
    idle,
    moveLeft,
    moveRight,
    dead
}

public enum EnemyBodyColor //적 파부색
{
    blue,
    red,
    green,
    white
}

public enum EnemyBodyType //적 몸통 스타일
{
    cafe1,
    cafe2,
    camping1,
    camping2,
    club1,
    club2,
    normal1,
    normal2,
    student1,
    student2,
    dress,
    suit
}

public enum EnemyEyeColor //적 눈 색
{
    red1,
    emerald1,
    blue1,
    skyblue1,
    red2,
    emerald2,
    blue2,
    skyblue2
}

public class EnemySprite : MonoBehaviour
{
    private Animator enemyAnim; //애니메이터 불러오기
    public GameObject spriteObj; //적 최상위 오브젝트

    //눈과 head색은 라이브러리 에셋 교체가 아닌 리졸버에서 교체로 리졸버 불러오가
    public SpriteResolver eyeSpriteResolver;
    public SpriteResolver headSpriteResolver;

    //스프라이트 라이브러리 에셋
    public SpriteLibraryAsset[] hairSpriteLibraryAssets; //헤어스타일
    public SpriteLibraryAsset eyesSpriteLibraryAssets; //눈 색깔
    public SpriteLibraryAsset headSkinSpriteLibraryAssets; //머리 색깔

    public SpriteLibraryAsset[] blueBodySpriteLibraryAssets; //파란피부 몸통
    public SpriteLibraryAsset[] greenBodySpriteLibraryAssets; //초록피부 몸통
    public SpriteLibraryAsset[] redBodySpriteLibraryAssets; //빨간피부 몸통
    public SpriteLibraryAsset[] whiteBodySpriteLibraryAssets; //하얀피부 몸통

    //스프라이트 라이브러리
    public SpriteLibrary hairSpriteLib;
    public SpriteLibrary eyesSpriteLib;
    public SpriteLibrary headSkinSpriteLib;
    public SpriteLibrary bodySpriteLib;

    private string enemyEyeColorCategory; //적 눈 색깔 카테고리 지정

    public GameObject flipObject; //적 오브젝트 좌우 플립하는데 최상위 오브젝트 뒤집으면 이동이 꼬일 것 같아서 바로 아래 오브젝트 뒤집기용
    //커스터마이징 옵션 생성
    public EnemyHairColor enemyHairColor = EnemyHairColor.black;
    public EnemyHairType enemyHairType = EnemyHairType.hair1;
    public EnemyBodyColor enemyBodyColor = EnemyBodyColor.green;
    public EnemyBodyType enemyBodyType = EnemyBodyType.normal1;
    public EnemyEyeColor enemyEyeColor = EnemyEyeColor.red1;

    public EnemyState state = EnemyState.idle;

    public List<SpriteRenderer> layerOrder;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        
        flipObject = transform.Find("Enemy Set").gameObject; //최상위 오브젝트 직후 하위 오브젝트 미리 불러오기

        enemyEyeColorCategory = "eye1_red"; //변수 값 초기화 (필요없을 것 같긴 함)

        RandomAppear();

        StartCoroutine(BlinkEyes()); //눈 깜빡임 코루틴 실행
    }

    //프리팹 생성 시 적 외형 조합 후 게임에 넣는 방식이라면 외형 선택은 Start문 안에서 하는게 비용이 더 적게 들어갈 듯 합니다.
    //enemyHairType, enemyBodyType, enemyBodyColor, enemyEyeColor 로 판별하는 switch문은 Start문에 넣어도 작동 가능 예상
    //다만 실시간으로 외형 변경은 적용 X, 컴포넌트 불러오기 스크립트 아래에 집어넣어야 함.

    //눈 색상 관련은 코루틴에서 색상이 변경되는 방식이라 관련 코드 수정 후 확인 필

    void Update()
    {
        switch (state) //운동 애니메이션 변경, '반드시 Update문 안에 있어야 함', 별도 조건문으로 움직임 조작 시 (ex: 키보드 입력 발생 시 위치 이동하면서 같이 변경) case문 속 1~4줄 스크립트 전부 실행 필요
        {
            case EnemyState.idle: //정지
                enemyAnim.SetInteger("state", 0); //애니메이션 판별용
                break;
            case EnemyState.moveLeft: //왼쪽 방향
                enemyAnim.SetInteger("state", 1);
                flipObject.transform.localScale=new Vector3(-1f, 1f, 1f); //오브젝트 방향 뒤집기, 최상위 오브젝트 X
                enemyAnim.SetBool("isLeft", true); //퇴장 시 사라지는 방향 판별용
                break;
            case EnemyState.moveRight: //오른쪽 방향
                enemyAnim.SetInteger("state", 1);
                flipObject.transform.localScale = new Vector3(1f, 1f, 1f);
                enemyAnim.SetBool("isLeft", false);
                break;
            case EnemyState.dead: //퇴장
                enemyAnim.SetInteger("state", 2);
                StopCoroutine(BlinkEyes()); //눈 깜빡임 정지
                eyeSpriteResolver.SetCategoryAndLabel(enemyEyeColorCategory, "closed"); //웃는 눈으로 변경
                eyeSpriteResolver.ResolveSpriteToSpriteRenderer();
                break;
        }
    }

    public void RandomAppear()
    {
        enemyHairColor = (EnemyHairColor)Random.Range(0, 4);
        enemyHairType = (EnemyHairType)Random.Range(0, 4);
        enemyBodyColor = (EnemyBodyColor)Random.Range(0, 4);
        enemyEyeColor = (EnemyEyeColor)Random.Range(0, 8);

        switch(GameManager.gameManager.stageThemes[GameManager.gameManager.stage - 1])
        {
            case Stage_Theme.BAR:
                if(Random.Range(0, 2) == 0)
                {
                    enemyBodyType = EnemyBodyType.club1;
                }
                else
                {
                    enemyBodyType = EnemyBodyType.club2;
                }
                break;
            case Stage_Theme.CAFE:
                if (Random.Range(0, 2) == 0)
                {
                    enemyBodyType = EnemyBodyType.cafe1;
                }
                else
                {
                    enemyBodyType = EnemyBodyType.cafe2;
                }
                break;
            case Stage_Theme.CAMPING:
                if (Random.Range(0, 2) == 0)
                {
                    enemyBodyType = EnemyBodyType.camping1;
                }
                else
                {
                    enemyBodyType = EnemyBodyType.camping2;
                }
                break;
            case Stage_Theme.NORMAL:
                if (Random.Range(0, 2) == 0)
                {
                    enemyBodyType = EnemyBodyType.normal1;
                }
                else
                {
                    enemyBodyType = EnemyBodyType.normal2;
                }
                break;
            case Stage_Theme.RESTORANT:
                if (Random.Range(0, 2) == 0)
                {
                    enemyBodyType = EnemyBodyType.suit;
                }
                else
                {
                    enemyBodyType = EnemyBodyType.dress;
                }
                break;
            case Stage_Theme.SCHOOL:
                if (Random.Range(0, 2) == 0)
                {
                    enemyBodyType = EnemyBodyType.student1;
                }
                else
                {
                    enemyBodyType = EnemyBodyType.student2;
                }
                break;
            case Stage_Theme.STREET:
                enemyBodyType = (EnemyBodyType)Random.Range(0, 12);
                break;
        }

        switch (enemyHairType) //헤어 종류 밑 색 선택
        {
            case EnemyHairType.hair1:
                if (enemyHairColor == EnemyHairColor.black)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[0];
                else if (enemyHairColor == EnemyHairColor.blond)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[1];
                else if (enemyHairColor == EnemyHairColor.red)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[2];
                else
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[3];
                break;
            case EnemyHairType.hair2:
                if (enemyHairColor == EnemyHairColor.black)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[4];
                else if (enemyHairColor == EnemyHairColor.blond)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[5];
                else if (enemyHairColor == EnemyHairColor.red)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[6];
                else
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[7];
                break;
            case EnemyHairType.hair3:
                if (enemyHairColor == EnemyHairColor.black)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[8];
                else if (enemyHairColor == EnemyHairColor.blond)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[9];
                else if (enemyHairColor == EnemyHairColor.red)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[10];
                else
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[11];
                break;
            case EnemyHairType.hair4:
                if (enemyHairColor == EnemyHairColor.black)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[12];
                else if (enemyHairColor == EnemyHairColor.blond)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[13];
                else if (enemyHairColor == EnemyHairColor.red)
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[14];
                else
                    hairSpriteLib.spriteLibraryAsset = hairSpriteLibraryAssets[15];
                break;
        }

        switch (enemyBodyType) //몸통 색상&스타일 변경
        {
            case EnemyBodyType.cafe1:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[0];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[0];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[0];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[0];
                break;
            case EnemyBodyType.cafe2:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[1];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[1];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[1];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[1];
                break;
            case EnemyBodyType.camping1:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[2];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[2];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[2];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[2];
                break;
            case EnemyBodyType.camping2:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[3];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[3];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[3];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[3];
                break;
            case EnemyBodyType.club1:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[4];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[4];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[4];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[4];
                break;
            case EnemyBodyType.club2:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[5];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[5];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[5];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[5];
                break;
            case EnemyBodyType.normal1:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[6];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[6];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[6];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[6];
                break;
            case EnemyBodyType.normal2:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[7];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[7];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[7];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[7];
                break;
            case EnemyBodyType.student1:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[8];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[8];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[8];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[8];
                break;
            case EnemyBodyType.student2:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[9];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[9];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[9];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[9];
                break;
            case EnemyBodyType.dress:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[10];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[10];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[10];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[10];
                break;
            case EnemyBodyType.suit:
                if (enemyBodyColor == EnemyBodyColor.blue)
                    bodySpriteLib.spriteLibraryAsset = blueBodySpriteLibraryAssets[11];
                else if (enemyBodyColor == EnemyBodyColor.green)
                    bodySpriteLib.spriteLibraryAsset = greenBodySpriteLibraryAssets[11];
                else if (enemyBodyColor == EnemyBodyColor.red)
                    bodySpriteLib.spriteLibraryAsset = redBodySpriteLibraryAssets[11];
                else
                    bodySpriteLib.spriteLibraryAsset = whiteBodySpriteLibraryAssets[11];
                break;
        }

        switch (enemyBodyColor) //머리 피부색 변경, 몸통 피부색과 동기화
        {
            case EnemyBodyColor.green:
                headSpriteResolver.SetCategoryAndLabel("head", "head1");
                headSpriteResolver.ResolveSpriteToSpriteRenderer();
                break;
            case EnemyBodyColor.blue:
                headSpriteResolver.SetCategoryAndLabel("head", "head2");
                headSpriteResolver.ResolveSpriteToSpriteRenderer();
                break;
            case EnemyBodyColor.white:
                headSpriteResolver.SetCategoryAndLabel("head", "head3");
                headSpriteResolver.ResolveSpriteToSpriteRenderer();
                break;
            case EnemyBodyColor.red:
                headSpriteResolver.SetCategoryAndLabel("head", "head4");
                headSpriteResolver.ResolveSpriteToSpriteRenderer();
                break;
        }

        switch (enemyEyeColor) //눈 색깔 카테고리 변경, 인게임 반영은 눈 깜빡임 코루틴에서 변경됨
        {
            case EnemyEyeColor.red1:
                enemyEyeColorCategory = "eye1_red";
                break;
            case EnemyEyeColor.emerald1:
                enemyEyeColorCategory = "eye1_emerald";
                break;
            case EnemyEyeColor.blue1:
                enemyEyeColorCategory = "eye1_blue";
                break;
            case EnemyEyeColor.skyblue1:
                enemyEyeColorCategory = "eye1_skyblue";
                break;
            case EnemyEyeColor.red2:
                enemyEyeColorCategory = "eye2_red";
                break;
            case EnemyEyeColor.emerald2:
                enemyEyeColorCategory = "eye2_emerald";
                break;
            case EnemyEyeColor.blue2:
                enemyEyeColorCategory = "eye2_blue";
                break;
            case EnemyEyeColor.skyblue2:
                enemyEyeColorCategory = "eye2_skyblue";
                break;
        }
    }

    IEnumerator BlinkEyes() //눈 깜빡임 코루틴, 적 많아져서 작동 코루틴 개수 많아지면 성능 저하 되려나 모르겠음, 저하 시 그냥 time으로 조건문 만들어서 update 안에 집어넣어야 함
    {
        float blinkCycle;

        while(true) 
        {
            blinkCycle = Random.Range(0.1f, 3.8f); //깜빡이는 속도 랜덤하게
            eyeSpriteResolver.SetCategoryAndLabel(enemyEyeColorCategory, "eye1");
            eyeSpriteResolver.ResolveSpriteToSpriteRenderer();

            yield return new WaitForSeconds(blinkCycle);

            eyeSpriteResolver.SetCategoryAndLabel(enemyEyeColorCategory, "eye2");
            eyeSpriteResolver.ResolveSpriteToSpriteRenderer();

            yield return new WaitForSeconds(0.2f); //감는 시간
        }
    }

    public List<Sprite> getEnemySprite()
    {
        List<Sprite> result = new List<Sprite>();

        result.Add(headSkinSpriteLib.gameObject.GetComponent<SpriteRenderer>().sprite);
        result.Add(hairSpriteLib.gameObject.GetComponent<SpriteRenderer>().sprite);
        result.Add(eyesSpriteLib.gameObject.GetComponent<SpriteRenderer>().sprite);
        result.Add(bodySpriteLib.gameObject.GetComponent<SpriteRenderer>().sprite);

        return result;
    }

    public void SetLayerOrder(int order)
    {
        foreach(SpriteRenderer renderer in layerOrder)
        {
            renderer.sortingOrder += 5 * order;
        }
    }
}
