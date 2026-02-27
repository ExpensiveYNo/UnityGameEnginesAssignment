using UnityEngine;

public class Open_Close_Animation : MonoBehaviour
{
    private Animator mAnimator;

    // Start is called before the first frame update void Start()
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (mAnimator != null)
            if (Input.GetKeyDown(KeyCode.O))
            {
                mAnimator.SetTrigger("TriOpen");
            }

        if (mAnimator != null)
            if (Input.GetKeyDown(KeyCode.C))
            {
                mAnimator.SetTrigger("TriClose");
            }

    }
}
