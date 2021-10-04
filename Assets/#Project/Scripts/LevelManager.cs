using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // gére tout le systeme
using System.IO; // entrée sortie
using System.Runtime.Serialization.Formatters.Binary; // crée un formateur

[Serializable]
public class GameData
{
    public int AshColor;
    public int[] CubesColor; // on va spécifier sa longueur plus tard
    public float[] AshPosition = new float[3];
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameData data;
    public void Save() 
    {
        FileStream file = File.Create(Application.persistentDataPath + "/data.dat"); // ouverture de mon flux/ file comme du streaming
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            GameData data = new GameData();
            AshBehavior ash = FindObjectOfType<AshBehavior>();

            data.AshColor = ash.colorIndex;
            data.AshPosition[0] = ash.transform.position.x;
            data.AshPosition[1] = ash.transform.position.y;
            data.AshPosition[2] = ash.transform.position.z;

            CubeBehavior[] cubes = FindObjectsOfType<CubeBehavior>();
            data.CubesColor = new int[cubes.Length]; // crée un tableau qu'on instancie avec le même nbre que CubesColor
            for(int i=0; i < cubes.Length; i++)
            {
                data.CubesColor[i] = cubes[i].colorIndex; 
            }

            bf.Serialize(file,data);
        }
        finally
        {
            file.Close(); // se ferme quoi qu'il arrive à mon file!
        }
    
    }
    void Start()
    {
        if(instance == null)
        {
            instance = this; // levelmanager alpha, le primo
            DontDestroyOnLoad(gameObject); // il soit bien à sa place
        }
        else
        {
            Destroy(gameObject); // si c'est pas unique alors détruis toi 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Q))
        {
            Save();
            Application.Quit();
        }
    }
}
