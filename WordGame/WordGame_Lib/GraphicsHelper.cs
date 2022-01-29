using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WordGame_Lib.Ui;

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
                        iTexture.Bounds,
                        Color.White), 
                    iEffect));
        }

        public static void DrawString(SpriteFont iFont, string iText, Vector2 iPosition, Color iColor, float iScaling = 1.0f, float iSpacing = 0.0f)
        {
            Debug.Assert(sSpriteBatch != null);
            ThisIterationDrawPlans.Add(new DrawPlan(
                () =>
                {
                    iFont.Spacing = iSpacing;
                    sSpriteBatch.DrawString(iFont, iText, iPosition, iColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
                },
                null));
        }

        public static void DrawStringWithBorder(SpriteFont iFont, string iText, Vector2 iPosition, float iOffset, Color iInnerColor, Color iOuterColor, float iScaling = 1.0f, float iSpacing = 0.0f)
        {
            Debug.Assert(sSpriteBatch != null);
            ThisIterationDrawPlans.Add(new DrawPlan(
                () =>
                {
                    iFont.Spacing = iSpacing;
                    sSpriteBatch.DrawString(iFont, iText, iPosition + new Vector2(-iOffset, -iOffset), iOuterColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
                    sSpriteBatch.DrawString(iFont, iText, iPosition + new Vector2(-iOffset, iOffset), iOuterColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
                    sSpriteBatch.DrawString(iFont, iText, iPosition + new Vector2(iOffset, -iOffset), iOuterColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
                    sSpriteBatch.DrawString(iFont, iText, iPosition + new Vector2(iOffset, iOffset), iOuterColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
                    sSpriteBatch.DrawString(iFont, iText, iPosition, iInnerColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
                },
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



        // ReSharper disable InconsistentNaming
        public static void CalculatePointLightShaderParameters(IReadOnlyList<PointLight> iAllPoints, out Vector2[] oPositions, out Vector4[] oColors, out float[] oRadii, out float[] oIntensity)
        {
            const int maxLights = 30;

            oPositions = new Vector2[maxLights];
            oColors = new Vector4[maxLights];
            oRadii = new float[maxLights];
            oIntensity = new float[maxLights];

            for (var ii = 0; ii < maxLights; ii++)
            {
                if (ii < iAllPoints.Count && iAllPoints[ii].Intensity >= 1.0f)
                {
                    var pointLightData = iAllPoints[ii];
                    // I think the Y coordinate of shader math has 0 at the bottom of the screen and counts positive going up
                    oPositions[ii] = new Vector2(pointLightData.Position.X, pointLightData.Position.Y);
                    oColors[ii] = new Vector4(pointLightData.LightColor.R / 255f, pointLightData.LightColor.G / 255f, pointLightData.LightColor.B / 255f, pointLightData.LightColor.A / 255f);
                    oRadii[ii] = pointLightData.Radius;
                    oIntensity[ii] = pointLightData.Intensity;
                }
                else
                {
                    oPositions[ii] = Vector2.Zero;
                    oColors[ii] = Vector4.Zero;
                    oRadii[ii] = 0.0f;
                    oIntensity[ii] = 0.0f;
                }
            }
        }
        // ReSharper restore InconsistentNaming

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
