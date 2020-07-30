using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

记得用管理员测试

//https://blog.csdn.net/erikait/article/details/71637167
//HKEY_CLASSES_ROOT：该注册表项目里面的设置是保证Windows浏览器能够选择正确的应用程序打开相应文件的关键所在。
//在Windows 2000之后，该注册表项目中的文件关联设置分别存在上面所说的两个注册表项中去了。
//而HKEY_CLASS_ROOT 注册表项则成为融合（注意，对于相关的文件类型，HKEY_CURRENT_USER下面的设置会覆盖HKEY_LOCAL_MACHINE下面的设置）上面两个注册表项内容的一个镜像。
//为了更新文件关联设置，你必须更新"HKEY_CURRENT_USER\Software\Classess"或者"HKEY_LOCAL_MACHINE\Software\Classess"下的注册表项，而不是直接更新HKEY_CLASS_ROOT下的注册表项。

namespace EasyPictureViewer.FileIcons
{
    public static class RegisterIcons
    {
        //%SystemRoot%\System32\SHELL32.dll

        public class FileTypeRegInfo
        {
            /// <summary>
            /// 目标类型文件的扩展名
            /// </summary>
            public string ExtendName; //".xcf"

            /// <summary>
            /// 目标文件类型说明
            /// </summary>
            public string Description; //"XCodeFactory项目文件"

            /// <summary>
            /// 目标类型文件关联的图标
            /// </summary>
            public string IcoPath;

            /// <summary>
            /// 打开目标类型文件的应用程序
            /// </summary>
            public string ExePath;

            public FileTypeRegInfo()
            {
            }

            public FileTypeRegInfo(string extendName)
            {
                this.ExtendName = extendName;
            }
        }

        /// <summary>
        /// FileTypeRegister 用于注册自定义的文件类型。
        /// zhuweisky 2005.08.31
        /// </summary>
        public class FileTypeRegister
        {
            #region RegisterFileType
            /// <summary>
            /// RegisterFileType 使文件类型与对应的图标及应用程序关联起来。
            /// </summary>        
            public static void RegisterFileType(FileTypeRegInfo regInfo)
            {
                //if (RegistryHelper.FileTypeRegistered(regInfo.ExtendName))
                //{
                //    return;
                //}

                string relationName = regInfo.ExtendName.Substring(1, regInfo.ExtendName.Length - 1).ToUpper() + "_FileType";

                RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
                fileTypeKey.SetValue("", relationName);
                fileTypeKey.Close();

                RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
                relationKey.SetValue("", regInfo.Description);

                RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
                iconKey.SetValue("", regInfo.IcoPath);

                RegistryKey shellKey = relationKey.CreateSubKey("Shell");
                RegistryKey openKey = shellKey.CreateSubKey("Open");
                RegistryKey commandKey = openKey.CreateSubKey("Command");
                commandKey.SetValue("", regInfo.ExePath + " %1");

                relationKey.Close();
            }

            /// <summary>
            /// GetFileTypeRegInfo 得到指定文件类型关联信息
            /// </summary>        
            public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
            {
                //if (!RegistryHelper.FileTypeRegistered(extendName))
                //{
                //    return null;
                //}

                FileTypeRegInfo regInfo = new FileTypeRegInfo(extendName);

                string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
                RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName);
                regInfo.Description = relationKey.GetValue("").ToString();

                RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");
                regInfo.IcoPath = iconKey.GetValue("").ToString();

                RegistryKey shellKey = relationKey.OpenSubKey("Shell");
                RegistryKey openKey = shellKey.OpenSubKey("Open");
                RegistryKey commandKey = openKey.OpenSubKey("Command");
                string temp = commandKey.GetValue("").ToString();
                regInfo.ExePath = temp.Substring(0, temp.Length - 3);

                return regInfo;
            }

            /// <summary>
            /// UpdateFileTypeRegInfo 更新指定文件类型关联信息
            /// </summary>    
            public static bool UpdateFileTypeRegInfo(FileTypeRegInfo regInfo)
            {
                //if (!RegistryHelper.FileTypeRegistered(regInfo.ExtendName))
                //{
                //    return false;
                //}


                string extendName = regInfo.ExtendName;
                string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
                RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName, true);
                relationKey.SetValue("", regInfo.Description);

                RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon", true);
                iconKey.SetValue("", regInfo.IcoPath);

                RegistryKey shellKey = relationKey.OpenSubKey("Shell");
                RegistryKey openKey = shellKey.OpenSubKey("Open");
                RegistryKey commandKey = openKey.OpenSubKey("Command", true);
                commandKey.SetValue("", regInfo.ExePath + " %1");

                relationKey.Close();

                return true;
            }

            /// <summary>
            /// FileTypeRegistered 指定文件类型是否已经注册
            /// </summary>        
            public static bool FileTypeRegistered(string extendName)
            {
                RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
                if (softwareKey != null)
                {
                    return true;
                }

                return false;
            }
            #endregion
        }

        public static string DefaultIconKeyName { get { return "DefaultIcon"; } }

        public static void Register(bool force = false)
        {
            string assName = Assembly.GetExecutingAssembly().GetName().Name;
            Dictionary<string, string> iconNames = SupportImageFiles.GetFileIconNames();
            RegistryKey temp;

            foreach(var one in iconNames)
            {
                if (one.Key == "")
                {
                    ;
                }
                else if (Regex.Match(one.Key, "^\\.*").Success)
                {
                    if (!Registered(one.Key))
                    {
                        temp = CreateKey(one.Key, true);
                        temp.SetValue("", one.Value, RegistryValueKind.String);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unknown error.");
                }
            }
        }

        public static bool Registered(string name)
        {
            RegistryKey sub;
            sub = Registry.ClassesRoot.OpenSubKey(name);
            if (sub != null)
            {
                sub.Close();
                return true;
            }
            sub.Close();
            return false;
        }

        public static RegistryKey CreateKey(string name, bool needReturn = false)
        {
            RegistryKey sub;
            //存在一定访问冲突，但无法解决。
            sub = Registry.ClassesRoot.CreateSubKey(name);

            if (sub == null)
            {
                throw new Exception(String.Format("Can't create or open the key \"{0}\".", name));
            }

            if (needReturn)
            {
                return sub;
            }
            else
            {
                sub.Close();
                return null;
            }
        }
    }
}
