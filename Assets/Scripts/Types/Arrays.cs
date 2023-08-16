using UnityEngine;

public class Arrays : MonoBehaviour
{
    #region Fields
    [SerializeField] private int[] _listOfNumbers = new int[10];
    [SerializeField] private GameObject[] _listOfGameObjects = new GameObject[2];
    [SerializeField] private GameObject _prefabObject;
    int[,] twoDimensionalArray =
    {
        { 1, 2, 2, 3 },
        { 3, 4, 6, 8 }
    };
    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    void Start()
    {

    }

    
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}
