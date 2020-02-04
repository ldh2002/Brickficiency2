using Brickficiency.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ref List<Store> storeList, List<Item> itemList, Algorithm alg, PreProcess preProc)
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
            ref List<Store> storeList,
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

            // Create two or three BitArrays for each store:
            // 1. Store has enough quantity of the different items 
            // 2. Store has some but not enough quantity of the different items
            // 3. Store has some of the different items
            // Don't know yet wether to use 2. or 3. or both.
            // Question: where do I put the arrays? I'll try to add it to the stores directly.
            sortStoreItems(itemList);
            storeList = sortStoreListItems(storeList, itemList);
            createBitArraysforStoreList(storeList, itemList);

            // Sort the stores in descending order by qty of the first 1-4 items on the sorted wanted list
            // depending on whether or not the number of items on the list is at least that large.
            storeList = SortStoresByItemsAvailable(storeList, itemList);
            storeList = RemoveInferiorShops(storeList, itemList);
            AddStatus(Environment.NewLine + " Number of not inferior stores : " + StoreDictionary.Count + Environment.NewLine);
        }

        private List<Item> SortItemsByStoreAvailability(List<Item> itemList)
        {
            return itemList.OrderBy(i => i.availstores).ToList();
        }

        private List<Store> sortStoreListItems(List<Store> storeList, List<Item> itemList)
        {
            List<Store> storeListNew = new List<Store>();
            foreach (Store store in storeList)
            {
                storeListNew.Add(new Store(store.getName(), StoreDictionary[store.getName()]));
            }
            return storeListNew;
        }

        private void sortStoreItems(List<Item> itemList)
        {
            Dictionary<string, Dictionary<string, StoreItem>> StoreDictionaryTemp = new Dictionary<string, Dictionary<string, StoreItem>>();
            Dictionary<string, StoreItem> storeItems;
            foreach (KeyValuePair<string, Dictionary<string, StoreItem>> kvp in StoreDictionary)
            {
                StoreItem storeItem = new StoreItem();
                storeItems = new Dictionary<string, StoreItem>();
                foreach (Item item in itemList)
                {
                    storeItems.Add(item.extid, StoreDictionary[kvp.Key][item.extid]);
                }
                StoreDictionaryTemp.Add(kvp.Key, storeItems);
            }
            foreach (KeyValuePair<string, Dictionary<string, StoreItem>> kvp in StoreDictionaryTemp)
            {
                StoreDictionary[kvp.Key] = StoreDictionaryTemp[kvp.Key];
            }

        }

        private void createBitArraysforStoreList(List<Store> storeList, List<Item> itemList)
        {
            BitArray[] bitArrays = new BitArray[3];
            foreach (Store store in storeList)
            {
                store.createBitArrays(itemList);
                for (int i = 0; i < 3; i++)
                {
                    bitArrays[i] = store.getBitArray(i);
                }
                BitArrayDictionary.Add(store.getName(), bitArrays);
            }
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
        private List<Store> SortStoresByItemsAvailableOld(List<Store> storeList, List<Item> itemList)
        {
            StoreComparerCustomOld sc = new StoreComparerCustomOld(itemList);
            storeList.Sort(sc);
            return storeList;
        }
        private List<Store> SortStoresByItemsAvailable(List<Store> storeList, List<Item> itemList)
        {
            StoreComparerCustom sc = new StoreComparerCustom(itemList);
            storeList.Sort(sc);
            return storeList;
        }

        private List<Store> RemoveInferiorShops(List<Store> storeList, List<Item> itemList)
        {
            // ToDo: switch to new BitArrays in Store class
            List<BitArray> storeBitArrayList = new List<BitArray>(storeList.Count);
            foreach (Store store in storeList)
            {
                BitArray itemBitArray = new BitArray(itemList.Count);
                foreach (Item item in itemList)
                {
                    if (store.getQty(item.extid) > item.qty)
                    {
                        // write into bitArray
                        itemBitArray.Set(itemList.IndexOf(item), true);
                    }
                }
                storeBitArrayList.Add(itemBitArray);


            }

            // find stores providing at least the same products
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

        #region New Algorithms

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

        // I will try to do the matching by comparing BitArrays
        private void KStoreCalcBitOperations(int k, List<Store> storeList, List<Item> itemList)
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
                    BitArray bitArrayEnough = new BitArray(itemList.Count);
                    BitArray bitArrayFew = new BitArray(itemList.Count);
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
                        bitArrayEnough.SetAll(false);
                        bitArrayFew.SetAll(false);
                        int[] current = subs.next();
                        Interlocked.Increment(ref longcount);
                        bool fail = false;
                        // I cannot imagine this is efficient. Here I'll switch to BitArrays.
                        // For this I need do prepare the BitArray data first. - done
                        // Btw., I need the BitArray data for Preprocessing, also.  -done
                        // ldh 2020.01.23 
                        /*
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
                        };
                        */

                        //start Bitarray logic
                        
                        for (int i = 0; i < k; i++)
                        {
                            bitArrayEnough.Or(storeList[current[i]].getBitArray(Store.bitArrayEnough));
                            bitArrayFew.Or(storeList[current[i]].getBitArray(Store.bitArrayFew));
                        };                                               
                        
                        for (int itemIndex = 0; itemIndex < itemList.Count; itemIndex++)
                        {
                            if (!bitArrayEnough.Get(itemIndex))
                             {
                                if (bitArrayFew.Get(itemIndex))
                                {
                                    int totalQty = 0;
                                    for (int i = 0; i < k; i++)
                                    {
                                        totalQty += storeList[current[i]].getQty(itemList[itemIndex].extid);
                                    }
                                    if (totalQty < itemList[itemIndex].qty)
                                    {
                                        fail = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    fail = true;
                                    break;
                                }
                            }                            
                        }
                        //end Bitarray logic
                        
                        if (!fail)
                        {
                            List<string> storeNames = new List<string>();
                            for (int i = 0; i < k; i++)
                            {
                                storeNames.Add(storeList[current[i]].getName());
                            }
                            addFinalMatch(storeNames, ref itemList);
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