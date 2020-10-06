using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;

public class TestAllSystem : MonoBehaviour
{
    Group group1, group2, group3, group4, group5, group6, group8;

    List<GameObject> gameObjects;
    List<Group> groups;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            FullTest();
    }

    /*void FullTest()
    {
        Debug.LogWarning("TestAllSystem упразнен. будет выполнен только тест производительности");
        PerformanceTest();
    }*/

    void FullTest()
    {
        Debug.LogWarning("Начат тест фреймворка");
        Stopwatch time = Stopwatch.StartNew();
        bool successfull = true;

        group1 = Group.Create(new ComponentsList<CompTest1, CompTest2>());
        group2 = Group.Create(new ComponentsList<CompTest1, CompTest2>());
        group3 = Group.Create(new ComponentsList<CompTest1>(), new ComponentsList<CompTest1>());
        group4 = Group.Create(new ComponentsList<CompTest1>(), new ComponentsList<CompTest2>());
        group5 = Group.Create(new ComponentsList<CompTest1>());
        group6 = Group.Create(new ComponentsList<CompTest7>());
        group8 = Group.Create(new ComponentsList(), new ComponentsList<CompTest1>());

        gameObjects = new List<GameObject>(); // лист всех GameObject задействованных в тесте
        groups = new List<Group>() { group1, group2, group3, group4, group5, group6 }; // лист всех Group задействованных в тесте


        if (group1.entities_count + group2.entities_count + group3.entities_count + group4.entities_count + group5.entities_count != 0)
            SendErrorMessage("При начале теста в группах находятся сущности");

        #region correct add component test

        GameObject ent1 = new GameObject();
        gameObjects.Add(ent1);
        ent1.AddComponent<TestEntity1>();
        if (ent1.GetComponent<TestEntity1>().GetCmp<CompTest1>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent1.GetComponent<TestEntity1>().GetCmp<CompTest2>() != null)
            SendErrorMessage("Ошибочное добавление компонента");

        GameObject ent2 = new GameObject();
        gameObjects.Add(ent2);
        ent2.AddComponent<TestEntity2>();
        if (ent2.GetComponent<TestEntity2>().GetCmp<CompTest1>() == null)
            SendErrorMessage("Компонент не был добавлен");
        if (ent2.GetComponent<TestEntity2>().GetCmp<CompTest2>() == null)
            SendErrorMessage("Компонент не был добавлен");

        GameObject ent3 = new GameObject();
        gameObjects.Add(ent3);
        ent3.AddComponent<TestEntity3>();

        #endregion correct add component test

        #region correct num of entityes in group test

        if (group1.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе.");
        if (group2.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group3.entities_count != 0)
            SendErrorMessage("Игнорирование исключений");
        if (group4.entities_count != 1)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group5.entities_count != 3)
            SendErrorMessage("Неправильное количество сущностей в группе");

        if (ent1.GetComponent<TestEntity1>().GetAllComponents().Count != 1)
            SendErrorMessage("Добавление повторяющихся компонентов");

        if (ent2.GetComponent<TestEntity2>().GetAllComponents().Count != 4)
            SendErrorMessage("Неправильное добавление компонентов");

        ent2.GetComponent<EntityBase>().RemoveCmp<CompTest2>();
        if (group5.entities_count != 3)
            SendErrorMessage("Некорректное поведение групп при удалении компонента");
        ent2.GetComponent<EntityBase>().AddCmp<CompTest2>();


        ent1.GetComponent<EntityBase>().AddCmp<CompTest2>();
        if (group1.entities_count != 3)
            SendErrorMessage("Некорректное поведение групп при добавлении компонента");
        ent1.GetComponent<EntityBase>().RemoveCmp<CompTest2>();

        GameObject ent4 = new GameObject();
        gameObjects.Add(ent4);
        ent4.AddComponent<TestEntity1>();

        if (group1.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group2.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group3.entities_count != 0)
            SendErrorMessage("Игнорирование исключений");
        if (group4.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group5.entities_count != 4)
            SendErrorMessage("Неправильное количество сущностей в группе");

        ent1.GetComponent<TestEntity1>().RemoveCmp(typeof(CompTest1));
        if (ent1.GetComponent<TestEntity1>().GetCmp<CompTest1>() != null)
            SendErrorMessage("Некорректное удаление компонента");

        if (group1.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group2.entities_count != 2)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group3.entities_count != 0)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group4.entities_count != 1)
            SendErrorMessage("Неправильное количество сущностей в группе");
        if (group5.entities_count != 3)
            SendErrorMessage("Неправильное количество сущностей в группе");

        #endregion correct num of entityes in group test

        #region Enable/Disable Test

        if (group6.entities_count != 0)
            SendErrorMessage("Некорректное поведение группы");

        int start_count = group8.entities_count;

        GameObject ent7 = new GameObject();
        gameObjects.Add(ent7);
        ent7.AddComponent<TestEntity7>();

        if (group8.entities_count != start_count + 1)
            SendErrorMessage("Некорректное поведение группы при пустом листе компонентов ");
        group8.Delete(); // должна быть здесь

        if (group6.entities_count != 1)
            SendErrorMessage("Некорректное поведение группы");

        ent7.SetActive(false);

        if (group6.entities_count != 0)
            SendErrorMessage("Некорректное поведение при деактивации объекта");

        if (Storage.GetComponent<CompTest7>(ent7.GetComponent<EntityBase>().entity) != null)
            SendErrorMessage("Некорректное поведение при деактивации объекта");

        ent7.SetActive(true);

        if (Storage.GetComponent<CompTest7>(ent7.GetComponent<EntityBase>().entity) == null)
            SendErrorMessage("Некорректное поведение при повторной активации объекта");

        #endregion Enable/Disable Test

        #region CreateGroupInRuntime

        Group group7 = Group.Create(new ComponentsList<CompTest1, CompTest2, CompTest3, CompTest4>());
        groups.Add(group7);
        if (group7.entities_count == 0)
            SendErrorMessage("Ошибки при создании группы в рантайме");

        Group groupNullGroup = Group.Create(new ComponentsList());
        if (groupNullGroup.entities_count == EntityBase.entity_count)
            SendErrorMessage("Ошибки при создании группы с пустым листом компонентов");
        groupNullGroup.Delete();

        #endregion CreateGroupInRuntime

        #region DeleteGameObjects

        for (int i = 0; i < gameObjects.Count; i++)
            Destroy(gameObjects[i]);



        #endregion DeleteGameObjects

        #region Others

        int count = 0;
        for (int i = 0; i < groups.Count; i++)
            count += groups[i].entities_count;

        if (count != 0)
        {
            string mes = "";
            for (int i = 0; i < groups.Count; i++)
                mes += groups[i].entities_count;

            SendErrorMessage("при окончании теста в группах находятся сущности, хотя они были удалены " + mes);
        }

        #endregion Others

        #region DeleteGroups

        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].Delete();
        }

        #endregion DeleteGroups

        if (successfull)
        {
            Debug.Log("тест закончен успешно за " + time.ElapsedMilliseconds + " ms");
            PerformanceTest();
        }
        else
            Debug.LogError("тест закончен с ошибками за " + time.ElapsedMilliseconds + " ms");

        void SendErrorMessage(string message)
        {
            successfull = false;
            Debug.LogError(message);
        }
    }


    void PerformanceTest()
    {
        long test_time = 0;
        group1 = Group.Create(new ComponentsList<CompTest1, CompTest2, CompTest3, CompTest4, CompTest5, CompTest6>());
        group2 = Group.Create(new ComponentsList<CompTest1, CompTest2>(), new ComponentsList<CompTest3, CompTest4, CompTest5, CompTest6>());

        List<Group> groups = new List<Group> { group1, group2 };
        //List<Entity> entities = new List<Entity>();


        //for (int i = 0; i < 10; i++)
        //    entities.Add(EntityCreator.Entity1());

        Debug.LogWarning("начат тест производительности фреймворка");

        int different = 100;

        test_time += TestContains(different);
        test_time += TestIEnumerator(different);
        test_time += TestAddRemoveComponent(different);
        test_time += TestAddRemoveGroup(different);

        //for (int i = 0; i < entities.Count; i++)
        //    Destroy(entities[i]);

        Debug.Log("Тест, сложностью " + different + " закончен за " + test_time + " ms");

        long TestContains(int difficult)
        {
            Stopwatch time = Stopwatch.StartNew();

            for (int i = 0; i < difficult; i++)
            {
                foreach (Group group in groups)
                {
                    foreach (int ent in group)
                    {
                        group.Contains(ent);
                    }
                }
            }

            long total_time = time.ElapsedMilliseconds;

            Debug.Log("Contains test complete for " + total_time + " ms");

            return total_time;
        }

        long TestIEnumerator(int difficult)
        {
            Stopwatch time = Stopwatch.StartNew();

            for (int i = 0; i < difficult; i++)
            {
                foreach (Group group in groups)
                {
                    foreach (int ent in group)
                    {
                        ;// пустая строчка
                    }
                }
            }

            long total_time = time.ElapsedMilliseconds;

            Debug.Log("IEnumerator test complete for " + total_time + " ms");

            return total_time;
        }

        long TestAddRemoveComponent(int difficult)
        {
            GameObject testObj = new GameObject();

            Stopwatch time = Stopwatch.StartNew();
            EntityBase entityBase = testObj.AddComponent<Entity>();

            for (int i = 0; i < difficult; i++)
            {
                entityBase.RemoveCmp<CompTest1>();
                entityBase.AddCmp<CompTest1>();
            }

            long total_time = time.ElapsedMilliseconds;
            Debug.Log("AddRemoveComponent test complete for " + total_time + " ms");

            Destroy(testObj);

            return total_time;
        }

        long TestAddRemoveGroup(int difficult)
        {
            GameObject testObj = new GameObject();

            Stopwatch time = Stopwatch.StartNew();
            EntityBase entityBase = testObj.AddComponent<Entity>();

            for (int i = 0; i < difficult; i++)
            {
                entityBase.RemoveCmp<CompTest3>();
                entityBase.RemoveCmp<CompTest4>();
                entityBase.RemoveCmp<CompTest5>();
                entityBase.RemoveCmp<CompTest6>();

                entityBase.AddCmp<CompTest3>();
                entityBase.AddCmp<CompTest4>();
                entityBase.AddCmp<CompTest5>();
                entityBase.AddCmp<CompTest6>();
            }

            long total_time = time.ElapsedMilliseconds;
            Debug.Log("AddRemoveGroup test complete for " + total_time + " ms");

            Destroy(testObj);

            return total_time;
        }

    }
}

