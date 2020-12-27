using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Code about HittingPoint
//it is used when user is in Attack mode.
//if user touch the HittingPointm, User give damage to monster
public class HittingPoint : MonoBehaviour
{
    float MovingAngle;//direction of moving -> arctan(movingangle)
    Vector2 unit_vector;//Unit vector of HittingPoint

    // Start is called before the first frame update
    void Start()
    {
        this.name = "HittingPoint";

        MovingAngle = Random.Range(0.0f, 360.0f);//0.0~360.0
        unit_vector = new Vector2(Mathf.Cos(MovingAngle), Mathf.Sin(MovingAngle));//단위벡터 생성
        
        //Set initial position
        string now_scene_name = SceneManager.GetActiveScene().name;

        GameObject UI = GameObject.Find("UI");
        this.transform.SetParent(UI.transform);
        
        float x, y;
        x = Random.Range(-450.0f, 450.0f);  y = Random.Range(-752.0f, -10.0f);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        
    }

    // Update is called once per frame
    void Update()
    {
        //{
        //we let Hittingpoint move within {x | -500 <=  x <= 500}, {y | -742 <= y <= 500} <- Hitting_Point_MovingArea
       
        this.GetComponent<RectTransform>().anchoredPosition += (8 * unit_vector);
        
        //if Hitting point colide with border of 'Hitting_Point_MovingArea', it bounce to 
        if(this.GetComponent<RectTransform>().anchoredPosition.x + (8 * unit_vector.x) >= 460.0f)//colide with right end
            unit_vector.x *= -1.0f;
        else if(this.GetComponent<RectTransform>().anchoredPosition.x + (8 * unit_vector.x) <= -460.0f)//colide with left end
            unit_vector.x *= -1.0f;
                
        if(this.GetComponent<RectTransform>().anchoredPosition.y + (8 * unit_vector.y) >= 0.0f)//colide with upper end
            unit_vector.y *= -1.0f; 
        else if(this.GetComponent<RectTransform>().anchoredPosition.y + (8 * unit_vector.y) <= -762.0f)//colide with down end
            unit_vector.y *= -1.0f;
 
        
    }
    //onClick Function. you attach to 'HittingButton' object.
    public void onClick_HittingPoint()
    {
        combat_script.isTouched_HittingPoint = true;   
    }
}
