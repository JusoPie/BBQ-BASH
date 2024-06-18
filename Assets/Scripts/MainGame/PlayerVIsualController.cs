using System.Collections;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform canvasTr;
    private readonly int isMovingHash = Animator.StringToHash("isWalking");
    private readonly int isAttackingHash = Animator.StringToHash("isAttacking");
    private readonly int isOnAirHash = Animator.StringToHash("isOnAir");
    private bool isFacingRight = true;
    private bool init;
    private Vector3 originalPlayerScale;
    private Vector3 originalCanvasScale;


    private void Start()
    {

        originalPlayerScale = this.transform.localScale;
        originalCanvasScale = canvasTr.transform.localScale;

        init = true;
    }

    public void TriggerDieAnimation() 
    {
        const string TRIGGER = "die";
        animator.SetTrigger(TRIGGER);
    }

    public void TriggerRespawnAnimation()
    {
        const string TRIGGER = "respawn";
        animator.SetTrigger(TRIGGER);
    }

    public void RendererVisuals(Vector2 velocity, bool isAttacking) 
    {
        if (!init) return;

        var isMoving = velocity.x > 0.1f || velocity.x < -0.1f;
        var isAirborne = velocity.y > 0.1f || velocity.y < -0.1f;

        animator.SetBool(isMovingHash, isMoving);
        
        animator.SetBool(isAttackingHash, isAttacking);

        animator.SetBool(isOnAirHash, isAirborne);
        animator.SetFloat("y-Velocity", velocity.y);
    }

    public void TriggerAttackAnimation()
    {
        const string TRIGGER = "attack";
        animator.SetTrigger(TRIGGER);
    }

    public void TriggerDamageAnimation()
    {
        const string TRIGGER = "damage";
        animator.SetTrigger(TRIGGER);
    }

    



    public void UpdateScaleTransforms(Vector2 velocity) 
    {
        if (!init) return;

        if (velocity.x > 0.1f)
        {
            isFacingRight = true;
        }
        else if (velocity.x < -0.1f) 
        {
            isFacingRight = false;
        }

        SetObjectLocalScaleBasedOnDir(gameObject, originalPlayerScale);
        SetObjectLocalScaleBasedOnDir(canvasTr.gameObject, originalCanvasScale);
    }

    private void SetObjectLocalScaleBasedOnDir(GameObject obj, Vector3 originalScale) 
    {
        var yValue = originalScale.y;
        var zValue = originalScale.z;
        var xValue = isFacingRight ? originalScale.x : -originalScale.x;
        obj.transform.localScale = new Vector3(xValue, yValue, zValue);
    }
}
