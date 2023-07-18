using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class InputManager 
{
    public static Actions Actions = new Actions();

    private static InputMode _inputMode { get; set; } = InputMode.Gameplay;

    static InputManager()  // what to do if we change scenes? Track scene statically, or have external setters
    {
        Actions.Enable();
        SetMode(InputMode.Gameplay);
    }

    public static void SetMode(InputMode mode)
    {
        void SetActiveMaps(bool gameplay, bool ui, bool visualiser)
        {
            if (gameplay)
            {
                Actions.gameplay.Enable();
            }
            else
            {
                Actions.gameplay.Disable();
            }

            if (ui)
            {
                Actions.ui.Enable();
            }
            else
            {
                Actions.ui.Disable();
            }

            if (visualiser)
            {
                Actions.visualiser.Enable();
            }
            else
            {
                Actions.visualiser.Disable();
            }
        }

        _inputMode = mode;

        if (_inputMode == InputMode.Gameplay)
        {
            SetActiveMaps(true, true, false);

            Actions.gameplay.primary.Enable();
            Actions.gameplay.secondary.Enable();
        }
        else if (_inputMode == InputMode.UI)
        {
            SetActiveMaps(true, true, false);

            Actions.gameplay.primary.Disable();
            Actions.gameplay.secondary.Disable();
        }
        else if (_inputMode == InputMode.Visualiser)
        {
            SetActiveMaps(false, false, true);
        }
    }

}

public enum InputMode
{
    Gameplay,
    UI,
    Visualiser
}
