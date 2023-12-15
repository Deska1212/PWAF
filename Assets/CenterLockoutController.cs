using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.Mathematics;
using UnityEngine;

public class CenterLockoutController : MonoBehaviour
{
    [SerializeField] private Transform leftTf;
    [SerializeField] private Transform rightTf;

    [SerializeField] private Collider2D leftCollider;
    [SerializeField] private Collider2D rightCollider;
    [SerializeField] private Collider2D compositeCollider;
    
    
    [SerializeField] private Vector2 leftRestPosition;
    [SerializeField] private Vector2 rightRestPosition;

    [SerializeField] private GameObject centerLockoutParticleEffect;

    [SerializeField] private float activationTime = 2.5f;

    [SerializeField] private LockoutState lockoutState;

    [SerializeField] private AnimationCurve customCurve;

    private bool particleCanSpawn;
    private LTDescr animation;
    

    private void Start()
    {
        leftRestPosition = Camera.main.ViewportToWorldPoint(new Vector3(-0.05f, 0.5f));
        rightRestPosition = Camera.main.ViewportToWorldPoint(new Vector3(1.05f, 0.5f));
        leftTf.position = leftRestPosition;
        rightTf.position = rightRestPosition;
        particleCanSpawn = true;
        lockoutState = LockoutState.DISABLED;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            EnableCenterLockout();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftRestPosition, 0.1f);
        Gizmos.DrawWireSphere(rightRestPosition, 0.1f);
        
    }

    public void EnableCenterLockout()
    {
        float startingXPos = leftRestPosition.x;
        
        // Cancel existing tweens
        LeanTween.cancel(leftTf.gameObject);
        LeanTween.cancel(rightTf.gameObject);
        
        if (lockoutState != LockoutState.DISABLED)
        {
            // We are already either locking or unlocking (we can't already be locked in this function)
            // We also assume that the left and right lockout objects are in the same mirrored position (negative for left)
            startingXPos = leftTf.position.x;
        }

        animation = LeanTween.value(leftTf.gameObject, SetLeftXPos, startingXPos, 0f, activationTime).setEase(customCurve).setOnComplete(OnLocked);
        LeanTween.value(rightTf.gameObject, SetRightXPos, -startingXPos, 0f, activationTime).setEase(customCurve);
        lockoutState = LockoutState.LOCKING;
    }

    public void DisableCenterLockout()
    {
        float startingXPos = 0f;
        
        LeanTween.cancel(leftTf.gameObject);
        LeanTween.cancel(rightTf.gameObject);

        if (lockoutState != LockoutState.LOCKED)
        {
            // We are not starting our tween from a locked position
            startingXPos = leftTf.position.x;
        }

        
        LeanTween.value(leftTf.gameObject, SetLeftXPos, startingXPos, leftRestPosition.x, activationTime).setEase(LeanTweenType.easeInOutSine).setOnComplete(OnDisabled);
        LeanTween.value(rightTf.gameObject, SetRightXPos, -startingXPos, rightRestPosition.x, activationTime).setEase(LeanTweenType.easeInOutSine);
        compositeCollider.enabled = false;
        leftCollider.enabled = true;
        rightCollider.enabled = true;
        lockoutState = LockoutState.UNLOCKING;
    }

    private void SetLeftXPos(float x)
    {
        // Only the left side checks if we are locked as its a mirror
        if (animation.passed > 0.6f * activationTime && particleCanSpawn)
        {
            particleCanSpawn = false;
            Instantiate(centerLockoutParticleEffect, Vector2.zero, Quaternion.identity);
        }


        if (Mathf.Approximately(x, leftRestPosition.x) && lockoutState == LockoutState.UNLOCKING)
        {
            lockoutState = LockoutState.DISABLED;
        }

        Vector2 pos = new Vector2(x, 0f);
        leftTf.position = pos;
        
    }
    
    private void SetRightXPos(float x)
    {
        Vector2 pos = new Vector2(x, 0f);
        rightTf.position = pos;
    }

    private void OnLocked()
    {
        lockoutState = LockoutState.LOCKED;
        leftCollider.enabled = false;
        rightCollider.enabled = false;
        compositeCollider.enabled = true;
    }

    private void OnDisabled()
    {
        lockoutState = LockoutState.DISABLED;
        particleCanSpawn = true;
    }




}

enum LockoutState
{
    DISABLED,
    UNLOCKING,
    LOCKING,
    LOCKED
}