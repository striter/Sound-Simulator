﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKeys : InteractorBase {
    public string S_PickupTips;
    public int I_KeyIndex;

    public override bool TryInteract()
    {
        GameManager.Instance.PickupKey(I_KeyIndex);
        UIManager.Instance.AddTips(S_PickupTips);
        AudioManager.Play("Key_Pickup",this.gameObject);
        UIManager.Instance.AddSubtitle("Key_Pickup");
        UIManager.Instance.AddTips("Key Pickup");
        transform.SetActivate(false);
        return true;
    }
}
