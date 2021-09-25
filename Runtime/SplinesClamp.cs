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
      menu = "Spline/Standard",
      name = "Clamp",
      iconName = "GeneratorIcons/Constant",
      colorType = typeof(SplineSys),
      disengageable = true,
      helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/map_generators/constant")]
    public class SplineClamp : Generator, IInlet<SplineSys>, IOutlet<SplineSys>
    {

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(SplineClamp));
#endif

        [Val("Input", "Inlet")] public readonly Inlet<SplineSys> input = new Inlet<SplineSys>();
        public enum Clamp { Off, Full, Active }
        [Val("Clamp")] public Clamp clamp;

        public IEnumerable<IInlet<object>> Inlets() { yield return input; }

        public override void Generate(TileData data, StopToken stop)
        {
            SplineSys spline = data.ReadInletProduct(this);
            if (spline == null || !enabled) return;

            if (stop != null && stop.stop) return;
            if (clamp == Clamp.Full) spline.Clamp((Vector3)data.area.full.worldPos, (Vector3)data.area.full.worldSize);
            if (clamp == Clamp.Active) spline.Clamp((Vector3)data.area.active.worldPos, (Vector3)data.area.active.worldSize);

            if (stop != null && stop.stop) return;
            data.StoreProduct(this, spline);
        }
    }


  
}