using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveInFile : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Write_File("C:\\Users\\ronny_f6nb3z1\\Desktop\\Blackboard_Media\\writing.txt", "Test war wieeederrr erfolgreich!");
    }

    public void Write_File(string path, string data)
    {
        //write in file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(data);
        writer.Close();
    }
}
