using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public bool fix = false;
    public Animator playerAnimator;
    public Animator bossAnimator;
    public RuntimeAnimatorController playerAnim;
    public RuntimeAnimatorController bossAnim;
    public PlayableDirector director;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerAnim = playerAnimator.runtimeAnimatorController;
        playerAnimator.runtimeAnimatorController = null;
        bossAnim = bossAnimator.runtimeAnimatorController;
        bossAnimator.runtimeAnimatorController = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && !fix)
        {
            fix = true;
            playerAnimator.runtimeAnimatorController = playerAnim;
            bossAnimator.runtimeAnimatorController = bossAnim;
        }
    }
}
