using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Code about button which used in Combat scene.
public class combat_button : MonoBehaviour
{
    public static bool isAttackTurn;//it is used to check is User in Attack mode or Defence mode.
    public static int WhatDefenceButtonispicked;//What button did user touch in Defence mode.
    public static bool isSkillCast;//if user touch the Skill button, it changes to 'true'
    public Button Skill_Button;//Skill button
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        isAttackTurn = true; //at First, User is in Attack mode
        isSkillCast = false; 
        WhatDefenceButtonispicked = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttackTurn)//공격 차례면
        {
            Skill_Button.gameObject.SetActive(true);
        }
        else//수비 차례면
        {
            Skill_Button.gameObject.SetActive(false);
        }
    }

    //function about skill(onClick Function. it is used in Button object)
    public void onSkillButton()
    {
        //if skill is ready
        if(combat_script.isSkillReady == true)
        {
            Debug.Log("스킬 발동!");
            isSkillCast = true;
        }
    }

    //These are onClick function of Defence Button. 
    //in Defence mode, monster choose number in range of 1 to 9, and show in 9 defence buttons. you can see the button which has another color to others.(i explain about this in combat_script.cs)
    //so, if user touch one of 9 defence buttons, the number in button user touched is recorded and use it in Defence_turn() of 'Combat_script' class.
    public void onDefenceButton_1()
    {
        WhatDefenceButtonispicked = 1;
    }
    public void onDefenceButton_2()
    {
        WhatDefenceButtonispicked = 2;
    }
    public void onDefenceButton_3()
    {
        WhatDefenceButtonispicked = 3;
    }
    public void onDefenceButton_4() 
    {
        WhatDefenceButtonispicked = 4;
    }
    public void onDefenceButton_5() 
    {
        WhatDefenceButtonispicked = 5;
    }
    public void onDefenceButton_6() 
    {
        WhatDefenceButtonispicked = 6;
    }
    public void onDefenceButton_7() 
    {
        WhatDefenceButtonispicked = 7;
    }
    public void onDefenceButton_8() 
    {
        WhatDefenceButtonispicked = 8;
    }
    public void onDefenceButton_9()
    {
        WhatDefenceButtonispicked = 9;
    }
}
