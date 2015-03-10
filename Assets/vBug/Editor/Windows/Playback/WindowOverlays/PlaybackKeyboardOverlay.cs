using System;
using UnityEditor;
using UnityEngine;
using Frankfort.VBug.Internal;
using System.Collections.Generic;

namespace Frankfort.VBug.Editor
{
    
    public class PlaybackKeyboardOverlay : BasePlaybackOverlay
    {

        private class Key
        {
            public KeyCode primCode;
            public KeyCode secCode;
            public float scale = 1f;
            public string primLabel;
            public string secLabel;

            public Key(KeyCode primCode)
            {
                this.primCode = primCode;
                this.primLabel = primCode.ToString();
            }

            public Key(KeyCode primCode, KeyCode secCode)
            {
                this.primCode = primCode;
                this.secCode = secCode;
                this.primLabel = primCode.ToString();
            }

            public Key(KeyCode primCode, float scale)
            {
                this.primCode = primCode;
                this.primLabel = primCode.ToString();
                this.scale = scale;
            }

            public Key(KeyCode primCode, KeyCode secCode, float scale)
            {
                this.primCode = primCode;
                this.secCode = secCode;
                this.scale = scale;
                this.primLabel = primCode.ToString();
            }

            public Key(KeyCode primCode, string primLabel)
            {
                this.primCode = primCode;
                this.primLabel = primLabel;
            }
            
            public Key(KeyCode primCode, KeyCode secCode, string primLabel, string secLabel)
            {
                this.primCode = primCode;
                this.secCode = secCode;
                this.primLabel = primLabel;
                this.secLabel = secLabel;
            }

            public Key(KeyCode primCode, KeyCode secCode, float scale, string primLabel, string secLabel)
            {
                this.primCode = primCode;
                this.secCode = secCode;
                this.scale = scale;
                this.primLabel = primLabel;
                this.secLabel = secLabel;
            }

            public Key(KeyCode primCode, float scale, string primLabel)
            {
                this.primCode = primCode;
                this.scale = scale;
                this.primLabel = primLabel;
            }
        }




        //--------------- Keyboard layout --------------------
        private float buttonSizeX = 30;
        private float buttonSizeY = 25;
        private float spacing = 3;


        private Key[][] layout = new Key[][]
        {
            new Key[]
            {  
                new Key(KeyCode.Escape, 1.5f, "Esc"), new Key(KeyCode.F1, 0.75f), new Key(KeyCode.F2, 0.75f), new Key(KeyCode.F3, 0.75f), 
                new Key(KeyCode.F4, 0.75f), new Key(KeyCode.F5, 0.75f), new Key(KeyCode.F6, 0.75f), new Key(KeyCode.F7, 0.75f), 
                new Key(KeyCode.F8, 0.75f), new Key(KeyCode.F9, 0.75f), new Key(KeyCode.F10, 0.75f), new Key(KeyCode.F11, 0.75f), 
                new Key(KeyCode.F12, 0.75f), new Key(KeyCode.SysReq, KeyCode.Print, 1.5f, "SysRq", "PrtSc"),
                new Key(KeyCode.Break, KeyCode.Pause, 1.5f), new Key(KeyCode.Insert, 1.5f, "Ins"), new Key(KeyCode.Delete, "Del") },
            new Key[]
            {  
                new Key(KeyCode.BackQuote, 1.5f, "~"), new Key(KeyCode.Alpha1, KeyCode.Exclaim, "1", "!"), new Key(KeyCode.Alpha2, KeyCode.At, "2", "@"), new Key(KeyCode.Alpha3, KeyCode.Hash, "3", "#"), 
                new Key(KeyCode.Alpha4, KeyCode.Dollar, "4", "$"), new Key(KeyCode.Alpha5, (KeyCode)37, "5", "%"), new Key(KeyCode.Alpha6, KeyCode.Caret, "6", "^"), new Key(KeyCode.Alpha7, KeyCode.Ampersand, "7", "&"), 
                new Key(KeyCode.Alpha8, KeyCode.Asterisk, "8", "*"), new Key(KeyCode.Alpha9, KeyCode.LeftParen, "9", "("), new Key(KeyCode.Alpha0, KeyCode.RightParen, "0", ")"), new Key(KeyCode.Minus, KeyCode.Underscore, "-", "_"), 
                new Key(KeyCode.Plus, KeyCode.Equals, "+", "="), new Key(KeyCode.Backspace,2.5f) },
            new Key[]
            {  
                new Key(KeyCode.Tab, 2.25f), new Key(KeyCode.Q), new Key(KeyCode.W), new Key(KeyCode.E), 
                new Key(KeyCode.R), new Key(KeyCode.T), new Key(KeyCode.Y), new Key(KeyCode.U), 
                new Key(KeyCode.I), new Key(KeyCode.O), new Key(KeyCode.P), new Key(KeyCode.LeftBracket, (KeyCode)123, "[", "{"), 
                new Key(KeyCode.RightBracket, (KeyCode)125, "]", "}"), new Key(KeyCode.Backslash, (KeyCode)124, "\\", "|" )},
            new Key[]
            {  
                new Key(KeyCode.CapsLock, 2.5f), new Key(KeyCode.A), new Key(KeyCode.S), new Key(KeyCode.D), 
                new Key(KeyCode.F), new Key(KeyCode.G), new Key(KeyCode.H), new Key(KeyCode.J), new Key(KeyCode.K), 
                new Key(KeyCode.L), new Key(KeyCode.Semicolon, KeyCode.Colon, ";", ":"), new Key(KeyCode.Quote, KeyCode.DoubleQuote, "'", "\""), new Key(KeyCode.Return, 3.5f, "Enter")},
            new Key[]
            {  
                new Key(KeyCode.LeftShift, 3f, "Shift"), new Key(KeyCode.Z), new Key(KeyCode.X), new Key(KeyCode.C), 
                new Key(KeyCode.V), new Key(KeyCode.B), new Key(KeyCode.N), new Key(KeyCode.M), 
                new Key(KeyCode.Comma , KeyCode.Less, ",", "<"), new Key(KeyCode.Period, KeyCode.Greater, ".", ">"), new Key(KeyCode.Slash, KeyCode.Question, "/", "?"), new Key(KeyCode.RightShift, 3f, "Shift")},      
            new Key[]
            {  
                new Key(KeyCode.LeftControl, "Ctrl"), new Key(KeyCode.LeftCommand, "Cmd"), new Key(KeyCode.LeftAlt, "Alt"), new Key(KeyCode.Space, 6.5f, " "), 
                new Key(KeyCode.RightAlt, "Alt"), new Key(KeyCode.RightCommand, "Cmd"), new Key(KeyCode.RightControl, "Ctrl"), 
                new Key(KeyCode.PageUp, KeyCode.Home, 1.5f, "Pg Up", "Home"), new Key(KeyCode.UpArrow, "▲") , new Key(KeyCode.PageDown, KeyCode.End, 1.5f, "Pg Dn", "End")}, 
            new Key[]
            {
                new Key(KeyCode.None, 13.6f), new Key(KeyCode.LeftArrow, "◀"), new Key(KeyCode.DownArrow, "▼"), new Key(KeyCode.RightArrow, "▶"), new Key(KeyCode.None)}
        };
        //--------------- Keyboard layout --------------------



