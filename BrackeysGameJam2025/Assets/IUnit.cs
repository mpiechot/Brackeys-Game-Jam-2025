﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameJam
{
    public interface IUnit
    {
        GameObject Unit { get; }

        void GetHit(int damage);
    }
}
