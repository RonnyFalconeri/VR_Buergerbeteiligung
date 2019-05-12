using UnityEngine;

public class AvatarAnimation : MonoBehaviour
{
    private Animator animator;
    int idle = 0;
    int walk = 1;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            animator.SetInteger("state", walk);
        }
        else if (Input.GetKey("a"))
        {
            animator.SetInteger("state", walk);
        }
        else if (Input.GetKey("s"))
        {
            animator.SetInteger("state", walk);
        }
        else if (Input.GetKey("d"))
        {
            animator.SetInteger("state", walk);
        }
        else
        {
            animator.SetInteger("state", idle);
        }
    }
}
