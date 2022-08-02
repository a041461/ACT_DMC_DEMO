using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CamerShakeAction : Action
{
    public override TaskStatus OnUpdate()
    {
        CameraHandle.Instance.CameraShake();
        return TaskStatus.Success;
    }
}
