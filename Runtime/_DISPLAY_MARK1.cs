using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using MapMagic.Nodes;

namespace Twobob.Mm2
{
  

    [System.Serializable]
    [GeneratorMenu(menu = "Map/Modifiers", name = "_Display", iconName = "GeneratorIcons/Contrast", disengageable = true,
            helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Contrast", menuName ="ACEDISPLAY", priority =2)]
    public class _DISPLAY_MARK1 : Generator, IInlet<MatrixWorld>, IOutlet<MatrixWorld>
    {

       

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(_DISPLAY_MARK1));
#endif

       // [Val("Hardness")] public float myvalue = 0.0f;

        public override void Generate(TileData data, StopToken stop)
        {
            if (stop != null && stop.stop) return;
            MatrixWorld src = data.ReadInletProduct(this);
            if (src == null) return;
            MatrixWorld clamp = new MatrixWorld(src);

            clamp.Clamp01();

            data.StoreProduct(this, clamp);
        }
    }
}