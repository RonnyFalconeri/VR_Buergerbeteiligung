using UnityEngine;
using System.IO;

namespace VRRoom
{
    public class VoteMaster : MonoBehaviour
    {
        private int Yes_Voters = 0;
        private int No_Voters = 0;
        private int Amount_Voters = 0;
        private string Voting_Name;

        public void Vote(string Opinion)
        {
            Amount_Voters++;
            if (Opinion == "yes")
            {
                Yes_Voters++;
            } else
            if (Opinion == "no")
            {
                No_Voters++;
            }
        }

        public void Reset_Voting()
        {
            Yes_Voters = 0;
            No_Voters = 0;
            Amount_Voters = 0;
        }

        public int Get_Voter_Count()
        {
            return Amount_Voters;
        }

        public void Get_Result()
        {
            Debug.Log("Result (Yes | No):  " + Yes_Voters + " | " + No_Voters);
        }

        public void Save_Result()
        {
            string Result = Voting_Name+": (Yes | No):  " + Yes_Voters + " | " + No_Voters;
            Write_File(Result, "C:\\Users\\ronny_f6nb3z1\\Desktop\\Blackboard_Media\\"+Voting_Name+".txt");
        }

        private void Write_File(string path, string data)
        {
            //write text in file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(data);
            writer.Close();
        }
    }
}
