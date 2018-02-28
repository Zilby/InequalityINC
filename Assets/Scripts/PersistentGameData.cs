using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameData : MonoBehaviour {
    private static PersistentGameData instance;
    public Dictionary<string, string> data = new Dictionary<string, string> ();

    // Static singleton property
    public static PersistentGameData Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = new GameObject ("[GameData]").AddComponent<PersistentGameData> ()); }
    }

    public void Awake() {
        // Set default parameters for all required fields here
    }
}
