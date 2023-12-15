using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

    public class DeskaBehaviourSingleton<T> : DeskaBehaviour where T : Component
    {
        public static bool IsAwakened { get; private set; }
        public static bool IsStarted { get; private set; }
        public static bool IsDestroyed { get; private set; }


        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    if (IsDestroyed) return null;
                    Debug.Log($"{typeof(T)} Instance is null");

                    instance = FindExistingInstance() ?? CreateNewInstance();
                }

                return instance;
            }
        }

        // Find any other instances that might exist in the scene
        private static T FindExistingInstance()
        {
            T[] existingInstances = FindObjectsOfType<T>();
            
            // No instances found
            if (existingInstances == null || existingInstances.Length == 0) return null;

            return existingInstances[0];
        }
        
        // If no instance of the T DeskaBehaviour exist, create a new gameobject in the scene and add T to it
        private static T CreateNewInstance()
        {
            var containerGO = new GameObject(typeof(T).Name + "(Singleton)");
            return containerGO.AddComponent<T>();
        }

        protected virtual void SingletonAwake()
        {
            InitLogger();
        }

        protected virtual void SingletonStart()
        {
        }

        protected virtual void SingletonDestroy()
        {
        }

        protected virtual void NotifyInstanceRepeated()
        {
            Component.Destroy(this.GetComponent<T>());
        }

        #region UnityEvent Functions (WARNING - DO NOT OVERRRIDE THESE METHODS IN CHILD CLASSES!!!)
        
        protected void Awake()
        {
            
            T thisInstance = this.GetComponent<T>();
            
            // Init the singleton if the script is already in the scene on a game object
            if (instance == null)
            {
                instance = thisInstance;
            }
            else if (thisInstance != instance)
            {
                Debug.Log("Duplicated instance has been found");
                NotifyInstanceRepeated();
                return;
            }

            if (!IsAwakened)
            {
                SingletonAwake();
                IsAwakened = true;
            }
        }

        private void Start()
        {
            if(IsStarted) return;
            
            SingletonStart();
            IsStarted = true;
        }

        private void OnDestroy()
        {
            if (this != instance) return;

            IsDestroyed = true;
            IsStarted = false;
            IsAwakened = false;
            
            SingletonDestroy();
        }

        #endregion
        
    }

