using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicVerifier
{
    /// <summary>
    /// Non-Component script running after game started and verifing that GameLogic object containing Manager/Controller components is presented in scene
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    static void RunObjectsCheck()
    {
        VerifyObject("GameLogic");
        VerifyObject("MusicPlayer");
    }

    static void VerifyObject(string objectName)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectName);
        if (objects.Length == 0)
        {
            Debug.Log(objectName +" object was not found. Instantiating it now... (Consider add it to the scene in advance)");
            GameObject.Instantiate(Resources.Load("Prefabs/GameLogic"));
        } else if (objects.Length > 1)
        {
            Debug.Log("More than one " + objectName + " object were found. Destroying every one except first. (Consider remove multiplications in advance)");
            for (int i = 1; i < objects.Length; i++)
            {
                GameObject.Destroy(objects[i]);
            }
        }
    }

    static void VerifyMusicPlayerObject()
    {
        GameObject[] gameLogicObjects = GameObject.FindGameObjectsWithTag("MusicPlayer");
        if (gameLogicObjects.Length == 0)
        {
            Debug.Log("GameLogic object was not found. Instantiating it now... (Consider add it to the scene in advance)");
            GameObject.Instantiate(Resources.Load("Prefabs/MusicPlayer"));
        }
        else if (gameLogicObjects.Length > 1)
        {
            Debug.Log("More than one GameLogic object were found. Destroying every one except first. (Consider remove multiplications in advance)");
            for (int i = 1; i < gameLogicObjects.Length; i++)
            {
                GameObject.Destroy(gameLogicObjects[i]);
            }
        }
    }
}
