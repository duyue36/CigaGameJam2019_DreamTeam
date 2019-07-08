using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HIV;

public enum CompanyName { Baidu, XiaoMi, Musk};
public enum PolicyReputation { Negative, Positive};
public enum GameState { PickPolicy, Battle, FinalResult};
public enum Winner { Player1, Player2};
public enum CompanyReputation {OverwhelminglyPositive, MostlyPositive, Positive, Mixed, Negative, MostlyNegative, OverwhelminglyNegative};

public struct OnePolicy
{
    public CompanyName CompanyName;
    public int policyNumber;
    public string content;
    public PolicyReputation policyReputation;


}

public class PolicySystem : MonoBehaviour
{
    public static PolicySystem Instance
    {
        get; private set;
    }

    [Header("General Setting")]
    public CompanyName companyName;
    public int companyReputationValue;
    public CompanyReputation companyReputation = CompanyReputation.Mixed;

    public OnePolicy currentRoundPolicy;

    [Header("Round setting")]
    public int totalRoundCount = 3;      //游戏总轮次
    public int currentRound = 1;         //当前轮次
    public GameState gameState = GameState.PickPolicy;          //游戏状态：选择提案还是战斗中
    public Text indicationText;

    public OnePolicy roundWinPolicy;  //当前轮次胜出的提案

    [Header("Player1 Relate")]
    public Transform trans_Player1;
    public Button button_Choice_A_Player1;
    public Text text_Choice_A_Player1;
    public Button button_Choice_B_Player1;
    public Text text_Choice_B_Player1;
    public Button button_currentPendingChoice_Player1;

    public int currentChoiceNumber_Player1 = 1;
    public string currentChoiceContent_player1;
    public PolicyReputation currentChoiceReputation_player1;
    public OnePolicy currentPickedPolicy_player1; //玩家1当前提案

    [Header("Player2 Relate")]
    public Transform trans_Player2;
    public Button button_Choice_A_Player2;
    public Button button_Choice_B_Player2;
    public Text text_Choice_A_Player2;
    public Text text_Choice_B_Player2;

    public int currentChoiceNumber_Player2 = 1;
    public string currentChoiceContent_player2;
    public PolicyReputation currentChoiceReputation_player2;
    public OnePolicy currentPickedPolicy_player2;   //玩家2当前提案

    [Header("UI related")]
    public GameObject thinkFrame_player1;
    public GameObject thinkFrame_player2;
    public Vector3 offset_thinkFrame_p1 = new Vector3(0, 2, 0);
    public Vector3 offset_thinkFrame_p2 = new Vector3(0, 2, 0);
    public Color pendingColor;
    public Color notPendingColor;

    public Color color_Transparent;
    public Color color_notTransparent;

    

    [Header("Count Down")]
    public float pickPolicyLimitedTime = 2f;
    public Slider timeSlider_player1;
    public Slider timeSlider_player2;

    Coroutine coroutine_pickPolicy;

    public SpriteRenderer sp_number3;
    public SpriteRenderer sp_number2;
    public SpriteRenderer sp_number1;
    public SpriteRenderer sp_letsGo;

    [Header("Event And Result UI")]
    public Image image_closeBaiduSpace;
    public Image image_keepBaiduSpace;
    public Image image_weiZeXiLive;
    public Image image_weiZeXiDead;
    public Image image_HondYanHuoShui_good;
    public Image image_HondYanHuoShui_bad;

    public Image image_OverPostive;
    public Image image_MostPostive;
    public Image image_Postive;
    public Image image_Mixed;
    public Image image_Negative;
    public Image image_mostNegative;
    public Image image_OverNegative;

    public bool baiduSpaceEventTriggered;
    public bool weiZeXiEventTriggered;

    public bool goodEnd_baiduSpace;
    public bool goodEnd_weiZeXi;
    public bool goodEnd_HongYanHuoShui;


    public OnePolicy policy_one_positive_baidu_player1;
    public OnePolicy policy_one_negative_baidu_player1;
    public OnePolicy policy_two_positive_baidu_player1;
    public OnePolicy policy_two_negative_baidu_player1;
    public OnePolicy policy_three_positive_baidu_player1;
    public OnePolicy policy_three_negative_baidu_player1;

