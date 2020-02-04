using System;
using System.Collections;
using System.Collections.Generic;

namespace Brickficiency.Classes
{
    public class Store
    {
        public Store()
        {
            name = "";
            items = new Dictionary<string, StoreItem>();

        }
        public Store(string storeName, Dictionary<string, StoreItem> itemList)
        {
            name = storeName;
            items = itemList;
        }
        private string name;
        private Dictionary<string, StoreItem> items;
        private BitArray[] bitArrays;
        public const int bitArrayEnough = 0;
        public const int bitArraySome = 1;
        public const int bitArrayFew = 2;

        public StoreItem getItem(string id)
        {
            return items[id];
        }
        public decimal getPrice(string id)
        {
            return items[id].price;
        }

        public int getQty(string id)
        {
            return items[id].qty;
        }

        public string getColour(string id)
        {
            return items[id].colour;
        }

        public string getName()
        {
            return name;
        }

        public void createBitArrays(List<Item> itemList)
        {
            bitArrays = new BitArray[3];
            bitArrays[bitArrayEnough] = new BitArray(itemList.Count);
            bitArrays[bitArraySome] = new BitArray(itemList.Count);
            bitArrays[bitArrayFew] = new BitArray(itemList.Count);
                       
            foreach (Item item in itemList)
            {
                if (getQty(item.extid) > 0)
                {
                    bitArrays[bitArraySome].Set(itemList.IndexOf(item), true);
                    if (item.qty > getQty(item.extid))
                    {
                        bitArrays[bitArrayFew].Set(itemList.IndexOf(item), true);
                    }
                    else
                    {
                        bitArrays[bitArrayEnough].Set(itemList.IndexOf(item), true);
                    }
                }
                // else quantity=0, so bit remains false in all bitArrays
            }

        }

        public BitArray getBitArray(int BitArrayType)
        {
            return bitArrays[BitArrayType];
        }
    }

    public class StoreComparer : IComparer<Store>
    {
        private List<Item> items;
        public StoreComparer(List<Item> items)
        {
            this.items = items;
        }
        public int Compare(Store x, Store y)
        {
            int i = 0;
            while (i < items.Count && x.getQty(items[i].extid) == y.getQty(items[i].extid))
            {
                i++;
            }
            if (i == items.Count)
            {
                return 0;
            }
            else
            {
                // This is backwards because I want to sort descending.
                return y.getQty(items[i].extid) - x.getQty(items[i].extid);
            }
        }
    }

    public class StoreComparerCustomOld : IComparer<Store>
    {
        private List<Item> items;
        public StoreComparerCustomOld(List<Item> items)
        {
            this.items = items;
        }
        public int Compare(Store x, Store y)
        {
            int i = 0;
            // toDo: check for required quantity also

            double desiredquantityinshop = Math.Max(items[i].qty + 2, items[i].qty * 1.1 - 1);
            while (i < items.Count && x.getQty(items[i].extid) > desiredquantityinshop && y.getQty(items[i].extid) > desiredquantityinshop)
            {
                i++;
            }
            if (i == items.Count)
            {
                return 0;
            }
            else
            {
                // This is backwards because I want to sort descending.
                return y.getQty(items[i].extid) - x.getQty(items[i].extid);
            }
        }
    }

    public class StoreComparerCustom : IComparer<Store>
    {
        private List<Item> items;
        public StoreComparerCustom(List<Item> items)
        {
            this.items = items;
        }
        public int Compare(Store x, Store y)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (y.getBitArray(Store.bitArrayEnough).Get(i))
                {
                    if (x.getBitArray(Store.bitArrayEnough).Get(i))
                    {
                        i++;
                    }
                    else return 1;
                }
                else if (x.getBitArray(Store.bitArrayEnough).Get(i))
                {
                    return -1;
                }
                else if (y.getBitArray(Store.bitArrayFew).Get(i))
                {
                    if (x.getBitArray(Store.bitArrayFew).Get(i))
                    {
                        i++;
                    }
                    else return 1;
                }
                else if (x.getBitArray(Store.bitArrayFew).Get(i))
                {
                    return -1;
                }
                else i++;
            }
            return 0;
        }
    }
}
