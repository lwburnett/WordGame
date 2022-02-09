using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace WordGame_Lib
{
    public static class AssetHelper
    {
        public static T LoadContent<T>(string iContentName)
        {
            Debug.Assert(sContentManager != null);

            return sContentManager.Load<T>(iContentName);
        }

        public static void RegisterContentManager(ContentManager iContentManager)
        {
            Debug.Assert(sContentManager == null);
            sContentManager = iContentManager;
        }

        private static ContentManager sContentManager;
    }
}
