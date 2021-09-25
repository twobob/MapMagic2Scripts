using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Den.Tools;
using Den.Tools.Splines;
using Den.Tools.Matrices;
using Den.Tools.GUI;
//using MapMagic.Gui;
using MapMagic.Nodes;
using MapMagic.Products;
using System.Linq;

namespace Twobob.Mm2
{
    
    [System.Serializable]
    [GeneratorMenu(
menu = "Spline/Modifiers",
name = "Bendy",
iconName = "GeneratorIcons/Constant",
colorType = typeof(SplineSys),
disengageable = true,
helpLink = "https://gitlab.com/denispahunov/mapmagic/wikis/map_generators/constant")]
    public class BendyV1 : Generator, IInlet<SplineSys>, IOutlet<SplineSys>
    {
        [Val("Input", "Inlet")] public readonly Inlet<SplineSys> input = new Inlet<SplineSys>();

        [Val("Output", "Outlet")] public readonly Outlet<SplineSys> output = new Outlet<SplineSys>();

       

        /// <summary>
        /// Make the random "repeatable"
        /// </summary>
        [Val("Repeatable?")] public bool useNoise = false;

        /// <summary>
        /// use this to get unique patterns without changing anything else.
        /// </summary>
        [Val("ImaginaryPart")] public float FractalStep = 1f;


        [Val("Divisions")] public int divisions = 4;

        //  [Val("Wiggly")] public float wiggliness = 1f;

        /// <summary>
        /// Not entirely sure...
        /// </summary>
        //   [Val("NodeType?")] public Node.TangentType nodeType = Node.TangentType.auto;

        //    [Val("Bendy?")] public bool doBendy = false;

        [Val("Bendiness")] public float bendiness = 1f;

        [Val("Relax?")] public bool doRelax = false;

        [Val("RelaxIterations")] public int ri = 4;

        [Val("RelaxBlur")] public float blur = 1f;


        private Vector2 Full_I_Value = Vector2.zero;


#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EnlistInMenu() => MapMagic.Nodes.GUI.CreateRightClick.generatorTypes.Add(typeof(BendyV1));
#endif 

