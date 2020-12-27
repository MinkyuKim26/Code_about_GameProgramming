//Code for Combat
//This file is a code for combat with monster
//You can attach it any GameObject. I attached it to Empty Gameobject and name it to 'Combat_script'
//It is too easy code and you can understand very soon.


//Include(or import) part
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//How to Play in Combat mode 
    //1. spawning Monster and User. you(Programmer) can use any method to write code about User and Monster. 
    //2. After User and Monster are spawned in Scene, User take attack Turn First.
    //2-1. User should touch the 'Hitting_Point' in the Screen.(the skill button is moving in the Screen.) if user can not touch the Hitting_Point within the time you determine, user don't hit the monster.
    //2-2. User can user Skill. As I said before, Skill is special things. User can choose either normal attack or Skill. User can't choose both of them. you can change it. 
    //3. when attack Turn is finish, you are in defending turn. 
    //3-1. The Monster choose the number which is in range of 1 to 9, and express it in screen. you can see 9 buttons in the screen. the button 1~9 are here and the button of number which monster choose have another color.
    //3-2. User should touch the button of another color within the time,you determine, to defending monster's attack. if user can not do this, user get damage which monster give
    //4. repeat step 2 ~ 3 untill User or Monster has no HP point.
    //5. if the combat is finished, Combat_popup is created and give reward or penalty to User. the reward or penalty of result is up to you. 
    //I set 'if User win, user get exp point and item. but if Monster win, User lose exp point and set HP point to 50% of User's Full HP point'

public class combat_script : MonoBehaviour
{
    //Variables
    float turn_Counting_Time;//It is a time about turn. User and Monster take turns of attack and defending. 
    float turn_time;//The time of Turn. if 'turn_Counting_Time' is more than 'turn_time', turn is chaneged.
    bool isTimetoTurnChange;//Variable about turn. when turn_Counting_Time is more than the time i set to switching turn, it change to 'true'

    public static float Skill_cooltimeCount;//counting cooltime of Skill. Skill is special things. you choose the effect of skill. 
    float User_Skill_MaxCoolTime; //the CoolTime of Skill. 
    public static bool isSkillReady;//Variable about User's skill is ready. if 'Skill_cooltimeCount' is more than User_Skill_MaxCoolTime, it change to 'true'.
    
    //The spec of monster is in 'Monster' class. it is in 'Monser_class.cs'
    Monster monster;//It is just example code. You can choose How choose monstet's spec in another way

    int monster_attack_num;//The Number monster choose in User's defending Turn.

    public Text monster_damaged_text;//Show Damage which Monster get from user.
    public Text player_damaged_text;//Show Damage Which User get from monster.

    public GameObject Combat_popup;//the popup window which is created when combat is over

    //in attack Turn()
    public GameObject Hitting_Point;//The Button which is used in attack Turn.
    public GameObject Hitting_Point_MovingArea;//The Area where Hitting_Point move in
    public static bool isTouched_HittingPoint;//Check if User touch Hitting_Point in attack turn.

    public Text Combat_status_Text;//Show how combat is going(ex : evade monster's attack)
    
