using System;
using System.Collections.Generic;

/// <summary>
/// A Bag is a simple data structure that gives a random item from inside the bag and remove (hide) the item upon giving it.
/// Elements can be reused without adding them again.
/// I ended up not using it because it would just add an unnecessary layer to getting questions. Would be more useful in a real life scenario.
/// </summary>
public class Bag<T>
{
    private List<T> m_bag;
    private List<T> m_usedItemsBag;
    private Random random;

    public Bag()
    {
        m_bag = new List<T>();
        m_usedItemsBag = new List<T>();
        random = new Random();
    }

    /// <summary>
    /// Adds a new Item to the bag.
    /// </summary>
    /// <param name="newItem"></param>
    public void Add(T newItem)
    {
        // A simple null check.
        if (newItem == null) return;
        m_bag.Add(newItem);
    }

    /// <summary>
    /// Gets an item from the bag. If the bag is empty, refreshes the item list for reuse.
    /// </summary>
    /// <returns>If the bag is completely empty a default value (null or 0). If not, returns a random item from the bag. </returns>
    public T GetItem()
    {
        T itemToGet = default;

        if (m_bag.Count > 0) // If bag not empty, get a random item and return.
        {
            int index = random.Next(m_bag.Count);
            itemToGet = m_bag[index];
            m_usedItemsBag.Add(itemToGet);
            m_bag.RemoveAt(index);
        } else if (m_usedItemsBag.Count > 0) // if bag is empty but the used items are not, refresh it and try again.
        {
            RefreshBag();
            GetItem();
        }

        return itemToGet;
    }


    public void RefreshBag()
    {
        if (m_bag.Count > 0 || m_usedItemsBag.Count < 1) return;

        while (m_usedItemsBag.Count > 0)
        {
            m_bag.Add(m_usedItemsBag[0]);
            m_usedItemsBag.RemoveAt(0);
        }
    }


    /// <summary>
    /// Remove any item from the bag entirely.
    /// </summary>
    /// <param name="itemToRemove"> Removes any item that references the same memory as itemToRemove</param>
    public void Remove(T itemToRemove)
    {
        if (itemToRemove is null) return;

        m_bag.RemoveAll((T item) => {
            return item.Equals(itemToRemove);
        });

        m_usedItemsBag.RemoveAll((T item) => {
            return item.Equals(itemToRemove);
        });
    }
}