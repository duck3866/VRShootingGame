using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour, IHandleObject
{
   public bool Grabbed { get; set; }
   public bool parentObjectIsRight { get; set; }

   public void EnterGrabbing(GameObject grabbingTransform)
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
   public bool IsCanGrab()
   {
      if (Grabbed)
      {
         return false;
      }
      else
      {
         return true;
      }
   }
}
