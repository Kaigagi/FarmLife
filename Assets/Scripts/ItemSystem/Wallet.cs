namespace ItemSystem
{
    public class Wallet
    {
        private int _gold = 0;
        private int _silver = 0;
        private int _copper = 0;

        public void AddGold(int amount)
        {
            _gold += amount;
        }
        
        public void AddSilver(int amount)
        {
            _silver += amount;
        }
        
        public void AddCopper(int amount)
        {
            _copper += amount;
        }

        public void SubtractGold(int amount)
        {
            if (_gold - amount < 0)
            {
                return;
            }

            _gold -= amount;
        } 
        
        public void SubtractSilver(int amount)
        {
            if (_gold - amount < 0)
            {
                return;
            }

            _silver -= amount;
        } 
        
        public void SubtractCopper(int amount)
        {
            if (_gold - amount < 0)
            {
                return;
            }

            _copper -= amount;
        } 
    }
}