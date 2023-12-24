using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Header("Player Settings")]
    private Rigidbody Rigidbody;
    [SerializeField]
    private float JumpForce;
    
    private void OnCollisionEnter(Collision other)
    {
        GameData.ComboActivated = false;
        GameData.ComboMultiplier = 1;
        if (other.transform.CompareTag("RedZone")) GameHelper.Singleton.GameOver();
        else Rigidbody.velocity = new Vector3(0f, JumpForce * Time.deltaTime, 0f);
        Quaternion rot = Quaternion.Euler(-90f, Random.Range(0, 360), 0);
        GameObject splash = Instantiate(GameData.SplashObject, transform.position - (Vector3.up * 0.100f), rot);
        splash.transform.SetParent(other.transform.parent);
    }
}
