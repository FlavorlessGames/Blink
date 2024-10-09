using UnityEngine.UIElements;
using System;

namespace fgames.Menus
{
    public static class Utils
    {
        public static Button NewButton(string text, Action onClick)
        {
            Button newButton = new Button(onClick);
            newButton.text = text;
            return newButton;
        }

        public static Toggle NewToggle(string text, bool value, Action<bool> onChange)
        {
            Toggle newToggle = new Toggle(text);
            newToggle.value = value;
            newToggle.RegisterCallback<ChangeEvent<bool>>((evt) => {
                // UnityEngine.Debug.Log(evt.newValue);
                // valueChanged?.Invoke(evt.newValue);
                onChange(evt.newValue);
            });
            return newToggle;
        }
    }
}