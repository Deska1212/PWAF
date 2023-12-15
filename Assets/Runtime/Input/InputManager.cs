using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



namespace Input
{
        
    /// <summary>
    /// This class serves as the central hub for all input-related functionality.
    /// It handles raw touch input data from Unity and processes it into a format that can be used by game systems.
    /// It should differentiate between touches from Player 1 and Player 2, based on their touch locations on the screen.
    /// </summary>
    public class InputManager : DeskaBehaviourSingleton<InputManager>
    {
        #region EVENTS

        public UnityEvent<Vector2> BluePaddleTouchInput;
        public UnityEvent<Vector2> RedPaddleTouchInput;

        #endregion
        
        #region PRIVATE FIELDS

        [SerializeField] private Vector2 botPlayerTouchPos;
        [SerializeField] private Vector2 topPlayerTouchPos;
        

        #endregion


        #region UNITY EVENT FUNCTIONS

        // Update is called once per frame
        private void Update()
        {
            if (MatchManager.Instance.IsInfiniteMatch)
            {
                HandleTouchInputForInfiniteMatch();
            }
            else
            {
                HandleTouchInput();
            }
        }

        #endregion

        private void HandleTouchInput()
        {

            foreach (var touch in UnityEngine.Input.touches)
            {
                // Bottom touch position
                if (touch.position.y < Screen.height / 2f)
                {
                    botPlayerTouchPos = touch.position;
                    Vector2 touchToWorldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    BluePaddleTouchInput?.Invoke(touchToWorldPoint);
                }
                // Top touch position
                else if (touch.position.y > Screen.height / 2f)
                {
                    topPlayerTouchPos = touch.position;
                    Vector2 touchToWorldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    RedPaddleTouchInput?.Invoke(touchToWorldPoint);
                }
            }
        }
        
        private void HandleTouchInputForInfiniteMatch()
        {

            foreach (var touch in UnityEngine.Input.touches)
            {
                // Bottom touch position
                if (touch.position.y < Screen.height / 2f)
                {
                    botPlayerTouchPos = touch.position;
                    Vector2 touchToWorldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    BluePaddleTouchInput?.Invoke(touchToWorldPoint);
                }
                
            }
        }


    }

}