using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Den.Tools;
using Den.Tools.Splines;
using Den.Tools.Matrices;
using Den.Tools.GUI;
using MapMagic.Core;
using MapMagic.Products;
using MapMagic.Nodes;

namespace Twobob.Mm2
{

    [System.Serializable]
    [GeneratorMenu(
       menu = "Spline",
       name = "Subdivide",
       iconName = "GeneratorIcons/Constant",
       colorType = typeof(SplineSys),
       disengageable = true,
       helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/map_generators/constant")]
    public class Subdivide : Generator, IInlet<SplineSys>, IOutlet<SplineSys>
    {

        [Val("Input", "Inlet")] public readonly Inlet<SplineSys> splineIn = new Inlet<SplineSys>();
        //  [Val("Spline", "Height")] public readonly Inlet<MatrixWorld> heightIn = new Inlet<MatrixWorld>();

        [Val("Output", "Outlet")] public readonly Outlet<SplineSys> output = new Outlet<SplineSys>();

        [Val("Divisions")] public int divisions = 2;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(Subdivide));
#endif

        public override void Generate(TileData data, StopToken stop)
        {
            SplineSys src = data.ReadInletProduct(this);
            SplineSys dst = new SplineSys(src);

            if (src == null) return;

            if (!enabled)
            {
                data.StoreProduct(this, dst);
                return;
            }

            /// if (data.isDraft) return;
            dst.Subdivide(divisions);
            data.StoreProduct(this, dst);

        }

    }

}

