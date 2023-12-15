using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;
using Unity.VisualScripting;
using Random = System.Random;

namespace Core
{
    public class PaddleController : DeskaBehaviour
    {
        #region PRIVATE FIELDS

        [SerializeField] private MatchManager.Side paddleSide;
        [SerializeField] private GameObject indicator;
        [SerializeField] private PaddleDriveParticleSystem paddleDriveParticleSystem;
        [SerializeField] private GameObject paddleSpriteGameObject;

        [SerializeField] private float forceModifier;
        private float currentPaddleWidth;
        

        #endregion
        
        // Null Coalescence Operator
        // private IBallController Ball = MatchManager.Instance.GetBall() ?? new BallController();
        
        #region UNITY EVENT FUNCTIONS

        private void Start()
        {
            BindInputEvents();
            MatchManager.Instance.OnRoundReset?.AddListener(ResetPaddleToStartPos);
        }
        
        private void OnDestroy()
        {
            
        }
        

        #endregion

        #region PUBLICS

        public float GetPaddleForceModifier()
        {
            return forceModifier;
        }
        

        public MatchManager.Side GetPaddleSide()
        {
            switch (paddleSide)
            {
                case MatchManager.Side.RED:
                    return MatchManager.Side.RED;
                    break;
                case MatchManager.Side.BLUE:
                    return MatchManager.Side.BLUE;
                    break;
            }
            return 0;
        }

        public void TogglePlusPaddleSizeEffect(bool newValue)
        {
            float desiredXFactor = newValue ? Constants.PADDLE_UPSIZE_ACTIVE_WIDTH : Constants.PADDLE_STANDARD_WIDTH;
            Vector2 desiredScale = new Vector2(desiredXFactor, transform.localScale.y);
            LeanTween.scale(gameObject, desiredScale, Constants.PADDLE_SIZE_PLUS_SCALE_TWEEN_TIME).setEaseOutBounce();
        }

        public void TogglePaddleDriveEffect(bool newValue)
        {
            float mod = newValue ? Constants.PADDLE_DRIVE_FORCE_MODIFIER : Constants.PADDLE_BASE_FORCE_MODIFIER;
            if (newValue == true)
            {
                LeanTween.moveLocalX(paddleSpriteGameObject, -0.03f, 0.025f).setOnComplete(StartDriveEffectShake);
            }
            else
            {
                paddleSpriteGameObject.transform.localPosition = Vector2.zero;
            }

            forceModifier = mod;
            paddleDriveParticleSystem.ToggleEffect(newValue);
        }

        private void StartDriveEffectShake()
        {
            LeanTween.moveLocalX(paddleSpriteGameObject, 0.03f, 0.05f).setLoopPingPong((int)(Constants.PADDLE_DRIVE_EFFECT_TIME / 0.1f));
        }


        #endregion
        
        #region PRIVATE METHODS

        private void OnTouchInputRecieved(Vector2 input)
        {
            MovePaddleToPosition(input);
     
        }

        private void MovePaddleToPosition(Vector2 input)
        {
            Vector2 position = new Vector2(input.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, position, Constants.PADDLE_BASE_MOVE_DELTA * Time.deltaTime);
            
            if (indicator != null)
            {
                Vector2 indicatorNewPosition = new Vector2(input.x, indicator.transform.position.y);
                indicator.transform.position = Vector2.MoveTowards(indicator.transform.position, indicatorNewPosition, Constants.PADDLE_BASE_MOVE_DELTA * Time.deltaTime);
            }

        }

        private void BindInputEvents()
        {
            MatchManager.Side side = GetPaddleSide();
            if (side == MatchManager.Side.BLUE)
            {
                InputManager.Instance.BluePaddleTouchInput?.AddListener(OnTouchInputRecieved);
            }
            else
            {
                InputManager.Instance.RedPaddleTouchInput?.AddListener(OnTouchInputRecieved);
            }
        }
        
        private void UnbindInputEvents()
        {
            MatchManager.Side side = GetPaddleSide();
            if (side == MatchManager.Side.BLUE)
            {
                InputManager.Instance.BluePaddleTouchInput?.RemoveListener(OnTouchInputRecieved);
            }
            else
            {
                Debug.Log(InputManager.Instance);
                InputManager.Instance.RedPaddleTouchInput?.RemoveListener(OnTouchInputRecieved);
            }
        }

        private void ResetPaddleToStartPos()
        {
            transform.position = GetPaddleSide() == MatchManager.Side.BLUE ? Constants.BLUE_PADDLE_START_POS : Constants.RED_PADDLE_START_POS;
            indicator.transform.position = Vector2.zero;
        }

        


        #endregion
    }

    
    
    
}
