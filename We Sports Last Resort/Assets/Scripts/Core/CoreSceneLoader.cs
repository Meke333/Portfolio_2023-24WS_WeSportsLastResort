using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.SingletonClass;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class CoreSceneLoader : SingletonClass<CoreSceneLoader>
    {
        private Dictionary<string, Scene> _loadedScenes = new Dictionary<string, Scene>();
        private Dictionary<string, Scene> _allScenes = new Dictionary<string, Scene>();

        private Dictionary<string, Scene> _additionalScenes = new Dictionary<string, Scene>();

        [SerializeField] private LoadSceneSetups _activeLoadSceneSetups;

        [SerializeField] private LoadSceneSetups[] allSceneSetups;

        public Scene[] removeScenes;

        public override async void Awake()
        {
            
            base.Awake();
            
            int length = SceneManager.sceneCount;

            for (int i = 0; i < length; i++)
            {
                AddSceneInDictionary(SceneManager.GetSceneAt(i), _loadedScenes);
            }
            
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = UnityEngine.SceneManagement.SceneUtility
                    .GetScenePathByBuildIndex(i);
                
                Debug.Log(scenes[i]);
                
                AddSceneInDictionary(scenes[i], _allScenes);
                
            }

            await Task.Yield();
            
            Debug.LogWarning("Awake");

            LoadMultiSceneSetups(allSceneSetups[0], _loadedScenes);
            
                
        }

        private async void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedEvent;
            SceneManager.sceneUnloaded += SceneUnloadedEvent;
            SceneManager.activeSceneChanged += ActiveSceneChangedEvent;

            await Task.Yield();
            CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad += ProcessAction_OnNewSceneSetupLoad;
            CoreEventManager.Instance.SceneEvents.OnCloseGame += ProcessAction_OnCloseGame;
            CoreEventManager.Instance.SceneEvents.OnNewSceneToLoadAdditive += LoadSceneAdditiveAdditional;
            CoreEventManager.Instance.SceneEvents.OnSingleSceneToUnload += UnloadAdditiveSceneAdditional;
            CoreEventManager.Instance.SceneEvents.OnToggleSceneToLoadOrUnload += ToggleSceneToLoadOrUnload;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoadedEvent;
            SceneManager.sceneUnloaded -= SceneUnloadedEvent;
            SceneManager.activeSceneChanged -= ActiveSceneChangedEvent;
            
            CoreEventManager.Instance.SceneEvents.OnNewSceneSetupLoad -= ProcessAction_OnNewSceneSetupLoad;
            CoreEventManager.Instance.SceneEvents.OnCloseGame -= ProcessAction_OnCloseGame;
            CoreEventManager.Instance.SceneEvents.OnNewSceneToLoadAdditive -= LoadSceneAdditiveAdditional;
            CoreEventManager.Instance.SceneEvents.OnSingleSceneToUnload -= UnloadAdditiveSceneAdditional;
            CoreEventManager.Instance.SceneEvents.OnToggleSceneToLoadOrUnload -= ToggleSceneToLoadOrUnload;
        }

        #region LoadSceneMode.Single

        public void LoadSceneAlone(Scene scene)
        {
            _activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Single);
            
            RemoveAllScenesInDictionary(_loadedScenes);
            AddSceneInDictionary(scene, _loadedScenes);
        }

        public void LoadSceneAlone(string path)
        {
            if (!_allScenes.ContainsKey(path) || !_loadedScenes.ContainsKey(path))
                return;

            Scene scene = _loadedScenes[path];

            _activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
            
            RemoveAllScenesInDictionary(_loadedScenes);
            AddSceneInDictionary(scene, _loadedScenes);
        }

        #endregion

        #region LoadSceneMode.Additive

        public void LoadSceneAdditive(Scene scene)
        {
            _activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Additive);
            
            AddSceneInDictionary(scene, _loadedScenes);
        }
        
        public void LoadSceneAdditive(string path)
        {
            if (!_allScenes.ContainsKey(path))
                return;
            
            _activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            AddSceneInDictionary(path, _loadedScenes);
        }
        
        public void LoadSceneAdditiveAdditional(Scene scene)
        {
            _activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Additive);
            
            AddSceneInDictionary(scene, _additionalScenes);
        }

        public void LoadSceneAdditiveAdditional(string path)
        {
            if (!_allScenes.ContainsKey(path))
                return;
            
            _activeLoadSceneSetups = null;
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
            
            AddSceneInDictionary(path, _additionalScenes);
        }

        public void LoadMultiSceneSetups(LoadSceneSetups loadSceneSetup, Dictionary<string, Scene> dictionaryToPutInNewScenes)
        {
            Debug.LogWarning("LOADMULTISCENESETUPS");
            _activeLoadSceneSetups = loadSceneSetup;

            //
            Dictionary<string, Scene> temp = new Dictionary<string, Scene>();

            for (int i = 0; i< loadSceneSetup.scenes.Length; i++)
            {
                string path = loadSceneSetup.scenes[i].path;

                if (dictionaryToPutInNewScenes.ContainsKey(path))
                {
                    temp.Add(path, dictionaryToPutInNewScenes[path]);
                    dictionaryToPutInNewScenes.Remove(path);
                    continue;
                }

                SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
                
                AddSceneInDictionary(SceneManager.GetSceneByPath(path), temp);
                
            }

            /*Scene[]*/ removeScenes = new Scene[dictionaryToPutInNewScenes.Count];
            dictionaryToPutInNewScenes.Values.CopyTo(removeScenes, 0);

            for (int i = 0; i < removeScenes.Length; i++)
            {
                UnloadAdditiveSceneAdditional(removeScenes[i]);
            }

            _loadedScenes = temp;

            /*foreach (SceneInformation s in loadSceneSetup.scenes)
            {
                if (dictionaryToPutInNewScenes.ContainsKey(s.path))
                    continue;

                SceneManager.LoadSceneAsync(s.path, LoadSceneMode.Additive);

                AddSceneInDictionary(SceneManager.GetSceneByPath(s.path), _loadedScenes);

            }*/

        }

        public void UnloadAdditiveScene(Scene scene)
        {
            SceneManager.UnloadSceneAsync(scene);

            RemoveSceneInDictionary(scene, _loadedScenes);
        }

        public void UnloadAdditiveScene(string path)
        {
            if (!_loadedScenes.ContainsKey(path))
                return;
            
            SceneManager.UnloadSceneAsync(path);

            RemoveSceneInDictionary(path, _loadedScenes);
        }
        
        public void UnloadAdditiveSceneAdditional(Scene scene)
        {
            SceneManager.UnloadSceneAsync(scene);

            RemoveSceneInDictionary(scene, _additionalScenes);
        }

        public void UnloadAdditiveSceneAdditional(string path)
        {
            if (!_additionalScenes.ContainsKey(path))
                return;
            
            SceneManager.UnloadSceneAsync(path);

            RemoveSceneInDictionary(path, _additionalScenes);
        }

        public void ToggleSceneToLoadOrUnload(string path)
        {
            
            //bool isLoaded = _loadedScenes.ContainsKey(path);
            bool isLoadedAdditional = _additionalScenes.ContainsKey(path);

            /*switch (isLoaded)
            {
                case true:
                    UnloadAdditiveScene(path);
                    break;
                case false:
                    LoadSceneAdditive(path);
                    break;
            }*/

            switch (isLoadedAdditional)
            {
                case true:
                    
                    UnloadAdditiveSceneAdditional(path);
                    break;
                case false:
                    LoadSceneAdditiveAdditional(path);
                    break;
            }

        }

        #endregion

        #region Events

        async void SceneLoadedEvent(Scene scene, LoadSceneMode loadSceneMode)
        {
            await Task.Yield();
            
            if (loadSceneMode == LoadSceneMode.Single)
                return;
            
            if (_activeLoadSceneSetups == null)
                return;

            foreach (SceneInformation s in _activeLoadSceneSetups.scenes)
            {
                if (!scene.path.Equals(s.path))
                    continue;
                
                if (!s.isLoaded)
                {
                    UnloadAdditiveSceneAdditional(s.path);
                    continue;
                }

                if (!s.isActive) continue;
                
                SceneManager.SetActiveScene(_loadedScenes[s.path]);

            }
        }

        void SceneUnloadedEvent(Scene scene)
        {
            
        }

        void ActiveSceneChangedEvent(Scene previous, Scene active)
        {
            
        }

        void ProcessAction_OnNewSceneSetupLoad(SceneSetups setup)
        {
            Debug.LogWarning("OnNewSceneSetupLoad Fired");
            LoadSceneSetups newSceneSetup = null;
            
            Debug.LogWarning((int) setup);

            //if (!((int)setup < allSceneSetups.Length))
                //return;

            newSceneSetup = allSceneSetups[(int)setup];

            LoadMultiSceneSetups(newSceneSetup, _loadedScenes);
        }

        void ProcessAction_OnCloseGame()
        {
            Application.Quit();
        }

        #endregion

        

        #region Dictionary Methods

        void AddSceneInDictionary(Scene scene, Dictionary<string, Scene> dictionary)
        {
            if (dictionary.ContainsKey(scene.path))
                return;
            
            dictionary.Add(scene.path, scene);
        }

        void AddSceneInDictionary(string path, Dictionary<string, Scene> dictionary)
        {
            if (dictionary.ContainsKey(path))
                return;
            
            dictionary.Add(path, SceneManager.GetSceneByPath(path));
        }

        void RemoveSceneInDictionary(Scene scene, Dictionary<string, Scene> dictionary)
        {
            dictionary.Remove(scene.path);
        }

        void RemoveSceneInDictionary(string path, Dictionary<string, Scene> dictionary)
        {
            dictionary.Remove(path);
        }

        void RemoveAllScenesInDictionary(Dictionary<string, Scene> dictionary)
        {
            dictionary.Clear();
        }

        #endregion
        
    }
    
    public enum SceneSetups
    {
        MainMenu,   //0
        Gameplay,   //1
        Cutscene1,  //2
        Cutscene2,  //3
        Cutscene3,  //4
        ResetLevel, //5
        TutorialLevel, //6
        
    }

    public enum Scenes
    {
        Core_Gameplay,
        CameraLighting_Level1,
        Environment_Level1,
        ZombieManager,
        Entities_Level1_Enemies,
        Entities_Level1_Player,
        Entities_Level1_Background,
        UI_Gameplay,
        UI_StartScreen
    }
}
