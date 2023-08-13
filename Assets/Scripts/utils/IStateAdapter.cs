using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateAdapter
{
    public void StateSetup();
    public void StateCleanup();
    public void StatefulUpdate();
}
