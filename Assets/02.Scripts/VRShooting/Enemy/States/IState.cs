using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    public void Init(T controller);
    public void OperateEnter();
    public void OperateUpdate();
    public void OperateExit();
}