        public override void Generate(TileData data, StopToken stop)
        {

            SplineSys src = data.ReadInletProduct(this);
       

            if (src == null) return;



            SplineSys bend = new SplineSys(src);

            if (!enabled)
            {
                data.StoreProduct(this, src);
                return;
            }

            // setup the clamp mask
            var tileLocation = data.area.Coord.ToVector3(1000);
            var tileSize = new Vector3(1000, 500, 1000);

            // now magically create perfect size slices for this tile.  Thanks Denis.
          


            //if (dst.NodesCount == 0)
            //{
            //    data.StoreProduct(this, src);
            //    return;

            //}

            // avoid non offsets for our imaginary pair.
            if (FractalStep == 0)
                FractalStep = 1;

            // setup the imaginary offset
            Full_I_Value = new Vector2(FractalStep, FractalStep);

            /// if (data.isDraft) return;
            bend.Subdivide(divisions);

            bend.Clamp(tileLocation, tileSize);

            //foreach (var item in dst.lines)
            //{
            //    for (int i = 1; i < item.NodesCount - 1; i++)
            //    {

            //        var cur = item.GetNodePos(i);
            //        var nv = Vector3.zero;
            //        var newFull_I_Value = new Vector3(Full_I_Value.x, 0, Full_I_Value.y);
            //        var loc = new Vector3(cur.x, 0, cur.y);

            //        if (useNoise)
            //        {
            //             nv = ReturnWigglyVector3UsingPerlinNoise(wiggliness, cur, true);
            //            // sanity check.

            //           // nv = new Vector3(cur.x + ReturnPerlinNoiseValueAtSpot(wiggliness, cur), cur.y, cur.z + ReturnPerlinNoiseValueAtSpot(wiggliness, cur + newFull_I_Value));

            //           // var holder = new Vector3(cur.x + ReturnWiggly(wiggliness), cur.y, cur.z + ReturnWiggly(wiggliness));
            //        }
            //        else
            //        {
            //            nv = new Vector3(cur.x + ReturnWiggly(wiggliness), cur.y, cur.z + ReturnWiggly(wiggliness));
            //          //  var holder = new Vector3(cur.x + ReturnPerlinNoiseValueAtSpot(wiggliness, cur), cur.y, cur.z + ReturnPerlinNoiseValueAtSpot(wiggliness, cur + newFull_I_Value));

            //        }
            //        item.SetNodePos(i, nv);
            //    }
            //}



            //if (doRelax)
            //{
            //    dst.Relax(blur, ri);
            //}

            //   SplineSys bend = new SplineSys(dst);


            // now magically create perfect size slices for this tile.  Thanks Denis.
            //  bend.Clamp(tileLocation, tileSize);

            // bend nodes



            for (int i = 0; i < bend.lines.Length; i++)
                {
                    for (int j = 0; j < bend.lines[i].segments.Length; j++)
                    {



                        // Skip the very first two.
                    if (j > 0)
                        {

                            Node start = bend.lines[i].segments[j].start;
                            start.type = Node.TangentType.auto;
                            Vector2 startpos = new Vector2(start.pos.x, start.pos.z);


                            if (useNoise)
                            {
                                Vector3 place = ReturnWigglyVector3UsingPerlinNoise(bendiness, startpos.V3(), false);

                            start.dir = place;
                                      //  (FlipALocationCoin(startpos)) ?
                                      ////start.dir + 
                                      //place :
                                      //// start.dir 
                                      //-place;

                            }
                            else
                            {
                                Vector3 place = ReturnWigglyVector3(bendiness);

                            start.dir = place;
                                   //  (FlipALocationCoin(startpos)) ?
                                   //// start.dir + 
                                   //place :
                                   //// start.dir
                                   //-place;
                            }



                            bend.lines[i].segments[j].start = start;
                        }
                        // Skip the lasties
                        if (j < bend.lines[i].segments.Length)
                        {

                            Node end = bend.lines[i].segments[j].end;
                            end.type = Node.TangentType.auto;
                            Vector2 endpos = new Vector2(end.pos.x, end.pos.z);

                            if (useNoise)
                            {

                                Vector3 place = ReturnWigglyVector3UsingPerlinNoise(bendiness, endpos.V3(), false);

                            end.dir = place; 
                                      //  (FlipALocationCoin(endpos)) ?
                                      //// end.dir + 
                                      //place :
                                      ////  end.dir
                                      //-place;
                            }
                            else
                            {

                                Vector3 place =
                                ReturnWigglyVector3(bendiness);
                            end.dir = place;
                                //(FlipALocationCoin(endpos)) ?
                                //// end.dir + 
                                //place :
                                ////  end.dir 
                                //-place;
                            }


                            bend.lines[i].segments[j].end = end;
                        }
                    }
                
                }
            bend.Update();
            //if (useNoise)
            //{
            //    for (int i = 0; i < dst.lines.Length; i++)
            //    {
            //        for (int j = 0; j < dst.lines[i].segments.Length; j++)
            //        {
            //            // Skip the very first.
            //            if (j > 0)
            //            {

            //                Node start = dst.lines[i].segments[j].start;
            //                Vector2 startpos = new Vector2(start.pos.x, start.pos.z);
            //                Vector3 mag = ReturnWigglyVector3UsingPerlinNoise(bendiness, startpos);

            //                start.dir =
            //                     (FlipALocationCoin(startpos)) ?
            //                   start.dir + mag :
            //                   start.dir - mag ;

            //                start.type = nodeType;
            //            }
            //            // Skip the last
            //            if (j < dst.lines[i].segments.Length - 1)
            //            {

            //                Node end = dst.lines[i].segments[j].end;
            //                Vector2 endpos = new Vector2(end.pos.x, end.pos.z);
            //                Vector3 mag = ReturnWigglyVector3UsingPerlinNoise(bendiness, endpos);

            //                end.dir =
            //                    (FlipALocationCoin(endpos)) ?
            //                   end.dir + mag :
            //                   end.dir - mag;

            //                end.type = nodeType;
            //            }
            //        }
            //    }
            //}

            //if (doBendy)
            //{
            //    if (bend.NodesCount == 0)
            //    {
            //        data.StoreProduct(this, src);
            //        return;

            //    }
            //    // now magically create perfect size slices for this tile.  Thanks Denis.
            //  //  bend.Clamp(tileLocation, tileSize);

            //    data.StoreProduct(this, bend);


            //}
            //else
            //{


            //if (bend.NodesCount == 0)
            //{
            //    data.StoreProduct(this, src);
            //    return;

            //}



            // now magically create perfect size slices for this tile.  Thanks Denis.
            bend.Clamp(tileLocation, tileSize);

            data.StoreProduct(this, bend);
            //  }
        }

