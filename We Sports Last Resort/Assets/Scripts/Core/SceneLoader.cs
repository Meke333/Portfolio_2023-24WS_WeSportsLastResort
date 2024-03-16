using System;
using System.Collections.Generic;
using General.SingletonClass;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : SingletonClass<SceneLoader>
    {
        private Dictionary<string, Scene> _loadedScenes = new Dictionary<string, Scene>();
        private Dictionary<string, Scene> _allScenes = new Dictionary<string, Scene>();

        private LoadSceneSetups _activeLoadSceneSetups;

        [SerializeField] private LoadSceneSetups[] allSceneSetups;

        public override void Awake()
        {
            int length = SceneManager.sceneCount;

            for (int i = 0; i < length; i++)
            {
                AddSceneInDictionary(SceneManager.GetSceneAt(i), _loadedScenes);
            }

            LoadMultiSceneSetups(allSceneSetups[0], _loadedScenes);
            
            base.Awake();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedEvent;
            SceneManager.sceneUnloaded += SceneUnloadedEvent;
            SceneManager.activeSceneChanged += ActiveSceneChangedEvent;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoadedEvent;
            SceneManager.sceneUnloaded -= SceneUnloadedEvent;
            SceneManager.activeSceneChanged -= ActiveSceneChangedEvent;
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
            SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
            AddSceneInDictionary(_allScenes[path], _loadedScenes);
        }

        public void LoadMultiSceneSetups(LoadSceneSetups loadSceneSetup, Dictionary<string, Scene> dictionary)
        {
            _activeLoadSceneSetups = loadSceneSetup;
            
            foreach (SceneInformation s in loadSceneSetup.scenes)
            {
                if (dictionary.ContainsKey(s.path))
                    continue;

                SceneManager.LoadSceneAsync(s.path, LoadSceneMode.Additive);

                AddSceneInDictionary(SceneManager.GetSceneByPath(s.path), _loadedScenes);

            }

            
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

            RemoveSceneInDictionary(_loadedScenes[path], _loadedScenes);
        }

        #endregion

        #region Events

        void SceneLoadedEvent(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (loadSceneMode == LoadSceneMode.Single)
                return;
            
            foreach (SceneInformation s in _activeLoadSceneSetups.scenes)
            {
                if (!scene.path.Equals(s.path))
                    continue;
                
                if (!s.isLoaded)
                {
                    UnloadAdditiveScene(s.path);
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

        #endregion

        

        #region Dictionary Methods

        void AddSceneInDictionary(Scene scene, Dictionary<string, Scene> dictionary)
        {
            if (dictionary.ContainsKey(scene.path))
                return;
            
            dictionary.Add(scene.path, scene);
        }

        void RemoveSceneInDictionary(Scene scene, Dictionary<string, Scene> dictionary)
        {
            dictionary.Remove(scene.path);
        }

        void RemoveAllScenesInDictionary(Dictionary<string, Scene> dictionary)
        {
            dictionary.Clear();
        }

        #endregion
        
    }
}
