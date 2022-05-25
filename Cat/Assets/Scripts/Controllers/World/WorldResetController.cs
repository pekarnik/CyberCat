using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class WorldResetController : MonoBehaviour
{
    private struct TransformState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public TransformState(Vector3 pos, Quaternion rot, Vector3 sca)
        {
            position = pos;
            rotation = rot;
            scale = sca;
        }
    }
    // Start is called before the first frame update

    // private Transform[] 
    private Dictionary<string, TransformState> states = new Dictionary<string, TransformState>();
    void Start()
    {
        SaveState();
    }

    // voi WorldResetController()
    // {
    //     SaveState
    // }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetState();
        }
        
    }

    void SaveState() {
        ResetableGameObject[] gameObjects = FindObjectsOfType<MonoBehaviour>().OfType<ResetableGameObject>().ToArray();
        
        foreach(ResetableGameObject resetableGameObject in gameObjects) {
            GameObject gameObject = resetableGameObject.CurrentGameObject;

            Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Quaternion rotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
            Vector3 scale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            string gameObjectId = GUID.Generate().ToString();
            gameObject.name = gameObject.name + "_" + gameObjectId;

            states.Add(gameObject.name, new TransformState(position, rotation, scale));
        }
    }
    void ResetState(){
        foreach (KeyValuePair<string, TransformState> state in states) {
            GameObject gameObject = GameObject.Find(state.Key);

            gameObject.transform.position = state.Value.position;
            gameObject.transform.rotation = state.Value.rotation;
            gameObject.transform.localScale = state.Value.scale;

        }
    }
}
