using System.Collections;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform canvasTr;
    [SerializeField] private float attackCooldown = 0.2f;
    private readonly int isMovingHash = Animator.StringToHash("isWalking");
    private readonly int isAttackingHash = Animator.StringToHash("attack");
    private readonly int isDamagedHash = Animator.StringToHash("damage");
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

    public void RendererVisuals(Vector2 velocity) 
    {
        if (!init) return;

        var isMoving = velocity.x > 0.1f || velocity.x < -0.1f;

        animator.SetBool(isMovingHash, isMoving);
    }

    public void AttackAnimation() 
    {
        animator.SetTrigger(isAttackingHash);
    }

    public void PlayerDamageAnimation() 
    {
        animator.SetTrigger(isDamagedHash);
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
