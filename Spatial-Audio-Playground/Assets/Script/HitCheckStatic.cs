﻿using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class HitCheckStatic : HitCheckBase {
    public override enum_HitCheckType E_HitCheckType => enum_HitCheckType.Static;
    public enum_GroundMaterialType E_Mateiral = enum_GroundMaterialType.Invalid;
}
