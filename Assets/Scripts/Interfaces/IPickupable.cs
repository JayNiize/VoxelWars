using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable : IActionable
{
    public void ShowPickupInfo();

    public void HidePickupInfo();
}