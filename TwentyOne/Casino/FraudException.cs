using System;
using System.Collections.Generic;
using System.Text;

namespace Casino
{
    public class FraudException : Exception  //It's inheriting exception which already exists in the .NET Framework.
    {
        //We will create constructors to do this that will in reality be constructors inherited from the Exception class. 
        //We are doing this to catch a very specific exception to handle it differently.

        public FraudException()
            : base() { }                //This is inheriting from the base constructor defined in Exception. It's the same as saying new Exception. It's jsut inheriting.
        public FraudException(string message)
            : base(message) { }

        //We are overloading this constructor method.
        //All we are doing here is creating two constructors one of them overloading the other, and having them impliment the exact same implimentation that exists for exception

    }
}
