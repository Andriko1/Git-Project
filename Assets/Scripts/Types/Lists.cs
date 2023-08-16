using UnityEngine;
using System.Collections.Generic;

public class Lists : MonoBehaviour
{
    #region Fields
    [SerializeField] private List<int> listOfNumbers = new List<int>();

    #endregion

    #region Properties
    #endregion

    #region Unity Methods
    void Start()
    { 
        listOfNumbers.Add(2);
        listOfNumbers.Add(5);
        listOfNumbers.Add(4);

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
