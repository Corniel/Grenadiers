//-----------------------------------------------------------------------
// <copyright>
// Copyright © Corniel Nobel 2019-current
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Runtime.Serialization;

namespace Grenadiers
{
    [Serializable]
    public class NilObjectUnavailable : NullReferenceException
    {
        public NilObjectUnavailable() { }

        public NilObjectUnavailable(string message) : base(message) { }

        public NilObjectUnavailable(string message, Exception innerException) : base(message, innerException) { }

        protected NilObjectUnavailable(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static NilObjectUnavailable For(Type type)
            => new NilObjectUnavailable($"Nil value not available for {type}.");
    }
}
