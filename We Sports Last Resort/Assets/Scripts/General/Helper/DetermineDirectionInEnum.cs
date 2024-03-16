using System;
using UnityEngine;

namespace General.Helper
{
    public static class DetermineDirectionInEnum
    {
        private static readonly float x_threshold = 0.5f;
        private static readonly float y_threshold = 0.5f;
        private static readonly float diagonal_threshold = 0.025f;

        public static DirectionEnum GetDirectionEnum(Vector2 input)
        {
            DirectionEnum result = DirectionEnum.None;
            

            Vector2 a = input.normalized;
            
            float x_abs = Mathf.Abs(a.x);
            float y_abs = Mathf.Abs(a.y);

            if (x_abs < x_threshold || y_abs < y_threshold)
                result = DirectionEnum.None;
            
            //Check if approx. x == y
            if (Mathf.Abs(x_abs - y_abs) < diagonal_threshold)
            {
                Tuple<bool, bool> isPositiveValues = 
                    new Tuple<bool, bool>((a.x > 0), (a.y > 0));


                switch (isPositiveValues)
                {
                    case (true, true):
                        result = DirectionEnum.UpRight;
                        break;
                    case (true, false):
                        result = DirectionEnum.DownRight;
                        break;
                    case (false, false):
                        result = DirectionEnum.DownLeft;
                        break;
                    case (false, true):
                        result = DirectionEnum.UpLeft;
                        break;
                }
            }
            else if (x_abs > y_abs)
            {
                result = (a.x > 0) ? DirectionEnum.Right : DirectionEnum.Left;
            }
            else if (x_abs < y_abs)
            {
                result = (a.y > 0) ? DirectionEnum.Up : DirectionEnum.Down;
            }
            
            //Debug.Log("Slice Direction: " + result);
            return result;
        }
    
    }

    public enum DirectionEnum
    {
        None,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
}
