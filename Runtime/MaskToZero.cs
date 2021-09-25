using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Core;
using MapMagic.Products;
using MapMagic.Nodes;

namespace Twobob.Mm2
{
   
    /// <summary>
    /// Provides a silent NULL internal B output for anything not matching the mask. 
    /// </summary>
    [System.Serializable]
    [GeneratorMenu(menu = "Map/Modifiers", name = "MaskToZero", iconName = "GeneratorIcons/MapMask", disengageable = true,
        helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/MapMask")]
    public class MaskToZeroMARK1 : Generator, IMultiInlet, IOutlet<MatrixWorld>
    {


#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(MaskToZeroMARK1));
#endif


        [Val("Input A", "Inlet")]
        public readonly Inlet<MatrixWorld> aIn = new Inlet<MatrixWorld>();

        [Val("Mask", "Inlet")]
        public readonly Inlet<MatrixWorld> maskIn = new Inlet<MatrixWorld>();
        public IEnumerable<IInlet<object>> Inlets() { yield return aIn; yield return maskIn; }

        [Val("Invert")]
        public bool invert = false;

        [Val("Falloff")]
        public bool falloff = false;


        public override void Generate(TileData data, StopToken stop)
        {
            if (stop != null && stop.stop) return;
            MatrixWorld matrixA = data.ReadInletProduct(aIn);

            MatrixWorld matrixB = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
            matrixB.Fill(0);

            MatrixWorld mask = data.ReadInletProduct(maskIn);
            if (matrixA == null || matrixB == null) return;
            if (!enabled || mask == null) { data.StoreProduct(this, matrixA); return; }

            if (stop != null && stop.stop) return;
            MatrixWorld dst = new MatrixWorld(matrixA);

            if (stop != null && stop.stop) return;
            dst.Mix(matrixB, mask, 0, 1, invert, falloff, 1);

            if (stop != null && stop.stop) return;
            data.StoreProduct(this, dst);
        }
    }
}