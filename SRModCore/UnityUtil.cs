﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Il2Cppcom.Kluge.XR.Utils;
using Il2CppInterop.Runtime;
using MelonLoader;
using UnityEngine;
using static Il2CppRootMotion.FinalIK.RagdollUtility;

namespace SRModCore
{
    public class UnityUtil
    {
        /// <summary>
        /// Finds the root transforms in the current Unity scene and logs out their names.
        /// Useful for finding a root to start logging hierarchies with when nothing is known.
        /// </summary>
        /// <param name="logger"></param>
        public static void LogRootTransformsInScene(SRLogger logger)
        {
            var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                logger.Msg($"Root '{root.name}'");
            }
        }

        /// <summary>
        /// Retrieve the root(most parent) transform from the given transform.
        /// </summary>
        /// <param name="transform">Starting Transform in the hierarchy</param>
        /// <returns>Topmost transform in hierarchy. Returns null if transform is null</returns>
        public static Transform GetRootTransform(Transform transform)
        {
            return GetParentTransformByName(transform, null);
        }

        /// <summary>
        /// Go up the hierarchy until the parent with the given name is reached. Returns the topmost element if target not found.
        /// </summary>
        /// <param name="transform">Starting Transform in the hierarchy</param>
        /// <param name="targetName">Name of target parent Transform</param>
        /// <returns>Transform of target if found as a parent, else the root Transform of the hierarchy. Returns null if given transform is null</returns>
        public static Transform GetParentTransformByName(Transform transform, string targetName)
        {
            if (transform == null)
            {
                return null;
            }

            Transform currentTransform = transform;
            while (currentTransform.name != targetName && currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
            }

            return currentTransform;
        }

        /**
         * Prints out the game object tree/hierarchy below the given root object to the logs.
         * Useful for finding objects to clone/instantiate :)
         */
        public static void LogGameObjectHierarchy(SRLogger logger, Transform root, int indentLevel = 0)
        {
            if (root == null)
            {
                return;
            }

            string tabs = "";
            for (int i = 0; i < indentLevel; i++)
            {
                tabs += "\t";
            }

            // Root
            logger.Msg(string.Format(
                "{0}{1} at local position {2} (global position {3}) with rotation {4} (local rotation {5})",
                tabs,
                root.name,
                root.localPosition,
                root.position,
                root.rotation,
                root.localRotation
            ));
            if (root is RectTransform)
            {
                logger.Msg(string.Format(
                    "{0} rect: {1}, anchor min {2}, max {3}, local scale: {4}, sizeDelta: {5}",
                    tabs,
                    ((RectTransform)root).rect,
                    ((RectTransform)root).anchorMin,
                    ((RectTransform)root).anchorMax,
                    ((RectTransform)root).localScale,
                    ((RectTransform)root).sizeDelta
                ));
            }

            // Children
            for (int i = 0; i < root.childCount; i++)
            {
                LogGameObjectHierarchy(logger, root.GetChild(i), indentLevel + 1);
            }
        }

        /**
         * Prints out the components attached to the given game object and all children.
         * Useful for describing objects to clone/instantiate
         */
        public static void LogComponentsRecursive(SRLogger logger, Transform root, string tabs = "")
        {
            if (root == null)
            {
                return;
            }

            // Root
            logger.Msg(string.Format(
                "{0}{1} components:",
                tabs,
                root.name
            ));

            // Components
            tabs += "\t";
            foreach (Component component in root.GetComponents<Component>())
            {
                var il2cppType = component.GetIl2CppType();
                if (il2cppType == Il2CppType.Of<SpriteRenderer>(component))
                {
                    var asSpriteRenderer = component.Cast<SpriteRenderer>();
                    logger.Msg(string.Format(
                        "{0}{1} ({2}). Sprite: {3}, Color: {4}, Size: {5}",
                        tabs,
                        component.name,
                        il2cppType.FullName,
                        asSpriteRenderer.sprite,
                        asSpriteRenderer.color,
                        asSpriteRenderer.size
                    ));
                }
                else if (il2cppType == Il2CppType.Of<RectTransform>(component))
                {
                    var asRect = component.Cast<RectTransform>();
                    logger.Msg(string.Format(
                        "{0}{1} ({2}). Rect: {3}, anchor min {4}, max {5}, scale: {6}, local scale: {7}, sizeDelta: {8}",
                        tabs,
                        component.name,
                        il2cppType.FullName,
                        asRect.rect,
                        asRect.anchorMin,
                        asRect.anchorMax,
                        component.transform.localScale,
                        asRect.localScale,
                        asRect.sizeDelta
                    ));
                }
                else
                {
                    logger.Msg(string.Format(
                        "{0}{1} ({2})",
                        tabs,
                        component.name,
                        il2cppType.FullName
                    ));
                }
            }

            // Children
            for (int i = 0; i < root.childCount; i++)
            {
                LogComponentsRecursive(logger, root.GetChild(i), tabs);
            }
        }

        /// <summary>
        /// Deletes all immediate children from parent whose names aren't in the given array.
        /// If a null array is given, all children are deleted.
        /// </summary>
        /// <param name="parent">Parent of deleted children</param>
        /// <param name="whitelistedNames">GameObject names of immediate children to not delete. If null, all are deleted.</param>
        public static void DeleteChildren(SRLogger logger, Transform parent, string[] whitelistedNames = null)
        {
            if (parent == null)
            {
                return;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                // Don't delete whitelisted or anything created by this mod
                if (whitelistedNames == null || (!whitelistedNames.Contains(child.name) && !child.name.StartsWith("pm_")))
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Set active status of all immediate children from parent whose names aren't in the given array.
        /// If a null array is given, all children are set.
        /// </summary>
        /// <param name="parent">Parent Transform</param>
        /// <param name="active">Whether children should be active or not</param>
        /// <param name="excludedNames">GameObject names of immediate children to not set. If null, all are set.</param>
        public static void SetChildrenActive(Transform parent, bool active, string[] excludedNames = null)
        {
            if (parent == null)
            {
                return;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (excludedNames == null || !excludedNames.Contains(child.name))
                {
                    child.gameObject.SetActive(active);
                }
            }
        }

        public static Sprite CreateSpriteFromAssemblyResource(SRLogger logger, Assembly assemblyWithSpriteResource, string path)
        {
            try
            {
                Stream binStream = assemblyWithSpriteResource.GetManifestResourceStream(path);
                MemoryStream mStream = new MemoryStream();
                binStream.CopyTo(mStream);

                // Size doesn't matter; will be replaced by loaded image
                Texture2D iconTexture = new Texture2D(2, 2);
                iconTexture.LoadImage(mStream.ToArray());
                return Sprite.Create(iconTexture, new Rect(0.0f, 0.0f, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
            }
            catch (Exception e)
            {
                logger.Error("Failed to load sprite from path " + path, e);
                return null;
            }
        }
    }
}
