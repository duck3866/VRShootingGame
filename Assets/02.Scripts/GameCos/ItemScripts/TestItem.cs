using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour, IHandleObject
{
   public bool Grabbed { get; set; }

   public void EnterGrabbing()
   {
      Grabbed = true;
   }

   public void ExitGrabbing()
   {
      Grabbed = false;
   }

   public void ItemUse()
   {
      
   }

   public void InputButtonEvent()
   {
      
   }
}
