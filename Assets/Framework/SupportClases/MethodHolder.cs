using System.Reflection;
using UnityEngine;


namespace RangerV
{

    [System.Serializable]
    public class MethodHolder
    {
        public string type_name;
        public string method_name;
        public string assembly_name;
        public Component component;


        //MethodInfo cashedMethodInfo;

        public void StartMethod()
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;
            Assembly.Load(assembly_name).GetType(type_name).GetMethod(method_name, bindingFlags).Invoke(component, null);
        }

        /// <summary>
        /// проверка введенных данных на корректность путем попытки получения записанного ранее метода с помощбю рефлексии
        /// </summary>
        public void DataCheckOnCorrect()
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;
            Assembly.Load(assembly_name).GetType(type_name).GetMethod(method_name, bindingFlags);
        }
    }

}
