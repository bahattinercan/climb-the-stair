using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    [SerializeField] private StairStep stairStep;
    private float lerpDuration = .6f;
    private bool canMove = true;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canMove == true && GameManager.instance.canPlay)
        {
            canMove = false;
            stairStep = StairSpawner.instance.SpawnStair();
            AudioManager.instance.PlaySFX(SfxType.tinyClick);
            GameManager.instance.StepUp();
            transform.Rotate(new Vector3(0, StairSpawner.instance.BaseStairRotation.y, 0));            
            StartCoroutine(Move(stairStep.stepPos.position));
        }
    }

    private IEnumerator Move(Vector3 pos)
    {
        canMove = false;
        anim.SetBool("isMoving", true);
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, pos, timeElapsed / lerpDuration/2);
            GameManager.instance.background.Scroll();
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isMoving", false);
        canMove = true;
    }
}