    void Start()
    {
        //Init
        turn_Counting_Time = 0.0f;
        turn_time = 0.75f; 

        isSkillReady = true;
        isTimetoTurnChange = false;

        Combat_popup.SetActive(false);
        isTouched_HittingPoint = false;

        User_Skill_MaxCoolTime = 3.0f;//I set the cooltime of skill to 3.0 sec.

        monster = new Monster(1);

        Hitting_Point_MovingArea.SetActive(false);
        monster_damaged_text.gameObject.SetActive(false);
        player_damaged_text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //The Text showing in the Screen. these are variable about turn.
        float Remaining_time_float;//Calculating the remaining Time.
        string Remaining_time_text;//Convert the float to string because only String variable can be displayed in Screen
        
        //When combat is not finished
        if(Combat_popup.active == false)
        {
            if(isSkillReady == false && Skill_cooltimeCount == 0) //When Skill is used(start conting cooltime)
            {
                Skill_button.transform.GetChild(0).GetComponent<Text>().text = "charging...";
                Skill_button.transform.GetComponent<Image>().color = new Color(0.8f, 0.4f, 0.4f);

                Skill_button.transform.GetComponent<Image>().fillAmount = Skill_cooltimeCount / User_Skill_MaxCoolTime;

                Skill_cooltimeCount+=Time.deltaTime;//counting time
            }
            else if(Skill_cooltimeCount >= User_Skill_MaxCoolTime)//When Skill's coolTime is over
            {
                isSkillReady = true;

                Skill_button.transform.GetComponent<Image>().fillAmount = 1.0f;
                Skill_button.transform.GetChild(0).GetComponent<Text>().text = "Skill";
                Skill_button.transform.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
            }
            else if(Skill_cooltimeCount > 0.0f && Skill_cooltimeCount < User_Skill_MaxCoolTime)//when counting colltime.
            {
                Skill_button.transform.GetComponent<Image>().fillAmount = Skill_cooltimeCount / User_Skill_MaxCoolTime;

                Skill_cooltimeCount+=Time.deltaTime;
            }

            Remaining_time_float = turn_time - turn_Counting_Time;//calculating the remaining Turn_time.

            if(Remaining_time_float < 0.0f) Remaining_time_float = 0.0f;//if turn_Counting_Time > turn_time, set Remaining time to 0 sec.

            //Monster_attack_signals : if user is in 'defence turn', monster choose number within 1 to 9. 'Monster_attack_signals' is the group of button which have numbers in range of 1 to 9.

            if(combat_button.isattackTurn)//if Player is in attack Turn
            {
                if(Monster_attack_signals.active != false) Monster_attack_signals.SetActive(false);//check if 'Monster_attack_signals' is active
                Attack_turn();

                Remaining_time_text = "Remaining attack Time\n" + Remaining_time_float.ToString("N2");//Show remaining time
                Remaining_time.text = Remaining_time_text;
            }
            else//if Player is in defending turn
            {
                if(Monster_attack_signals.active != true) Monster_attack_signals.SetActive(true);//check if 'Monster_attack_signals' is active
                Defence_turn();

                Remaining_time_text = "Remaining Defending Time\n" + Remaining_time_float.ToString("N2");//Show remaining time
                Remaining_time.text = Remaining_time_text;
            }

            //if attack or defence turn is over
            if(isTimetoTurnChange == true)
            {
                //variables about counting time are initialized
                turn_Counting_Time = 0.0f;
                isTimetoTurnChange = false;

                //If user is in attack turn, it changes to defence turn.
                if(combat_button.isattackTurn == true)
                {
                    player_damaged_text.text = "0";
                    monster_attack_num = Random_attack_num();

                    combat_button.isattackTurn = false;//change to defence turn

                    //removing 'HittingPoint'
                    GameObject temp = GameObject.Find("HittingPoint");
                    Destroy(temp);
                    Hitting_Point_MovingArea.SetActive(false);
                }
                //If user is in defence turn, it changes to attack turn.
                else
                {
                    monster_damaged_text.text = "0";
                    combat_button.isattackTurn = true;//change to attack turn
                    //initialize variables about defence
                    monster_attack_num = -1;
                    combat_button.WhatDefenceButtonispicked = 0;
                }  
            }
        }

        //After attack or defence turn is over, check if HP of user or montser is less than 0
        if(monster.HP < 0)
        {
            monster.HP = 0;
        }
        if(User_Status.now_hp <  0)
        {
            User_Status.now_hp = 0;
        }

        //if monster's HP is equals to 0 (user wins the combat)
        if(monster.HP == 0 && Combat_popup.active == false)
        {
            Combat_popup.SetActive(true);//create Combat_popup
            Combat_popup.transform.GetChild(0).GetComponent<Text>().text = "You win!";
            
            //set rewards
            int added_exp = (monster.level - 1) * 5;
            int exp_point = 10 + added_exp;
            int gold_point = 5 + (monster.level - 1) + Random.Range(0, 6); 

            Combat_popup.transform.GetChild(1).GetComponent<Text>().text = "i win the combat.\n\n exp :  + " + exp_point.ToString() + "\n" + "gold : " + gold_point.ToString() + "\n";

            User_Status.now_exp += exp_point;
            User_Status.money += gold_point;
        }
        //if user's HP is equals to 0 (user losses the combat)
        else if(User_Status.now_hp == 0 && Combat_popup.active == false)
        {
            Combat_popup.SetActive(true);//create Combat_popup
            Combat_popup.transform.GetChild(0).GetComponent<Text>().text = "You loss!";

            Combat_popup.transform.GetChild(1).GetComponent<Text>().text = "I loss the combat.";

           //set penalty
           User_Status.now_exp -= 10;
           if(User_Status.now_exp < 0) User_Status.now_exp = 0;
        }
    }

