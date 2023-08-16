using System;
using UnityEditor;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    #region Fields
    private static PickupSpawner _instance;
    private float _nukeWeight = 0.3333f;    //  
    private float _healthWeight = 0.3334f;  //  Keeping the sum of these 3 values equal to 1 will spawn a pickup 50% of the time, if the sum is 2, pickups will spawn 100% of the time.
    private float _gunWeight = 0.3333f;     //
    #endregion

    #region Properties
    public static PickupSpawner Instance
    {
        get { return _instance; }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    #region Public Methods
    public void SpawnPU(float r, Vector2 position)
    {
        switch (r) {
            case float f when f > 0.0f && f <= _nukeWeight:
                GameObject PU1 = new GameObject("NukePU", typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(NukePU));
                PU1.transform.position = position;
                PU1.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/HexagonFlatTop.png");
                PU1.GetComponent<SpriteRenderer>().color = Color.yellow;
                PU1.transform.localScale = new Vector3(0.4f, 0.4f);
                PU1.GetComponent<Rigidbody2D>().isKinematic = true;
                PU1.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                PU1.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                GameManager.Instance.Pickups.Add(PU1);
                break;
            case float f when f > _nukeWeight & f <= _healthWeight + _nukeWeight:
                GameObject PU2 = new GameObject("HealthPU", typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(HealthPU));
                PU2.transform.position = position;
                PU2.transform.localScale = new Vector3(0.15f, 0.4f);
                PU2.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
                PU2.GetComponent<SpriteRenderer>().color = Color.red;
                PU2.GetComponent<Rigidbody2D>().isKinematic = true;
                PU2.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                PU2.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                GameObject othersprite = new GameObject("Child", typeof(SpriteRenderer));
                othersprite.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
                othersprite.GetComponent<SpriteRenderer>().color = Color.red;
                othersprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                othersprite.transform.localScale = new Vector3(0.15f, 0.4f);
                othersprite.transform.position = position;
                othersprite.transform.SetParent(PU2.transform);
                GameManager.Instance.Pickups.Add(PU2);
                break;
            case float f when f > _healthWeight + _nukeWeight & f <= _gunWeight + _healthWeight + _nukeWeight:
                GameObject PU3 = new GameObject("GunPU", typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(NukePU));
                PU3.transform.position = position;
                PU3.GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Circle.png");
                PU3.GetComponent<SpriteRenderer>().color = Color.magenta;
                PU3.transform.localScale = new Vector3(0.4f, 0.4f);
                PU3.GetComponent<Rigidbody2D>().isKinematic = true;
                PU3.GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
                PU3.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                GameManager.Instance.Pickups.Add(PU3);
                break;
            default:
                break;
        }
    }
    #endregion

    #region Private Methods
    #endregion
}
