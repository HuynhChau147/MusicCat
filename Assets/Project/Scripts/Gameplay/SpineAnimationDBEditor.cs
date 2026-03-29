using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

#if UNITY_EDITOR
[CustomEditor(typeof(SpineAnimationDB))]
public class SpineAnimationDBEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Enum"))
        {
            GenerateEnum((SpineAnimationDB)target);
        }
    }

    void GenerateEnum(SpineAnimationDB db)
    {
        string enumName = db.AnimNamePrefix; // ví dụ: CatAnim
        string className = enumName + "Name"; // CatAnimName
        string path = $"./Assets/Project/Scripts/Anim{enumName}.cs";

        StringBuilder sb = new StringBuilder();

        // ===== CLASS STRING =====
        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");

        foreach (var anim in db.animations)
        {
            if (anim == null) continue;

            string cleanName = CleanName(anim.name);
            string originalName = anim.name;

            sb.AppendLine($"    public const string {cleanName} = \"{originalName}\";");
        }

        sb.AppendLine("}");

        File.WriteAllText(path, sb.ToString());
        AssetDatabase.Refresh();

        Debug.Log($"Generated {enumName} + {className}");
    }

    string CleanName(string name)
    {
        name = System.Text.RegularExpressions.Regex.Replace(name, "[^a-zA-Z0-9_]", "");

        if (char.IsDigit(name[0]))
            name = "_" + name;

        return name;
    }
}
#endif