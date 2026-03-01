using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class Open_Close_Animation : MonoBehaviour
{
    [SerializeField] GameObject door;

    private Animator mAnimator;

    public AudioClip moveSound;

    public bool flag = false, is_open = false;

    // Start is called before the first frame update void Start()
    void Start()
    {
        mAnimator = door.GetComponent<Animator>();
    }
    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        flag = true;
    }

    private void OnTriggerExit(Collider other)
    {
        flag = false;

    }

    void Update()
    {
        
            if (mAnimator != null)
                if (flag && Input.GetKeyDown(KeyCode.O))
                {
                    if (!is_open)
                    {
                        mAnimator.SetTrigger("TriOpen");
                        AudioSource.PlayClipAtPoint(moveSound, transform.position);
                        is_open = true;
                    }
                    else
                    {
                        mAnimator.SetTrigger("TriClose");
                        AudioSource.PlayClipAtPoint(moveSound, transform.position);
                        is_open = false;
                    }
                }
    }
}
