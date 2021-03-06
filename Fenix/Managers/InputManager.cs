﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fenix.Managers
{
    public class InputManager
    {
        public KeyboardState CurrentKeyboard { get; private set; }
        public KeyboardState LastKeyboard { get; private set; }
        public MouseState CurrentMouse { get; private set; }
        public MouseState LastMouse { get; private set; }
        public bool DrawCursor { get; set; }

        public Vector2 MouseVelocity { get { return (_mouseDeltas[0] + _mouseDeltas[1] + _mouseDeltas[2]) / 3; } }
        public Point CurrentMousePosition { get { return new Point((int)(CurrentMouse.X * WidthRatio), (int)(CurrentMouse.Y * HeightRatio)); } }
        public Point LastMousePosition { get { return new Point((int)(LastMouse.X * WidthRatio), (int)(LastMouse.Y * HeightRatio)); } }
        public Point MouseDelta { get { return new Point(LastMouse.X - CurrentMouse.X, LastMouse.Y - CurrentMouse.Y); } }
        public float WidthRatio { get { return Engine.Settings.Get<float>("Graphics.Virtual.Width") / Engine.Settings.Get<float>("Graphics.Window.Width"); } }
        public float HeightRatio { get { return Engine.Settings.Get<float>("Graphics.Virtual.Height") / Engine.Settings.Get<float>("Graphics.Window.Height"); } }

        private Vector2[] _mouseDeltas = new Vector2[3];

        public InputManager()
        {
            DrawCursor = true;
        }

        internal void Update()
        {
            LastKeyboard = CurrentKeyboard;
            CurrentKeyboard = Keyboard.GetState();

            LastMouse = CurrentMouse;
            CurrentMouse = Mouse.GetState();

            for (int i = 0; i < _mouseDeltas.Length - 1; i++)
                _mouseDeltas[i] = _mouseDeltas[i + 1];
            _mouseDeltas[_mouseDeltas.Length - 1] = new Vector2(MouseDelta.X, MouseDelta.Y);
        }

        internal void Draw()
        {
            if (DrawCursor)
                Engine.SpriteBatch.Draw(Engine.UISheet.Texture, new Vector2(CurrentMouse.X * WidthRatio, CurrentMouse.Y * HeightRatio), Engine.UISheet["Cursor"], Color.White);
        }

        public bool IsLeftPress()
        {
            return CurrentMouse.LeftButton == ButtonState.Pressed;
        }

        public bool WasLeftPress()
        {
            return LastMouse.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftClick()
        {
            return CurrentMouse.LeftButton == ButtonState.Released && LastMouse.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightPress()
        {
            return CurrentMouse.RightButton == ButtonState.Pressed;
        }

        public bool IsRightClick()
        {
            return CurrentMouse.RightButton == ButtonState.Released && LastMouse.RightButton == ButtonState.Pressed;
        }

        public bool IsKeyPress(Keys key)
        {
            return CurrentKeyboard.IsKeyDown(key);
        }

        public bool IsNewKeyPress(Keys key)
        {
            return CurrentKeyboard.IsKeyDown(key) && LastKeyboard.IsKeyUp(key);
        }

        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);
        }
        
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }
        
        public bool IsMenuUp()
        {
            return IsNewKeyPress(Keys.Up);
        }
        
        public bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down);
        }
        
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }
    }
}
