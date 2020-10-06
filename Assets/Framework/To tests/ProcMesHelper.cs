using System.Collections.Generic;
using UnityEditor;
using RangerV;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class ProcMesHelper
{

    public static string CreateSupportProcInfo(EntityBase entityBase)
    {
        if (entityBase == null)
            Debug.LogError("entityBase == null");
        if (entityBase.GetComponents<ComponentBase>().Where((ComponentBase cmp) => cmp == null).ToArray().Length != 0)
            Debug.LogError("cmp == null");

        List<GroupTypeName> groupTypeNames = ShowAllProc();
        List<string> EntityCmps = entityBase.GetComponents<ComponentBase>().Select((ComponentBase cmp) => cmp.GetType().Name).ToList();

        string mes = "сущность управляется процессингами:\n";

        for (int i = 0; i < groupTypeNames.Count; i++)
        {
            string temp_mes = "";

            for (int j = 0; j < groupTypeNames[i].CmpList.Count; j++)
            {
                if (!EntityCmps.Contains(groupTypeNames[i].CmpList[j]))
                {
                    goto end;
                }
                temp_mes += "\t" + "cmp - " + groupTypeNames[i].CmpList[j] + " \n";
            }

            temp_mes += "\n";

            for (int j = 0; j < groupTypeNames[i].ExcList.Count; j++)
            {
                if (EntityCmps.Contains(groupTypeNames[i].ExcList[j]))
                {
                    goto end;
                }

                temp_mes += "\t" + "exc - " + groupTypeNames[i].ExcList[j] + " \n";
            }


            mes += groupTypeNames[i].type_name + " \n";

        end:;
        }

        return mes;

    }

    static List<GroupTypeName> ShowAllProc()
    {
        Type[] types = Assembly.GetCallingAssembly().GetExportedTypes().Where((type) => type?.BaseType == typeof(ProcessingBase) && type != typeof(ProcMesHelper)).ToArray();

        List<GroupTypeName> groupTypeNames = new List<GroupTypeName>();

        for (int i = 0; i < types.Length; i++)
        {
            string[] guids;

            guids = AssetDatabase.FindAssets(types[i].Name);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path != "" && path != null)
                {
                    FileStream fileStream = File.OpenRead(path);

                    byte[] array = new byte[fileStream.Length];

                    fileStream.Read(array, 0, array.Length);

                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    groupTypeNames.AddRange(FindGroupTypes(textFromFile, types[i].Name));
                    fileStream.Close();
                }
            }
        }

        return groupTypeNames;
    }


    static List<GroupTypeName> FindGroupTypes(string textFromFile, string type_name)
    {
        List<GroupTypeName> groupTypeNames = new List<GroupTypeName>();
        List<int> groupStartPosIndexes = new List<int>();



        int start_index = textFromFile.IndexOf("Group.Create", 0);

        while (start_index != -1)
        {
            groupStartPosIndexes.Add(start_index);
            start_index = textFromFile.IndexOf("Group.Create", start_index + 1);
        }

        for (int i = 0; i < groupStartPosIndexes.Count; i++)
        {
            groupTypeNames.Add(FindGroupTypeNames(textFromFile, groupStartPosIndexes[i], type_name));
        }

        #region Debug

        //Debug.Log("скрипт " + type_name + " имеет групп: " + groupStartPosIndexes.Count);

        /**for (int i = 0; i < groupTypeNames.Count; i++)
        {
            Debug.Log("группа " + i);

            for (int j = 0; j < groupTypeNames[i].CmpList.Count; j++)
            {
                Debug.Log("cmp - " + groupTypeNames[i].CmpList[j]);
            }

            for (int j = 0; j < groupTypeNames[i].ExcList.Count; j++)
            {
                Debug.Log("exc - " + groupTypeNames[i].ExcList[j]);
            }
        }*/

        #endregion Debug

        return groupTypeNames;

    }

    static GroupTypeName FindGroupTypeNames(string textFromFile, int start_index, string type_name)
    {
        List<string> CmpList = new List<string>();
        List<string> ExcList = new List<string>();

        if (start_index != -1)
        {
            int end_index = textFromFile.IndexOf(";", start_index);


            #region Find Copmponents Names


            int start_cmp_index = textFromFile.IndexOf("<", start_index);
            int end_cmp_index = textFromFile.IndexOf(">", start_cmp_index);

            int temp_index = start_cmp_index + 1;

            while (temp_index < end_cmp_index)
            {
                string cmp = textFromFile.Substring(temp_index, Math.Min(textFromFile.IndexOf(",", temp_index), end_cmp_index) - temp_index);
                CmpList.Add(cmp);
                temp_index = textFromFile.IndexOf(",", temp_index) + ", ".Length;
            }

            #endregion Find Copmponents Names

            #region Find Exception Names

            int start_exc_index = textFromFile.IndexOf("<", end_cmp_index);

            if (start_exc_index != -1 && start_exc_index < end_index)
            {

                int end_exc_index = textFromFile.IndexOf(">", start_exc_index);

                temp_index = start_exc_index + 1;

                while (temp_index < end_exc_index)
                {
                    string exc = textFromFile.Substring(temp_index, Math.Min(textFromFile.IndexOf(",", temp_index), end_exc_index) - temp_index);
                    ExcList.Add(exc);

                    temp_index = textFromFile.IndexOf(",", temp_index) + ", ".Length;
                }

            }

            #endregion Find Exception Names
        }

        return new GroupTypeName(CmpList, ExcList, type_name);
    }

    private class GroupTypeName
    {
        public List<string> CmpList;
        public List<string> ExcList;
        public string type_name;

        public GroupTypeName(List<string> cmpList, List<string> excList, string type_name)
        {
            CmpList = cmpList;
            ExcList = excList;
            this.type_name = type_name;
        }
    }
}
