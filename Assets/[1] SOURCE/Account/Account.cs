using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account : MonoBehaviour
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public static Account Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUserData(string firstName,  string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
