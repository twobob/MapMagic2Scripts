using Den.Tools.Matrices;
using MapMagic.Products;
using MapMagic.Nodes;

namespace Twobob.Mm2
{

    [System.Serializable]
    [GeneratorMenu(menu = "Map/Modifiers", name = "Invert", iconName = "GeneratorIcons/Contrast", disengageable = true,
            helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Contrast", menuName ="Invert", priority =1)]
    public class Invert_MARK1 : Generator, IInlet<MatrixWorld>, IOutlet<MatrixWorld>
    {

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(Invert_MARK1));
#endif 

        public override void Generate(TileData data, StopToken stop)
        {
            if (stop != null && stop.stop) return;
            MatrixWorld src = data.ReadInletProduct(this);
            if (src == null) return;
            MatrixWorld clamp = new MatrixWorld(src);
            clamp.Clamp01();
            clamp.Invert();
            data.StoreProduct(this, clamp);
        }
    }
}