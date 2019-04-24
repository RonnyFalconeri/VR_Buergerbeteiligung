using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voter : MonoBehaviour
{
    private Voting Voting;

    public void Vote_Yes()
    {
        Voting.Vote_Yes();
    }

    public void Vote_No()
    {
        Voting.Vote_No();
    }

    public void Vote_Nothing()
    {
        Voting.Vote_Nothing();
    }

    public void Get_Voting()
    {
        // Get Voting object from Moderator, which instantiates the voting

    }
}
