using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyOnRightClick : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(this.TryGetComponent<Robot>(out Robot robot))
            {
                GameManager.RegisterRobotDead(robot.TeamColor);
            }
            Destroy(gameObject);
        }
    }
}
