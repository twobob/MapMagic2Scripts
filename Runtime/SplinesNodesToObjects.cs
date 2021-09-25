using Den.Tools;
using Den.Tools.Splines;
using MapMagic.Products;
using System.Linq;
using MapMagic.Nodes;

namespace Twobob.Mm2
{
    [System.Serializable]
    [GeneratorMenu(menu = "Spline/Modifiers", name = "NodeToObject", iconName = "GeneratorIcons/Constant", disengageable = true,
        colorType = typeof(SplineSys),
        helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/map_generators/constant")]
    public class SplinesNodesToObjects : Generator, IInlet<SplineSys>, IOutlet<TransitionsList>
    {

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(SplinesNodesToObjects));
#endif

        public override void Generate(TileData data, StopToken stop)
        {
            SplineSys splineSys = data.ReadInletProduct(this);
            if (splineSys == null || !enabled) return;

             if (stop != null && stop.stop) return;
            SplineSys bend = new SplineSys(splineSys);

            var objs = new TransitionsList();

            foreach (var item in bend.GetAllPoints(resPerUnit: 1).SelectMany(list => list))
            { objs.Add(new Transition { pos = item });            }

            if (stop != null && stop.stop) return;

            data.StoreProduct(this, objs);
        }
    }
}
