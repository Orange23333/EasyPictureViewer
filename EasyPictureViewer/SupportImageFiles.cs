using System;
using System.Collections.Generic;
using System.Text;

namespace EasyPictureViewer
{
    public static class SupportImageFiles
    {
        private static Dictionary<string, List<string>> extensions = new Dictionary<string, List<string>>();

        private static bool initialised = false;
        public static void Init()
        {
            if (!initialised)
            {
                //*.bmp;*.gif;*.ico;*.jpg;*.png;*.wdp;*.tiff;

                extensions.Add("Bitmap file", new List<string>(new string[] { "bmp", "dib" }));
                extensions.Add("Joint Photographic Experts Group file", new List<string>(new string[] { "jpg", "jpeg", "jpe", "jfif" }));
                extensions.Add("Graphics Interchange Format file", new List<string>(new string[] { "gif" }));
                extensions.Add("Tag Image File Format file", new List<string>(new string[] { "tif", "tiff" }));
                extensions.Add("Portable Network Graphics file", new List<string>(new string[] { "png" }));
                extensions.Add("Icon file", new List<string>(new string[] { "ico" }));
                extensions.Add("Windows Media Photo file", new List<string>(new string[] { "wdp" }));

                initialised = true;
            }
        }

        public static string GetFilter()
        {
            Init();

            StringBuilder sb = new StringBuilder();

            sb.Append("All support files|");
            foreach (var extType in extensions)
            {
                foreach (string extname in extType.Value)
                {
                    sb.Append("*.");
                    sb.Append(extname);
                    sb.Append(";");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("|");

            foreach (var extType in extensions)
            {
                sb.Append(extType.Key);
                sb.Append("|");
                foreach(string extname in extType.Value)
                {
                    sb.Append("*.");
                    sb.Append(extname);
                    sb.Append(";");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("|");
            }

            sb.Append("All files|*.*");

            return sb.ToString();
        }

        public static string GetFilenameRegularExpression()
        {
            return GetRegularExpression(false, true);
        }
        public static string GetExtensionRegularExpression()
        {
            return GetRegularExpression(true, true);
        }
        public static string GetExtensionWithoutPointRegularExpression()
        {
            return GetRegularExpression(true, false);
        }
        public static string GetRegularExpression(bool onlyExtension, bool hasPoint)
        {
            Init();

            StringBuilder sb = new StringBuilder();
            string temp;
            if (onlyExtension)
            {
                if (hasPoint)
                {
                    temp = "(^.";
                }
                else
                {
                    temp = "(^";
                }
            }
            else
            {
                if (hasPoint)
                {
                    temp = "(*.";
                }
                else
                {
                    throw new ArgumentException("A point is necessary when there is not only extension", "hasPoint");
                }
            }

            foreach (var extType in extensions)
            {
                foreach (string extname in extType.Value)
                {
                    sb.Append(temp);
                    sb.Append(extname);
                    sb.Append("$)|");
                }
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}
