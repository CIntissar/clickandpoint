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
    public bool loaded = true;
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
    
    public void Load()
    {
        GameData data = null; // = null / Instance toujours = null afin que la condition en bas, soit toujours vraie!
        
        if(File.Exists(Application.persistentDataPath + "/data.dat"))
        {

            FileStream file = File.Open(Application.persistentDataPath + "/data.dat",FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                data = bf.Deserialize(file) as GameData;
            }
            finally
            {
                file.Close();
            }
        }
        Initialize(data);

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

    public void Initialize(GameData data = null) 
    {
        // GameData data = null -> pour permettre de charger avec ou sans gamedata!

        AshBehavior ash = FindObjectOfType<AshBehavior>();
        CubeBehavior[] cubes = FindObjectsOfType<CubeBehavior>();

        if (data != null)
        {
            ash.colorIndex = data.AshColor;
            Vector3 position = new Vector3(
                data.AshPosition[0],
                data.AshPosition[1],
                data.AshPosition[2]
            );

            ash.transform.position = position;

            for(int i = 0; i < cubes.Length; i++)
            {
                cubes[i].colorIndex = data.CubesColor[i];
            }
        }

        ash.Initialize();
        
        for(int i = 0; i < cubes.Length; i++)
        {
            cubes[i].Initialize();
        }
    }
}
