using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// Image 型の拡張メソッドを管理するクラス
/// </summary>
public static class ImageExt
{
    /// <summary>
    /// sprite を設定します
    /// </summary>
    public static void SetSpriteAndSnap(this Image self, Sprite sprite)
    {
        self.sprite = sprite;
        self.SetNativeSize();
    }
}

public class MemorizeMainScript : MonoBehaviour
{
    public Image QuestionImage;
    public Text QuestionWord;
    public Image AnsweredImage;
    public Text AnsweredWord;
    public Text QuestionableNumber;
    public Text MemorizedPoint;

    public Sprite[] QuestionScripts;
    public AudioClip[] AnswerAudioClips;

    public Text WrongType;
    public Text TypingWords;
    public string[] AnswerWords;
    public string[] AnswerWordsJapanese;
    public string[] AnswerWordsHiragana;
    public bool[] IsQuestionable;
    public int[] RandomSort;

    public string Answer = "";
    int QuestionNumber = 0;
    int QuestionableNumberMax = 5;
    int QuestionableNumberMaxPrevious = 5;
    int TypingNumber = 0;//次に回答する文字が何番目なのか
    int TypingNumberHiragana = 0;//次に回答するひらがなが何番目なのか
    int WrongTypeCount = 0;
    bool IsAnswer = true;
    int a = 0, b = 0, c = 0;
    float fa = 0.0f, fb = 0.0f, fc = 0.0f;//float型一時変数
    float TmpFloat = 0;
    float[] RandomValueMax;
    public AudioSource Sound;
    // Start is called before the first frame update
    void Start()
    {
        QuestionScripts = Resources.LoadAll<Sprite>("Sprites/");
        AnswerAudioClips = Resources.LoadAll<AudioClip>("AudioClips/");
        RandomValueMax = new float[QuestionScripts.Length];
        AnswerWords = new string[QuestionScripts.Length];
        IsQuestionable = new bool[AnswerWords.Length];//出題可能かどうか
        RandomSort = new int[AnswerWords.Length];
        float[] RandomValues = new float[AnswerWords.Length];
        for (int i = 0; i < QuestionScripts.Length; i++)
        {
            RandomSort[i] = i;
        }
        for (int i = QuestionScripts.Length - 1; i >= 0; i--)
        {
            a = Random.Range(0, QuestionScripts.Length - 1);
            b = RandomSort[i];
            RandomSort[i] = RandomSort[a];
            RandomSort[a] = b;
        }
        for (int i = 0; i < QuestionScripts.Length; i++)
        {
            AnswerWords[i] = QuestionScripts[i].name;
            RandomValueMax[i] = 100.0f;
            if (i < 5)
            {
                IsQuestionable[RandomSort[i]] = true;
            }
            else
            {
                IsQuestionable[RandomSort[i]] = false;
            }
        }

        AnswerWordsJapanese = new string[] { "アブハジア", "アフガニスタン", "アイルランド",
            "アイスランド", "アメリカ合衆国", "アンドラ", "アンゴラ",
            "アンティグアバーブーダ", "アラブ首長国連邦", "アルバニア", "アルジェリア",
            "アルメニア", "アルツァフ共和国","アルゼンチン","アゼルバイジャン","バーレーン",
            "バハマ","バングラデシュ","バヌアツ","バルバドス","バチカン","ベナン","ベネズエラ",
            "ベラルーシ","ベリーズ","ベルギー","ベトナム","ボリビア","ボスニアヘルツェゴビナ",
            "ボツワナ","ブータン","ブラジル","ブルガリア","ブルキナファソ","ブルネイ","ブルンジ",
            "大韓民国","デンマーク","ドイツ","ドミニカ国","ドミニカ共和国","エクアドル",
            "沿ドニエストル","エリトリア","エルサルバドル","エストニア","エチオピア",
            "エジプト","フィンランド","フィリピン","フィジー","ガーナ","ガボン","ガイアナ",
            "ガンビア","ギニア","ギニアビサウ","ギリシャ","グアテマラ","グレナダ","ハイチ",
            "ハンガリー","東ティモール","ホンジュラス","フランス","イエメン","イギリス",
            "インド","インドネシア","イラク","イラン","イスラエル","イタリア","ジャマイカ",
            "ジョージア","カーボベルデ","カメルーン","カナダ","カンボジア","カタール","カザフスタン",
            "ケニア","キプロス","キリバス","キルギス","北キプロス","コートジボワール",
            "コモロ","コンゴ共和国","コンゴ民主共和国","コロンビア","コソボ","コスタリカ",
            "クック諸島","クロアチア","クウェート","キューバ","マーシャル諸島","マダガスカル",
            "マケドニア","マラウイ","マレーシア","マリ","マルタ","メキシコ","ミクロネシア連邦",
            "南アフリカ共和国","南オセチア","南スーダン","モーリシャス","モーリタニア",
            "モナコ","モンゴル","モンテネグロ","モロッコ","モルディブ","モルドバ","モザンビーク",
            "ミャンマー","ナイジェリア","ナミビア","ナウル","ネパール","日本","ニジェール",
            "ニカラグア","西サハラ","ニウエ","ノルウェー","ニュージーランド","オーストラリア",
            "オーストリア","オマーン","オランダ","パキスタン","パナマ","パプアニューギニア",
            "パラグアイ","パラオ","パレスチナ","ペルー","ポーランド","ポルトガル","ラオス","ラトビア",
            "レバノン","レソト","リベリア","リビア","リヒテンシュタイン","リトアニア","ロシア","ルーマニア",
            "ルクセンブルク","ルワンダ","サモア","サンマリノ","サントメ・プリンシペ","サウジアラビア",
            "セーシェル","赤道ギニア","セネガル","セントビンセントグレナディーン","セントクリストファー・ネイビス",
            "セルビア","シエラレオネ","シンガポール","シリア","ソマリア","ソマリランド",
            "ソロモン諸島","スーダン","スイス","スペイン","スリナム","スリランカ","スロバキア",
            "スロベニア","スワジランド","スウェーデン","タイ","タンザニア","タジキスタン",
            "チリ","トーゴ","トンガ","トリニダードトバゴ","トルコ","トルクメニスタン",
            "ツバル","チャド","チェコ","朝鮮民主主義人民共和国","チュニジア","中華民国",
            "中華人民共和国","中央アフリカ共和国","ウガンダ","ウクライナ","ウルグアイ",
            "ウズベキスタン","ヨルダン","ザンビア","ジブチ","ジンバブエ" };
        QuestionNumber = RandomSort[0];
        QuestionImage.SetSpriteAndSnap(QuestionScripts[QuestionNumber]);
        MemorizedPoint.text = (RandomValueMax[QuestionNumber]).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            QuestionableNumberMax = Mathf.Min(QuestionableNumberMax + 1, QuestionScripts.Length - 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            QuestionableNumberMax = Mathf.Max(QuestionableNumberMax - 1, 5);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            QuestionableNumberMax = Mathf.Min(QuestionableNumberMax + 10, QuestionScripts.Length - 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            QuestionableNumberMax = Mathf.Max(QuestionableNumberMax - 10, 5);
        }
        if (QuestionableNumberMax != QuestionableNumberMaxPrevious)
        {
            QuestionableNumber.text = QuestionableNumberMax.ToString();
            QuestionableNumberMaxPrevious = QuestionableNumberMax;
            for (int i = 0; i < QuestionScripts.Length; i++)
            {
                if (i < QuestionableNumberMax)
                {
                    IsQuestionable[RandomSort[i]] = true;
                }
                else
                {
                    IsQuestionable[RandomSort[i]] = false;
                }
            }
        }
        if (Input.anyKeyDown)
        {
            foreach (var c in Input.inputString)
            {
                //　バックスペースキー
                if (c == "\b"[0])
                {
                    Debug.Log("BackSpace");
                }
                //　EnterかReturn
                else if (c == "\n"[0] || c == "\r"[0])
                {
                    IsAnswer = false;
                    QuestionWord.text = AnswerWordsJapanese[QuestionNumber];
                    Debug.Log("EnterOrReturn");
                }
                else if (c == "\t"[0])
                {
                    Debug.Log("Tab");
                    //　Spaceキーを取得（
                }
                else if (Input.GetKeyDown("space"))
                {
                    Debug.Log("Space");
                }
                else
                {
                    //　空じゃなければ文字を表示
                    if (c.ToString() != "")
                    {
                        if (AnswerWords[QuestionNumber][TypingNumber] == c)
                        {
                            TypingWords.text += c.ToString();
                            TypingNumber++;
                            if (TypingWords.text == AnswerWords[QuestionNumber])
                            {
                                AnsweredWord.text = AnswerWordsJapanese[QuestionNumber];
                                AnsweredImage.SetSpriteAndSnap(QuestionScripts[QuestionNumber]);
                                AnsweredImage.transform.localPosition = new Vector3(0, 192, 0);
                                if (IsAnswer)
                                {
                                    RandomValueMax[QuestionNumber] = Mathf.Min(RandomValueMax[QuestionNumber] * 1.25f, 10000.0f);
                                }
                                else
                                {
                                    RandomValueMax[QuestionNumber] = Mathf.Max(RandomValueMax[QuestionNumber] * 0.8f, 10.0f);
                                }
                                Sound.PlayOneShot(AnswerAudioClips[QuestionNumber]);
                                Debug.Log(RandomValueMax[QuestionNumber]);
                                TmpFloat = 100000;
                                for (int i = 0; i < QuestionScripts.Length; i++)
                                {
                                    fa = Random.Range(0.0f, RandomValueMax[i]);
                                    if (IsQuestionable[i] == true && i != QuestionNumber && fa < TmpFloat)
                                    {
                                        TmpFloat = fa;
                                        a = i;
                                        Debug.Log(fa);
                                    }
                                }
                                QuestionNumber = a;
                                MemorizedPoint.text = (RandomValueMax[QuestionNumber]).ToString();
                                QuestionImage.SetSpriteAndSnap(QuestionScripts[QuestionNumber]);
                                WrongTypeCount = 0;
                                TypingNumber = 0;
                                IsAnswer = true;
                                TypingWords.text = "";
                                QuestionWord.text = "";
                            }
                        }
                        else
                        {
                            WrongType.text = c.ToString();
                            WrongTypeCount++;
                            if (WrongTypeCount == 4)
                            {
                                IsAnswer = false;
                                QuestionWord.text = AnswerWordsJapanese[QuestionNumber];
                            }
                        }
                    }
                }
            }
        }
        AnsweredImage.transform.localPosition = Vector3.Lerp(AnsweredImage.transform.localPosition, new Vector3(0, -192, 0), 0.01f);
    }
}