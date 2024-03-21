using UnityEngine;
using System.Collections;

public class CharacterAnimationController : MonoBehaviour
{
    private Animation anim;
    private RigidbodyCharacterController characterController;

    public AnimationClip idleAnimation;
    public AnimationClip walkForwardAnimation;
    public AnimationClip walkBackwardAnimation;
    public AnimationClip walkRightAnimation;
    public AnimationClip walkLeftAnimation;
    public AnimationClip runAnimation;
    public AnimationClip runBackwardAnimation;
    public AnimationClip jumpAnimation;
    public AnimationClip crouchAnimation;

    void Start()
    {
        anim = GetComponent<Animation>();
        characterController = GetComponent<RigidbodyCharacterController>();

        if (anim == null)
        {
            Debug.LogError("Animation component not found on the object.");
        }

        if (characterController == null)
        {
            Debug.LogError("RigidbodyCharacterController component not found on the object.");
        }

        characterController.OnWalk += (horizontal, vertical) => HandleWalkAnimation(horizontal, vertical);
        characterController.OnRun += (horizontal, vertical) => HandleRunAnimation(horizontal, vertical);
        characterController.OnJump += HandleJumpAnimation;
        characterController.OnCrouch += HandleCrouchAnimation;
        characterController.OnIdle += HandleIdleAnimation;
    }

    private void HandleWalkAnimation(float horizontal, float vertical)
    {
        if (vertical > 0)
            PlayAnimation(walkForwardAnimation);
        else if (vertical < 0)
            PlayAnimation(walkBackwardAnimation);
        else if (horizontal > 0)
            PlayAnimation(walkRightAnimation);
        else if (horizontal < 0)
            PlayAnimation(walkLeftAnimation);
        else
            PlayAnimation(idleAnimation);
    }

    private void HandleRunAnimation(float horizontal, float vertical)
    {
        if (vertical < 0)
            PlayAnimation(runBackwardAnimation);
        else
            PlayAnimation(runAnimation);
    }

    private void HandleJumpAnimation()
    {
        Debug.Log("Jump animation");
        PlayAnimation(jumpAnimation);
        StartCoroutine(WaitForAnimation(jumpAnimation));
    }

    IEnumerator WaitForAnimation(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        HandleIdleAnimation();
    }

    private void HandleCrouchAnimation()
    {

        Debug.Log("Crouch");
        PlayAnimation(crouchAnimation);
    }

    private void HandleIdleAnimation()
    {
        if (!anim.isPlaying || anim.clip != idleAnimation)
        {
            PlayAnimation(idleAnimation);
        }
    }

    void PlayAnimation(AnimationClip clip)
    {
        if (anim.isPlaying && anim.clip == clip)
        {
            return; // Already playing this animation
        }

        anim.clip = clip;
        anim.Play();
    }
}
