using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib
{
    public static class GraphicsHelper
    {
        static GraphicsHelper()
        {
            ThisIterationDrawPlans = new List<DrawPlan>();
        }

        public static Texture2D CreateTexture(Color[] iColorData, int iWidth, int iHeight)
        {
            Debug.Assert(sGraphicsDevice != null);

            var texture = new Texture2D(sGraphicsDevice, iWidth, iHeight);
            texture.SetData(iColorData);
            return texture;
        }

        public static T LoadContent<T>(string iContentName)
        {
            Debug.Assert(sContentManager != null);

            return sContentManager.Load<T>(iContentName);
        }

        public static void DrawTexture(Texture2D iTexture, Vector2 iPosition, Effect iEffect = null)
        {
            Debug.Assert(sSpriteBatch != null);
            ThisIterationDrawPlans.Add(new DrawPlan(() => sSpriteBatch.Draw(iTexture, iPosition, Color.White), iEffect));
        }

        public static void DrawTexture(Texture2D iTexture, Rectangle iTargetBounds, Effect iEffect = null)
        {
            Debug.Assert(sSpriteBatch != null);
            ThisIterationDrawPlans.Add(
                new DrawPlan(() => sSpriteBatch.Draw(
                        iTexture,
                        iTargetBounds,
                        new Rectangle(iTargetBounds.X, iTargetBounds.Y, iTexture.Width, iTexture.Height),
                        Color.White), 
                    iEffect));
        }

        public static void DrawString(SpriteFont iFont, string iText, Vector2 iPosition, Color iColor, float iScaling = 1.0f)
        {
            Debug.Assert(sSpriteBatch != null);
            ThisIterationDrawPlans.Add(new DrawPlan(
                () => sSpriteBatch.DrawString(iFont, iText, iPosition, iColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f), 
                    null));
        }

        public static void Flush()
        {
            var inSpriteBatch = false;
            foreach (var drawPlan in ThisIterationDrawPlans)
            {
                if (drawPlan.DrawEffect == null)
                {
                    if (!inSpriteBatch)
                    {
                        sSpriteBatch.Begin();
                        inSpriteBatch = true;
                    }
                    drawPlan.DrawAction();
                }
                else
                {
                    if (inSpriteBatch)
                    {
                        sSpriteBatch.End();
                    }
                    sSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, effect: drawPlan.DrawEffect);
                    drawPlan.DrawAction();
                    sSpriteBatch.End();
                    inSpriteBatch = false;
                }
            }

            if (inSpriteBatch)
                sSpriteBatch.End();

            ThisIterationDrawPlans.Clear();
        }

        public static Rectangle GamePlayArea
        {
            get
            {
                Debug.Assert(sGamePlayArea.HasValue);

                return sGamePlayArea ?? Rectangle.Empty;
            }
        }
        private static Rectangle? sGamePlayArea;

        public static void RegisterGraphicsDevice(GraphicsDevice iGraphicsDevice)
        {
            Debug.Assert(sGraphicsDevice == null);
            sGraphicsDevice = iGraphicsDevice;
        }

        public static void RegisterSpriteBatch(SpriteBatch iSpriteBatch)
        {
            Debug.Assert(sSpriteBatch == null);
            sSpriteBatch = iSpriteBatch;
        }

        public static void RegisterContentManager(ContentManager iContentManager)
        {
            Debug.Assert(sContentManager == null);
            sContentManager = iContentManager;
        }

        public static void RegisterGamePlayArea(Rectangle iGamePlayArea)
        {
            Debug.Assert(!sGamePlayArea.HasValue);
            sGamePlayArea = iGamePlayArea;
        }

        private static GraphicsDevice sGraphicsDevice;
        private static SpriteBatch sSpriteBatch;
        private static ContentManager sContentManager;

        private static readonly List<DrawPlan> ThisIterationDrawPlans;

        private class DrawPlan
        {
            public DrawPlan(Action iDrawAction, Effect iEffect)
            {
                DrawAction = iDrawAction;
                DrawEffect = iEffect;
            }

            public Action DrawAction { get; }
            public Effect DrawEffect { get; }
        }
    }
}
