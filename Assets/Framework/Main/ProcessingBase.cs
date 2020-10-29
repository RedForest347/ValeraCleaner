


namespace RangerV
{
    /// <summary>
    /// для инициалицацию необходимой даты процессингов можно применять интерфейсы: ICustomAwake и ICustomStart.
    /// Их различия:
    ///     сначала выполняется ICustomAwake для всех процессингов
    ///     затем, производится инициализация всех сущностей уже находящихся на сцене (при старте сцены)
    ///     затем, выполняется ICustomStart для всех процессингов
    /// 
    ///     при ребилде повторно выполняется ICustomStart. ICustomAwake при ребилде не выполняется
    /// 
    /// добавление компонента в ManagerUpdate производится в стартере уровня (например, в Level1Starter), куда нужно вручную вписывать процессинг
    /// 
    /// действия процессина проходят в CustomUpdate (или иных Custom апдейтах)
    /// 
    /// при остановке (удалении) процессинга, при наличии интерфейса ICustomDisable выполняется функция OnDisable.
    /// 
    /// следует именовать [имя_процессинга]Proc
    /// 
    /// 
    /// при добавлении метода в event компонента следует пользоваться данной логикой:
    ///     public void OnAwake()
    ///     {
    ///         foreach (int button in ButtonGroup)
    ///             OnAddEnt(button);
    ///
    ///         ButtonGroup.OnAddEntity += OnAddEnt;
    ///         ButtonGroup.OnAfterRemoveEntity += OnRemoveEnt;
    ///     }
    /// , т.е. сначала проверять все сущности, которые уже находятся в группах, затем добавлять в OnAddEntity, OnAfterRemoveEntity, 
    /// либо пользоваться перегрузкой GroupCreate()
    /// 
    /// 
    /// </summary>
    public class ProcessingBase
    {
        
    }
}



#region ProcBlank

#region Proc Simple
/*
using UnityEngine;
using RangerV;

public class DDD : ProcessingBase, ICustomStart, ICustomUpdate
{
    Group Group = Group.Create(new ComponentsList<DDD>());

    public void OnStart()
    {

    }

    public void CustomUpdate()
    {

    }
}
*/
#endregion Proc Simple




#region Proc Hard
/*

using UnityEngine;
using RangerV;

public class DDD : ProcessingBase, ICustomAwake, ICustomStart, ICustomUpdate, ICustomDisable
{
    Group Group = Group.Create(new ComponentsList<DDD>());

    public void OnAwake()
    {

    }

    public void OnStart()
    {
        group.InitEvents(OnAdd, OnRemove);
    }

    void OnAdd(int ent)
    {
        Storage.GetComponent<DDD>(ent).OnSelect += OnSelect;
    }

    void OnRemove(int ent)
    {
        Storage.GetComponent<DDD>(ent).OnSelect -= OnSelect;
    }

    void OnSelect(int ent)
    {

    }

    public void CustomUpdate()
    {

    }


    public void OnCustomDisable()
    {
        group.DeinitEvents(OnAdd, OnRemove);
    }
}


*/
#endregion Proc Hard

#endregion ProcBlank
