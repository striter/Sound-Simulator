﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class AudioManager : SimpleSingletonMono<AudioManager> {
    public AK.Wwise.Event Ww_FootStep;
    public AK.Wwise.Switch Ww_FootStepSwitchConcrete, Ww_FootStepSwitchCarpet, Ww_FootStepSwitchStair, Ww_FootStepSwitchFloor;
    public static void PostEvent(string eventName,GameObject obj)
    {
        AkSoundEngine.PostEvent(eventName,obj);
    }
    public static void PlayFootStep(enum_GroundMaterialType mat, GameObject obj)
    {
        switch (mat)
        {
            case enum_GroundMaterialType.Concrete:
                Instance.Ww_FootStepSwitchConcrete.SetValue(obj);
                break;
            case enum_GroundMaterialType.Floor:
                Instance.Ww_FootStepSwitchFloor.SetValue(obj);
                break;
            case enum_GroundMaterialType.Carpet:
                Instance.Ww_FootStepSwitchCarpet.SetValue(obj);
                break;
            case enum_GroundMaterialType.Stair:
                Instance.Ww_FootStepSwitchStair.SetValue(obj);
                break;
        }
        Instance.Ww_FootStep.Post(obj);
    }
    public static void SwitchGameStatus(bool searchMode)
    {
        if (!searchMode)
        {
            AkSoundEngine.SetState("WorldState", "A");
        }
        else
        {
            AkSoundEngine.SetState("WorldState", "B");
        }
    }
}