using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using System;



public class GameManger : MonoBehaviour
{


    // using inspector
    public PlayerMoving player;
    public CircleController circleController;
    public GameObject Restart_Button;
    public GameObject menuSet;
    public QusetBox questBox;
    public QuestManager questManager;

    public AudioController audio; //seongik 221221



    //Canvas setting
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Text talkText;
    public Image portraitImg;
    public GameObject storePanel;
    public StoreBuyText storeBuyText;
    public GameObject playerStatPanel;
    public PlayerStatText playerStatText;
    public GameObject HelpPanel;
    public Text playerLevelText;
    public Text playerHpText;
    public GameObject barricadeobj; //성익 수정
    public GameObject SearchPanel;
    public GameObject UIPanel;


    public GameObject PortalMessagePanel;

    // Inspector hide
    [HideInInspector] GameObject GameSavePanel;
    [HideInInspector] public static int StageIndex;
    [HideInInspector] public bool isSlow;
    [HideInInspector] public GameObject scanObject;
    [HideInInspector] public bool isTalkPanelActive;
    [HideInInspector] public bool isStoreActive;
    [HideInInspector] public bool isPlayerStatPanel;
    [HideInInspector] public bool isHelpPanel;
    [HideInInspector] public int talkIndex;    
    [HideInInspector]public int AttackPoint_Price = 10;
    [HideInInspector]public int HpPoint_Price = 10;
    [HideInInspector]public int CircleSpeed_Price = 100;
    [HideInInspector]public int CircleSize_Price = 100;
    [HideInInspector]public int CircleCount_Price = 1000;
    [HideInInspector]int[] SceneKey = new int[3];
    [HideInInspector]int[] PaperNo = new int[10];
    [HideInInspector]int[] StoneNo = new int[10];
    [HideInInspector]bool isMeat = false;
    private static float PlayTime;
    public static int isLoad;
    public static Text tx1, tx2;
    public static string Save1Text, Save2Text;
    public static int quest_var1 = 0;
    void Start()
    {
        // timer = 0; //성익
        // waitingTime = 200; //성익
        if(isLoad == 0)
        {
            isLoad = 5;
        }
        else if (isLoad == 1)
        {
            GameLoad1();
            isLoad = 5;
            playerStatText.StatTextUpLoad();
        }
        else if (isLoad == 2)
        {
            GameLoad2();
            isLoad = 5;
            playerStatText.StatTextUpLoad();
        }
        else if(isLoad == 5)
        {
            SceneLoad();
            if (StageIndex == 1)
            {
                GameObject store = GameObject.FindWithTag("Store");
                player.transform.position = store.transform.position;
            }
            playerStatText.StatTextUpLoad();           
        }
        else
        {
            isLoad = 5;
        }
    }
    private void Awake()
    {
        playerLevelText.text = "Lv . " + player.PlayerLevel.ToString();
        playerHpText.text = player.CurrentHp.ToString() + " / " + player.PlayerHp.ToString();
        isSlow = false;
        isPlayerStatPanel = false;
        if (StageIndex == 1)
        {
            HelpPanel.gameObject.SetActive(true);
        }

        GameSavePanel = menuSet.transform.GetChild(3).gameObject; //1218 WJ
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            MenuSetButton();
        }
        PlayTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        playerLevelText.text = "Lv . " + player.PlayerLevel.ToString();
        playerHpText.text = player.CurrentHp.ToString() + " / " + player.PlayerHp.ToString();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerReposition();
            HealthDown(10); //���� 10
        }
    }
  
    public void HealthDown(float damage)
    {
        if (player.CurrentHp > 0)
        {
            player.CurrentHp -= damage;
            audio.PlaySound("PLAYERDAMAGED"); //seongik 221221
            if(player.CurrentHp <= 0) 
            {
                player.OnDie();
                audio.PlaySound("DIE"); //seongik 221221
                Text buttonText = Restart_Button.GetComponentInChildren<Text>();
                Restart_Button.SetActive(true);
            }
        }
        else
        {
            player.CurrentHp -= damage;
            player.OnDie();
            audio.PlaySound("DIE"); //seongik 221221
            Text buttonText = Restart_Button.GetComponentInChildren<Text>();
            Restart_Button.SetActive(true);
        }
    }
    public void PlayerReposition()
    {
        player.transform.position=new Vector3(0, 0, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        player.CurrentHp = player.PlayerHp;
        SceneSave();
        SceneManager.LoadScene(StageIndex);
    }
    public void MenuSetStart()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }
    public void GameSave_1()
    {
        PlayerPrefs.SetFloat("Save1_PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("Save1_PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("Save1_PlayerLv", player.PlayerLevel);
        PlayerPrefs.SetFloat("Save1_PlayerCurrentExp", player.CurrentExp);
        PlayerPrefs.SetFloat("Save1_PlayerAtKDMG", player.PlayerAtkDmg);
        PlayerPrefs.SetFloat("Save1_PlayerHp", player.PlayerHp);
        PlayerPrefs.SetFloat("Save1_PlayerCurrentHp", player.CurrentHp);
        PlayerPrefs.SetInt("Save1_SceneIndex", StageIndex);
        PlayerPrefs.SetInt("Save1_AbilityPoint", player.AbilityPoint);
        PlayerPrefs.SetFloat("Save1_CircleSpeed", CircleController.AngleSpeed);
        PlayerPrefs.SetFloat("Save1_CircleSize", CircleController.CircleSize_var);
        PlayerPrefs.SetInt("Save1_CircleCount", CircleController.circleCount);
        PlayerPrefs.SetInt("Save1_AttackPointPrice", AttackPoint_Price);
        PlayerPrefs.SetInt("Save1_HpPointPrice", HpPoint_Price);
        PlayerPrefs.SetInt("Save1_CircleSpeedPrice", CircleSpeed_Price);
        PlayerPrefs.SetInt("Save1_CircleSizePrice", CircleSize_Price);
        PlayerPrefs.SetInt("Save1_CircleCountPrice", CircleCount_Price);
        PlayerPrefs.SetInt("Save1_Money", player.Money);
        PlayerPrefs.SetFloat("Save1_Time", PlayTime);
        string strArr = "";  // SceneKey data save with string data
        for (int i = 0; i < SceneKey.Length; i++) 
        {
            strArr = strArr + SceneKey[i];
            if (i < SceneKey.Length - 1) 
            {
                strArr = strArr + ",";
            }
        }
        PlayerPrefs.SetString("Save1_SceneKeyData", strArr); 
       
        float playTimeMinute = Mathf.Round(PlayTime/60);
        tx1 = GameSavePanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        tx1.text = "<SAVE 1>\r\nLevel : " + player.PlayerLevel.ToString() + "     Coin : " + player.Money.ToString() + "\r\nPlay Time : "
            + playTimeMinute.ToString()+"m";
        Save1Text = tx1.text;
        PlayerPrefs.SetString("Save1_SaveText", Save1Text);

        PlayerPrefs.Save();

        menuSet.transform.GetChild(3).gameObject.SetActive(false);       
    }
    public void GameSave_2()
    {
        PlayerPrefs.SetFloat("Save2_PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("Save2_PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("Save2_PlayerLv", player.PlayerLevel);
        PlayerPrefs.SetFloat("Save2_PlayerCurrentExp", player.CurrentExp);
        PlayerPrefs.SetFloat("Save2_PlayerAtKDMG", player.PlayerAtkDmg);
        PlayerPrefs.SetFloat("Save2_PlayerHp", player.PlayerHp);
        PlayerPrefs.SetFloat("Save2_PlayerCurrentHp", player.CurrentHp);
        PlayerPrefs.SetInt("Save2_SceneIndex", StageIndex);
        PlayerPrefs.SetInt("Save2_AbilityPoint", player.AbilityPoint);
        PlayerPrefs.SetFloat("Save2_CircleSpeed", CircleController.AngleSpeed);
        PlayerPrefs.SetFloat("Save2_CircleSize", CircleController.CircleSize_var);
        PlayerPrefs.SetInt("Save2_CircleCount", CircleController.circleCount);
        PlayerPrefs.SetInt("Save2_AttackPointPrice", AttackPoint_Price);
        PlayerPrefs.SetInt("Save2_HpPointPrice", HpPoint_Price);
        PlayerPrefs.SetInt("Save2_CircleSpeedPrice", CircleSpeed_Price);
        PlayerPrefs.SetInt("Save2_CircleSizePrice", CircleSize_Price);
        PlayerPrefs.SetInt("Save2_CircleCountPrice", CircleCount_Price);
        PlayerPrefs.SetInt("Save2_Money", player.Money);
        PlayerPrefs.SetFloat("Save1_Time", PlayTime);

        string strArr = "";  // SceneKey data save with string data
        for (int i = 0; i < SceneKey.Length; i++)
        {
            strArr = strArr + SceneKey[i];
            if (i < SceneKey.Length - 1)
            {
                strArr = strArr + ",";
            }
        }
        PlayerPrefs.SetString("Save2_SceneKeyData", strArr);


        float playTimeMinute = Mathf.Round(PlayTime / 60);
        tx2 = GameSavePanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        tx2.text = "<SAVE 2>\r\nLevel : " + player.PlayerLevel.ToString() + "     Coin : " + player.Money.ToString() + "\r\nPlay Time : "
            + playTimeMinute.ToString() + "m";
        Save2Text = tx2.text;
        PlayerPrefs.SetString("Save2_SaveText", Save2Text);

        PlayerPrefs.Save();

        menuSet.transform.GetChild(3).gameObject.SetActive(false);
    }
    public void GameLoad1()
    {
        float x = PlayerPrefs.GetFloat("Save1_PlayerX");
        float y = PlayerPrefs.GetFloat("Save1_PlayerY");
        int PlayerLevel= PlayerPrefs.GetInt("Save1_PlayerLv");
        float CurrentExp = PlayerPrefs.GetFloat("Save1_PlayerCurrentExp");
        float PlayerAtkDmg = PlayerPrefs.GetFloat("Save1_PlayerAtKDMG");
        float PlayerHp = PlayerPrefs.GetFloat("Save1_PlayerHp");
        float CurrentHp = PlayerPrefs.GetFloat("Save1_PlayerCurrentHp");
        int abilityPoint = PlayerPrefs.GetInt("Save1_AbilityPoint");
        float circleSpeed = PlayerPrefs.GetFloat("Save1_CircleSpeed");
        float circleSize = PlayerPrefs.GetFloat("Save1_CircleSize");
        int circleCount = PlayerPrefs.GetInt("Save1_CircleCount");
        int attackPointPrice = PlayerPrefs.GetInt("Save1_AttackPointPrice");
        int HpPointPrice = PlayerPrefs.GetInt("Save1_HpPointPrice");
        int circleSpeedPrice = PlayerPrefs.GetInt("Save1_CircleSpeedPrice");
        int circleSizePrice = PlayerPrefs.GetInt("Save1_CircleSizePrice");
        int circleCountPrice = PlayerPrefs.GetInt("Save1_CircleCountPrice");
        float playtime = PlayerPrefs.GetFloat("Save1_Time");

        string[] dataArr = PlayerPrefs.GetString("Save1_SceneKeyData").Split(','); 
        int[] sceneKey = new int[dataArr.Length]; 
        for (int i = 0; i < dataArr.Length; i++)
        {
            sceneKey[i] = System.Convert.ToInt32(dataArr[i]); 
        }
        SceneKey = sceneKey;
        StageIndex = PlayerPrefs.GetInt("Save1_SceneIndex");
        int Money = PlayerPrefs.GetInt("Save1_Money");

        player.transform.position = new Vector3(x, y, 0);
        player.PlayerLevel = PlayerLevel;
        player.CurrentExp = CurrentExp;
        player.PlayerAtkDmg = PlayerAtkDmg;
        player.PlayerHp = PlayerHp;
        player.CurrentHp = CurrentHp;
        player.AbilityPoint = abilityPoint;
        player.Money = Money;
        CircleController.AngleSpeed = circleSpeed;
        CircleController.CircleSize_var = circleSize;
        CircleController.circleCount = circleCount;
        AttackPoint_Price = attackPointPrice;
        HpPoint_Price = HpPointPrice;
        CircleSpeed_Price = circleSpeedPrice;
        CircleSize_Price = circleSizePrice;
        CircleCount_Price = circleCountPrice;
        PlayTime = playtime;
    }
    public void GameLoad2()
    {
        float x = PlayerPrefs.GetFloat("Save2_PlayerX");
        float y = PlayerPrefs.GetFloat("Save2_PlayerY");
        int PlayerLevel = PlayerPrefs.GetInt("Save2_PlayerLv");
        float CurrentExp = PlayerPrefs.GetFloat("Save2_PlayerCurrentExp");
        float PlayerAtkDmg = PlayerPrefs.GetFloat("Save2_PlayerAtKDMG");
        float PlayerHp = PlayerPrefs.GetFloat("Save2_PlayerHp");
        float CurrentHp = PlayerPrefs.GetFloat("Save2_PlayerCurrentHp");
        int abilityPoint = PlayerPrefs.GetInt("Save2_AbilityPoint");
        float circleSpeed = PlayerPrefs.GetFloat("Save2_CircleSpeed");
        float circleSize = PlayerPrefs.GetFloat("Save2_CircleSize");
        int circleCount = PlayerPrefs.GetInt("Save2_CircleCount");
        int attackPointPrice = PlayerPrefs.GetInt("Save2_AttackPointPrice");
        int HpPointPrice = PlayerPrefs.GetInt("Save2_HpPointPrice");
        int circleSpeedPrice = PlayerPrefs.GetInt("Save2_CircleSpeedPrice");
        int circleSizePrice = PlayerPrefs.GetInt("Save2_CircleSizePrice");
        int circleCountPrice = PlayerPrefs.GetInt("Save2_CircleCountPrice");
        float playtime = PlayerPrefs.GetFloat("Save2_Time");

        string[] dataArr = PlayerPrefs.GetString("Save2_SceneKeyData").Split(',');
        int[] sceneKey = new int[dataArr.Length];
        for (int i = 0; i < dataArr.Length; i++)
        {
            sceneKey[i] = System.Convert.ToInt32(dataArr[i]);
        }
        SceneKey = sceneKey;
        StageIndex = PlayerPrefs.GetInt("Save2_SceneIndex");
        int Money = PlayerPrefs.GetInt("Save2_Money");

        player.transform.position = new Vector3(x, y, 0);
        player.PlayerLevel = PlayerLevel;
        player.CurrentExp = CurrentExp;
        player.PlayerAtkDmg = PlayerAtkDmg;
        player.PlayerHp = PlayerHp;
        player.CurrentHp = CurrentHp;
        player.AbilityPoint = abilityPoint;
        player.Money = Money;
        CircleController.AngleSpeed = circleSpeed;
        CircleController.CircleSize_var = circleSize;
        CircleController.circleCount = circleCount;
        AttackPoint_Price = attackPointPrice;
        HpPoint_Price = HpPointPrice;
        CircleSpeed_Price = circleSpeedPrice;
        CircleSize_Price = circleSizePrice;
        CircleCount_Price = circleCountPrice;
        PlayTime = playtime;

    }
    public void GameSaveTextDataLoad()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Text text1 = canvas.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        Text text2 = canvas.transform.GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        string Save1_Text = PlayerPrefs.GetString("Save1_SaveText");
        string Save2_Text = PlayerPrefs.GetString("Save2_SaveText");
        if(Save1_Text == null)
        {
            text1.text = "No Data";
        }
        else
        {
            text1.text = Save1_Text;
        }
        if (Save2_Text == null)
        {
            text2.text = "No Data";
        }
        else
        {
            text2.text =  Save2_Text; ;
        }
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        StageIndex = 0;
        SceneManager.LoadScene(StageIndex);
        //Application.Quit();
    }
    public void SceneSave()
    {
        PlayerPrefs.SetInt("PlayerLv", player.PlayerLevel);
        PlayerPrefs.SetFloat("PlayerCurrentExp", player.CurrentExp);
        PlayerPrefs.SetFloat("PlayerAtKDMG", player.PlayerAtkDmg);
        PlayerPrefs.SetFloat("PlayerHp", player.PlayerHp);
        PlayerPrefs.SetFloat("PlayerCurrentHp", player.CurrentHp);
        PlayerPrefs.SetInt("AbilityPoint", player.AbilityPoint);
        PlayerPrefs.SetFloat("CircleSpeed", CircleController.AngleSpeed);
        PlayerPrefs.SetFloat("CircleSize", CircleController.CircleSize_var);
        PlayerPrefs.SetInt("CircleCount", CircleController.circleCount);
        PlayerPrefs.SetInt("AttackPointPrice", AttackPoint_Price);
        PlayerPrefs.SetInt("HpPointPrice", HpPoint_Price);
        PlayerPrefs.SetInt("CircleSpeedPrice", CircleSpeed_Price);
        PlayerPrefs.SetInt("CircleSizePrice", CircleSize_Price);
        PlayerPrefs.SetInt("CircleCountPrice", CircleCount_Price);
        PlayerPrefs.SetInt("Money", player.Money);

        string strArr = ""; //   SceneKey data save
        for (int i = 0; i < SceneKey.Length; i++) // SceneKey data save with string data
        {
            strArr = strArr + SceneKey[i];
            if (i < SceneKey.Length - 1) // �
            {
                strArr = strArr + ",";
            }
        }
        PlayerPrefs.SetString("SceneKeyData", strArr); 
        PlayerPrefs.Save();
    }
    public void SceneLoad()
    {      
        int PlayerLevel = PlayerPrefs.GetInt("PlayerLv");
        float CurrentExp = PlayerPrefs.GetFloat("PlayerCurrentExp");
        float PlayerAtkDmg = PlayerPrefs.GetFloat("PlayerAtKDMG");
        float PlayerHp = PlayerPrefs.GetFloat("PlayerHp");
        float CurrentHp = PlayerPrefs.GetFloat("PlayerCurrentHp");
        int abilityPoint = PlayerPrefs.GetInt("AbilityPoint");
        float circleSpeed = PlayerPrefs.GetFloat("CircleSpeed");
        float circleSize = PlayerPrefs.GetFloat("CircleSize");
        int circleCount = PlayerPrefs.GetInt("CircleCount");
        int attackPointPrice = PlayerPrefs.GetInt("AttackPointPrice");
        int HpPointPrice = PlayerPrefs.GetInt("HpPointPrice");
        int circleSpeedPrice = PlayerPrefs.GetInt("CircleSpeedPrice");
        int circleSizePrice = PlayerPrefs.GetInt("CircleSizePrice");
        int circleCountPrice = PlayerPrefs.GetInt("CircleCountPrice");

        string[] dataArr = PlayerPrefs.GetString("SceneKeyData").Split(','); 
        int[] sceneKey = new int[dataArr.Length]; // scenekey data load
        for (int i = 0; i < dataArr.Length; i++)
        {
            sceneKey[i] = System.Convert.ToInt32(dataArr[i]); 
        }
        SceneKey = sceneKey;

        int Money = PlayerPrefs.GetInt("Money");
     
        player.PlayerLevel = PlayerLevel;
        player.CurrentExp = CurrentExp;
        player.PlayerAtkDmg = PlayerAtkDmg;
        player.PlayerHp = PlayerHp;
        player.CurrentHp = CurrentHp;
        player.AbilityPoint = abilityPoint;
        player.Money = Money;
        CircleController.AngleSpeed = circleSpeed;
        CircleController.CircleSize_var = circleSize;
        CircleController.circleCount = circleCount;
        AttackPoint_Price = attackPointPrice;
        HpPoint_Price = HpPointPrice;
        CircleSpeed_Price = circleSpeedPrice;
        CircleSize_Price = circleSizePrice;
        CircleCount_Price = circleCountPrice;

    }
    public void NextStage()
    {
        StageIndex = 1;
        SceneSave();
        SceneManager.LoadScene(StageIndex);

    }
    public void GameStartButton()
    {
        isLoad = 0;
        StageIndex = 1;

        SceneManager.LoadScene(StageIndex);
    }

    public void GameSaveButton()
    {
        //menuSet.transform.GetChild(3).gameObject.SetActive(true);
        GameSavePanel.gameObject.SetActive(true);
        Text text1 = GameSavePanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        Text text2 = GameSavePanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        if(Save1Text == null)
        {
            text1.text = "Save 1";
        }
        else
        {
            text1.text = Save1Text;
        }
        if (Save2Text == null)
        {
            text2.text = "Save 2";
        }
        else
        {
            text2.text = Save2Text;
        }
    }
   
    public void GameLoad1Button()
    {
        isLoad = 1;
        int SceneIndex = PlayerPrefs.GetInt("Save1_SceneIndex");
        StageIndex = SceneIndex;

        SceneManager.LoadScene(StageIndex);
    }
    public void GameLoad2Button()
    {
        isLoad = 2;
        int SceneIndex = PlayerPrefs.GetInt("Save2_SceneIndex");
        StageIndex = SceneIndex;

        SceneManager.LoadScene(StageIndex);
    }
    public void ItemSlowSkill()
    {
        isSlow = true;
        Invoke("ItemSlowSkillEnd", 20f);
    }
    public void ItemSlowSkillEnd()
    {
        isSlow = false;
    }
    public void SearchAction(GameObject scan_Object)
    {
        scanObject = scan_Object;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        if (objData.isNpc == true)
        {
            //Talk(objData.id, objData.isNpc);
            //talkPanel.SetActive(isTalkPanelActive);
            if (objData.id == 20)
            {
                if (quest_var1 == 0)
                {                  
                    if (isMeat == true && isTalkPanelActive == false && talkIndex == 0 && questManager.questActionIndex < 3)
                    {
                        questManager.questActionIndex++;
                    }
                    QuestTalk(objData.id, objData.isNpc);
                    talkPanel.SetActive(isTalkPanelActive);

                    if (objData.id == 20 && questManager.questActionIndex == 2 && quest_var1 == 0)
                    {
                        quest_var1 = 1;
                        GameObject item_circle = Instantiate(questBox.Item_1);
                        Vector3 vec1 = new Vector2(-1f, 3f);
                        item_circle.transform.position = scanObject.transform.position + vec1;
                        Rigidbody2D itemRigid = item_circle.GetComponent<Rigidbody2D>();
                        Vector2 vec2 = new Vector2(-2, 8);
                        itemRigid.AddForce(vec2, ForceMode2D.Impulse);
                        item_circle.SetActive(true);
                    }
                }
                else
                {
                    questManager.questId = 13;
                    QuestTalk(objData.id, objData.isNpc);
                    talkPanel.SetActive(isTalkPanelActive);
                }
            }
            if (objData.id == 30)
            {
                if(questManager.questId != 20 || questManager.questId != 21 || questManager.questId != 22)
                {
                    questManager.questId = 20;
                    questManager.questActionIndex = 0;
                }
                QuestTalk(objData.id, objData.isNpc);
                talkPanel.SetActive(isTalkPanelActive);            
            }
        }
        else if (objData.QuestItem == true)
        {
            if (objData.id == 1100)
            {
                Talk(objData.id, objData.isNpc);
                talkPanel.SetActive(isTalkPanelActive);
                CircleController.circleCount = 2;
                scanObject.SetActive(false);
            }
            if (objData.id == 1101)
            {
                isMeat = true;
                scanObject.gameObject.SetActive(false);
            }
        }
        else if (objData.isStore == true)
        {   
            if(objData.id == 200)
            {
                OpenStore();
            }
        }
        else if (objData.isKey == true)
        {
            if(objData.id == 602)
            {
                Talk(objData.id, objData.isNpc);
                talkPanel.SetActive(isTalkPanelActive);
                SceneKey[0] = 1;
                audio.PlaySound("BossKey");
                scanObject.SetActive(false);
            }
            if (objData.id == 603)
            {
                Talk(objData.id, objData.isNpc);
                talkPanel.SetActive(isTalkPanelActive);
                SceneKey[1] = 1;
                audio.PlaySound("BossKey");
                scanObject.SetActive(false);
            }
            if (objData.id == 604)
            {
                Talk(objData.id, objData.isNpc);
                talkPanel.SetActive(isTalkPanelActive);
                SceneKey[2] = 1;
                audio.PlaySound("BossKey");
                scanObject.SetActive(false);
            }
        }
        else if (objData.isSign == true) //성익
        {
            Talk(objData.id, objData.isNpc);
            talkPanel.SetActive(isTalkPanelActive);          
        }
        else if (objData.isDoor == true)
        {
            if (objData.id == 702)
            {
                if (SceneKey[0] == 1)
                {
                    audio.PlaySound("DoorOpen");
                    Talk(objData.id, objData.isNpc);
                    talkPanel.SetActive(isTalkPanelActive);
                    OpenFinalSceneDoor(scanObject);
                }
                else
                {
                    Talk(705, false);
                    talkPanel.SetActive(isTalkPanelActive);
                }
            }
            if (objData.id == 703)
            {
                if (SceneKey[1] == 1)
                {
                    audio.PlaySound("DoorOpen");
                    Talk(objData.id, objData.isNpc);
                    talkPanel.SetActive(isTalkPanelActive);
                    OpenFinalSceneDoor(scanObject);
                }
                else
                {
                    Talk(705, false);
                    talkPanel.SetActive(isTalkPanelActive);
                }
            }
            if (objData.id == 704)
            {
                if (SceneKey[2] == 1)
                {
                    audio.PlaySound("BossDoorOpen");
                    Talk(objData.id, objData.isNpc);
                    talkPanel.SetActive(isTalkPanelActive);
                    OpenFinalSceneDoor(scanObject);
                }
                else
                {
                    Talk(705, false);
                    talkPanel.SetActive(isTalkPanelActive);
                }
            }
            if (objData.id == 901) //성익
            {
                if (PaperNo[0] == 1 && PaperNo[1] == 1)
                {
                    Debug.Log("check");
                    Animator anim = objData.GetComponent<Animator>();
                    anim.SetTrigger("Blockbreak");
                    StartCoroutine("Breakdown");
                }
            }
            if (objData.id == 902) //성익
            {
                if (PaperNo[2] == 1 && PaperNo[3] == 1)
                {
                    Animator anim = objData.GetComponent<Animator>();
                    anim.SetTrigger("Blockbreak");
                    StartCoroutine("Breakdown");
                }
            }
        }
        else if(objData.isTeleport == true)
        {
            if(objData.id== 500)
            {
                audio.PlaySound("TELEPORT");//1221 from jeongik
                StageIndex = 2;
                SceneSave();
                SceneManager.LoadScene(2);
            }
            else if (objData.id == 501)
            {
                audio.PlaySound("TELEPORT");//1221 from jeongik
                StageIndex = 3;
                SceneSave();
                SceneManager.LoadScene(3);
            }
            else if (objData.id == 502)
            {
                audio.PlaySound("TELEPORT");//1221 from jeongik
                StageIndex = 4;
                SceneSave();
                SceneManager.LoadScene(4);
            }
            if (objData.id == 503)
            {
                audio.PlaySound("BossPortal");//1221 from jeongik
                StageIndex = 5;
                SceneSave();
                SceneManager.LoadScene(5);
            }
            if (objData.id == 505)
            {
                StageIndex = 1;
                SceneSave();
                SceneManager.LoadScene(1);
            }
        }
        else if(objData.isPaper == true) //성익
        {
            if(objData.id == 801)
            {
                audio.PlaySound("PickItem");
                Debug.Log("ok");
                Debug.Log(scanObject);
                //Talk(objData.id, objData.isNpc);
                //talkPanel.SetActive(isTalkPanelActive);
                PaperNo[0] = 1;
                scanObject.SetActive(false);
            }
            else if(objData.id == 802)
            {
                audio.PlaySound("PickItem");
                PaperNo[1] = 1;
                scanObject.SetActive(false);
            }
             else if(objData.id == 803)
            {
                audio.PlaySound("PickItem");
                PaperNo[2] = 1;
                scanObject.SetActive(false);
            }
             else if(objData.id == 804)
            {
                audio.PlaySound("PickItem");
                PaperNo[3] = 1;
                scanObject.SetActive(false);
            }
        }
        else if(objData.isStone == true) //성익
        {
            if(objData.id == 1001)
            {
                audio.PlaySound("PickItem");
                StoneNo[0] = 1;
                scanObject.SetActive(false);
                if(StoneNo[0] == 1 && StoneNo[1] == 1 && StoneNo[2] == 1)
                {
                    barricadeobj.SetActive(false);
                }
            }
            if(objData.id == 1002)
            {
                audio.PlaySound("PickItem");
                StoneNo[1] = 1;
                scanObject.SetActive(false);
                if(StoneNo[0] == 1 && StoneNo[1] == 1 && StoneNo[2] == 1)
                {
                    barricadeobj.SetActive(false);
                }
            }
            if(objData.id == 1003)
            {
                audio.PlaySound("PickItem");
                StoneNo[2] = 1;
                scanObject.SetActive(false);
                if(StoneNo[0] == 1 && StoneNo[1] == 1 && StoneNo[2] == 1)
                {
                    barricadeobj.SetActive(false);
                }
            }
        }
        //from jeongik
        else if (objData.isWaterMapObject == true)
        {   
            if(objData.id == 0)
            {
                //Debug.Log("watermapobject");
                WaterMapController.ScanObject(scanObject);
            }
            
        }
        else if (objData.isPortal == true) //WJ
        {
            if(objData.id == 510)
            {
                PortalMessagePanel.SetActive(true);
                scanObject.SetActive(false);
                SearchPanel.SetActive(false);               
            }
        }
    }
    public void Talk(int id, bool isNpc) // 1211 from jeongik
    {
        string talkData = talkManager.GetTalk(id, talkIndex);
        if (talkData == null)
        {
            isTalkPanelActive = false;
            talkIndex = 0;
            return;
        }
        if (isNpc)
        {
            talkText.text = talkData.Split(':')[0];
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        isTalkPanelActive = true;
        talkIndex++;
    }
    public void QuestTalk(int id, bool isNpc) // 1211 from jeongik
    {
        int questTalkIndex = questManager.GetQusetTalkIndex(id);
        string talkData = talkManager.GetTalk(id+ questTalkIndex, talkIndex);
        if (talkData == null)
        {
            isTalkPanelActive = false;
            talkIndex = 0;
            return;
        }
        if (isNpc)
        {
            talkText.text = talkData.Split(':')[0];
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        isTalkPanelActive = true;
        talkIndex++;
    }
    void OpenStore()
    {
        isStoreActive = true;
        storePanel.SetActive(true);
    }
    public void CloseStore()
    {
        isStoreActive = false;
        storePanel.SetActive(false);
    }
    public void BuyItem(string whatItem)
    {
        switch (whatItem)
        {
            case "ATTACKPOINT":
                if (player.Money >= AttackPoint_Price)
                {
                    player.Money -= AttackPoint_Price;
                   
                    if (AttackPoint_Price >= 100)
                    {
                        AttackPoint_Price += 100;
                    }
                    else
                    {
                        AttackPoint_Price += 10;
                    }
                    storeBuyText.Buy_ATTACKPOINT_Text();
                    player.PlayerAtkDmg += 1;
                    playerStatText.PlayerATK_Text();
                    playerStatText.PlayerTotalDmg_Text();
                    playerStatText.PlayerMoney_Text();
                    audio.PlaySound("LEVELUP");
                }
                break;
            case "HPPOINT":
                if (player.Money >= HpPoint_Price)
                {
                    player.Money -= HpPoint_Price;                    
                    if(HpPoint_Price >= 100)
                    {
                        HpPoint_Price +=100;
                    }
                    else
                    {
                        HpPoint_Price += 10;
                    }
                    storeBuyText.Buy_HPPOINT_Text();                   
                    player.PlayerHp += 10;
                    player.CurrentHp += 10;
                    player.HPUP();
                    playerStatText.PlayerHP_Text();
                    playerStatText.PlayerMoney_Text();
                    audio.PlaySound("LEVELUP");
                }
                break;
            case "CIRCLESPEED":
                if (player.Money >= CircleSpeed_Price)
                {
                    player.Money -= CircleSpeed_Price;
                    CircleSpeed_Price += CircleSpeed_Price;
                    storeBuyText.Buy_CircleSpeed_Text();
                    CircleController.AngleSpeed += 0.5f; //1215 from jeongik
                    playerStatText.CircleSpeed_Text();
                    playerStatText.PlayerMoney_Text();
                    audio.PlaySound("LEVELUP");
                }
                break;
            case "CIRCLESIZE":
                if (player.Money >= CircleSize_Price)
                {
                    player.Money -= CircleSize_Price;
                    CircleSize_Price += CircleSize_Price;
                    storeBuyText.Buy_CircleSize_Text();
                    CircleController.CircleSize_var += 0.2f; // 1215 from jeongik
                    playerStatText.CircleSize_Text();
                    playerStatText.PlayerMoney_Text();
                    audio.PlaySound("LEVELUP");
                }
                break;
            case "CIRCLECOUNT":  //1218 WJ
                if (player.Money >= CircleCount_Price)
                {
                    if (CircleController.circleCount == 2)
                    {
                        player.Money -= CircleCount_Price;
                        CircleCount_Price += CircleCount_Price;
                        storeBuyText.Buy_CircleCount_Text();
                        CircleController.circleCount += 1; 
                        playerStatText.CircleCount_Text();
                        playerStatText.PlayerMoney_Text();
                        audio.PlaySound("LEVELUP");
                    }
                    else if (CircleController.circleCount == 3)
                    {
                        player.Money -= CircleCount_Price;
                        CircleCount_Price = 9999999;
                        storeBuyText.Buy_CircleCount_Text();
                        CircleController.circleCount += 1; 
                        playerStatText.CircleCount_Text();
                        playerStatText.PlayerMoney_Text();
                        audio.PlaySound("LEVELUP");
                    }
                    else if (CircleController.circleCount == 4)
                    {        
                        
                    }
                }
                break;

        }
    }
    public void Buy_ATTACKPOINT_Button()   // 상점 구매시 누르는 버튼
    {
        BuyItem("ATTACKPOINT");
    }
    public void Buy_HPPOINT_Button()
    {
        BuyItem("HPPOINT");
    }
    public void Buy_CircleSpeed_Button()  
    {
        BuyItem("CIRCLESPEED");
    }
    public void Buy_CircleSize_Button()
    {
        BuyItem("CIRCLESIZE");
    }
    public void Buy_CircleCount_Button()
    {
        BuyItem("CIRCLECOUNT");
    }
    public void OpenPlayerStat()   // 스탯 오픈 관련 
    {
        playerStatPanel.SetActive(true);
        isPlayerStatPanel = true;
    }
    public void ClosePlayerStat()
    {
        playerStatPanel.SetActive(false);
        isPlayerStatPanel = false;
    }
    public void PlayerStatButton()
    {
        if(isPlayerStatPanel == false)
        {
            OpenPlayerStat();
        }
        else
        {
            ClosePlayerStat();
        }
    }
    public void OpenHelp()   // 도움말 오픈
    {
        HelpPanel.SetActive(true);
        isHelpPanel = true;
    }
    public void CloseHelp()
    {
        HelpPanel.SetActive(false);
        isHelpPanel = false;
    }
    public void HelpButton()
    {
        if (isHelpPanel == false)
        {
            OpenHelp();
        }
        else
        {
            CloseHelp();
        }
    }
    public void MenuSetButton()
    {
        if (menuSet.activeSelf)
        {
            menuSet.transform.GetChild(3).gameObject.SetActive(false);
            menuSet.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            menuSet.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void OpenFinalSceneDoor(GameObject scanObject)
    {
        scanObject.SetActive(false);
    }
    IEnumerator Breakdown()
    {
        audio.PlaySound("ROCKBREAK");
        yield return new WaitForSeconds(2f);
        scanObject.SetActive(false);
    }
}
