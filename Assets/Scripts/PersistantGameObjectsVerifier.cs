using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicVerifier
{
    /// <summary>
    /// Non-Component script running after game started and verifying that some objects are presented in scene
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    static void RunObjectsCheck()
    {
        VerifyObject("GameLogic");
        VerifyObject("MusicPlayer");


        Application.targetFrameRate = 165;
    }

    static void VerifyObject(string objectTag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectTag);
        if (objects.Length == 0)
        {
            Debug.Log(objectTag + " tagged object was not found. Instantiating it now... (Consider add it to the scene in advance)");
            GameObject.Instantiate(Resources.Load("Prefabs/" + objectTag));
        } else if (objects.Length > 1)
        {
            Debug.Log("More than one " + objectTag + " tagged object were found. Destroying every one except first. (Consider remove multiplications in advance)");
            for (int i = 1; i < objects.Length; i++)
            {
                GameObject.Destroy(objects[i]);
            }
        }
    }
}
