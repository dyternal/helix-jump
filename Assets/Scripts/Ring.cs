using UnityEngine;

public class Ring : MonoBehaviour
{
    
    void Update()
    {
        if (transform.position.y - 1.5f > GameData.PlayerObject.transform.position.y)
        {
            Vector3 pos = new Vector3(0f, GameData.LastRing.transform.position.y - GameData.SpaceBetweenPlatforms, 0f);
            GameHelper.Singleton.RingSpawner(pos);
            GameData.PoleTransform.localScale = new Vector3(1f, GameData.PoleTransform.localScale.y + 3f, 1f);
            GameData.PoleTransform.position = new Vector3(0f, GameData.PoleTransform.position.y - 3f, 0f);
            if (GameData.ComboActivated) GameData.ComboMultiplier++;
            GameData.ComboActivated = true;
            
            GameHelper.Singleton.GiveScore(25 * GameData.ComboMultiplier);
            
            Destroy(this.gameObject);
        }
    }
}
