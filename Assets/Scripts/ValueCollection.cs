﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ValueCollection : UnityEngine.Object {
    public class Value : UnityEngine.Object {

        public float Base;
        public float Modifier;
        public float Derived { get { return Base + Base * Modifier; } }
    }

    #region " events "
    public event System.Action<Object, ValueType, ValueSubtype, float> ValueChanged;

    protected void RaiseValueChanged(ValueType type, ValueSubtype subtype, float oldValue) {
        if (ValueChanged != null) ValueChanged(this, type, subtype, oldValue);
    }
    #endregion

    protected Dictionary<ValueType, Value> _Stats = new Dictionary<ValueType, Value>();


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
            if (!_Stats.ContainsKey(type))
                throw new InvalidOperationException("ValueCollection does not contain a value of type " + type.ToString());

            switch (subtype) {
                case ValueSubtype.Base: { return _Stats[type].Base; }
                case ValueSubtype.Modifier: { return _Stats[type].Modifier; }
                default: { return _Stats[type].Derived; }
            }
        }
        set {
            if (!_Stats.ContainsKey(type))
                _Stats.Add(type, new Value());

            float old = 0;
            switch (subtype) {

                case ValueSubtype.Base:
                    old = _Stats[type].Base;
                    _Stats[type].Base = value;
                    break;
                case ValueSubtype.Modifier:
                    old = _Stats[type].Modifier;
                    _Stats[type].Modifier = value;
                    break;
                case ValueSubtype.Derived:
                    throw new System.InvalidOperationException("Cannot set EntityStat (" + type.ToString() + ") value directly.  MUST set Base or Modifier instead.");
            }
            RaiseValueChanged(type, subtype, old);
        }
    }

    public bool Remove(ValueType stat) {
        return _Stats.Remove(stat);
    }
}

