using UnityEngine;
using Cysharp.Threading.Tasks;

public class BuildingAnimation : MonoBehaviour {

    public AnimationCurve animationCurve = null;

    public bool createDone;

   


    private void Start()
    {
        createDone = false;
       CreateAnim().Forget();
    }
    


    public async UniTask CreateAnim()
    {
        float time = 0;
        while (animationCurve.Evaluate(time) > 1.00f&&this !=null)
        {
            
            time += Time.deltaTime;
            transform.localScale = new Vector3(1, animationCurve.Evaluate(time), 1);
            await UniTask.Yield();
        }
        createDone = true;
    }

    

}
