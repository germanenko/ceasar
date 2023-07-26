using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Germanenko.Framework
{

    public interface ITick
    {

        void Tick(float delta);

    }


    public interface ITickFixed
    {

        void TickFixed(float delta);

    }


    public interface ITickLate
    {

        void TickLate(float delta);

    }

}
