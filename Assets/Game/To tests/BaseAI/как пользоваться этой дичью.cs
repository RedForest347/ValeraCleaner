/*
 на сцене должен быть Pathfinder (класс называется AstarPath, но имя компонента изменено для инспектора и в инспекторе оно будет именно Pathfinder),
 причем только один

 далее, объект АИ (болванка, которая должна ходить)
 на ней должны быть добавлены:
 AILerp и Seeker (Seeker добавляется автоматически при добавлении AILerp)
 
 кастомный скрипт, который делает следующую дичь:
 aILerp.destination = newPosition;
 , где 
     AILerp aILerp;
     aILerp = GetComponent<AILerp>(); (компонент, который присутствует на болванке, которая должна искать путь[далее, сыщик])
 
     foreach (var item in astarPath.ScanAsync()) ;
     , где astarPath - AstarPath astarPath (он же Pathfinder), который находится на сцене


 дал
 
 
 
 
 
 
 */