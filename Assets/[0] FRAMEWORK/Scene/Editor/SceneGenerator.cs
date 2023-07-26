using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Germanenko
{

    [InitializeOnLoad]
    public class SceneGenerator
    {

        static SceneGenerator()
        {

            EditorSceneManager.newSceneCreated += SceneCreating;

        }


        public static void SceneCreating(Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {

            var setupFolder = new GameObject("[SETUP]").transform;
            var lightFolder = new GameObject("Lights").transform;
            lightFolder.parent = setupFolder;

            if (GameObject.Find("Directional Light") != null)
            {
                var lightGO = GameObject.Find("Directional Light").transform;
                lightGO.parent = lightFolder;
            }

            var cameraFolder = new GameObject("Cameras").transform;
            var camGO = Camera.main.transform;
            cameraFolder.parent = setupFolder;
            camGO.parent = cameraFolder;

            var worldFolder = new GameObject("[WORLD]").transform;
            new GameObject("Static").transform.parent = worldFolder;
            new GameObject("Dynamic").transform.parent = worldFolder;

            new GameObject("[UI]");

        }

    }

}
