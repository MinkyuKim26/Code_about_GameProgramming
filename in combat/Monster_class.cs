//The class of monster. 

public class Monster 
{
    public int level;
    public int HP;

    public Monster(int level)
    {
        this.level = level;
        HP = 50*level;
    }
}