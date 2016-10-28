using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.GlobalInfos;
using Assets.UnityElementsAccessClasses;
/// <summary>
/// Responsible for switching of frames
/// And possibly other tasks
/// </summary>
public class GUIManager : MonoBehaviour {
    
    private static List<Canvas> frames;

    private static List<GameObject> getAllSceneGameObjects(GameObject gameObj)
    {
        var gObjs = new List<GameObject>();
        gObjs.AddRange(gameObj.scene.GetRootGameObjects());
        return gObjs;
    }

    // Use this for initialization
    void Start() {
        frames = new List<Canvas>();
        frames.AddRange(getAllSceneGameObjects(gameObject).Find(x => x.GetComponentInChildren(typeof(Canvas))).GetComponentsInChildren<Canvas>());
        switchFrames(GlobalStrings.loginCanvas);
        // (frames[frames.IndexOf(frames.Find(i => i.name == GlobalStrings.loginCanvas))].transform)
    }

    public static void switchFrames(string canvasName)
    {
        if (canvasNameExists(canvasName)) {

            frames.ForEach(x => x.gameObject.SetActive(x.name == canvasName || x.name == GlobalStrings.mainCanvas));
            frames.Find(x => x.name == canvasName).transform.position = frames.Find(x => x.name == GlobalStrings.mainCanvas).transform.position;
            //frames.Find(x => x.name == GlobalStrings.mainCanvas).transform.LookAt(frames.Find(x => x.name == canvasName).transform);
        }
        else
            throw new System.ArgumentException(ElementLocator.elementNotFoundInUnity(canvasName));

    }

    public static bool canvasNameExists(string name)
    {
        return frames.Find(x => x.gameObject.name == name);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