#region Test Components


[Serializable]
[HideComponent]
public class CompTest1 : ComponentBase
{
    [Pool]
    public float health1;
    [Pool]
    public int ammo1;
}

[Serializable]
[HideComponent]
public class CompTest2 : ComponentBase
{
    [Pool]
    public float health2;
    [Pool]
    public int ammo2;
}

[Serializable]
[HideComponent]
public class CompTest3 : ComponentBase
{
    [Pool]
    public float health3;
    [Pool]
    public int ammo3;
}

[Serializable]
[HideComponent]
public class CompTest4 : ComponentBase
{
    public float health4;
    public int ammo4;
}

[Serializable]
[HideComponent]
public class CompTest5 : ComponentBase
{
    public float health5;
    public int ammo5;
}

[Serializable]
[HideComponent]
public class CompTest6 : ComponentBase
{
    public float health6;
    public int ammo6;
}

[Serializable]
[HideComponent]
public class CompTest7 : ComponentBase
{
    public float health7;
    public int ammo7;
}

#endregion Test Components


#region TestEntities

public class TestEntity1 : Entity
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest1>();
        AddCmp<CompTest1>();
        AddCmp<CompTest1>();
    }
}

public class TestEntity2 : Entity
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest2>();
        AddCmp<CompTest3>();
        AddCmp<CompTest4>();
    }
}

