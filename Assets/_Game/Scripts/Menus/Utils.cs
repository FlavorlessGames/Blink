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
    }
}