using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// First note for debugging:
// Are you calling Update?
public class Cooldown
{
    /// <summary>
    /// Called when any one of this cooldown's charges become available
    /// </summary>
    public Action OnAvailable;

    /// <summary>
    /// True when at least one charge on this cooldown is available for consumption
    /// </summary>
    public bool Available
    {
        get
        {
            return GetNextAvailableCharge() != -1;
        }
    }

    /// <summary>
    /// Returns how many charges are available
    /// </summary>
    public int ChargesAvailable
    {
        get
        {
            int result = 0;
            foreach( bool charge in _charges )
            {
                if( charge )
                {
                    result += 1;
                }
            }

            return result;
        }
    }
    public int chargesTotal;

    public float Duration { get; set; }

    // Each element is the start time for a charge
    private float[] _startTimes;

    // Each element represents if a charge is available
    public bool[] _charges;

    // Determines whether cooldowns are replenished in parallel
    // or one-by-one
    public bool updateCooldownsInParallel = false;

    private bool _isPaused = false;
    private float pauseTime;
    public bool IsPaused
    {
        get
        {
            return _isPaused;
        }
        set
        {
            _isPaused = value;
            
            if( value )
            {
                pauseTime = Time.time;
            }
            else
            {
                float pauseDuration = Time.time - pauseTime;
                for(int i = 0; i < _startTimes.Length; i++)
                {
                    _startTimes[i] = _startTimes[i] + pauseDuration;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">The duration of this cooldown</param>
    /// <param name="charges">How many charges should be tracked for this cooldown</param>
    public Cooldown(float duration, int charges = 1, bool parallelCooldown = false)
    {
        Duration = duration;
        _startTimes = new float[charges];
        _charges = new bool[charges];

        chargesTotal = charges;

        updateCooldownsInParallel = parallelCooldown;
        
        for(int i = 0; i < charges; i++)
        {
            _startTimes[i] = Time.time - duration;
            _charges[i] = true;
        }
    }

    /// <summary>
    /// Call in Monobehavior.Update()
    /// </summary>
    public void Update()
    {
        if( !_isPaused )
        {
            int closestUnavailable = GetClosestUnavailableCharge();
            if (closestUnavailable != -1)
            {
                if(Time.time > _startTimes[closestUnavailable] + Duration)
                {
                    _charges[closestUnavailable] = true;

                    // If we're not updating cooldowns in parallel,
                    // set the starting time of the next unavailable charge
                    // to the current time, so that charges are replenished
                    // one at a time, instead of depending on when they
                    // were consumed
                    if (!updateCooldownsInParallel)
                    {
                        closestUnavailable = GetClosestUnavailableCharge();
                        if (closestUnavailable != -1)
                        {
                            _startTimes[closestUnavailable] = Time.time;
                        }
                    }

                    OnAvailable?.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Call to consume an available charge of this cooldown
    /// Returns if the operation was successful
    /// </summary>
    public bool Consume()
    {
        int available = GetNextAvailableCharge();

        if (available != -1)
        {
            _startTimes[available] = Time.time;
            _charges[available] = false;

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Consumes all charges at once
    /// </summary>
    public bool ConsumeAllCharges()
    {
        for(int i = 0; i < _charges.Length; i++)
        {
            _startTimes[i] = Time.time;
            _charges[i] = false;
        }

        return true;
    }

    /// <summary>
    /// Checks if any charges are available
    /// </summary>
    public bool IsAvailable()
    {
        return ChargesAvailable != 0;
    }

    /// <summary>
    /// Call to get the index of the next available charge
    /// </summary>
    private int GetNextAvailableCharge()
    {
        int available = -1;

        for (int i = 0; i < _charges.Length; i++)
        {
            if (_charges[i])
            {
                available = i;
                break;
            }
        }

        return available;
    }

    private int GetClosestUnavailableCharge()
    {
        int foundIndex = -1;
        // check the first element manually because -1 is not a valid index
        if (!_charges[0])
        {
            foundIndex = 0;
        }

        // check the rest of the elements
        if(_charges.Length > 1)
        {
            for (int i = 1; i < _startTimes.Length; i++)
            {
                if(foundIndex != -1)
                {
                    if(_startTimes[foundIndex] > _startTimes[i] && !_charges[i])
                    {
                        foundIndex = i;
                    }
                } else
                {
                    if(!_charges[i])
                    {
                        foundIndex = i;
                    }
                }
            }
        }

        return foundIndex;
    }

    /// <summary>
    /// Resets the timer on the closest unavailable charge
    /// </summary>
    public bool ResetClosestUnavailableCharge()
    {
        int closestUnavailable = GetClosestUnavailableCharge();

        if (closestUnavailable != -1)
        {
            _startTimes[closestUnavailable] = Time.time;
            _charges[closestUnavailable] = true;

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Refreshes the timer on the closest unavailable charge, making it instantly available
    /// </summary>
    public bool RefreshClosestUnavailableCharge()
    {
        int closestUnavailable = GetClosestUnavailableCharge();

        if (closestUnavailable != -1)
        {
            _startTimes[closestUnavailable] = Time.time - Duration;

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Call to get the closest charge that is counting down
    /// </summary>
    public float GetClosestUnavailableRemainingTime()
    {
        int closestUnavailable = GetClosestUnavailableCharge();
        if (closestUnavailable != -1)
        {
            if (!_isPaused)
            {
                return _startTimes[closestUnavailable] + Duration - Time.time;
            }
            else
            {
                return _startTimes[closestUnavailable] + Duration - pauseTime;
            }
        }
        else
        {
            return 0;
        }
    }

    public void SetChargeAmount( int amount )
    {
        // Either we truncate the data
        // or we have a bunch of extra data
        Array.Resize<bool>(ref _charges, amount);
        Array.Resize<float>(ref _startTimes, amount);

        // If we truncated the data, this shouldn't run
        // If we have a bunch of extra data, this will populate them
        for( int i = Mathf.Min(amount, chargesTotal); i < Mathf.Max(amount, chargesTotal); i++)
        {
            _startTimes[i] = Time.time - Duration;
            _charges[i] = true;
        }

        chargesTotal = amount;
    }

    /// <summary>
    /// Adjust the closest charge that is counting down to elongate or shorten.
    /// Positive number elongates.
    /// Negative number shortens.
    /// <param name="amount">The amount to adjust</param>
    /// </summary>
    public void AdjustClosestUnavailableCharge( float amount )
    {
        int closestUnavailable = GetClosestUnavailableCharge();
        if (closestUnavailable != -1)
        {
            // If we are adjusting by a positive amount, then we want the charge to take longer
            // So if we add to the existing startTime, then we make the startTime happen sooner
            // And close the gap between then and now, meaning it will need to take longer to
            // reach Duration
            _startTimes[closestUnavailable] = _startTimes[closestUnavailable] + amount;
        }
    }
}

