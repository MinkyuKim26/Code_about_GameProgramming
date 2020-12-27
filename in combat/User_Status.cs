using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking; 
using System.Collections; 
using UnityEngine.SceneManagement;

using System.IO;
using UnityEngine;
using UnityEngine.UI;
using GoShared;

//I recommend You to initialize this class when you turn on the game. 
public class User_Status : MonoBehaviour
{
    public string user_name;//유저 이름
    public int level;//레벨
    public int userID;//유저 고유 ID. 추후 다른 방식으로 고유ID를 설정할 수 있다.
    public int money;//유저가 들고있는 돈
    public int now_hp;//유저 현재 체력
    public int max_hp;//유저 최대 체력
    public float max_exp;//레벨업에 필요한 경험치
    public float now_exp;//현재 경험치. 메세지 클래스에서 상승 시켜주는게 좋을듯 하다.
    public int att_point;//공격력, attack point
    public int def_point;//방어력, defence point

    // Start is called before the first frame update
    void Awake()
    {
        isReset = false;

        //Load File
        string Filestr = "";
        if(Application.platform == RuntimePlatform.Android)
        {
            Filestr = Application.persistentDataPath + "/Savedata.json";
        }
        else
        {
            Filestr = Application.dataPath + "/SaveData.json";
        }

        System.IO.FileInfo fi = new System.IO.FileInfo(Filestr);

        if(fi.Exists) 
        {
            LoadGameData(Filestr);
            isReset = true;
        }
        else
        //Application.persistentDataPath + "/Savedata.json" 경로에 파일이 없다. 그래서 계속 이쪽으로 넘어오는 것이다.
        {
            init_user_stat();
        }
        Save_UserData();
    }

    // Update is called once per frame
    void Update()
    {
        if(now_exp >= max_exp)
        {
            level_up();
        }    
    }

    void level_up()
    {
        isLevelUp = true;

        while(now_exp >= max_exp)
        {
            level++;
            now_exp -= max_exp;
            max_exp = 100 * Mathf.Pow(1.1f, (level - 1));
           
            max_hp = (int)Mathf.Round( 100 * Mathf.Pow(1.1f, level - 1) );
            now_hp = max_hp;//레벨업 하면 체력을 풀로 만들어줌.
            
            att_point++;
            def_point++;
        }
        Save_UserData();
    }

    void init_user_stat()
    {
        user_name = "test_name";
        level = 1;//레벨 받아오기
        userID = 0;//고유ID 받아오기
        now_exp = 0;//경험치 받아오기
         
        max_hp = (int)Mathf.Round(  100 * Mathf.Pow(1.1f, level - 1) );
        now_hp = .max_hp;

        att_point = 5 + (level - 1);//공격력 받아오기. 레벨과 무공에 따라 공격력이 결정됨.
        def_point = 5 + (level - 1);//방어력 받아오기. 레벨과 무공에 따라 방어력이 결정됨.

        money = 1000;//처음에 1000원 지급
        //Friend_Database.Add("friend");//친구목록 불러오기

        max_exp = 100 * Mathf.Pow(1.1f, (level - 1));//레벨업 하는데 필요한 경험치
    }
    
    public static void Save_UserData()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            string dataAsJson = JsonUtility.ToJson ();
            string filePath = Application.persistentDataPath + "/SaveData.json";
            File.WriteAllText (filePath, dataAsJson);
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/SaveData.json", JsonUtility.ToJson()); 
        }
    }

    void LoadGameData(string Filestr)
    {
        //DB에서 데이터를 받아올 때 쓰는 함수. 처음 게임에 들어온 유저는 init_user_stat()함수를 불러오도록 하자.
        
        string dataAsJson = File.ReadAllText(Filestr); 

        Debug.Log(dataAsJson);
        Debug.Log(dataAsJson != "");

        User_Status temp_class = JsonUtility.FromJson<User_Status>(dataAsJson);
        
        user_name = temp_class.user_name;
        level = temp_class.level;
        userID = temp_class.userID;
        money = temp_class.money;

        now_hp = temp_class.now_hp;
        max_hp = temp_class.max_hp;

        now_exp = temp_class.now_exp;
        max_exp = temp_class.max_exp;

        att_point = temp_class.att_point;
        def_point = temp_class.def_point;

        return temp_class;
    }
}
