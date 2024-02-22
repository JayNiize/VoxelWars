using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionable
{
    public void ShowActionInfo();

    public void HideActionInfo();

    void ExecuteAction();
}