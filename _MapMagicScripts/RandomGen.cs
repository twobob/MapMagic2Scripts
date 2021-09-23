using UnityEngine;
using System.Security.Cryptography;
using System;


    public static class RandomGen
    {
        private static RNGCryptoServiceProvider _global =
            new RNGCryptoServiceProvider();
        [ThreadStatic]
        private static System.Random _local;


        public static bool FlipACoin()
        {
            System.Random inst = _local;
            if (inst == null)
            {
                byte[] buffer = new byte[4];
                _global.GetBytes(buffer);
                _local = inst = new System.Random(
                    BitConverter.ToInt32(buffer, 0));
            }
            return inst.Next() % 2 == 0;
        }

    public static int NextValidRandomPatchAmountFromTGOSRange()
    {
        return
RandomGen.Next(TownGlobalObjectService.PatchCap, TownGlobalObjectService.PatchFloor);
    }

        public static int Next(int Ceil = int.MaxValue, int Floor = int.MinValue)
        {
            System.Random inst = _local;
            if (inst == null)
            {
                byte[] buffer = new byte[4];
                _global.GetBytes(buffer);
                _local = inst = new System.Random(
                    BitConverter.ToInt32(buffer, 0));
            }
            return Mathf.Max(Floor, inst.Next() % Mathf.Max(1, Ceil));
        }
    }



