namespace RSLib
{
    using RSLib.Maths;

    /// <summary>
    /// Class used to manage a health system. Every living unit can have an instance on this class and listen to the Killed event to be notified when dead.
    /// Methods of this class should be accessed by some methods implemented by an interface (something like ILivingUnit) so that there can be a clean
    /// way to handle additional conditions, heal or damage sources, etc.
    /// </summary>
    public class HealthSystem
    {
        private int _health;

        public HealthSystem(int initHealth)
        {
            MaxHealth = initHealth;
            Health = initHealth;
        }
        
        public delegate void KilledEventHandler();
        public delegate void HealthChangedEventHandler(int newHealth);

        public event KilledEventHandler Killed;
        public event HealthChangedEventHandler HealthChanged;

        public int Health
        {
            get => _health;
            private set
            {
                _health = value.Clamp(0, MaxHealth);
                if (IsDead)
                    Killed?.Invoke();
                else
                    HealthChanged?.Invoke(_health);
            }
        }

        public float HealthPercentage => (float)Health / MaxHealth;

        public bool IsDead => Health == 0;

        public int MaxHealth { get; private set; }

        /// <summary>Instantly changes the maximum health. Health is reduced if new maximum health is less than health value.</summary>
        /// <param name="newValue">New maximum health value.</param>
        /// <param name="increaseHealth">Does health also increase if new maximum health is higher than its previous value.</param>
        public void ChangeMaxHealth(int newValue, bool increaseHealth = true)
        {
            int previousMaxHealth = MaxHealth;
            MaxHealth = newValue;

            if (MaxHealth > previousMaxHealth && increaseHealth)
                Health += MaxHealth - previousMaxHealth;
            else if (MaxHealth < Health)
                Health = MaxHealth;
        }

        /// <summary>Removes a given amount of health points.</summary>
        /// <param name="amount">Amount to remove.</param>
        public void Damage(int amount)
        {
            Health -= amount;
        }

        /// <summary>Restores a given amount of health points.</summary>
        /// <param name="amount">Amount to restore.</param>
        /// <param name="ignoreIfDead">If true, heal is not applied if current health is equal to 0.</param>
        public void Heal(int amount, bool ignoreIfDead = true)
        {
            if (IsDead && ignoreIfDead)
                return;

            int previousHealth = Health;
            Health += amount;
        }

        /// <summary>Sets health value to maximum health value.</summary>
        /// <param name="ignoreIfDead">If true, heal is not applied if current health is equal to 0.</param>
        public void HealFull(bool ignoreIfDead = true)
        {
            if (IsDead && ignoreIfDead)
                return;

            int previousHealth = Health;
            Health = MaxHealth;
        }

        /// <summary>Sets health value to 0, and then kills the unit and triggers the Killed event.</summary>
        public void Kill()
        {
            if (IsDead)
            {
                UnityEngine.Debug.LogWarning("HealthSystem.Kill() WARNING: Can not kill an already dead unit, aborting.");
                return;
            }

            Health = 0;
        }
    }
}