    void Attack_turn()
    {
        //Creating Hitting Point
        if(GameObject.Find("HittingPoint") == false)
        {
            Instantiate(Hitting_Point, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            Hitting_Point_MovingArea.SetActive(true);
        }

        int applied_damage = User_Status.att_point - monster.level;

        if(combat_button.isSkillCast)//if User touch 'skill button';
        {
            //It is just example code. i recommand you to edit this part
           applied_damage *=2; 
        }

        if(isTouched_HittingPoint == true || combat_button.isSkillCast)
        {
            //if 'applied_damage' is less than 0, it means that Monster is stronger than user. So, User give monster to minimun Damage or to nothing.
            if(applied_damage <= 0) 
            { 
                int random_num = Random.Range(0, 4 + Mathf.Abs(applied_damage));//chosse tue number wihin 0 to '3 + applied_damage'. 
                //This means that the monster's guarding possibility to user's attack is determined by gap between the power of monster and user.
                //because monster is stronger than user, possibility of attack is low.

                if(random_num > 1) //if random_num is more than 1 (2,3,...)
                {
                    applied_damage = 0; //monster evade user's attack
                }
                else//if random_num is less than 2 (0, 1)
                {
                    applied_damage = Mathf.FloorToInt(basic_damage / 6); //user give monster to mininum damage. i set the minimin damage to (basic_damage/6)
                }    
            }
             
            monster.HP -= applied_damage;
            
            monster_damaged_text.text = (-1 * applied_damage).ToString();//Showing How much does i give the monster to damage.
            monster_damaged_text.gameObject.SetActive(true);

            isTimetoTurnChange = true;//After i give monster to damage, Turn is changed.

            if(applied_damage == 0)    
            {
                Combat_status_Text.text += "\nMonster evaded my attack!!";
            }
            
            isTouched_HittingPoint = false;
        }

        //if i do nothing and time passes more than 'turn_time'
        else if(turn_Counting_Time >= turn_time)
        {
            isTimetoTurnChange = true;
        }
        else    
        {
            turn_Counting_Time += Time.deltaTime;//time counting
        }   

    }
    int Random_attack_num()//Monster choose the number which is in range of 1 to 9
    {
        int ran_num;

        ran_num = Random.Range(1, 10);//create random number which is in range of 1 to 9

        return ran_num;
    }

    void Defence_turn()
    {
        //set the button whose number is equal to monster_attack_num to another color.
        Monster_attack_signals.transform.GetChild(monster_attack_num).GetComponent<Image>().color = new Color(1.0f, 100.0f/255.0f, 100.0f/255.0f);

        Monster monster = GoMap.GOMap_monster.monster_class_list[Picked_monster_number];

        int monster_Damage = 20 + (monster.level);//set monster's damage
        int applied_damage = monster_Damage - User_Status.def_point;//user will get this damage.

        if(applied_damage < 0) applied_damage = 0;//if applied_damage is less than 0, it set to 0.

        if(combat_button.WhatDefenceButtonispicked != 0)//if user touch the button which has the number within 1 to 9.
        {
            //if user touch the correct button, user get less Damage.
            if(monster_attack_num == combat_button.WhatDefenceButtonispicked)
            {
                applied_damage /= 3;
            }

            //whatever user touch one of these buttons, user get the chance to guard the monstet's attack
            int ran_num = Random.Range(0, 4 + Mathf.Abs( Mathf.FloorToInt(applied_damage))); //chosse tue number wihin 0 to '3 + applied_damage'. 
            //This means that if user touch correct button, the possibility of guarding monster's attack is up or user get less damage than other cases.

            if(ran_num <= 1)//if ran_num is 0 or 1, user guard the monster's attack
            {
                applied_damage = 0;
                Combat_status_Text.text = "\nI guard montser's attack!!";
            }
            else//in other cases
            {
                if(applied_damage == 0) applied_damage = 1;//user should get damage more than 0 when user fail to guard monster's attack.
                
                User_Status.now_hp -= applied_damage;
            }

            //Turn is changed
            isTimetoTurnChange = true;

            Monster_attack_signals.transform.GetChild(monster_attack_num).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);//init the color of button which has number equals to 'monster_attack_num'
            player_damaged_text.text = (-1 * applied_damage).ToString();//Show the damage user get from monster.
        }
        else//if user do not touch button
        {
            if(turn_Counting_Time >= turn_time)//if user do not touch button with in 'turn_time' (do nothing within 'turn time')
            {
                //It is same to the case when user can't touch the button which has number euqals to 'monster_attack_num'
                int ran_num = Random.Range(0, 4 + Mathf.Abs( Mathf.FloorToInt(applied_damage))); //chosse the number wihin 0 to '3 + applied_damage'. 
                if(ran_num <= 1)
                {
                    applied_damage = 0;
                    Combat_status_Text.text = "\nI guard montser's attack!!";
                }
                else
                {
                    if(applied_damage == 0) applied_damage = 1;
                    
                    User_Status.now_hp -= applied_damage;
                }                
                isTimetoTurnChange = true;

                Monster_attack_signals.transform.GetChild(monster_attack_num).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                player_damaged_text.text = (-1 * applied_damage).ToString();
            }
            else    
            {
                turn_Counting_Time += Time.deltaTime;//time counting
            }
        }
    }
}