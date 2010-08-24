﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashingDemo
{
    public sealed class NodeLocator<T> : INodeLocator<T> where T :INodeIndentity
    {
        private const int ServerAddressMutations = 100;

        // holds all server keys for mapping an item key to the server consistently
        private uint[] keys;
        // used to lookup a server based on its key
        private Dictionary<uint, T> servers = new Dictionary<uint, T>(new UIntEqualityComparer());
        private bool isInitialized;

        void INodeLocator<T>.Initialize(IList<T> nodes)
        {
            if (this.isInitialized)
                throw new InvalidOperationException("Instance is already initialized.");

            // locking on this is rude but easy
            lock (this)
            {
                if (this.isInitialized)
                    throw new InvalidOperationException("Instance is already initialized.");

                this.keys = new uint[nodes.Count * ServerAddressMutations];

                int nodeIdx = 0;

                foreach (T node in nodes)
                {
                    List<uint> tmpKeys = GenerateKeys(node, ServerAddressMutations);

                    tmpKeys.ForEach(delegate(uint k)
                    {
                        this.servers[k] = node;
                    });

                    tmpKeys.CopyTo(this.keys, nodeIdx);
                    nodeIdx += ServerAddressMutations;
                }

                Array.Sort<uint>(this.keys);

                this.isInitialized = true;
            }
        }

        T INodeLocator<T>.Locate(string key)
        {
            if (!this.isInitialized)
                throw new InvalidOperationException("You must call Initialize first");

            if (key == null)
                throw new ArgumentNullException("key");

            if (this.keys.Length == 0)
                return default(T);

            uint itemKeyHash = BitConverter.ToUInt32(new FNV1a().ComputeHash(Encoding.Unicode.GetBytes(key)), 0);

            // get the index of the server assigned to this hash
            int foundIndex = Array.BinarySearch<uint>(this.keys, itemKeyHash);

            // no exact match
            if (foundIndex < 0)
            {
                // this is the nearest server in the list
                foundIndex = ~foundIndex;

                if (foundIndex == 0)
                {
                    // it's smaller than everything, so use the last server (with the highest key)
                    foundIndex = this.keys.Length - 1;
                }
                else if (foundIndex >= this.keys.Length)
                {
                    // the key was larger than all server keys, so return the first server
                    foundIndex = 0;
                }
            }

            if (foundIndex < 0 || foundIndex > this.keys.Length)
                return default(T);

            return this.servers[this.keys[foundIndex]];
        }

        private static List<uint> GenerateKeys(T node, int numberOfKeys)
        {
            const int KeyLength = 4;
            const int PartCount = 1; // (ModifiedFNV.HashSize / 8) / KeyLength; // HashSize is in bits, uint is 4 byte long

            //if (partCount < 1)
            //    throw new ArgumentOutOfRangeException("The hash algorithm must provide at least 32 bits long hashes");

            List<uint> k = new List<uint>(PartCount * numberOfKeys);

            // every server is registered numberOfKeys times
            // using UInt32s generated from the different parts of the hash
            // i.e. hash is 64 bit:
            // 00 00 aa bb 00 00 cc dd
            // server will be stored with keys 0x0000aabb & 0x0000ccdd
            // (or a bit differently based on the little/big indianness of the host)
            string address = node.Indentity.ToString();

            for (int i = 0; i < numberOfKeys; i++)
            {
                byte[] data = new FNV1a().ComputeHash(Encoding.ASCII.GetBytes(String.Concat(address, "-", i)));

                for (int h = 0; h < PartCount; h++)
                {
                    k.Add(BitConverter.ToUInt32(data, h * KeyLength));
                }
            }

            return k;
        }
    }
}
