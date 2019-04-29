using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voting : MonoBehaviour
{
    
    private int Yes_Voters = 0;
    private int No_Voters = 0;
    private int Abstention_Voters = 0;
    private int Amount_Voters = 0;
    

    // Set voting values
    public void Vote_Yes()
    {
        this.Yes_Voters++;
        this.Amount_Voters++;
    }

    public void Vote_No()
    {
        this.No_Voters++;
        this.Amount_Voters++;
    }

    public void Vote_Nothing()
    {
        this.Abstention_Voters++;
        this.Amount_Voters++;
    }

    public void Reset_Voting()
    {
        this.Yes_Voters = 0;
        this.No_Voters = 0;
        this.Abstention_Voters = 0;
        this.Amount_Voters = 0;
    }


    // Get voting values
    public int Get_Yes_Votes()
    {
        return this.Yes_Voters;
    }

    public int Get_No_Votes()
    {
        return this.No_Voters;
    }

    public int Get_Abstention_Votes()
    {
        return this.Abstention_Voters;
    }

    public void Get_Result()
    {
        Debug.Log("Result (Yes | No | Nothing): "+this.Yes_Voters+" | "+this.No_Voters+" | "+this.Abstention_Voters);
    }

}