        // persistence?
        private bool FlipALocationCoin(Vector2 location)
        {

            return ReturnPerlinNoiseValueAtSpot(1, location) < 0f;
        }

        //private bool FlipACoin()
        //{
        //    // to make it easy to replace with 
        //    //    return (UnityEngine.Random.value < 0.5f);

        //    return RandomGen.FlipACoin();

        //}


        private Vector3 ReturnWigglyVector3UsingPerlinNoise(float factor, Vector3 location, bool addOffset)
        {

            //  var loc = new Vector3(location.x, 0, location.y);
            var newFull_I_Value = new Vector3(Full_I_Value.x, 0, Full_I_Value.y);

            //   new Vector3(cur.x + ReturnPerlinNoiseValueAtSpot(wiggliness, loc), cur.y, cur.z + ReturnPerlinNoiseValueAtSpot(wiggliness, loc + newFull_I_Value));



            var ret =

             new Vector3(
                ReturnPerlinNoiseValueAtSpot(factor, location),
                0,
                 ReturnPerlinNoiseValueAtSpot(factor, location + newFull_I_Value)
                 );
            if (addOffset)
            {
                return ret + location;
            }
            return ret + new Vector3(0, 1, 0);

        }



        private float ReturnPerlinNoiseValueAtSpot(float factor, Vector2 location)
        {

            var ret = ((Mathf.PerlinNoise(location.x, location.y) - 0.5f) * 2) * factor;


            //// do a true / false coin flip.
            //if (FlipALocationCoin(location))

            //    return -ret;
            //else
            return ret;
        }


        private float ReturnWiggly(float factor)
        {
            // to  simplify changing the random here is an example.
            //   return (((UnityEngine.Random.value - 0.5f) * 2) * factor);
            return RandomGen.Next(10, -10) * 0.1f * factor;
        }

        private Vector3 ReturnWigglyVector3(float factor)
        {

            return new Vector3(RandomGen.Next(10, -10) * 0.1f * factor, 0, RandomGen.Next(10, -10) * factor);
        }


        private Vector3 ReturnWigglyVector3UsingFractalNoise(float factor, Vector2 location, Noise randomnoise)
        {

            return new Vector3(
                ReturnFractalNoiseValueAtSpot(factor, location, randomnoise),
                0,
                 ReturnFractalNoiseValueAtSpot(factor, location + Full_I_Value, randomnoise)
                 );
        }


        private float ReturnFractalNoiseValueAtSpot(float factor, Vector2 location, Noise randomnoise)
        {

            var FractalType = 2;
            var iterations = 1;
            var detail = 0.5f;
            var turbulence = 0f;

            var ret = (randomnoise.Fractal(location.x, location.y, 1, iterations, detail, turbulence, FractalType)
                * 2 - 1) * 1 * factor;

            // do a true / false coin flip.
            if (FlipALocationCoin(location))

                return -ret;
            else
                return ret;
        }
    }
}