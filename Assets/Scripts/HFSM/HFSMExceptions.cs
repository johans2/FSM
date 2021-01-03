using System;

namespace HFSM {
    public class DuplicateSubStateException : Exception {
        public DuplicateSubStateException(string msg) : base(msg) { }
    }

    public class DuplicateTransitionException : Exception {
        public DuplicateTransitionException(string msg) : base(msg) { }
    }
    
    public class NeglectedTriggerException : Exception {
        public NeglectedTriggerException(string msg) : base(msg) { }
    }
}