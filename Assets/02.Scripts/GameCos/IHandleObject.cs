using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IHandleObject
{
    public bool Grabbed { get; set; }
    public void EnterGrabbing();
    public void ExitGrabbing();
    public void ItemUse();
    public void InputButtonEvent();
}
