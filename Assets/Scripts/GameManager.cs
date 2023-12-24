using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        GameHelper.Singleton.InitGameData();
        GameHelper.Singleton.SectionSpawner();
    }

    private void Update()
    {
        GameData.ScoreText.text = GameData.GameScore.ToString();
    }
}

public class GameHelper : MonoBehaviour
{
    public static GameHelper Singleton = new GameHelper();
    public void InitGameData()
    {
        GameData.PlayerObject = GameObject.FindWithTag("Player");
        GameData.Player = GameData.PlayerObject.GetComponent<Player>();
        GameData.LevelTransform = GameObject.FindWithTag("LevelParent").transform;
        GameData.PoleTransform = GameObject.Find("Pole").transform;
        GameData.ScoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        GameData.GameOverObject = GameObject.Find("GameOver");
        GameData.GameScoreText = GameData.GameOverObject.transform.GetChild(3).GetComponent<TMP_Text>();
        GameData.GameScore = 0;
        
        GameData.GameOverObject.SetActive(false);
    }

    public async void AssetLoader()
    {
        var platform = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Platform.prefab");
        var splash = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Splash.prefab");
        var redzonemat = Addressables.LoadAssetAsync<Material>("Assets/Materials/RedZone.mat");
        await platform.Task;
        await splash.Task;
        await redzonemat.Task;

        if (platform.IsDone)
        {
            GameData.PlatformInstance = platform.Result;
        }

        if (splash.IsDone)
        {
            GameData.SplashObject = splash.Result;
        }
        if (redzonemat.IsDone)
        {
            GameData.RedZone = redzonemat.Result;
        }
    }
    
    public void RingSpawner(Vector3 pos)
    {
        GameObject ParentObject = new GameObject("Ring");
        ParentObject.transform.SetParent(GameData.LevelTransform);
        ParentObject.transform.AddComponent<Ring>();
        ParentObject.transform.position = pos;
        GameData.LastRing = ParentObject.transform;
        
        int EmptyPatternCount = Random.Range(1, 3);

        bool IsPreviousPatternEmpty = false;
        for (int i = 0; i < 8; i++)
        {
            if (!IsPreviousPatternEmpty)
            {
                if (EmptyPatternCount > 0)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0) // rand == 0 dont create platform
                    {
                        IsPreviousPatternEmpty = true;
                        EmptyPatternCount--;
                    }
                    else // rand == 1 create platform
                    {
                        GameObject platform = Instantiate(GameData.PlatformInstance, pos, Quaternion.Euler(0f, (i * 45), 0f));
                        platform.transform.SetParent(ParentObject.transform);
                    }
                }
                else
                {
                    GameObject platform = Instantiate(GameData.PlatformInstance, pos, Quaternion.Euler(0f, (i * 45), 0f));
                    platform.transform.SetParent(ParentObject.transform);
                }
            }
            else
            {
                GameObject platform = Instantiate(GameData.PlatformInstance, pos, Quaternion.Euler(0f, (i * 45), 0f));
                platform.transform.SetParent(ParentObject.transform);
                IsPreviousPatternEmpty = false;
            }
        }
        
        CreateRedZone(ParentObject.transform);
    }
    
    public void SectionSpawner()
    {
        for (int i = 0; i < 9; i++)
        {
            Vector3 SpawnPos = new Vector3(0, (23 - (i * GameData.SpaceBetweenPlatforms)));
            RingSpawner(SpawnPos);
        }
    }

    private void CreateRedZone(Transform ring)
    {
        int RedZoneCount = Random.Range(1, 3);

        for (int i = 0; i < RedZoneCount; i++)
        {
            int RandomChild = Random.Range(0, ring.childCount);
            ring.GetChild(RandomChild).gameObject.GetComponent<Renderer>().material = GameData.RedZone;
            ring.GetChild(RandomChild).transform.tag = "RedZone";
        }
    }

    public void GiveScore(int amount)
    {
        GameData.GameScore += amount;
    }

    public void GameOver()
    {
        GameData.GameOverObject.SetActive(true);
        GameData.GameScoreText.text = GameData.GameScore.ToString();
        Time.timeScale = 0;
    }
}

public class GameData
{
    public static Player Player;    
    public static GameObject PlayerObject;

    public static GameObject PlatformInstance = null;
    public static Transform LevelTransform;
    public static Transform LastRing;
    public static Transform PoleTransform;
    public static int GameScore = 0;
    
    public const float PlatformRotationOffset = 45f;
    public const float SpaceBetweenPlatforms = 2f;

    public static bool ComboActivated;
    public static int ComboMultiplier = 1;
    public static TMP_Text ScoreText;
    public static TMP_Text GameScoreText;
    public static GameObject GameOverObject;
    public static GameObject SplashObject;

    public static Material RedZone;

}
