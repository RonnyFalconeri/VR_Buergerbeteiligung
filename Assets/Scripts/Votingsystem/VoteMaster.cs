using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteMaster : MonoBehaviour
{
    private int Yes_Voters = 0;
    private int No_Voters = 0;
    private int Amount_Voters = 0;


    public void Vote(string Opinion)
    {
        if(Opinion == "yes")
        {
            Yes_Voters++;
            Amount_Voters++;

        } else
        if (Opinion == "no")
        {
            No_Voters++;
            Amount_Voters++;
        }
    }

    public void Reset_Voting()
    {
        Yes_Voters = 0;
        No_Voters = 0;
        Amount_Voters = 0;
    }


    public void Get_Result()
    {
        Debug.Log("Result (Yes | No):  " + Yes_Voters + " | " + No_Voters);
    }
}
