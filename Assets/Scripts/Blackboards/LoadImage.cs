using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{

    private string FilePath = "C:\\Users\\ronny_f6nb3z1\\Desktop\\Blackboard_Media\\west3.jpg";
    byte[] FileData;
    private RawImage Media_RawImage;
    public Texture Media_Texture;
    private Texture2D texture = null;



    // Start is called before the first frame update
    void Start()
    {

        if (File.Exists(FilePath)) // ERROR: The name 'File' does not exist in the current context?
        {
            FileData = File.ReadAllBytes(FilePath); // ERROR: The name 'File' does not exist in the current context?
            texture = new Texture2D(2, 2);
            texture.LoadImage(FileData);
        }

        Media_RawImage = GetComponent<RawImage>();
        Media_RawImage.texture = texture;

    }
    
}
