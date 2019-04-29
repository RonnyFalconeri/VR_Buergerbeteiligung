using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voter : MonoBehaviour
{
    public Voting Voting;
    private bool Voted = false;

    void Update()
    {
        // Controls for positioning: G, H, J, K
        if (Input.GetKey("g"))
        {
            Vote_Yes();

        } else
        if (Input.GetKey("h"))
        {
            Vote_No();

        } else
        if (Input.GetKey("j"))
        {
            Vote_Nothing();

        } else
        if (Input.GetKey("k"))
        {
            Voting.Get_Result();

        }

    }

    public void Vote_Yes()
    {
        if(!this.Voted)
        {
            Voting.Vote_Yes();
            this.Voted = true;
        }
    }

    public void Vote_No()
    {
        if (!this.Voted)
        {
            Voting.Vote_No();
            this.Voted = true;
        }
    }

    public void Vote_Nothing()
    {
        if (!this.Voted)
        {
            Voting.Vote_Nothing();
            this.Voted = true;
        }
    }
    
}
