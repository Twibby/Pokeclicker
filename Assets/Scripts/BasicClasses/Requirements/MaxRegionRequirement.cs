using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxRegionRequirement : Requirement
{
    public MaxRegionRequirement(Region region = Region.None, Requirement.AchievementOption option = AchievementOption.more) 
        : base((double)region, option)
    {
        
    }

    public override double GetProgress()
    {
        // TODO
        // Change this currentMaxRegion with user choice (in a setting panel ?)
        return System.Math.Min((double)PlayerSettings.CurrentMaxRegion, this.requiredValue);
    }

    public override string Hint()
    {
        return "You need to reach the " + ((Region)this.requiredValue).ToString();

    }
}
