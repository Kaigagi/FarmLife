using UnityEngine.Events;

namespace ItemSystem
{
    public class QuickItemBar
    {
        public static readonly int MaxQuickItemAccessAmount = 8;

        public UnityAction<int> OnChangeSelectItem = delegate {  };

        private readonly Item[] _quickItemAccessBar = new Item[MaxQuickItemAccessAmount];
        private int _selectedItem = 0;

        public int SelectedItem => _selectedItem;

        private bool CheckPosition(int position)
        {
            if (position < 0 || position >= MaxQuickItemAccessAmount)
            {
                return false;
            }
            return true;
        }

        /**
         * Access quick access bar using array index
         */
        public Item SelectItem(int position)
        {
            if (!CheckPosition(position))
            {
                return null;
            }

            _selectedItem = position;
            OnChangeSelectItem?.Invoke(_selectedItem);
            return _quickItemAccessBar[position];
        }

        public void SwapItemInBar(int positionA, int positionB)
        {
            if (!CheckPosition(positionA) || !CheckPosition(positionB))
            {
                return;
            }

            (_quickItemAccessBar[positionA], _quickItemAccessBar[positionB]) =
                (_quickItemAccessBar[positionB], _quickItemAccessBar[positionA]);
        }

        public Item RemoveItem(int position)
        {
            if (!CheckPosition(position))
            {
                return null;
            }

            Item removedStackableItem = _quickItemAccessBar[position];
            _quickItemAccessBar[position] = null;
            return removedStackableItem;
        }
    }
}