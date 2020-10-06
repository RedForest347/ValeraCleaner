
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