    public OnePolicy policy_one_positive_baidu_player2;
    public OnePolicy policy_one_negative_baidu_player2;
    public OnePolicy policy_two_positive_baidu_player2;
    public OnePolicy policy_two_negative_baidu_player2;
    public OnePolicy policy_three_positive_baidu_player2;
    public OnePolicy policy_three_negative_baidu_player2;

    private bool canTriggerNextStep = true;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
        
        button_currentPendingChoice_Player1 = button_Choice_A_Player1;

        InitiatePolicy();

        //if player choice baidu, initiate baidu choice

        RefreshPolicyChoiceContent();

        coroutine_pickPolicy = StartCoroutine(PickPolicyCountDown());  //倒计时

        FindObjectOfType<StickMan>().enabled = false;
        FindObjectOfType<StickMan2>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(trans_Player1 == null)
        {
            trans_Player1 = GameObject.FindWithTag("Player1").transform;
        }
        if(trans_Player2 == null)
        {
            trans_Player2 = GameObject.FindWithTag("Player2").transform;
        }

        InputUpdate();

        thinkFrame_player1.transform.position = Camera.main.WorldToScreenPoint(trans_Player1.position + offset_thinkFrame_p1);
        thinkFrame_player2.transform.position = Camera.main.WorldToScreenPoint(trans_Player2.position + offset_thinkFrame_p2);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space is pressed");
            //StartCoroutine(ShowEventImage(image_closeBaiduSpace, 5, 1f));
            NextStep();
        }

        
        //StartCoroutine(ShowEventImage(image_closeBaiduSpace, 5, 1f));

        
    }

    public void NextStep()
    {
        if (currentRound == 1 && gameState == GameState.PickPolicy && canTriggerNextStep)  //Round 1 提案 阶段进入 Round 1 Battle阶段
        {
            gameState = GameState.Battle;

            indicationText.text = "Round 1 Battle";

            ShowThinkFrame(false);

            canTriggerNextStep = false;
            StartCoroutine(UnlockNextStepCountDown());

            FindObjectOfType<StickMan>().enabled = true;
            FindObjectOfType<StickMan2>().enabled = true;
        }

        if (currentRound == 1 && gameState == GameState.Battle && canTriggerNextStep) //Round 1 battle 阶段进入 Round 2 提案 阶段
        {
            GameManager.Singleton.ReloadScene();
           // ExecuteWinnersPolicy(Winner.Player1);

            currentRound = 2;
            gameState = GameState.PickPolicy;

            indicationText.text = "Round 2 提案阶段";

            RefreshPolicyChoiceContent();
            coroutine_pickPolicy = StartCoroutine(PickPolicyCountDown());

            canTriggerNextStep = false;
            StartCoroutine(UnlockNextStepCountDown());

            ShowThinkFrame(true);

            FindObjectOfType<StickMan>().enabled = false;
            FindObjectOfType<StickMan2>().enabled = false;
        }

        if (currentRound == 2 && gameState == GameState.PickPolicy && canTriggerNextStep) //Round 2 提案 阶段进入 Round 2 Battle阶段
        {
            gameState = GameState.Battle;

            indicationText.text = "Round 2 Battle";

            canTriggerNextStep = false;
            StartCoroutine(UnlockNextStepCountDown());

            ShowThinkFrame(false);

            FindObjectOfType<StickMan>().enabled = true;
            FindObjectOfType<StickMan2>().enabled = true;
        }

        if (currentRound == 2 && gameState == GameState.Battle && canTriggerNextStep) //Round 2 battle 阶段进入 Round 3 提案 阶段
        {
            GameManager.Singleton.ReloadScene();

            currentRound = 3;
            gameState = GameState.PickPolicy;

            indicationText.text = "Round 3 提案阶段";

            RefreshPolicyChoiceContent();
            coroutine_pickPolicy = StartCoroutine(PickPolicyCountDown());

            canTriggerNextStep = false;
            StartCoroutine(UnlockNextStepCountDown());

            ShowThinkFrame(true);

            FindObjectOfType<StickMan>().enabled = false;
            FindObjectOfType<StickMan2>().enabled = false;
        }

        if (currentRound == 3 && gameState == GameState.PickPolicy && canTriggerNextStep) //Round 3 提案 阶段进入 Round 3 Battle阶段
        {
            gameState = GameState.Battle;

            indicationText.text = "Round 3 Battle";

            canTriggerNextStep = false;
            StartCoroutine(UnlockNextStepCountDown());

            ShowThinkFrame(false);

            FindObjectOfType<StickMan>().enabled = true;
            FindObjectOfType<StickMan2>().enabled = true;
        }

        if (currentRound == 3 && gameState == GameState.Battle && canTriggerNextStep) //进入最终结算阶段
        {
            //ExecuteWinnersPolicy(Winner.Player1);
            canTriggerNextStep = false;
            StartCoroutine(UnlockNextStepCountDown());

            gameState = GameState.FinalResult;

            indicationText.text = "公司最终名声为:" + companyReputation;

            PublishCompanyResult();
        }
    }

    void InitiatePolicy()
    {
        policy_one_positive_baidu_player1.content = "继续经营百度空间";
        policy_one_negative_baidu_player1.content = "关闭百度空间";

        policy_two_positive_baidu_player1.content = "将百度百科留在搜索第一页";
        policy_two_negative_baidu_player1.content = "为了贯彻竞价排名，百度百科不必留在搜索第一页";

        policy_three_positive_baidu_player1.content = "优化搜索算法";
        policy_three_negative_baidu_player1.content = "不优化搜索算法";

        policy_one_positive_baidu_player2.content = "留住陆奇，给他更大的权利";
        policy_one_negative_baidu_player2.content = "解聘陆奇";

        policy_two_positive_baidu_player2.content = "关闭竞价排名";
        policy_two_negative_baidu_player2.content = "将竞价排名转移到移动端";

        policy_three_positive_baidu_player2.content = "开发百度云文档";
        policy_three_negative_baidu_player2.content = "经营百度外卖";



    }

    void RefreshPolicyChoiceContent()
    {
        switch (companyName)
        {
            case CompanyName.Baidu:
                if(currentRound == 1)
                {
                    text_Choice_A_Player1.text = policy_one_positive_baidu_player1.content;
                    text_Choice_B_Player1.text = policy_one_negative_baidu_player1.content;

                    text_Choice_A_Player2.text = policy_one_negative_baidu_player2.content;
                    text_Choice_B_Player2.text = policy_one_positive_baidu_player2.content;


                }else if(currentRound == 2)
                {
                    text_Choice_A_Player1.text = policy_two_negative_baidu_player1.content;
                    text_Choice_B_Player1.text = policy_two_positive_baidu_player1.content ;

                    text_Choice_A_Player2.text = policy_two_positive_baidu_player2.content;
                    text_Choice_B_Player2.text = policy_two_negative_baidu_player2.content;
                }else if(currentRound == 3)
                {
                    text_Choice_A_Player1.text = policy_three_positive_baidu_player1.content;
                    text_Choice_B_Player1.text = policy_three_negative_baidu_player1.content;

                    text_Choice_A_Player2.text = policy_three_negative_baidu_player2.content;
                    text_Choice_B_Player2.text = policy_three_positive_baidu_player2.content;
                }

                currentChoiceContent_player1 = text_Choice_A_Player1.text; //默认选择项为A
                currentChoiceContent_player2 = text_Choice_A_Player2.text;

                currentChoiceNumber_Player1 = 1;                 //刷新选择序号
                currentChoiceNumber_Player2 = 1;

                button_Choice_A_Player1.image.color = pendingColor;      //重新刷新按钮颜色
                button_Choice_B_Player1.image.color = notPendingColor;

                break;
            case CompanyName.XiaoMi:
                break;
            case CompanyName.Musk:
                break;
        }
        //text_Choice_A_Player1.text = policy_one_positive_baidu_player1.content;
        
    }

    void InputUpdate()  //玩家输入更新
    {
        if(gameState == GameState.PickPolicy)
        {
            if (Input.GetKeyDown(KeyCode.W))   // player1选政策操作
            {
                Debug.Log("W is pressed");

                if (currentChoiceNumber_Player1 == 2)
                {
                    currentChoiceNumber_Player1 = 1;
                    currentChoiceContent_player1 = text_Choice_A_Player1.text;

                    button_Choice_A_Player1.image.color = pendingColor;
                    button_Choice_B_Player1.image.color = notPendingColor;
                }

            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("S is pressed");
                if(currentChoiceNumber_Player1 == 1)
                {
                    currentChoiceNumber_Player1 = 2;
                    currentChoiceContent_player1 = text_Choice_B_Player1.text;

                    button_Choice_A_Player1.image.color = notPendingColor;
                    button_Choice_B_Player1.image.color = pendingColor;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))   // player1选政策操作
            {
                Debug.Log("upArrow is pressed");

                if (currentChoiceNumber_Player2 == 2)
                {
                    currentChoiceNumber_Player2 = 1;
                    currentChoiceContent_player2 = text_Choice_A_Player2.text;

                    button_Choice_A_Player2.image.color = pendingColor;
                    button_Choice_B_Player2.image.color = notPendingColor;
                }

            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Debug.Log("down Arrow is pressed");
                if (currentChoiceNumber_Player2 == 1)
                {
                    currentChoiceNumber_Player2 = 2;
                    currentChoiceContent_player2 = text_Choice_B_Player2.text;

                    button_Choice_A_Player2.image.color = notPendingColor;
                    button_Choice_B_Player2.image.color = pendingColor;
                }
            }
        }
       
    }

    //void calculateRoundResult()  //计算单轮结果
    //{
    //    switch (companyName)
    //    {
    //        case CompanyName.Baidu:
    //            if (currentRound == 1)
    //            {
                    
    //                text_Choice_B_Player1.text = policy_one_negative_baidu_player1.content;

    //                text_Choice_A_Player2.text = policy_one_negative_baidu_player2.content;
    //                text_Choice_B_Player2.text = policy_one_positive_baidu_player2.content;


    //            }
    //            else if (currentRound == 2)
    //            {
    //                text_Choice_A_Player1.text = policy_two_negative_baidu_player1.content;
    //                text_Choice_B_Player1.text = policy_two_positive_baidu_player2.content;

    //                text_Choice_A_Player2.text = policy_two_positive_baidu_player2.content;
    //                text_Choice_B_Player2.text = policy_two_negative_baidu_player2.content;
    //            }
    //            else if (currentRound == 3)
    //            {
    //                text_Choice_A_Player1.text = policy_three_positive_baidu_player1.content;
    //                text_Choice_B_Player1.text = policy_three_negative_baidu_player1.content;

    //                text_Choice_A_Player2.text = policy_three_negative_baidu_player2.content;
    //                text_Choice_B_Player2.text = policy_three_positive_baidu_player2.content;
    //            }

    //            break;
    //        case CompanyName.XiaoMi:
    //            break;
    //        case CompanyName.Musk:
    //            break;
    //    }
    //}

    IEnumerator PickPolicyCountDown()   //选择提案倒计时
    {
        float timer = 0;
        float remainTime = pickPolicyLimitedTime;

        while (timer <= pickPolicyLimitedTime)
        {
            timer += Time.deltaTime;
            remainTime = pickPolicyLimitedTime - timer;
            timeSlider_player1.value = remainTime / pickPolicyLimitedTime;
            timeSlider_player2.value = remainTime / pickPolicyLimitedTime;

            yield return null;
        }

        //倒计时结束，选定当前悬停的提案
        finishPickPolicy();

        yield break;
    }



    void finishPickPolicy()   //结束提案环节，双方选定提案
    {
        switch (companyName)
        {
            case CompanyName.Baidu:
                if (currentRound == 1)
                {
                    if(currentChoiceContent_player1 == policy_one_positive_baidu_player1.content)
                    {
                        currentChoiceReputation_player1 = PolicyReputation.Positive;
                    }
                    else
                    {
                        currentChoiceReputation_player1 = PolicyReputation.Negative;
                    }

                    if(currentChoiceContent_player2 == policy_one_positive_baidu_player2.content)
                    {
                        currentChoiceReputation_player2 = PolicyReputation.Positive;
                    }
                    else
                    {
                        currentChoiceReputation_player2 = PolicyReputation.Negative;
                    }
                }
                else if (currentRound == 2)
                {
                    if (currentChoiceContent_player1 == policy_two_positive_baidu_player1.content)
                    {
                        currentChoiceReputation_player1 = PolicyReputation.Positive;
                    }
                    else
                    {
                        currentChoiceReputation_player1 = PolicyReputation.Negative;
                    }

                    if (currentChoiceContent_player2 == policy_two_positive_baidu_player2.content)
                    {
                        currentChoiceReputation_player2 = PolicyReputation.Positive;
                    }
                    else
                    {
                        currentChoiceReputation_player2 = PolicyReputation.Negative;
                    }
                }
                else if (currentRound == 3)
                {
                    if (currentChoiceContent_player1 == policy_three_positive_baidu_player1.content)
                    {
                        currentChoiceReputation_player1 = PolicyReputation.Positive;
                    }
                    else
                    {
                        currentChoiceReputation_player1 = PolicyReputation.Negative;
                    }

                    if (currentChoiceContent_player2 == policy_three_positive_baidu_player2.content)
                    {
                        currentChoiceReputation_player2 = PolicyReputation.Positive;
                    }
                    else
                    {
                        currentChoiceReputation_player2 = PolicyReputation.Negative;
                    }
                }

                break;
            case CompanyName.XiaoMi:
                break;
            case CompanyName.Musk:
                break;
        }

        NextStep();
    }

    public void ShowThinkFrame(bool showOrNot)
    {
        thinkFrame_player1.SetActive(showOrNot);
        thinkFrame_player2.SetActive(showOrNot);
    }

    public void ExecuteWinnersPolicy(Winner roundWinner)  //执行每轮胜利者的提案
    {
        if(roundWinner == Winner.Player1)
        {
            if(currentChoiceReputation_player1 == PolicyReputation.Positive)
            {
                companyReputationValue += 1;

              
            }
            else
            {
                companyReputationValue -= 1;

             
            }

            if(currentRound == 1)
            {
                baiduSpaceEventTriggered = true;

                if (currentChoiceReputation_player1 == PolicyReputation.Positive)
                {
                    goodEnd_baiduSpace = true;
                }
                else
                {
                    goodEnd_baiduSpace = false;
                }
            }
        }
        else  //roundWinner == Winner.Player2
        {
            if (currentChoiceReputation_player2 == PolicyReputation.Positive)
            {
                companyReputationValue += 1;
            }
            else
            {
                companyReputationValue -= 1;
            }

            if(currentRound == 2)
            {
                weiZeXiEventTriggered = true;

                if (currentChoiceReputation_player2 == PolicyReputation.Positive)
                {
                    goodEnd_weiZeXi = true;
                }
                else
                {
                    goodEnd_weiZeXi = false;
                }
            }
        }

        

        refreshReputation();
    }

    void refreshReputation()
    {
        switch (companyReputationValue)
        {
            case 3:
                companyReputation = CompanyReputation.OverwhelminglyPositive;
                break;
            case 2:
                companyReputation = CompanyReputation.MostlyPositive;
                break;
            case 1:
                companyReputation = CompanyReputation.Positive;
                break;
            case 0:
                companyReputation = CompanyReputation.Mixed;
                break;
            case -1:
                companyReputation = CompanyReputation.Negative;
                break;
            case -2:
                companyReputation = CompanyReputation.MostlyNegative;
                break;
            case -3:
                companyReputation = CompanyReputation.OverwhelminglyNegative;
                break;
        }
    }

    public void PublishCompanyResult()  //进入公司运营结算阶段
    {
        //先公布触发的事件
        StartCoroutine(TriggerCompanyEvent());

        //再公布最终声望值运营结果
    }

    
    IEnumerator TriggerCompanyEvent()
    {
        if(companyName == CompanyName.Baidu)
        {
            if (baiduSpaceEventTriggered)
            {
                if (goodEnd_baiduSpace)
                {
                    //show baiduspace good end
                    StartCoroutine(ShowEventImage(image_closeBaiduSpace, 5, 1f));
                }
                else
                {
                    StartCoroutine(ShowEventImage(image_keepBaiduSpace, 5, 1f));
                }
            }

            if (weiZeXiEventTriggered)
            {
                if (goodEnd_weiZeXi)            // Good end
                {
                    if (baiduSpaceEventTriggered)
                    {
                        while (imageEventNotFinisedYet)
                        {
                            yield return null;
                        }
                        StartCoroutine(ShowEventImage(image_weiZeXiLive, 5, 1f));
                    }
                    else  //未触发百度空间事件
                    {
                        StartCoroutine(ShowEventImage(image_weiZeXiLive, 5, 1f));
                    }
                }
                else   // Bad End
                {
                    if (baiduSpaceEventTriggered)
                    {
                        while (imageEventNotFinisedYet)
                        {
                            yield return null;
                        }
                        StartCoroutine(ShowEventImage(image_weiZeXiDead, 5, 1f));
                    }
                    else  //未触发百度空间事件
                    {
                        StartCoroutine(ShowEventImage(image_weiZeXiDead, 5, 1f));
                    }
                }
                
                
            }


            while (imageEventNotFinisedYet)
            {
                yield return null;
            }
            switch (companyReputation)
            {
                case CompanyReputation.OverwhelminglyPositive:
                    StartCoroutine(ShowEventImage(image_OverPostive, 10, 1f));

                    break;
                case CompanyReputation.MostlyPositive:
                    StartCoroutine(ShowEventImage(image_MostPostive, 10, 1f));
                    break;
                case CompanyReputation.Positive:
                    StartCoroutine(ShowEventImage(image_Postive, 10, 1f));
                    break;
                case CompanyReputation.Mixed:
                    StartCoroutine(ShowEventImage(image_Mixed, 10, 1f));
                    break;
                case CompanyReputation.Negative:
                    StartCoroutine(ShowEventImage(image_Negative, 10, 1f));
                    break;
                case CompanyReputation.MostlyNegative:
                    StartCoroutine(ShowEventImage(image_mostNegative, 10, 1f));
                    break;
                case CompanyReputation.OverwhelminglyNegative:
                    StartCoroutine(ShowEventImage(image_OverNegative, 10, 1f));
                    break;
            }
        }
        yield break;
    }


    public bool imageEventNotFinisedYet = false;  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    IEnumerator ShowEventImage(Image targetImage, float duration, float fadeInOutDuration)
    {
        if(fadeInOutDuration > duration)
        {
            duration = 2 * fadeInOutDuration;
        }

        imageEventNotFinisedYet = true;

        float timer = 0;
        float lerp = 0;

        targetImage.gameObject.SetActive(true);
        targetImage.color = color_Transparent;

        while(timer <= duration)
        {
            timer += Time.deltaTime;
            if(timer <= fadeInOutDuration)
            {
                targetImage.color = Color.Lerp(color_Transparent, color_notTransparent, timer / fadeInOutDuration);
            }

            if(timer > (duration - fadeInOutDuration) && timer < duration)
            {
                lerp += (Time.deltaTime / fadeInOutDuration);
                
                targetImage.color = Color.Lerp(color_notTransparent, color_Transparent, lerp);
            }


            yield return null;
        }

        imageEventNotFinisedYet = false;
        yield break;
    }



    IEnumerator UnlockNextStepCountDown()   //选择提案倒计时
    {
        float timer = 0;
        canTriggerNextStep = false;

        while (timer <= 0.5)
        {
            timer += Time.deltaTime;

            yield return null;
        }
        canTriggerNextStep = true;
        yield break;
    }

    IEnumerator ThreeTwoOneCountDown()
    {
        yield return new WaitForSeconds(1f);
        sp_number3.gameObject.SetActive(true);
        StartCoroutine(Shake((sp_number3.transform), 0.25f, 0.2f));

        yield return new WaitForSeconds(0.8f);
        sp_number3.gameObject.SetActive(false);
        sp_number2.gameObject.SetActive(true);
        StartCoroutine(Shake((sp_number2.transform), 0.25f, 0.2f));

        yield return new WaitForSeconds(0.7f);
        sp_number2.gameObject.SetActive(false);
        sp_number1.gameObject.SetActive(true);
        StartCoroutine(Shake((sp_number1.transform), 0.25f, 0.2f));

        yield return new WaitForSeconds(0.5f);
        sp_number1.gameObject.SetActive(false);
        sp_letsGo.gameObject.SetActive(true);
        StartCoroutine(Shake((sp_letsGo.transform), 0.25f, 0.2f));

        yield return new WaitForSeconds(1.5f);
        sp_letsGo.gameObject.SetActive(false);


    }

    public IEnumerator Shake(Transform targetTransform, float duration, float magnitude)
    {
        Vector3 originalPos = targetTransform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            targetTransform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        targetTransform.localPosition = originalPos;

        yield break;
    }
}
