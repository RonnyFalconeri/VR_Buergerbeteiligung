using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRRoom
{

    public class Positioning : MonoBehaviour
    {

        // Declaring positions
        public GameObject MainPosition;
        public GameObject EastPosition;
        public GameObject WestPosition;
        public GameObject SouthPosition;

        // Declaring player
        public GameObject Player;



        // Start is called before the first frame update
        void Start()
        {
            // Position the player at MainPosition
            Move_player_to_position("main");

        }


        // Update is called once per frame
        void Update()
        {
            // Controls for positioning: G, H, J, K
            if(Input.GetKey("g"))
            {
                Move_player_to_position("main");

            } else
            if (Input.GetKey("j"))
            {
                Move_player_to_position("east");

            } else
            if (Input.GetKey("h"))
            {
                Move_player_to_position("west");

            } else
            if (Input.GetKey("k"))
            {
                Move_player_to_position("south");

            }

        }


        private void Move_player_to_position(string Position)
        {
            if(Position == "main")
            {
                Vector3 Position_vector = MainPosition.transform.position;  // Store Position as Vector3
                Player.transform.position = Position_vector;  // Move player to MainPosition
                Player.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));  // Rotate player view to main

            } else
            if (Position == "east")
            {
                Vector3 Position_vector = EastPosition.transform.position;  // Store Position as Vector3
                Player.transform.position = Position_vector;  // Move player to EastPosition
                Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));  // Rotate player view to east

            } else
            if (Position == "west")
            {
                Vector3 Position_vector = WestPosition.transform.position;  // Store Position as Vector3
                Player.transform.position = Position_vector;  // Move player to WestPosition
                Player.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));  // Rotate player view to west

            } else
            if (Position == "south")
            {
                Vector3 Position_vector = SouthPosition.transform.position;  // Store Position as Vector3
                Player.transform.position = Position_vector;  // Move player to SouthPosition
                Player.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));  // Rotate player view to south

            }

        }
    }

}


