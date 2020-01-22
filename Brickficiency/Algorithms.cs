﻿using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Brickficiency.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace Brickficiency
{
    public partial class MainWindow : Form
    {
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        // DO NOT MAKE ANY CHANGES BELOW THIS POINT!
        // The code below is used by the "standard" algorithms.  You may use it for your reference, but you should
        // not modify any of it.
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        public delegate void Algorithm(int k, List<Store> storeList, List<Item> itemList);

        public delegate void PreProcess(ref List<Store> storeList, ref List<Item> itemList);

        private void runTheAlgorithm(int minStores, int maxStores, bool continueWhenMatchesFound,
            //List<Store> storeList, List<Item> itemList, Algorithm alg, PreProcess preProc = null)
            List<Store> storeList, List<Item> itemList, Algorithm alg, PreProcess preProc)
        {
            // Pre-process the data
            if (preProc != null)
            {
                preProc(ref storeList, ref itemList);
            }

            // Now run the algorithms for the specified number of stores in the solution.
            for (int k = minStores; k <= maxStores; k++)
            {
                if ((!matchesfound || continueWhenMatchesFound) && !calcWorker.CancellationPending)
                {
                    beginCalculation(k, storeList.Count);
                    alg(k, storeList, itemList);
                    endCalculation(k);
                }
            }

        }

        private void runApproxAlgorithm(
            int minStores, 
            int maxStores, 
            int millisToRunEach, 
            List<Store> storeList,
            List<Item> itemList, 
            Algorithm alg,
            //PreProcess preProc = null)
            PreProcess preProc)
        {
            // Pre-process the data
            if (preProc != null)
            {
                preProc(ref storeList, ref itemList);
            }

            if (timeoutTimer != null)
            {
                timeoutTimer.Stop();
            }
            else
            {
                timeoutTimer = new System.Timers.Timer();
                timeoutTimer.Elapsed += timeoutTimer_Elapsed;
            }

            timeoutTimer.Interval = millisToRunEach;

            for (int k = minStores; k <= maxStores; k++)
            {
                if (!calcWorker.CancellationPending)
                {
                    beginCalculation(k, storeList.Count);
                    timeoutTimer.Start();
                    alg(k, storeList, itemList);
                    timeoutTimer.Stop();
                    endCalculation(k);
                }
            }

        }

        private void timeoutTimer_Elapsed(object sender, EventArgs e)
        {
            stopAlgorithmEarly = true;
            timeoutTimer.Stop();
        }

        #region Preprocessing stuff.

        public void StandardPreProcess(ref List<Store> storeList, ref List<Item> itemList)
        {
            // Sort the wanted list by availstores, ascending.  
            itemList = SortItemsByStoreAvailability(itemList);

            // Sort the stores in descending order by qty of the first 1-4 items on the sorted wanted list
            // depending on whether or not the number of items on the list is at least that large.
            storeList = SortStoresByNumberOfFirstSeveralItems(storeList, itemList);
        }
        
        public void CustomPreProcess(ref List<Store> storeList, ref List<Item> itemList)
        {
            // Sort the wanted list by availstores, ascending.  
            itemList = SortItemsByStoreAvailability(itemList);

            // Sort the stores in descending order by qty of the first 1-4 items on the sorted wanted list
            // depending on whether or not the number of items on the list is at least that large.
            storeList = SortStoresByItemsAvailable(storeList, itemList);
            // todo next: remove inferior shops    
            storeList = RemoveInferiorShops(storeList, itemList);
            AddStatus(Environment.NewLine + " Number of not inferior stores : " + StoreDictionary.Count + Environment.NewLine);
        }

        private List<Item> SortItemsByStoreAvailability(List<Item> itemList)
        {
            return itemList.OrderBy(i => i.availstores).ToList();     
        }

        // This sorts the stores so they are in decending order by the number of the first item on the wanted list.
        // If they have the same number, then they are ordered by the number of the second item, etc.
        private List<Store> SortStoresByNumberOfFirstSeveralItems(List<Store> storeList, List<Item> itemList)
        {
            StoreComparer sc = new StoreComparer(itemList);
            storeList.Sort(sc);
            return storeList;
        }

        // This sorts the stores so they are in decending order by the number of the first item on the wanted list.
        // If there is enough quantity, then they are ordered by the number of the second item, etc.
        private List<Store> SortStoresByItemsAvailable(List<Store> storeList, List<Item> itemList)
        {
            StoreComparerCustom sc = new StoreComparerCustom(itemList);
            storeList.Sort(sc);
            return storeList;
        }

        private List<Store> RemoveInferiorShops(List<Store> storeList, List<Item> itemList)
        {
            //toDo: 
            //1. add identifier for items available in store --> BitArray


            List<BitArray> storeBitArrayList = new List<BitArray>(storeList.Count);
            foreach (Store store in storeList)
            {
                BitArray itemBitArray = new BitArray(itemList.Count);
                foreach (Item item in itemList)
                {
                    if (store.getQty(item.extid) > item.qty)
                    {
                        // write into bitarray
                        itemBitArray.Set(itemList.IndexOf(item), true);
                    }
                }
                storeBitArrayList.Add(itemBitArray);


            }

            //2. find stores providing at least the same products

            int i = storeBitArrayList.Count - 1;
            int j = 0;
            while (i >= 0)
            {                
                while (j < i)
                {
                    if (storeBitArrayList[j] != storeBitArrayList[i] && storeBitArrayList[j].Or(storeBitArrayList[i]) == storeBitArrayList[j]) // there is another store providing everything this one provides
                    {
                        // now compare prices
                        Boolean isShopInferior = true;
                        foreach (Item item in itemList)
                                                    {
                            // if price is at least once lower
                            //if (item.price < storeList[j].getPrice(item.extid) && item.price > 0)
                            if (storeList[i].getPrice(item.extid) < storeList[j].getPrice(item.extid) && storeList[i].getPrice(item.extid) > 0)
                            {
                                isShopInferior = false;
                                break;
                            }

                        }
                        if (isShopInferior)
                        {
                            storeBitArrayList.RemoveAt(i);
                            StoreDictionary.Remove(storeList[i].getName()); //wie bekomme ich den shop zu fassen??
                            storeList.RemoveAt(i);
                            break;

                        }

                    }
                    j++;
                }
                i--;
            }
            return storeList;
        }

        #endregion

        #region New Algorithm

        private void KStoreCalc(int k, List<Store> storeList, List<Item> itemList)
        {
            if (k == 1)
            {
                OneStoreCalc(storeList, itemList);
            }
            else
            {
                // The stores are sorted by how many of the first item they have, and then by 2nd, 3rd, and 4th, etc.
                // Thus a valid solution must contain one of the first store in the list.  Once a store in the list has 0 of the first item
                // the rest will have 0 so it makes no sense to consider them as the first store since the rest of the store can't have that item
                // either, so no solution will ever be found.  This idea can be applied to the second store, etc., although it is more complicated.
                // So here we compute the last index of the first (up to) 5 items on the list. The last index of the 5th should be the final index.  
                // The last index of the rest might be smaller. I haven't figured out exactly how to use this information for items 2-5 yet since
                // it gets a little more complicated.
                // CAC, 2015-06-25
                int numToTrack = itemList.Count > 5 ? 5 : itemList.Count;
                int[] lastnonzeroindex = new int[numToTrack];
                for (int item = 0; item < numToTrack; item++)
                {
                    int j = storeList.Count - 1;
                    while (j >= 0 && storeList[j].getQty(itemList[item].extid) == 0)
                    {
                        j--;
                    }
                    lastnonzeroindex[item] = j;
                }
                //Debug.WriteLine("LastNonZero: " + intArrayToString(lastnonzeroindex));

                // Need to add one to the second argument since it is exclusive.
                Parallel.For(0, lastnonzeroindex[0] + 1, store1 =>
                {
                    if (calcWorker.CancellationPending || stopAlgorithmEarly)
                    {
                        return;
                    }

                    // Do the next k stores have enough of the first element?  
                    // If not, none of the rest will so quit. CAC, 2015-06-25
                    int totalQtyFirst = 0;
                    int lastToCheck = Math.Min(storeList.Count - 1, store1 + k);
                    for (int i = store1; i < lastToCheck; i++)
                    {
                        totalQtyFirst += storeList[i].getQty(itemList[0].extid);
                    }

                    if (totalQtyFirst < itemList[0].qty)
                    {
                        return;
                    }

                    int[] start = new int[k];
                    int[] end = new int[k];
                    for (int i = 0; i < k; i++)
                    {
                        start[i] = store1 + i;
                        end[i] = storeList.Count - k + i;
                        // The version below doesn't work.  We have to use a different method of omitting more
                        // possibilities based on lastnonzeroindex[i] for i>0.  Still need to think about this.
                        // I'm leaving it here commented out to remind me that I tried it and realized
                        // that it isn't correct.
                        // CAC, 2015-07-02.
                        //end[i] = (i < numToTrack) ? lastnonzeroindex[i] : storeList.Count - k + i;
                    }
                    end[0] = store1;
                    KSubsetGenerator subs = new KSubsetGenerator(storeList.Count, start, end);
                    while (subs.hasNext())
                    {
                        if (calcWorker.CancellationPending || stopAlgorithmEarly)
                        {
                            break;
                        }

                        int[] current = subs.next();
                        Interlocked.Increment(ref longcount);
                        bool fail = false;
                        foreach (Item item in itemList)
                        {
                            int totalQty = 0;
                            for (int i = 0; i < k; i++)
                            {
                                totalQty += storeList[current[i]].getQty(item.extid);
                            }
                            if (totalQty < item.qty)
                            {
                                fail = true;
                                break;
                            }
                        }
                        if (!fail)
                        {
                            List<string> storeNames = new List<string>();
                            for (int j = 0; j < k; j++)
                            {
                                storeNames.Add(storeList[current[j]].getName());
                            }
                            addFinalMatch(storeNames);
                        }
                    }
                    Progress();
                });
            }
        }

        #endregion

        private void OneStoreCalc(List<Store> storeList, List<Item> itemList)
        {
            for (int store1 = 0; store1 < storeList.Count; store1++)
            {
                foreach (Item item in itemList)
                {
                    if (storeList[store1].getQty(item.extid) < item.qty)
                    {
                        goto nextStore;
                    }
                }
                List<string> storeNames = new List<string>();
                storeNames.Add(storeList[store1].getName());
                addFinalMatch(storeNames);
                nextStore:
                Progress();
            }
        }
    }
}