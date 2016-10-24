using UnityEngine;
using System.Collections;
using System;

public interface IButtonable {
    string ButtonText { get; }
    string TooltipText { get; }
    bool IsInvokable { get; }
    bool IsVisibleOnDisable { get; }

    void Invoke();
}
