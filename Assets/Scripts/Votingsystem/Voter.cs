
using UnityEngine;

namespace VRRoom
{
    public class Voter : MonoBehaviour
    {
        public VoteMaster Voting;
        private bool Voted = false;

        void Update()
        {
            if (Input.GetKey("g"))
            {
                Vote_Yes();

            }
            else
            if (Input.GetKey("h"))
            {
                Vote_No();

            }

        }

        public void Vote_Yes()
        {
            if (!Voted)
            {
                Voting.Vote("yes");
                Voted = true;
            }
        }

        public void Vote_No()
        {
            if (!Voted)
            {
                Voting.Vote("no");
                Voted = true;
            }
        }
    } 
}
