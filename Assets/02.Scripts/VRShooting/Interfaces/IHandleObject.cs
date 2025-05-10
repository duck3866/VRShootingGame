using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IHandleObject
{
    public bool Grabbed { get; set; }
    public void EnterGrabbing(GameObject grabbingTransform);
    public void ExitGrabbing();
    public void ItemUse();
    public void InputButtonEvent();
    public bool IsCanGrab();
}