public class TestEntity3 : Entity
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest2>();
        AddCmp<CompTest3>();
        AddCmp<CompTest4>();
    }
}

public class TestEntity4 : Entity
{
    public override void Setup()
    {
        AddCmp<CompTest1>();
        AddCmp<CompTest2>();
        AddCmp<CompTest3>();
        AddCmp<CompTest4>();
        AddCmp<CompTest5>();
        AddCmp<CompTest6>();
        AddCmp<CompTest7>();
    }
}

public class TestEntity7 : Entity
{
    public override void Setup()
    {
        AddCmp<CompTest7>();
    }
}


#endregion TestEntities


public class EntityCreator
{
    public static Entity Entity1()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest1>();
        return entity;
    }

    public static Entity Entity2()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest2>();
        entity.AddCmp<CompTest3>();
        entity.AddCmp<CompTest4>();
        return entity;
    }

    public static Entity Entity3()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest2>();
        entity.AddCmp<CompTest3>();
        entity.AddCmp<CompTest4>();
        return entity;
    }

    public static Entity Entity4()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest1>();
        entity.AddCmp<CompTest2>();
        entity.AddCmp<CompTest3>();
        entity.AddCmp<CompTest4>();
        entity.AddCmp<CompTest5>();
        entity.AddCmp<CompTest6>();
        entity.AddCmp<CompTest7>();
        return entity;
    }

    public static Entity Entity7()
    {
        GameObject obj = new GameObject();
        Entity entity = obj.AddComponent<Entity>();

        entity.AddCmp<CompTest7>();
        return entity;
    }

}
