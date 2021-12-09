using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Text;
using System.Collections.Generic;
public class IOS_Adjust_2019_4x_PostProcess 
{
    //[MenuItem("Test/Test")]
    //static void Test()
    //{
    //    Adjust("Assets/z_Test");
    //    AssetDatabase.Refresh();
    //}
    [PostProcessBuildAttribute(9999)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
#if UNITY_2019_3_OR_NEWER
            Adjust(path);
#endif
        }
    }
    static void Adjust(string path)
    {
        var pod = Path.Combine(path, "Podfile");
        if (File.Exists(pod))
        {
            var fils = File.ReadAllText(pod);
            StringBuilder builder = new StringBuilder();
            var list = new List<string>();
            list.AddRange(fils.Split('\n'));
            int startIndex = -1;
            for (int i = 0; i < list.Count; i++)
            {
                var str = list[i];
                if (str.StartsWith("target"))
                {
                    startIndex = i;
                }
                else if (str.StartsWith("end"))
                {
                    if (startIndex >= 0 && startIndex + 1 == i)
                    {
                        if (list[startIndex].StartsWith("target"))
                        {
                            for (int j = startIndex; j <= i; j++)
                            {
                                list[j] = null;
                            }
                        }
                        startIndex = -1;
                    }
                }
            }
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    builder.Append(item);
                    builder.Append('\n');
                }
            }
            string output = pod;
            //for test
            //output = "Assets/z_Test/Podfile.txt";
            File.WriteAllText(output, builder.ToString());
        }
    }
}
