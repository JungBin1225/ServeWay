using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    // Start is called before the first frame update
    void Start()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // 0: Player
    // 1000: News Anchor
    // 2000: Mom
    // 3000: Friends
    // 4000: Dad
    // 5001: Alien - Mom
    // 5002: Alien - Son
    // 6000: Opening - After Tutorial

    void GenerateData()
    {
        talkData.Add(0, new string[] {
            // Living Room 1

            "나는 계속 요리사를 하고 싶다 했는데" +
            "\n엄마가 계속 반대했잖아!" +
            "\n난 요리 말곤 다른 일 할 생각 없어!",

            "어.....",

            // Dining Room

            "..........",

            "왜? 맛있어?",

            "아니 그정도야?" +
            "\n아빠는 왜 말이 없어?",

            "아빠...?" +
            "\n(앉은 채로 기절했어?!)",

            // Living Room 2

            " 알았어.....",

            // Dump

            "난 레시피대로 했는데 왜 이렇게 된 걸까.....",

            "(나 아저씨 아닌데...)" +
            "\n그래? 내가 만든 건데 먹어볼래?",

            "아니에요, 괜찮아요!",

            "(내 음식이 외계인 입맛에 딱 맞다니!" +
            "\n이거다! 난 지구의 음식을 외계인의" +
            "\n입맛에 맞춰 만드는 요리사를 해야겠어!)"
        });

        talkData.Add(1000, new string[] {
            "다음 소식입니다. 행성간 연합(UP)에서 전 우주인을 대상으로" +
            "\n관광하기 좋은 행성을 조사하였습니다." +
            "\n그 결과 우리 지구가 관광하기 좋은 행성 10위로 선정되었습니다." ,

            "우리 지구가 다른 행성과 교류하기 시작한지 약 15년이 지났는데," +
            "\n이 짧은 기간에 순위에 오른 행성은 지구가 유일하다고 합니다.",

            "하지만 이 순위도 아쉬워하는 사람들이 많았습니다." +
            "\n전문가들은 지구의 인기가 10위에 그치게 된 것은" +
            "\n음식이 가장 큰 원인이라고 지목하였습니다.",

            "다른 행성과 지구는 입맛이 매우 다르기 때문에" +
            "\n음식으로 인한 관광 수입은 거의 없다시피한 상황입니다."
        });

        talkData.Add(2000, new string[] {
            // Living Room 1

            "에휴... 우리 지구는 이렇게 잘나가는데" +
            "\n인주는 언제 취업을 할까.....",

            "니 요리 실력을 생각해봐라!" +
            "\n그 요리 실력으로 요리사가 되면" +
            "\n전 우주의 요리사를 모욕하는 거야!",

            // Dining Room
            
            "이야 이거 좋다!",

            "아니, 다이어트용으로 좋다." +
            "\n너무 맛없어서 식욕이 싹 사라지네.",

            // Living Room 2
            
            "이제 니 입에서 요리사 소리가 한 번 더 나오면" +
            "\n니가 요리가 될 줄 알아!",
            
            "아까 니가 한 요리나 버리고 와!"
        });

        talkData.Add(3000, new string[] {
            "으악 이게 뭐야!" +
            "\n이거 맛이 왜이래!",

            "생긴 건 맛있게 생겼는데 이런 맛이 나다니..." +
            "\n너 다른 의미로 재능이 있네."
        });

        talkData.Add(4000, new string[]
        {
            "....."
        });

        talkData.Add(5001, new string[]
        {
            "얘! 처음 보는 사람한테" +
            "\n예의없게 굴지 말랬지!",

            "그래?",

            "진짜 맛있네요!" +
            "\n지구의 음식은 우리 입맛과 너무 달라서" +
            "\n별로였는데, 이건 입맛에 딱 맞네요!"
        });
        talkData.Add(5002, new string[]
        {
            "엄마! 저기서 맛있는 냄새가 나!",

            "아저씨, 그 손에 든 음식 뭐예요?" +
            "\n저도 먹어보고 싶어요!",

            "와! 진짜 맛있다!" +
            "\n지구에서 먹어본 음식 중에 제일 맛있어!",

            "엄마도 이거 먹어봐! 진짜 맛있어!"
        });

        talkData.Add(6000, new string[]
            {
                "자! 이제 정식 요리사가 되는데" + "\n단 한 단계만 남았다!",
                "이 시험만 통과하면 이제 우주에서" + "\n인정받는 요리사가 될 수 있다!",
                "2년동안 요리를 혹독하게 공부했으니" + "\n요리 실력은 일류라고 봐도 무방하지!",
                "하지만 요리 실력만 있다고" + "\n일류 요리사가 되는 것이 아니다!",
                "수많은 유형의 손님들을" + "\n어떻게 대하는지도 중요하다!" + "\n이른바 접객 실력이라는 거지!",
                "마지막 시험에서는" + "\n요리, 접객 실력을 포함한" + "\n모든 요소를 평가한다!",
                "이 시험에 대한 규칙을 알려주마!" + "\n이쪽 문으로 들어오도록!"
            });

        talkData.Add(7000, new string[]
        {
            "사부님! 꼭 합격하고 오겠습니다!",
            "그래! 무운을 빌지!"
        });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
