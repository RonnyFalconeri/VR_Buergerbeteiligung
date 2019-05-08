using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{
    // Edit this two for each blackboard
    public string Room, Name;

    private string FilePath;
    byte[] FileData;
    private RawImage Media_RawImage;
    private Texture2D texture = null;


    // Start is called before the first frame update
    void Start()
    {

        FilePath = "C:\\Users\\ronny_f6nb3z1\\Desktop\\Blackboard_Media\\" + Room + "\\" + Name + ".jpg";

        // if file exists, load the image
        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            texture = new Texture2D(2, 2);
            texture.LoadImage(FileData);
        }

        // place the image on the canvas
        Media_RawImage = GetComponent<RawImage>();
        Media_RawImage.texture = texture;

    }
}
