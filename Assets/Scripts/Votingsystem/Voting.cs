using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voting : MonoBehaviour
{

    private string Voting_Name;
    private int Yes_Voters = 0;
    private int No_Voters = 0;
    private int Abstention_Voters = 0;
    private int Amount_Voters = 0;


    // Constructor
    public Voting(string pVoting_Name)
    {
        this.Voting_Name = pVoting_Name;
    }
    

    // Set voting values
    public void Vote_Yes()
    {
        this.Yes_Voters++;
        Debug.Log("Voted yes. " + this.Yes_Voters + " for 'Yes'");
    }

    public void Vote_No()
    {
        this.No_Voters++;
        Debug.Log("Voted no. " + this.No_Voters + " for 'No'");
    }

    public void Vote_Nothing()
    {
        this.Abstention_Voters++;
        Debug.Log("Voted nothing. " + this.Abstention_Voters + " for 'Nothing'");
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
}
