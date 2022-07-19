using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneSwap : MonoBehaviour
{
    public static SceneSwap _instance;
    public bool _NewGame = false;
    public Map map = new Map();

    public string _LoadedMap;
    
    public void Awake()
    {
       
        if(_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void swapScenes()
    {
        _NewGame = true;
        SceneManager.LoadScene(1);
    }

    public void LoadScenes(string s)
    {
        _NewGame = false;
        _LoadedMap = s;
        SceneManager.LoadScene(1);
    }

    public class Map
    {
        public string mapName = "";
        public int gridsizeX;
        public int gridsizeY;
        public int tilesize = 5;
    }

}
