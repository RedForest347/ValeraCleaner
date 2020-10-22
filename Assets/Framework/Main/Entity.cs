using System.Collections.Generic;
using UnityEngine;

namespace RangerV
{
    public class Entity : EntityBase
    {
        [HideInInspector]
        public List<bool> show_comp = new List<bool>();

        public override void Setup()
        {

        }
    }
}
