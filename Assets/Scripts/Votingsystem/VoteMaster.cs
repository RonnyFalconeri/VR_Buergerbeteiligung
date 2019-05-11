using UnityEngine;
using System.IO;

namespace VRRoom
{
    public class VoteMaster
    {
        private int Yes_Voters = 0;
        private int No_Voters = 0;
        private int Abstinence_Voters = 0;
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
            } else
            if (Opinion == "abstinence")
            {
                Abstinence_Voters++;
            }
        }

        public void Create_New_Voting(string name)
        {
            Yes_Voters = 0;
            No_Voters = 0;
            Abstinence_Voters = 0;
            Amount_Voters = 0;
            Voting_Name = name;
        }

        public string Get_Result()
        {
            return "Result (Yes | No | Abstinent):  " + Yes_Voters + " | " + No_Voters + " | " + Abstinence_Voters;
        }

        public void Save_Result()
        {
            string Result = Voting_Name + "\r\n " + Get_Result();
            Write_File(Result, Application.dataPath + "/Abstimmungsergebnisse/" + Voting_Name+".txt");
        }

        private void Write_File(string path, string data)
        {
            //write text in file
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(data);
            writer.Close();
        }
    }
}
