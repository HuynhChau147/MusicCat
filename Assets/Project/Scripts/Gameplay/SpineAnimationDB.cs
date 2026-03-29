using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Spine/Animation Database")]
public class SpineAnimationDB : ScriptableObject
{
    public string AnimNamePrefix = "";
    public List<AnimationReferenceAsset> animations;
}