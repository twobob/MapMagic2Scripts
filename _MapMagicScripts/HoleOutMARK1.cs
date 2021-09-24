using UnityEngine;
using System;
using System.Collections;
//using System.Collections.Generic;

//using Den.Tools;    
using Den.Tools.GUI;
using Den.Tools.Matrices;
//using MapMagic.Core;
using MapMagic.Products;
//using MapMagic.Terrains;
using System.Linq;

using UnityEngine.Profiling;
using Den.Tools;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(
		menu = "Map/Output", 
		name = "Hole", 
		section=2, 
		colorType = typeof(MatrixWorld), 
		iconName="GeneratorIcons/HeightOut",
		helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/output_generators/Height")]
	public class HoleOutMark1 : OutputGenerator, IInlet<MatrixWorld>
	{
		public OutputLevel outputLevel = OutputLevel.Draft | OutputLevel.Main;
		public override OutputLevel OutputLevel { get{ return outputLevel; } }

		public bool guiApplyType = false;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(HoleOutMark1));
#endif


        public override void Generate (TileData data, StopToken stop)
		{
			//loading source
			if (stop!=null && stop.stop) return;
			MatrixWorld src = data.ReadInletProduct(this);
			if (src == null) return; 
//			if (!enabled) { data.finalize.Remove(finalizeAction, this); return; }

			//adding to finalize
			if (stop!=null && stop.stop) return;
			if (enabled)
			{
				data.StoreOutput(this, typeof(HoleOutMark1), this, src);  //adding src since it's not changing
				data.MarkFinalize(Finalize, stop);
			}
			else 
				data.RemoveFinalize(finalizeAction);

			#if MM_DEBUG
			Log.Add("Height generated (id:" + id + " draft:" + data.isDraft + ")");
			#endif
		}


        static int SplitSizeRef = 1;

		public static FinalizeAction finalizeAction = Finalize; //class identified for FinalizeData
		public static void Finalize (TileData data, StopToken stop)
		{
       

            //determining resolutions
            if (stop!=null && stop.stop) return;
            //	Interpolation interpolation = (HoleOutMark1.Interpolation)data.globals.heightInterpolation;
            int upscale = 1; //GetUpscale(interpolation);
			int margins =  data.area.Margins;
			int matrixRes = (data.heights.rect.size.x - margins*2 - 1)*upscale + margins*2*upscale + 1;

			//creating upscaled/blurred height matrix
			if (stop!=null && stop.stop) return;
            Matrix matrix = data.heights;

			//clamping heights to 0-1 (otherwise culing issues can occur)
			matrix.Clamp01();

			//2Darray resolution (this should still match our 69x69 size input as well)
			int arrRes = matrix.rect.size.x - margins*upscale*2;

			//splits number (used for SetHeightsDelayLOD and Texture)
			int splitSize = SplitSizeRef = data.globals.heightSplit;
			int numSplits = arrRes / splitSize;
			if (arrRes % splitSize != 0) numSplits++;

            IApplyData applyData;

            

		//	if (applyType == ApplyType.SetHeights)
		//	{

                float[,] heights2Dfull = new float[arrRes, arrRes];
                bool[,] bool2Dfull = new bool[arrRes -1, arrRes-1];


                foreach ((HoleOutMark1 output, MatrixWorld product, MatrixWorld biomeMask)
                  in data.Outputs<HoleOutMark1, MatrixWorld, MatrixWorld>(typeof(HoleOutMark1), inSubs: true))
                {

                    Matrix othermatrix = product;

                    // This is our holes then
                    // product.


                    Coord heightsOffset = othermatrix .rect.offset + margins * upscale;
                    Matrix matrixbool = othermatrix;

                    matrix.ExportHeights(heights2Dfull, matrix.rect.offset + margins * upscale);

                    Coord heightsSize = new Coord(heights2Dfull.GetLength(1), heights2Dfull.GetLength(0));  //x and z swapped
                        CoordRect heightsRect = new CoordRect(heightsOffset, heightsSize);

                        CoordRect intersection = CoordRect.Intersected(matrixbool.rect, heightsRect);
                        Coord min = intersection.Min; Coord max = intersection.Max;

                        for (int x = min.x; x < max.x-1; x++)
                            for (int z = min.z; z < max.z-1; z++)
                            {
                                int matrixPos = (z - matrixbool.rect.offset.z) * matrixbool.rect.size.x + x - matrixbool.rect.offset.x;
                                int heightsPosX = x - heightsRect.offset.x;
                                int heightsPosZ = z - heightsRect.offset.z;

                                float val = matrixbool.arr[matrixPos];
                                bool2Dfull[heightsPosZ, heightsPosX] = val < float.Epsilon;
                            }
                   

                  

                }


                applyData = new ApplySetData() {heights2D=heights2Dfull, height=data.globals.height, bools2D= bool2Dfull };
		

			//pushing to apply
			if (stop!=null && stop.stop) return;
			Graph.OnOutputFinalized?.Invoke(typeof(HoleOutMark1), data, applyData, stop);
			data.MarkApply(applyData);

			#if MM_DEBUG
			Log.Add("HeightOut Finalized");
			#endif
		}


		public override void ClearApplied (TileData data, Terrain terrain)
		{
		//	TerrainData terrainData = terrain.terrainData;
		//	Vector3 terrainSize = terrainData.size;

	
		}

		public interface IApplyHeightData : IApplyData { } //common type for all height applies
		
		public class ApplySetData : IApplyData, IApplyHeightData
		{
			public float[,] heights2D;
            public bool[,] bools2D;
            public float height;
			public Coord offset;  //a partial rect to avoid reading-writing all of the terrain. Size is the size of the array. 0 is data 0. Max should not be more than data size.

			public void Read (Terrain terrain) 
			{ 
				int heightRes = terrain.terrainData.heightmapResolution;
				Read(terrain, new CoordRect(0,0,heightRes,heightRes));
			}

			public void Read (Terrain terrain, CoordRect rect) 
			{ 
				heights2D = terrain.terrainData.GetHeights(rect.offset.x, rect.offset.z, rect.size.x, rect.size.z);
				offset = rect.offset;
			}

			public void Apply (Terrain terrain)
			{
				if (terrain==null || terrain.Equals(null) || terrain.terrainData==null) return; //chunk removed during apply
				TerrainData data = terrain.terrainData;


                data.SetHoles(offset.x, offset.z, bools2D);

              //  terrain.Flush();

				#if MM_DEBUG
				Log.Add("HeightOut Applied Set");
				#endif
			}

			public static ApplySetData Empty 
				{get{ return new ApplySetData() { heights2D = new float[33,33] }; }}

			public int Resolution {get{ return heights2D.GetLength(0); }}
		}

		
	}
}