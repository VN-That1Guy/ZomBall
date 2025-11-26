

public class Player_Points
{
    private int points = 0;

    public int pts
    {
        get{ return points; }
        private set { 
            points = value;
        }
    }

    public void AddPoints(int amount)
    {
        pts += amount;
    }

    public void RemovePoints(int amount) 
    { 
        pts -= amount;
    }
}