using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The run-time parts of ValueCollection
/// </summary>
[Serializable]
public partial class ValueCollection  {
    [Serializable]
    public class Value {
        public ValueType Type;
        public float Base;
        public float Modifier;
        public float Derived { get { return Base + Base * Modifier; } }
    }

    /// <summary>
    /// Parse a collection of Presets, overriding any duplicate value types
    /// </summary>
    /// <param name="presets"></param>
    /// <param name="suppressEvents"></param>
    public void Inject(IEnumerable<Value> presets, bool suppressEvents = false) {
        if (presets == null)
            throw new System.ArgumentNullException("ValueCollection.Inject(): presets cannot be null.");

        var temp = SuppressEvents;
        SuppressEvents = suppressEvents;

        foreach (var preset in presets) {
            this[preset.Type, ValueSubtype.Base] = preset.Base;
            this[preset.Type, ValueSubtype.Modifier] = preset.Modifier;
        }

        SuppressEvents = temp;
    }

    /// <summary>
    /// Parse a collection of Presets, ADDING any duplicate value types
    /// </summary>
    /// <param name="presets"></param>
    /// <param name="suppressEvents"></param>
    public void Merge(IEnumerable<Value> presets, bool suppressEvents = false) {
        if (presets == null)
            throw new System.ArgumentNullException("ValueCollection.Merge(): presets cannot be null.");

        var temp = SuppressEvents;
        SuppressEvents = suppressEvents;

        foreach (var preset in presets) {
            this[preset.Type, ValueSubtype.Base] += preset.Base;
            this[preset.Type, ValueSubtype.Modifier] += preset.Modifier;
        }

        SuppressEvents = temp;
    }

    #region " events "
    public event System.Action<System.Object, ValueType, ValueSubtype, float> ValueChanged;

    protected void RaiseValueChanged(ValueType type, ValueSubtype subtype, float oldValue) {
        if (!SuppressEvents && ValueChanged != null) ValueChanged(this, type, subtype, oldValue);
    }
    #endregion

    public bool SuppressEvents { get; set; }
    protected Dictionary<ValueType, Value> _Stats = new Dictionary<ValueType, Value>();

    //[UnityEngine.HideInInspector]
    public List<Value> _Values = new List<Value>();

    /// <summary>
    /// Indexer.  Examples:
    ///   var x = new StatCollection();
    ///   x[StatType.MaxHealth, StatValueType.Base] = 50;
    ///   x[StatType.MaxHealth, StatValueType.Modifier] = 0.2; // 20% bonus
    ///   Assert( x[StatType.MaxHealth] == 60 ); // 50 + 20%
    /// </summary>
    /// <param name="type"></param>
    /// <param name="subtype"></param>
    /// <returns></returns>
    public float this[ValueType type, ValueSubtype subtype = ValueSubtype.Derived] {
        get {
            var storedValue = Fetch(type);

            if (storedValue == null) return 0;


            switch (subtype) {
                case ValueSubtype.Base: { return storedValue.Base; }
                case ValueSubtype.Modifier: { return storedValue.Modifier; }
                default: { return storedValue.Derived; }
            }
            
           
        }
        set {

            //if ( subtype == ValueSubtype.Derived ) throw new System.InvalidOperationException("ValueCollection[].Set: Cannot set EntityStat (" + type.ToString() + ") value directly.  MUST set Base or Modifier instead.");

            // Find list entry:
            var storedValue = Fetch(type);

            // If not found, create new
            if ( storedValue == null ) {
                storedValue = new Value { Type = type };
                _Values.Add(storedValue);
            }

            // For appropriate subtype, remember old value and set new value
            float old = 0;
            switch (subtype) {

                case ValueSubtype.Base:
                    old = storedValue.Base;
                    storedValue.Base = value;
                    break;
                case ValueSubtype.Modifier:
                    old = storedValue.Modifier;
                    storedValue.Modifier = value;
                    break;
                default:
                    old = storedValue.Derived;
                    storedValue.Base = value / (1 + storedValue.Modifier);
                    break;
            }

            // Raise "ValueChanged" event only if value actually changed
            if ( old != value )
                RaiseValueChanged(type, subtype, old);
        }
    }


    protected Value Fetch(ValueType type) {
        foreach (var item in _Values) 
            if (item.Type == type)
                return item;
        
        return null;
    }

    public bool Remove(ValueType stat) {
        return _Stats.Remove(stat);
    }
}

