using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Interfaces // adding the . Interface allows you to then have a whole section of interfaces if it was a large enough program. They then could be added to the header of a doc and this could help organize. 
{
    interface IWalkAway
    {

        void WalkAway(Player player);



    }
}

//THE .NET FRAMEWORK ALLOWS FOR MUTIPLE INHERITANCES FROM INTERFACES BUT NOT CLASSES

//EVERYTHING IN AN INTERFACE IS PUBLIC 
//ANY CLASS THAT INHERITS THIS WILL NOW HAVE TO HAVE IWALKAWAY() IN IT. 
//INTERFACE NAMEING MUST START WITH A CAPITAL I