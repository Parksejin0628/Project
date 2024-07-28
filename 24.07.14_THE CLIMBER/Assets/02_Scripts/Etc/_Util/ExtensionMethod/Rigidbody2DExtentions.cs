using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultSetting.Utility
{
    public static class Rigidbody2DExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        public static void SetVelocityX(this Rigidbody2D rig, float value)
        {
            rig.velocity = new Vector2(value, rig.velocity.y);
        }

        public static void SetVelocityY(this Rigidbody2D rig, float value)
        {
            rig.velocity = new Vector2(rig.velocity.x, value);
        }

        public static void ClampVelocityVector2(this Rigidbody2D rig, float clampValue)
        {
            rig.velocity = new Vector2(Mathf.Clamp(rig.velocity.x, -clampValue, clampValue), Mathf.Clamp(rig.velocity.y, -clampValue, clampValue));
        }

        public static void ClampVelocityX(this Rigidbody2D rig, float clampValue)
        {
            //rig.velocity = new Vector2(Mathf.Clamp(rig.velocity.x, -clampValue, clampValue), rig.velocity.y);
            rig.velocity = Vector2.Lerp(rig.velocity, new Vector2(Mathf.Clamp(rig.velocity.x, -clampValue, clampValue), rig.velocity.y), 0.05f);
        }

        public static void ClampVelocityY(this Rigidbody2D rig, float clampValue)
        {
            rig.velocity = new Vector2(rig.velocity.x, Mathf.Clamp(rig.velocity.y, -clampValue, clampValue));
        }
    }
}