        //--------------- Enum indexing --------------------
        private Dictionary<string, KeyCode> indexedKeyCodes;
        //--------------- Enum indexing --------------------
			


        public PlaybackKeyboardOverlay()
        {
            this.doDrawMainContainer = false;
            this.doDrawSubContainer = true;
        }


        public override bool DrawSubContainer(int currentFrame, vBugBaseWindow parent)
        {
            VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(currentFrame);
            if (slice == null || slice.humanInput == null)
                return false;

            foreach (HumanInputProviderData provider in slice.humanInput.providersData) {
                if (provider == null)
                    continue;

                if (provider.providerType == "KeyboardInputProvider") {
                    DrawVirtualKeyboard(provider, parent);
                    break;
                }
            }

            return true;
        }

        private void DrawVirtualKeyboard(HumanInputProviderData data, vBugBaseWindow parent)
        {
            Texture2D normal = VisualResources.keyboardNormal;
            Texture2D down = VisualResources.keyboardDown;
            Texture2D hold = VisualResources.keyboardHold;
            Texture2D up = VisualResources.keyboardUp;

            float x = 0;
            float y = 0;
            float halfHeight = buttonSizeY / 2;

            Rect minRect = GUILayoutUtility.GetRect(parent.position.width, (spacing + buttonSizeY) * (layout.Length + 1));
            
            float maxWidth = buttonSizeX * 17.5f;
            float startX = (minRect.width / 2f) - (maxWidth / 2f);
            float startY = minRect.y + (halfHeight);

            GUIStyle style = EditorHelper.styleLabelKeyboard;

            for (int r = 0; r < layout.Length; r++)
            {
                Key[] row = layout[r];
                for (int e = 0; e < row.Length; e++)
                {
                    Key key = row[e];
                    float width = Mathf.Floor(buttonSizeX * key.scale);
                    if (e == row.Length - 1)
                        width = maxWidth - x;

                    if (!string.IsNullOrEmpty(key.primLabel) && key.primCode != KeyCode.None)
                    {
                        Rect btnRect = new Rect(startX + x, startY + y, width, buttonSizeY);
                        VisualResources.DrawTexture(btnRect, GetKeyColor(key.primCode, key.secCode, data, normal, down, hold, up));
                        
                        if(!string.IsNullOrEmpty(key.secLabel))
                        {
                            GUI.contentColor = new Color(0.75f, 0.75f, 0.75f);
                            GUI.Label(new Rect(startX + x, startY + y, width, halfHeight), key.secLabel, style);
                            GUI.contentColor = Color.white;
                            GUI.Label(new Rect(startX + x, startY + y + halfHeight, width, halfHeight), key.primLabel, style);
                        }
                        else
                        {
                            GUI.Label(btnRect, key.primLabel, style);
                        }
                    }
                    x += width + spacing;
                }

                x = 0;
                y += buttonSizeY + spacing;
            }
        }



        private Texture2D GetKeyColor(KeyCode keyCode1, KeyCode keyCode2, HumanInputProviderData data, Texture2D normal, Texture2D down, Texture2D hold, Texture2D up)
        {
            if (data == null || data.basicInput == null || data.basicInput.Length == 0)
                return normal;

            if(indexedKeyCodes == null)
            {
                KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
                indexedKeyCodes = new Dictionary<string, KeyCode>();
                foreach (KeyCode key in keyCodes)
                {
                    string sKey = key.ToString();
                    if (!indexedKeyCodes.ContainsKey(sKey))
                        indexedKeyCodes.Add(sKey, key);
                }
            }

            foreach(BasicInputCommand command in data.basicInput)
            {
                if (indexedKeyCodes[command.id] == keyCode1 || indexedKeyCodes[command.id] == keyCode2)
                {
                    switch (command.state)
                    {
                        case HumanInputState.down:
                            return down;
                        case HumanInputState.hold:
                            return hold;
                        case HumanInputState.up:
                            return up;
                    }
                    break;
                }
            }

            return normal;
        }
    }
}