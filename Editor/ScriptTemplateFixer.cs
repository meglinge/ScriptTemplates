using System.Net.Mime;
using System.Linq;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptTemplateFixer : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string metaFilePath)
    {
        // 获取文件路径
        var filePath = metaFilePath.Replace(".meta", "");

        // 获取文件后缀
        switch (Path.GetExtension(filePath))
        {
            case ".cs":
                AddNameSpace(filePath);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 添加命名空间
    /// </summary>
    /// <param name="filePath"></param>
    public static void AddNameSpace(string filePath)
    {

        // 获取目录路径 基于Assets/...
        var path = Path.GetDirectoryName(filePath);
        Debug.Log(path);
        // 获取所有的目录
        var names = path.Split(Path.DirectorySeparatorChar).ToList();
        // 倒序遍历删除不需要的文件目录
        for (int index = names.Count - 1; index >= 0; index--)
        {
            var name = names[index];
            if (name == "Assets" || name == "Scripts")
            {
                names.RemoveAt(index);
                continue;
            }
            // 去掉文件可能的空格
            names[index] = name.Replace(" ", "");
        }
        if (names.Count == 0)
        {
            // 设置默认名
            names.Add(PlayerSettings.productName);
        }
        // 得到命名空间之后替换
        var nameSpace = string.Join('.', names);
        var fileContents = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, filePath));

        File.WriteAllText(filePath, fileContents.Replace("#NAMESPACE#", nameSpace));
        AssetDatabase.Refresh();
    }

}