using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Requirement
{
    public enum AchievementOption
    {
        less,
        equal,
        more,
    }
    public double requiredValue;
    public AchievementOption option ;

    protected Requirement(double requiredValue, AchievementOption option)
    {
        this.requiredValue = requiredValue;
        this.option = option;
    }

    public double getProgressPercentage()
    {
        return System.Math.Round(((this.GetProgress() / this.requiredValue) * 100), 1);
    }

    public bool isCompleted()
    {
        switch (this.option)
        {
            case AchievementOption.less:
                return this.GetProgress() < this.requiredValue;
            case AchievementOption.equal:
                return this.GetProgress() == this.requiredValue;
            case AchievementOption.more:
            default:
                return this.GetProgress() >= this.requiredValue;
        }
    }

    public abstract double GetProgress();
    public abstract string Hint();
}
