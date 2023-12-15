using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    

    public class DeskaBehaviourSingletonPersistent<T> : DeskaBehaviourSingleton<T> where T : Component
    {
        public static T Instance { get; private set; }

        protected override void SingletonAwake()
        {
            base.SingletonAwake();
            DontDestroyOnLoad(gameObject);
        }
    }
